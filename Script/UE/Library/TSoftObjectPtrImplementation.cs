using System;
using Script.CoreUObject;
using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static class TSoftObjectPtrImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TSoftObjectPtr_RegisterImplementation(nint A0, nint A1, nint A2);
#else
        private static nint __TSoftObjectPtr_RegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, void>
            __TSoftObjectPtr_RegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, void>)MethodBridge.Resolve(
                ref __TSoftObjectPtr_RegisterImplementation_Slot,
                "Script.Library.TSoftObjectPtrImplementation::TSoftObjectPtr_RegisterImplementation");
#endif

        public static unsafe void TSoftObjectPtr_RegisterImplementation<T>(TSoftObjectPtr<T> InSoftObjectPtr,
            nint InObject, Type InType) where T : UObject
        {
            __TSoftObjectPtr_RegisterImplementation(HandleData.Alloc(InSoftObjectPtr), InObject,
                HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __TSoftObjectPtr_IdenticalImplementation(nint A0, nint A1);
#else
        private static nint __TSoftObjectPtr_IdenticalImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __TSoftObjectPtr_IdenticalImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, byte>)MethodBridge.Resolve(
                ref __TSoftObjectPtr_IdenticalImplementation_Slot,
                "Script.Library.TSoftObjectPtrImplementation::TSoftObjectPtr_IdenticalImplementation");
#endif

        public static unsafe bool TSoftObjectPtr_IdenticalImplementation(nint InA, nint InB)
        {
            return __TSoftObjectPtr_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TSoftObjectPtr_UnRegisterImplementation(nint A0);
#else
        private static nint __TSoftObjectPtr_UnRegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __TSoftObjectPtr_UnRegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(
                ref __TSoftObjectPtr_UnRegisterImplementation_Slot,
                "Script.Library.TSoftObjectPtrImplementation::TSoftObjectPtr_UnRegisterImplementation");
#endif

        public static unsafe void TSoftObjectPtr_UnRegisterImplementation(nint InSoftObjectPtr)
        {
            __TSoftObjectPtr_UnRegisterImplementation(InSoftObjectPtr);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __TSoftObjectPtr_GetImplementation(nint A0);
#else
        private static nint __TSoftObjectPtr_GetImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __TSoftObjectPtr_GetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint>)MethodBridge.Resolve(ref __TSoftObjectPtr_GetImplementation_Slot,
                "Script.Library.TSoftObjectPtrImplementation::TSoftObjectPtr_GetImplementation");
#endif

        public static unsafe T TSoftObjectPtr_GetImplementation<T>(nint InSoftObjectPtr)
        {
            var Handle = __TSoftObjectPtr_GetImplementation(InSoftObjectPtr);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __TSoftObjectPtr_LoadSynchronousImplementation(nint A0);
#else
        private static nint __TSoftObjectPtr_LoadSynchronousImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __TSoftObjectPtr_LoadSynchronousImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint>)MethodBridge.Resolve(
                ref __TSoftObjectPtr_LoadSynchronousImplementation_Slot,
                "Script.Library.TSoftObjectPtrImplementation::TSoftObjectPtr_LoadSynchronousImplementation");
#endif

        public static unsafe T TSoftObjectPtr_LoadSynchronousImplementation<T>(nint InSoftObjectPtr)
        {
            var Handle = __TSoftObjectPtr_LoadSynchronousImplementation(InSoftObjectPtr);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }
    }
}