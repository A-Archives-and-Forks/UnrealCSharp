#if UE_5_3_OR_LATER
using System;
using Script.CoreUObject;
using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static class TOptionalImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TOptional_Register1Implementation(nint A0, nint A1);
#else
        private static nint __TOptional_Register1Implementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, void> __TOptional_Register1Implementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, void>)MethodBridge.Resolve(
                ref __TOptional_Register1Implementation_Slot,
                "Script.Library.TOptionalImplementation::TOptional_Register1Implementation");
#endif

        public static unsafe void TOptional_Register1Implementation<T>(TOptional<T> InOptional, Type InType)
        {
            __TOptional_Register1Implementation(HandleData.Alloc(InOptional), HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TOptional_Register2Implementation(nint A0, nint A1, nint A2);
#else
        private static nint __TOptional_Register2Implementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, void> __TOptional_Register2Implementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, void>)MethodBridge.Resolve(
                ref __TOptional_Register2Implementation_Slot,
                "Script.Library.TOptionalImplementation::TOptional_Register2Implementation");
#endif

        public static unsafe void TOptional_Register2Implementation<T>(TOptional<T> InOptional, T InValue, Type InType)
        {
            __TOptional_Register2Implementation(HandleData.Alloc(InOptional),
                HandleData.Alloc(InValue), HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __TOptional_IdenticalImplementation(nint A0, nint A1);
#else
        private static nint __TOptional_IdenticalImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __TOptional_IdenticalImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, byte>)MethodBridge.Resolve(
                ref __TOptional_IdenticalImplementation_Slot,
                "Script.Library.TOptionalImplementation::TOptional_IdenticalImplementation");
#endif

        public static unsafe bool TOptional_IdenticalImplementation(nint InA, nint InB)
        {
            return __TOptional_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TOptional_UnRegisterImplementation(nint A0);
#else
        private static nint __TOptional_UnRegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __TOptional_UnRegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(ref __TOptional_UnRegisterImplementation_Slot,
                "Script.Library.TOptionalImplementation::TOptional_UnRegisterImplementation");
#endif

        public static unsafe void TOptional_UnRegisterImplementation(nint InOptional)
        {
            __TOptional_UnRegisterImplementation(InOptional);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TOptional_ResetImplementation(nint A0);
#else
        private static nint __TOptional_ResetImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __TOptional_ResetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(ref __TOptional_ResetImplementation_Slot,
                "Script.Library.TOptionalImplementation::TOptional_ResetImplementation");
#endif

        public static unsafe void TOptional_ResetImplementation(nint InOptional)
        {
            __TOptional_ResetImplementation(InOptional);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __TOptional_IsSetImplementation(nint A0);
#else
        private static nint __TOptional_IsSetImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte> __TOptional_IsSetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte>)MethodBridge.Resolve(ref __TOptional_IsSetImplementation_Slot,
                "Script.Library.TOptionalImplementation::TOptional_IsSetImplementation");
#endif

        public static unsafe bool TOptional_IsSetImplementation(nint InOptional)
        {
            return __TOptional_IsSetImplementation(InOptional) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __TOptional_GetImplementation(nint A0);
#else
        private static nint __TOptional_GetImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __TOptional_GetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint>)MethodBridge.Resolve(ref __TOptional_GetImplementation_Slot,
                "Script.Library.TOptionalImplementation::TOptional_GetImplementation");
#endif

        public static unsafe object TOptional_GetImplementation(nint InOptional)
        {
            var Handle = __TOptional_GetImplementation(InOptional);

            return Handle != 0 ? HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TOptional_SetImplementation(nint A0, nint A1);
#else
        private static nint __TOptional_SetImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, void> __TOptional_SetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, void>)MethodBridge.Resolve(ref __TOptional_SetImplementation_Slot,
                "Script.Library.TOptionalImplementation::TOptional_SetImplementation");
#endif

        public static unsafe void TOptional_SetImplementation<T>(nint InOptional, T InValue)
        {
            __TOptional_SetImplementation(InOptional, HandleData.Alloc(InValue));
        }
    }
}
#endif