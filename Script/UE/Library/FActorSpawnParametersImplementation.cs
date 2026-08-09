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
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte> __FActorSpawnParameters_GetbNoFailImplementation;
#endif

        public static unsafe bool FActorSpawnParameters_GetbNoFailImplementation(nint InObject)
        {
#if !LEANCLR
            if (__FActorSpawnParameters_GetbNoFailImplementation == null)
            {
                __FActorSpawnParameters_GetbNoFailImplementation = (delegate* unmanaged[Cdecl]<nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.FActorSpawnParametersImplementation::FActorSpawnParameters_GetbNoFailImplementation");
            }
#endif

            return __FActorSpawnParameters_GetbNoFailImplementation(InObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FActorSpawnParameters_SetbNoFailImplementation(nint A0, byte A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte, void>
            __FActorSpawnParameters_SetbNoFailImplementation;
#endif

        public static unsafe void FActorSpawnParameters_SetbNoFailImplementation(nint InObject, bool InValue)
        {
#if !LEANCLR
            if (__FActorSpawnParameters_SetbNoFailImplementation == null)
            {
                __FActorSpawnParameters_SetbNoFailImplementation = (delegate* unmanaged[Cdecl]<nint, byte, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FActorSpawnParametersImplementation::FActorSpawnParameters_SetbNoFailImplementation");
            }
#endif

            __FActorSpawnParameters_SetbNoFailImplementation(InObject, (byte)(InValue ? 1 : 0));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __FActorSpawnParameters_GetbDeferConstructionImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte>
            __FActorSpawnParameters_GetbDeferConstructionImplementation;
#endif

        public static unsafe bool FActorSpawnParameters_GetbDeferConstructionImplementation(nint InObject)
        {
#if !LEANCLR
            if (__FActorSpawnParameters_GetbDeferConstructionImplementation == null)
            {
                __FActorSpawnParameters_GetbDeferConstructionImplementation = (delegate* unmanaged[Cdecl]<nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.FActorSpawnParametersImplementation::FActorSpawnParameters_GetbDeferConstructionImplementation");
            }
#endif

            return __FActorSpawnParameters_GetbDeferConstructionImplementation(InObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FActorSpawnParameters_SetbDeferConstructionImplementation(nint A0, byte A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte, void>
            __FActorSpawnParameters_SetbDeferConstructionImplementation;
#endif

        public static unsafe void FActorSpawnParameters_SetbDeferConstructionImplementation(nint InObject, bool InValue)
        {
#if !LEANCLR
            if (__FActorSpawnParameters_SetbDeferConstructionImplementation == null)
            {
                __FActorSpawnParameters_SetbDeferConstructionImplementation =
                    (delegate* unmanaged[Cdecl]<nint, byte, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FActorSpawnParametersImplementation::FActorSpawnParameters_SetbDeferConstructionImplementation");
            }
#endif

            __FActorSpawnParameters_SetbDeferConstructionImplementation(InObject, (byte)(InValue ? 1 : 0));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte>
            __FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation;
#endif

        public static unsafe bool FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation(nint InObject)
        {
#if !LEANCLR
            if (__FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation == null)
            {
                __FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation =
                    (delegate* unmanaged[Cdecl]<nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.FActorSpawnParametersImplementation::FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation");
            }
#endif

            return __FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation(InObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation(nint A0, byte A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte, void>
            __FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation;
#endif

        public static unsafe void FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation(nint InObject,
            bool InValue)
        {
#if !LEANCLR
            if (__FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation == null)
            {
                __FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation =
                    (delegate* unmanaged[Cdecl]<nint, byte, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FActorSpawnParametersImplementation::FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation");
            }
#endif

            __FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation(InObject, (byte)(InValue ? 1 : 0));
        }
    }
}