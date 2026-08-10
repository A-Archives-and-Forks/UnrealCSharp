using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static partial class FActorSpawnParametersImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __FActorSpawnParameters_GetbNoFailImplementation(nint A0);
#else
        private static nint __FActorSpawnParameters_GetbNoFailImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte> __FActorSpawnParameters_GetbNoFailImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte>)MethodBridge.Resolve(
                ref __FActorSpawnParameters_GetbNoFailImplementation_Slot,
                "Script.Library.FActorSpawnParametersImplementation::FActorSpawnParameters_GetbNoFailImplementation");
#endif

        public static unsafe bool FActorSpawnParameters_GetbNoFailImplementation(nint InObject)
        {
            return __FActorSpawnParameters_GetbNoFailImplementation(InObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FActorSpawnParameters_SetbNoFailImplementation(nint A0, byte A1);
#else
        private static nint __FActorSpawnParameters_SetbNoFailImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte, void>
            __FActorSpawnParameters_SetbNoFailImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte, void>)MethodBridge.Resolve(
                ref __FActorSpawnParameters_SetbNoFailImplementation_Slot,
                "Script.Library.FActorSpawnParametersImplementation::FActorSpawnParameters_SetbNoFailImplementation");
#endif

        public static unsafe void FActorSpawnParameters_SetbNoFailImplementation(nint InObject, bool InValue)
        {
            __FActorSpawnParameters_SetbNoFailImplementation(InObject, (byte)(InValue ? 1 : 0));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __FActorSpawnParameters_GetbDeferConstructionImplementation(nint A0);
#else
        private static nint __FActorSpawnParameters_GetbDeferConstructionImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte>
            __FActorSpawnParameters_GetbDeferConstructionImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte>)MethodBridge.Resolve(
                ref __FActorSpawnParameters_GetbDeferConstructionImplementation_Slot,
                "Script.Library.FActorSpawnParametersImplementation::FActorSpawnParameters_GetbDeferConstructionImplementation");
#endif

        public static unsafe bool FActorSpawnParameters_GetbDeferConstructionImplementation(nint InObject)
        {
            return __FActorSpawnParameters_GetbDeferConstructionImplementation(InObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FActorSpawnParameters_SetbDeferConstructionImplementation(nint A0, byte A1);
#else
        private static nint __FActorSpawnParameters_SetbDeferConstructionImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte, void>
            __FActorSpawnParameters_SetbDeferConstructionImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte, void>)MethodBridge.Resolve(
                ref __FActorSpawnParameters_SetbDeferConstructionImplementation_Slot,
                "Script.Library.FActorSpawnParametersImplementation::FActorSpawnParameters_SetbDeferConstructionImplementation");
#endif

        public static unsafe void FActorSpawnParameters_SetbDeferConstructionImplementation(nint InObject, bool InValue)
        {
            __FActorSpawnParameters_SetbDeferConstructionImplementation(InObject, (byte)(InValue ? 1 : 0));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation(nint A0);
#else
        private static nint __FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte>
            __FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte>)MethodBridge.Resolve(
                ref __FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation_Slot,
                "Script.Library.FActorSpawnParametersImplementation::FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation");
#endif

        public static unsafe bool FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation(nint InObject)
        {
            return __FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation(InObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation(nint A0, byte A1);
#else
        private static nint __FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte, void>
            __FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte, void>)MethodBridge.Resolve(
                ref __FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation_Slot,
                "Script.Library.FActorSpawnParametersImplementation::FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation");
#endif

        public static unsafe void FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation(nint InObject,
            bool InValue)
        {
            __FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation(InObject, (byte)(InValue ? 1 : 0));
        }
    }
}