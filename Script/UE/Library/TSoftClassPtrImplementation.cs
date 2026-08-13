using System;
using Script.CoreUObject;
using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static class TSoftClassPtrImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TSoftClassPtr_RegisterImplementation(nint A0, nint A1, nint A2);
#else
        private static nint __TSoftClassPtr_RegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, void>
            __TSoftClassPtr_RegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, void>)MethodBridge.Resolve(
                ref __TSoftClassPtr_RegisterImplementation_Slot,
                "Script.Library.TSoftClassPtrImplementation::TSoftClassPtr_RegisterImplementation");
#endif

        public static unsafe void TSoftClassPtr_RegisterImplementation<T>(TSoftClassPtr<T> InSoftClassPtr,
            nint InClass, Type InType) where T : UObject
        {
            __TSoftClassPtr_RegisterImplementation(HandleData.Alloc(InSoftClassPtr), InClass,
                HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __TSoftClassPtr_IdenticalImplementation(nint A0, nint A1);
#else
        private static nint __TSoftClassPtr_IdenticalImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __TSoftClassPtr_IdenticalImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, byte>)MethodBridge.Resolve(
                ref __TSoftClassPtr_IdenticalImplementation_Slot,
                "Script.Library.TSoftClassPtrImplementation::TSoftClassPtr_IdenticalImplementation");
#endif

        public static unsafe bool TSoftClassPtr_IdenticalImplementation(nint InA, nint InB)
        {
            return __TSoftClassPtr_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TSoftClassPtr_UnRegisterImplementation(nint A0);
#else
        private static nint __TSoftClassPtr_UnRegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __TSoftClassPtr_UnRegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(
                ref __TSoftClassPtr_UnRegisterImplementation_Slot,
                "Script.Library.TSoftClassPtrImplementation::TSoftClassPtr_UnRegisterImplementation");
#endif

        public static unsafe void TSoftClassPtr_UnRegisterImplementation(nint InSoftClassPtr)
        {
            __TSoftClassPtr_UnRegisterImplementation(InSoftClassPtr);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __TSoftClassPtr_GetImplementation(nint A0);
#else
        private static nint __TSoftClassPtr_GetImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __TSoftClassPtr_GetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint>)MethodBridge.Resolve(ref __TSoftClassPtr_GetImplementation_Slot,
                "Script.Library.TSoftClassPtrImplementation::TSoftClassPtr_GetImplementation");
#endif

        public static unsafe UClass TSoftClassPtr_GetImplementation(nint InSoftClassPtr)
        {
            var Handle = __TSoftClassPtr_GetImplementation(InSoftClassPtr);

            return Handle != 0 ? (UClass)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __TSoftClassPtr_LoadSynchronousImplementation(nint A0);
#else
        private static nint __TSoftClassPtr_LoadSynchronousImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __TSoftClassPtr_LoadSynchronousImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint>)MethodBridge.Resolve(
                ref __TSoftClassPtr_LoadSynchronousImplementation_Slot,
                "Script.Library.TSoftClassPtrImplementation::TSoftClassPtr_LoadSynchronousImplementation");
#endif

        public static unsafe UClass TSoftClassPtr_LoadSynchronousImplementation(nint InSoftClassPtr)
        {
            var Handle = __TSoftClassPtr_LoadSynchronousImplementation(InSoftClassPtr);

            return Handle != 0 ? (UClass)HandleData.GetObject(Handle) : null;
        }
    }
}