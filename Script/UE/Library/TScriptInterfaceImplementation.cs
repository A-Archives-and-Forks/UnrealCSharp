using System;
using Script.CoreUObject;
using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static class TScriptInterfaceImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TScriptInterface_RegisterImplementation(nint A0, nint A1, nint A2);
#else
        private static nint __TScriptInterface_RegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, void>
            __TScriptInterface_RegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, void>)MethodBridge.Resolve(
                ref __TScriptInterface_RegisterImplementation_Slot,
                "Script.Library.TScriptInterfaceImplementation::TScriptInterface_RegisterImplementation");
#endif

        public static unsafe void TScriptInterface_RegisterImplementation<T>(TScriptInterface<T> InScriptInterface,
            nint InObject, Type InType) where T : IInterface
        {
            __TScriptInterface_RegisterImplementation(HandleData.Alloc(InScriptInterface), InObject,
                HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __TScriptInterface_IdenticalImplementation(nint A0, nint A1);
#else
        private static nint __TScriptInterface_IdenticalImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __TScriptInterface_IdenticalImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, byte>)MethodBridge.Resolve(
                ref __TScriptInterface_IdenticalImplementation_Slot,
                "Script.Library.TScriptInterfaceImplementation::TScriptInterface_IdenticalImplementation");
#endif

        public static unsafe bool TScriptInterface_IdenticalImplementation(nint InA, nint InB)
        {
            return __TScriptInterface_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TScriptInterface_UnRegisterImplementation(nint A0);
#else
        private static nint __TScriptInterface_UnRegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __TScriptInterface_UnRegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(
                ref __TScriptInterface_UnRegisterImplementation_Slot,
                "Script.Library.TScriptInterfaceImplementation::TScriptInterface_UnRegisterImplementation");
#endif

        public static unsafe void TScriptInterface_UnRegisterImplementation(nint InScriptInterface)
        {
            __TScriptInterface_UnRegisterImplementation(InScriptInterface);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __TScriptInterface_GetObjectImplementation(nint A0);
#else
        private static nint __TScriptInterface_GetObjectImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __TScriptInterface_GetObjectImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint>)MethodBridge.Resolve(
                ref __TScriptInterface_GetObjectImplementation_Slot,
                "Script.Library.TScriptInterfaceImplementation::TScriptInterface_GetObjectImplementation");
#endif

        public static unsafe T TScriptInterface_GetObjectImplementation<T>(nint InScriptInterface)
        {
            var Handle = __TScriptInterface_GetObjectImplementation(InScriptInterface);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }
    }
}