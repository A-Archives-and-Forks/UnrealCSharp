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

        // Discovered once and reused: on the backend that reaches this path the script assemblies are
        // never unloaded, so these PropertyInfo objects stay valid across PIE sessions. Only the
        // already-proven reflection surface is used here (GetAssemblies / GetTypes / GetProperty /
        // SetValue) - see the design note on not narrowing this by assembly identity.
        private static System.Reflection.PropertyInfo[]? StaticSingletonProperties;

        private static int StaticSingletonAssemblyCount;

        private static void ResetStaticSingletons()
        {
            var Assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Re-discover if the assembly set grew since the last pass.
            if (StaticSingletonProperties == null || Assemblies.Length != StaticSingletonAssemblyCount)
            {
                StaticSingletonProperties = DiscoverStaticSingletonProperties(Assemblies);

                StaticSingletonAssemblyCount = Assemblies.Length;
            }

            foreach (var Property in StaticSingletonProperties)
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
        }

        private static System.Reflection.PropertyInfo[] DiscoverStaticSingletonProperties(
            System.Reflection.Assembly[] InAssemblies)
        {
            var Result = new System.Collections.Generic.List<System.Reflection.PropertyInfo>();

            foreach (var Assembly in InAssemblies)
            {
                System.Type[] Types;

                try
                {
                    Types = Assembly.GetTypes();
                }
                catch
                {
                    continue;
                }

                foreach (var Type in Types)
                {
                    AddStaticSingletonProperty(Result, Type, "StaticClassSingleton");

                    AddStaticSingletonProperty(Result, Type, "StaticStructSingleton");
                }
            }

            return Result.ToArray();
        }

        private static void AddStaticSingletonProperty(
            System.Collections.Generic.List<System.Reflection.PropertyInfo> OutProperties,
            System.Type InType, string InPropertyName)
        {
            try
            {
                var Property = InType.GetProperty(InPropertyName,
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Static);

                if (Property != null && Property.CanWrite)
                {
                    OutProperties.Add(Property);
                }
            }
            catch
            {
                // skip types that throw during reflection
            }
        }
    }
}