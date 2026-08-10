using Script.Engine;
using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static partial class UInputComponentImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __UInputComponent_GetDynamicBindingObjectImplementation(nint A0, nint A1);
#else
        private static nint __UInputComponent_GetDynamicBindingObjectImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint>
            __UInputComponent_GetDynamicBindingObjectImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint>)MethodBridge.Resolve(
                ref __UInputComponent_GetDynamicBindingObjectImplementation_Slot,
                "Script.Library.UInputComponentImplementation::UInputComponent_GetDynamicBindingObjectImplementation");
#endif

        public static unsafe T UInputComponent_GetDynamicBindingObjectImplementation<T>(
            nint InThisClass, nint InBindingClass) where T : UDynamicBlueprintBinding
        {
            var Handle = __UInputComponent_GetDynamicBindingObjectImplementation(InThisClass, InBindingClass);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_BindActionImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static nint __UInputComponent_BindActionImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __UInputComponent_BindActionImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)MethodBridge.Resolve(
                ref __UInputComponent_BindActionImplementation_Slot,
                "Script.Library.UInputComponentImplementation::UInputComponent_BindActionImplementation");
#endif

        public static unsafe void UInputComponent_BindActionImplementation(nint InObject,
            nint InInputActionDelegateBinding, nint InObjectToBindTo, nint InFunctionNameToBind)
        {
            __UInputComponent_BindActionImplementation(InObject, InInputActionDelegateBinding, InObjectToBindTo,
                InFunctionNameToBind);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_BindAxisImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static nint __UInputComponent_BindAxisImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __UInputComponent_BindAxisImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)MethodBridge.Resolve(
                ref __UInputComponent_BindAxisImplementation_Slot,
                "Script.Library.UInputComponentImplementation::UInputComponent_BindAxisImplementation");
#endif

        public static unsafe void UInputComponent_BindAxisImplementation(nint InObject, nint InInputAxisDelegateBinding,
            nint InObjectToBindTo, nint InFunctionNameToBind)
        {
            __UInputComponent_BindAxisImplementation(InObject, InInputAxisDelegateBinding, InObjectToBindTo,
                InFunctionNameToBind);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_BindAxisKeyImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static nint __UInputComponent_BindAxisKeyImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __UInputComponent_BindAxisKeyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)MethodBridge.Resolve(
                ref __UInputComponent_BindAxisKeyImplementation_Slot,
                "Script.Library.UInputComponentImplementation::UInputComponent_BindAxisKeyImplementation");
#endif

        public static unsafe void UInputComponent_BindAxisKeyImplementation(nint InObject,
            nint InInputAxisKeyDelegateBinding, nint InObjectToBindTo, nint InFunctionNameToBind)
        {
            __UInputComponent_BindAxisKeyImplementation(InObject, InInputAxisKeyDelegateBinding, InObjectToBindTo,
                InFunctionNameToBind);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_BindKeyImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static nint __UInputComponent_BindKeyImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __UInputComponent_BindKeyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)MethodBridge.Resolve(
                ref __UInputComponent_BindKeyImplementation_Slot,
                "Script.Library.UInputComponentImplementation::UInputComponent_BindKeyImplementation");
#endif

        public static unsafe void UInputComponent_BindKeyImplementation(nint InObject, nint InInputKeyDelegateBinding,
            nint InObjectToBindTo, nint InFunctionNameToBind)
        {
            __UInputComponent_BindKeyImplementation(InObject, InInputKeyDelegateBinding, InObjectToBindTo,
                InFunctionNameToBind);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_BindTouchImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static nint __UInputComponent_BindTouchImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __UInputComponent_BindTouchImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)MethodBridge.Resolve(
                ref __UInputComponent_BindTouchImplementation_Slot,
                "Script.Library.UInputComponentImplementation::UInputComponent_BindTouchImplementation");
#endif

        public static unsafe void UInputComponent_BindTouchImplementation(nint InObject,
            nint InInputTouchDelegateBinding, nint InObjectToBindTo, nint InFunctionNameToBind)
        {
            __UInputComponent_BindTouchImplementation(InObject, InInputTouchDelegateBinding, InObjectToBindTo,
                InFunctionNameToBind);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_BindVectorAxisImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static nint __UInputComponent_BindVectorAxisImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __UInputComponent_BindVectorAxisImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)MethodBridge.Resolve(
                ref __UInputComponent_BindVectorAxisImplementation_Slot,
                "Script.Library.UInputComponentImplementation::UInputComponent_BindVectorAxisImplementation");
#endif

        public static unsafe void UInputComponent_BindVectorAxisImplementation(nint InObject,
            nint InInputVectorAxisDelegateBinding, nint InObjectToBindTo, nint InFunctionNameToBind)
        {
            __UInputComponent_BindVectorAxisImplementation(InObject, InInputVectorAxisDelegateBinding, InObjectToBindTo,
                InFunctionNameToBind);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_ClearBindingValuesImplementation(nint A0);
#else
        private static nint __UInputComponent_ClearBindingValuesImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, void>
            __UInputComponent_ClearBindingValuesImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(
                ref __UInputComponent_ClearBindingValuesImplementation_Slot,
                "Script.Library.UInputComponentImplementation::UInputComponent_ClearBindingValuesImplementation");
#endif

        public static unsafe void UInputComponent_ClearBindingValuesImplementation(nint InObject)
        {
            __UInputComponent_ClearBindingValuesImplementation(InObject);
        }
    }
}