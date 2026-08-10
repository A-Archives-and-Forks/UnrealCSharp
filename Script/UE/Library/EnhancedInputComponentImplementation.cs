using Script.CoreUObject;
using Script.Engine;
using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static partial class UEnhancedInputComponentImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __UEnhancedInputComponent_GetDynamicBindingObjectImplementation(nint A0, nint A1);
#else
        private static nint __UEnhancedInputComponent_GetDynamicBindingObjectImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint>
            __UEnhancedInputComponent_GetDynamicBindingObjectImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint>)MethodBridge.Resolve(
                ref __UEnhancedInputComponent_GetDynamicBindingObjectImplementation_Slot,
                "Script.Library.UEnhancedInputComponentImplementation::UEnhancedInputComponent_GetDynamicBindingObjectImplementation");
#endif

        public static unsafe T UEnhancedInputComponent_GetDynamicBindingObjectImplementation<T>(
            nint InThisClass, nint InBindingClass) where T : UDynamicBlueprintBinding
        {
            var Handle = __UEnhancedInputComponent_GetDynamicBindingObjectImplementation(InThisClass, InBindingClass);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __UEnhancedInputComponent_BindActionImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static nint __UEnhancedInputComponent_BindActionImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint, nint>
            __UEnhancedInputComponent_BindActionImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, nint>)MethodBridge.Resolve(
                ref __UEnhancedInputComponent_BindActionImplementation_Slot,
                "Script.Library.UEnhancedInputComponentImplementation::UEnhancedInputComponent_BindActionImplementation");
#endif

        public static unsafe FEnhancedInputActionEventBinding UEnhancedInputComponent_BindActionImplementation(
            nint InObject, nint InBlueprintEnhancedInputActionBinding, nint InObjectToBindTo, nint InFunctionNameToBind)
        {
            var Handle = __UEnhancedInputComponent_BindActionImplementation(InObject,
                InBlueprintEnhancedInputActionBinding, InObjectToBindTo, InFunctionNameToBind);

            return Handle != 0 ? (FEnhancedInputActionEventBinding)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UEnhancedInputComponent_RemoveBindingImplementation(nint A0, nint A1);
#else
        private static nint __UEnhancedInputComponent_RemoveBindingImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, void>
            __UEnhancedInputComponent_RemoveBindingImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, void>)MethodBridge.Resolve(
                ref __UEnhancedInputComponent_RemoveBindingImplementation_Slot,
                "Script.Library.UEnhancedInputComponentImplementation::UEnhancedInputComponent_RemoveBindingImplementation");
#endif

        public static unsafe void UEnhancedInputComponent_RemoveBindingImplementation(nint InObject,
            nint InEnhancedInputActionEventBinding)
        {
            __UEnhancedInputComponent_RemoveBindingImplementation(InObject, InEnhancedInputActionEventBinding);
        }
    }
}