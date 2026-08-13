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
        private static nint __TSubclassOf_RegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, void> __TSubclassOf_RegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, void>)MethodBridge.Resolve(
                ref __TSubclassOf_RegisterImplementation_Slot,
                "Script.Library.TSubclassOfImplementation::TSubclassOf_RegisterImplementation");
#endif

        public static unsafe void TSubclassOf_RegisterImplementation<T>(TSubclassOf<T> InSubclassOf,
            nint InClass, Type InType) where T : UObject
        {
            __TSubclassOf_RegisterImplementation(HandleData.Alloc(InSubclassOf), InClass,
                HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __TSubclassOf_IdenticalImplementation(nint A0, nint A1);
#else
        private static nint __TSubclassOf_IdenticalImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __TSubclassOf_IdenticalImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, byte>)MethodBridge.Resolve(
                ref __TSubclassOf_IdenticalImplementation_Slot,
                "Script.Library.TSubclassOfImplementation::TSubclassOf_IdenticalImplementation");
#endif

        public static unsafe bool TSubclassOf_IdenticalImplementation(nint InA, nint InB)
        {
            return __TSubclassOf_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TSubclassOf_UnRegisterImplementation(nint A0);
#else
        private static nint __TSubclassOf_UnRegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __TSubclassOf_UnRegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(
                ref __TSubclassOf_UnRegisterImplementation_Slot,
                "Script.Library.TSubclassOfImplementation::TSubclassOf_UnRegisterImplementation");
#endif

        public static unsafe void TSubclassOf_UnRegisterImplementation(nint InSubclassOf)
        {
            __TSubclassOf_UnRegisterImplementation(InSubclassOf);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __TSubclassOf_GetImplementation(nint A0);
#else
        private static nint __TSubclassOf_GetImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __TSubclassOf_GetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint>)MethodBridge.Resolve(ref __TSubclassOf_GetImplementation_Slot,
                "Script.Library.TSubclassOfImplementation::TSubclassOf_GetImplementation");
#endif

        public static unsafe UClass TSubclassOf_GetImplementation(nint InSubclassOf)
        {
            var Handle = __TSubclassOf_GetImplementation(InSubclassOf);

            return Handle != 0 ? (UClass)HandleData.GetObject(Handle) : null;
        }
    }
}