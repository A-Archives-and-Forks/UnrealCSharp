using System;
using Script.CoreUObject;
using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static class TWeakObjectPtrImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TWeakObjectPtr_RegisterImplementation(nint A0, nint A1, nint A2);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, void>
            __TWeakObjectPtr_RegisterImplementation;
#endif

        public static unsafe void TWeakObjectPtr_RegisterImplementation<T>(TWeakObjectPtr<T> InWeakObjectPtr,
            nint InObject, Type InType) where T : UObject
        {
#if !LEANCLR
            if (__TWeakObjectPtr_RegisterImplementation == null)
            {
                __TWeakObjectPtr_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TWeakObjectPtrImplementation::TWeakObjectPtr_RegisterImplementation");
            }
#endif

            __TWeakObjectPtr_RegisterImplementation(HandleData.Alloc(InWeakObjectPtr), InObject,
                HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __TWeakObjectPtr_IdenticalImplementation(nint A0, nint A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __TWeakObjectPtr_IdenticalImplementation;
#endif

        public static unsafe bool TWeakObjectPtr_IdenticalImplementation(nint InA, nint InB)
        {
#if !LEANCLR
            if (__TWeakObjectPtr_IdenticalImplementation == null)
            {
                __TWeakObjectPtr_IdenticalImplementation = (delegate* unmanaged[Cdecl]<nint, nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.TWeakObjectPtrImplementation::TWeakObjectPtr_IdenticalImplementation");
            }
#endif

            return __TWeakObjectPtr_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TWeakObjectPtr_UnRegisterImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __TWeakObjectPtr_UnRegisterImplementation;
#endif

        public static unsafe void TWeakObjectPtr_UnRegisterImplementation(nint InWeakObjectPtr)
        {
#if !LEANCLR
            if (__TWeakObjectPtr_UnRegisterImplementation == null)
            {
                __TWeakObjectPtr_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TWeakObjectPtrImplementation::TWeakObjectPtr_UnRegisterImplementation");
            }
#endif

            __TWeakObjectPtr_UnRegisterImplementation(InWeakObjectPtr);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __TWeakObjectPtr_GetImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __TWeakObjectPtr_GetImplementation;
#endif

        public static unsafe T TWeakObjectPtr_GetImplementation<T>(nint InWeakObjectPtr)
        {
#if !LEANCLR
            if (__TWeakObjectPtr_GetImplementation == null)
            {
                __TWeakObjectPtr_GetImplementation = (delegate* unmanaged[Cdecl]<nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.TWeakObjectPtrImplementation::TWeakObjectPtr_GetImplementation");
            }
#endif

            var Handle = __TWeakObjectPtr_GetImplementation(InWeakObjectPtr);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }
    }
}