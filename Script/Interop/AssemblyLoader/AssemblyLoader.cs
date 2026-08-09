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
            HandleData.Clear();

            TypeBridge.Clear();

            // Null out every StaticClassSingleton / StaticStructSingleton cache so
            // StaticClass() / StaticStruct() re-fetch fresh wrappers on the next PIE
            // session. HandleData.Clear() invalidated all handle→object mappings; ??=
            // returns the stale (non-null but handle-less) cached wrapper otherwise.
            ResetStaticSingletons();

            if (Context != null)
            {
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
        }

        private static void ResetStaticSingletons()
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                System.Type[] types;
                try { types = asm.GetTypes(); }
                catch { continue; }

                foreach (var type in types)
                {
                    NullStaticSingletonProperty(type, "StaticClassSingleton");
                    NullStaticSingletonProperty(type, "StaticStructSingleton");
                }
            }
        }

        private static void NullStaticSingletonProperty(System.Type type, string propName)
        {
            try
            {
                var prop = type.GetProperty(propName,
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Static);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(null, null);
                }
            }
            catch
            {
                // skip types that throw during reflection
            }
        }
    }
}