using Script.CoreUObject;
using System;
using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static class TSubclassOfImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TSubclassOf_RegisterImplementation(nint A0, nint A1, nint A2);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, void> __TSubclassOf_RegisterImplementation;
#endif

        public static unsafe void TSubclassOf_RegisterImplementation<T>(TSubclassOf<T> InSubclassOf,
            nint InClass, Type InType) where T : UObject
        {
#if !LEANCLR
            if (__TSubclassOf_RegisterImplementation == null)
            {
                __TSubclassOf_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TSubclassOfImplementation::TSubclassOf_RegisterImplementation");
            }
#endif

            __TSubclassOf_RegisterImplementation(HandleData.Alloc(InSubclassOf), InClass,
                HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __TSubclassOf_IdenticalImplementation(nint A0, nint A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __TSubclassOf_IdenticalImplementation;
#endif

        public static unsafe bool TSubclassOf_IdenticalImplementation(nint InA, nint InB)
        {
#if !LEANCLR
            if (__TSubclassOf_IdenticalImplementation == null)
            {
                __TSubclassOf_IdenticalImplementation = (delegate* unmanaged[Cdecl]<nint, nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.TSubclassOfImplementation::TSubclassOf_IdenticalImplementation");
            }
#endif

            return __TSubclassOf_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TSubclassOf_UnRegisterImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __TSubclassOf_UnRegisterImplementation;
#endif

        public static unsafe void TSubclassOf_UnRegisterImplementation(nint InSubclassOf)
        {
#if !LEANCLR
            if (__TSubclassOf_UnRegisterImplementation == null)
            {
                __TSubclassOf_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TSubclassOfImplementation::TSubclassOf_UnRegisterImplementation");
            }
#endif

            __TSubclassOf_UnRegisterImplementation(InSubclassOf);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __TSubclassOf_GetImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __TSubclassOf_GetImplementation;
#endif

        public static unsafe UClass TSubclassOf_GetImplementation(nint InSubclassOf)
        {
#if !LEANCLR
            if (__TSubclassOf_GetImplementation == null)
            {
                __TSubclassOf_GetImplementation = (delegate* unmanaged[Cdecl]<nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.TSubclassOfImplementation::TSubclassOf_GetImplementation");
            }
#endif

            var Handle = __TSubclassOf_GetImplementation(InSubclassOf);

            return Handle != 0 ? (UClass)HandleData.GetObject(Handle) : null;
        }
    }
}