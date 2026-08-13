using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Interop
{
    public static class AssemblyLoader
    {
        private static UnrealAssemblyLoadContext? Context;

        internal static System.Runtime.Loader.AssemblyLoadContext? CurrentContext => Context;

        [UnmanagedCallersOnly]
        public static unsafe nint LoadFromStream(byte* InData, int InLength, char* InInPublishDirectory)
        {
            if (InData != null && InLength > 0)
            {
                Context ??= new UnrealAssemblyLoadContext(InInPublishDirectory is not null
                    ? new string(InInPublishDirectory)
                    : string.Empty);

                using var Stream = new UnmanagedMemoryStream(InData, InLength);

                return HandleData.Alloc(Context.LoadFromStream(Stream));
            }

            return 0;
        }

        [UnmanagedCallersOnly]
        public static void Unload()
        {
            // Unconditional (deliberately NOT inside the Context guard below): LeanCLR never creates a
            // load context — it loads assemblies through its own file loader, so LoadFromStream is never
            // called and Context stays null. Leaving these inside the guard meant they never ran there.
            HandleData.Clear();

            TypeBridge.Clear();

            if (Context != null)
            {
                // Mono/CoreCLR: every script static lives in this collectible context, so unloading it
                // discards the StaticClassSingleton / StaticStructSingleton caches along with everything
                // else. No explicit reset needed here - a reflection sweep would be pure waste.
                // The registry must still be emptied: it holds Type / PropertyInfo objects from that
                // context, and keeping them alive here would pin the context and defeat the unload.
                RegisteredStaticSingletons.Clear();

                var ContextWeakReference = new WeakReference(Context);

                try
                {
                    Context.Unload();
                }
                finally
                {
                    Context = null;
                }

                const int TimeLimit = 2000;

                var Stopwatch = System.Diagnostics.Stopwatch.StartNew();

                while (ContextWeakReference.IsAlive && Stopwatch.ElapsedMilliseconds < TimeLimit)
                {
                    GC.Collect();

                    GC.WaitForPendingFinalizers();
                }
            }
            else
            {
                // LeanCLR (no collectible context): the script assemblies and their statics survive
                // teardown, so StaticClass() / StaticStruct() would keep returning the wrapper cached by
                // `??=` - non-null, but holding a handle that HandleData.Clear() just invalidated. The
                // next PIE session then cannot spawn (the native ObjectRegistry is fresh and has no such
                // handle). Null the cached wrappers so they are re-fetched.
                ResetStaticSingletons();
            }
        }

        // P8.12: teardown used to walk every proxy type in every assembly and null all of them --
        // 13337 singleton properties in this project, and almost all of the writes were null over null
        // because StaticClass() / StaticStruct() had never been called for that type. Measured on the
        // LeanCLR interpreter that sweep cost 4356.99 ms per teardown, against 0.85 ms for everything
        // else Unload does. Generated StaticClass() / StaticStruct() now hand their type to
        // RegisterStaticClassSingleton / RegisterStaticStructSingleton the first time they actually
        // cache a wrapper (the ??= short-circuits, so this runs once per type and never again), which
        // turns teardown from O(all proxy types) into O(types actually used).
        //
        // A type appears here at most once: a proxy type declares either StaticClassSingleton or
        // StaticStructSingleton, never both. Registration deliberately resolves the PropertyInfo now
        // rather than at teardown, so teardown is nothing but the SetValue calls. No locking, matching
        // the surrounding code and the non-atomic ??= in the callers themselves.
        private static readonly System.Collections.Generic.Dictionary<Type, System.Reflection.PropertyInfo>
            RegisteredStaticSingletons = new();

        public static object? RegisterStaticClassSingleton(Type InType, object? InValue)
        {
            return RegisterStaticSingleton(InType, "StaticClassSingleton", InValue);
        }

        public static object? RegisterStaticStructSingleton(Type InType, object? InValue)
        {
            return RegisterStaticSingleton(InType, "StaticStructSingleton", InValue);
        }

        private static object? RegisterStaticSingleton(Type? InType, string InName, object? InValue)
        {
            // A null value means nothing got cached, so there is nothing to reset later.
            if (InType == null || InValue == null || RegisteredStaticSingletons.ContainsKey(InType))
            {
                return InValue;
            }

            try
            {
                var Property = InType.GetProperty(InName,
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

                if (Property != null && Property.CanWrite)
                {
                    RegisteredStaticSingletons[InType] = Property;
                }
            }
            catch
            {
                // A type that throws during reflection simply is not tracked; the caller still gets its
                // value. Worst case that one wrapper survives teardown, exactly as before P8.12.
            }

            return InValue;
        }

        private static void ResetStaticSingletons()
        {
            foreach (var Property in RegisteredStaticSingletons.Values)
            {
                try
                {
                    Property.SetValue(null, null);
                }
                catch
                {
                    // A setter that throws must not abort the remaining resets.
                }
            }

            RegisteredStaticSingletons.Clear();
        }
    }
}