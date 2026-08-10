using System;
using Script.CoreUObject;
using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static class TLazyObjectPtrImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TLazyObjectPtr_RegisterImplementation(nint A0, nint A1, nint A2);
#else
        private static nint __TLazyObjectPtr_RegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, void>
            __TLazyObjectPtr_RegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, void>)MethodBridge.Resolve(
                ref __TLazyObjectPtr_RegisterImplementation_Slot,
                "Script.Library.TLazyObjectPtrImplementation::TLazyObjectPtr_RegisterImplementation");
#endif

        public static unsafe void TLazyObjectPtr_RegisterImplementation<T>(TLazyObjectPtr<T> InLazyObjectPtr,
            nint InObject, Type InType) where T : UObject
        {
            __TLazyObjectPtr_RegisterImplementation(HandleData.Alloc(InLazyObjectPtr), InObject,
                HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __TLazyObjectPtr_IdenticalImplementation(nint A0, nint A1);
#else
        private static nint __TLazyObjectPtr_IdenticalImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __TLazyObjectPtr_IdenticalImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, byte>)MethodBridge.Resolve(
                ref __TLazyObjectPtr_IdenticalImplementation_Slot,
                "Script.Library.TLazyObjectPtrImplementation::TLazyObjectPtr_IdenticalImplementation");
#endif

        public static unsafe bool TLazyObjectPtr_IdenticalImplementation(nint InA, nint InB)
        {
            return __TLazyObjectPtr_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TLazyObjectPtr_UnRegisterImplementation(nint A0);
#else
        private static nint __TLazyObjectPtr_UnRegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __TLazyObjectPtr_UnRegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(
                ref __TLazyObjectPtr_UnRegisterImplementation_Slot,
                "Script.Library.TLazyObjectPtrImplementation::TLazyObjectPtr_UnRegisterImplementation");
#endif

        public static unsafe void TLazyObjectPtr_UnRegisterImplementation(nint InLazyObjectPtr)
        {
            __TLazyObjectPtr_UnRegisterImplementation(InLazyObjectPtr);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __TLazyObjectPtr_GetImplementation(nint A0);
#else
        private static nint __TLazyObjectPtr_GetImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __TLazyObjectPtr_GetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint>)MethodBridge.Resolve(ref __TLazyObjectPtr_GetImplementation_Slot,
                "Script.Library.TLazyObjectPtrImplementation::TLazyObjectPtr_GetImplementation");
#endif

        public static unsafe T TLazyObjectPtr_GetImplementation<T>(nint InLazyObjectPtr)
        {
            var Handle = __TLazyObjectPtr_GetImplementation(InLazyObjectPtr);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }
    }
}