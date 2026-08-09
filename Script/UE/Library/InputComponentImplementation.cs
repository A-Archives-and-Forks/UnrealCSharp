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
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint>
            __UInputComponent_GetDynamicBindingObjectImplementation;
#endif

        public static unsafe T UInputComponent_GetDynamicBindingObjectImplementation<T>(
            nint InThisClass, nint InBindingClass) where T : UDynamicBlueprintBinding
        {
#if !LEANCLR
            if (__UInputComponent_GetDynamicBindingObjectImplementation == null)
            {
                __UInputComponent_GetDynamicBindingObjectImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.UInputComponentImplementation::UInputComponent_GetDynamicBindingObjectImplementation");
            }
#endif

            var Handle = __UInputComponent_GetDynamicBindingObjectImplementation(InThisClass, InBindingClass);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_BindActionImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __UInputComponent_BindActionImplementation;
#endif

        public static unsafe void UInputComponent_BindActionImplementation(nint InObject,
            nint InInputActionDelegateBinding, nint InObjectToBindTo, nint InFunctionNameToBind)
        {
#if !LEANCLR
            if (__UInputComponent_BindActionImplementation == null)
            {
                __UInputComponent_BindActionImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.UInputComponentImplementation::UInputComponent_BindActionImplementation");
            }
#endif

            __UInputComponent_BindActionImplementation(InObject, InInputActionDelegateBinding, InObjectToBindTo,
                InFunctionNameToBind);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_BindAxisImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __UInputComponent_BindAxisImplementation;
#endif

        public static unsafe void UInputComponent_BindAxisImplementation(nint InObject, nint InInputAxisDelegateBinding,
            nint InObjectToBindTo, nint InFunctionNameToBind)
        {
#if !LEANCLR
            if (__UInputComponent_BindAxisImplementation == null)
            {
                __UInputComponent_BindAxisImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.UInputComponentImplementation::UInputComponent_BindAxisImplementation");
            }
#endif

            __UInputComponent_BindAxisImplementation(InObject, InInputAxisDelegateBinding, InObjectToBindTo,
                InFunctionNameToBind);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_BindAxisKeyImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __UInputComponent_BindAxisKeyImplementation;
#endif

        public static unsafe void UInputComponent_BindAxisKeyImplementation(nint InObject,
            nint InInputAxisKeyDelegateBinding, nint InObjectToBindTo, nint InFunctionNameToBind)
        {
#if !LEANCLR
            if (__UInputComponent_BindAxisKeyImplementation == null)
            {
                __UInputComponent_BindAxisKeyImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.UInputComponentImplementation::UInputComponent_BindAxisKeyImplementation");
            }
#endif

            __UInputComponent_BindAxisKeyImplementation(InObject, InInputAxisKeyDelegateBinding, InObjectToBindTo,
                InFunctionNameToBind);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_BindKeyImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __UInputComponent_BindKeyImplementation;
#endif

        public static unsafe void UInputComponent_BindKeyImplementation(nint InObject, nint InInputKeyDelegateBinding,
            nint InObjectToBindTo, nint InFunctionNameToBind)
        {
#if !LEANCLR
            if (__UInputComponent_BindKeyImplementation == null)
            {
                __UInputComponent_BindKeyImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.UInputComponentImplementation::UInputComponent_BindKeyImplementation");
            }
#endif

            __UInputComponent_BindKeyImplementation(InObject, InInputKeyDelegateBinding, InObjectToBindTo,
                InFunctionNameToBind);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_BindTouchImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __UInputComponent_BindTouchImplementation;
#endif

        public static unsafe void UInputComponent_BindTouchImplementation(nint InObject,
            nint InInputTouchDelegateBinding, nint InObjectToBindTo, nint InFunctionNameToBind)
        {
#if !LEANCLR
            if (__UInputComponent_BindTouchImplementation == null)
            {
                __UInputComponent_BindTouchImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.UInputComponentImplementation::UInputComponent_BindTouchImplementation");
            }
#endif

            __UInputComponent_BindTouchImplementation(InObject, InInputTouchDelegateBinding, InObjectToBindTo,
                InFunctionNameToBind);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_BindVectorAxisImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __UInputComponent_BindVectorAxisImplementation;
#endif

        public static unsafe void UInputComponent_BindVectorAxisImplementation(nint InObject,
            nint InInputVectorAxisDelegateBinding, nint InObjectToBindTo, nint InFunctionNameToBind)
        {
#if !LEANCLR
            if (__UInputComponent_BindVectorAxisImplementation == null)
            {
                __UInputComponent_BindVectorAxisImplementation =
                    (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.UInputComponentImplementation::UInputComponent_BindVectorAxisImplementation");
            }
#endif

            __UInputComponent_BindVectorAxisImplementation(InObject, InInputVectorAxisDelegateBinding, InObjectToBindTo,
                InFunctionNameToBind);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UInputComponent_ClearBindingValuesImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __UInputComponent_ClearBindingValuesImplementation;
#endif

        public static unsafe void UInputComponent_ClearBindingValuesImplementation(nint InObject)
        {
#if !LEANCLR
            if (__UInputComponent_ClearBindingValuesImplementation == null)
            {
                __UInputComponent_ClearBindingValuesImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.UInputComponentImplementation::UInputComponent_ClearBindingValuesImplementation");
            }
#endif

            __UInputComponent_ClearBindingValuesImplementation(InObject);
        }
    }
}