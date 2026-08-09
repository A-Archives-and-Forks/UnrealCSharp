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
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, void>
            __TScriptInterface_RegisterImplementation;
#endif

        public static unsafe void TScriptInterface_RegisterImplementation<T>(TScriptInterface<T> InScriptInterface,
            nint InObject, Type InType) where T : IInterface
        {
#if !LEANCLR
            if (__TScriptInterface_RegisterImplementation == null)
            {
                __TScriptInterface_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TScriptInterfaceImplementation::TScriptInterface_RegisterImplementation");
            }
#endif

            __TScriptInterface_RegisterImplementation(HandleData.Alloc(InScriptInterface), InObject,
                HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __TScriptInterface_IdenticalImplementation(nint A0, nint A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __TScriptInterface_IdenticalImplementation;
#endif

        public static unsafe bool TScriptInterface_IdenticalImplementation(nint InA, nint InB)
        {
#if !LEANCLR
            if (__TScriptInterface_IdenticalImplementation == null)
            {
                __TScriptInterface_IdenticalImplementation = (delegate* unmanaged[Cdecl]<nint, nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.TScriptInterfaceImplementation::TScriptInterface_IdenticalImplementation");
            }
#endif

            return __TScriptInterface_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __TScriptInterface_UnRegisterImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __TScriptInterface_UnRegisterImplementation;
#endif

        public static unsafe void TScriptInterface_UnRegisterImplementation(nint InScriptInterface)
        {
#if !LEANCLR
            if (__TScriptInterface_UnRegisterImplementation == null)
            {
                __TScriptInterface_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TScriptInterfaceImplementation::TScriptInterface_UnRegisterImplementation");
            }
#endif

            __TScriptInterface_UnRegisterImplementation(InScriptInterface);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __TScriptInterface_GetObjectImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __TScriptInterface_GetObjectImplementation;
#endif

        public static unsafe T TScriptInterface_GetObjectImplementation<T>(nint InScriptInterface)
        {
#if !LEANCLR
            if (__TScriptInterface_GetObjectImplementation == null)
            {
                __TScriptInterface_GetObjectImplementation = (delegate* unmanaged[Cdecl]<nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.TScriptInterfaceImplementation::TScriptInterface_GetObjectImplementation");
            }
#endif

            var Handle = __TScriptInterface_GetObjectImplementation(InScriptInterface);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }
    }
}