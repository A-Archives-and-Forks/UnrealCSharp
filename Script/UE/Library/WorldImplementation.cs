using Script.CoreUObject;
using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static partial class UWorldImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __UWorld_SpawnActorImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, nint>
            __UWorld_SpawnActorImplementation;
#endif

        public static unsafe T UWorld_SpawnActorImplementation<T>(nint InWorld, nint InClass,
            nint InTransform, nint InSpawnParameters) where T : UObject
        {
#if !LEANCLR
            if (__UWorld_SpawnActorImplementation == null)
            {
                __UWorld_SpawnActorImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.UWorldImplementation::UWorld_SpawnActorImplementation");
            }
#endif

            var Handle = __UWorld_SpawnActorImplementation(InWorld, InClass, InTransform, InSpawnParameters);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : null;
        }
    }
}