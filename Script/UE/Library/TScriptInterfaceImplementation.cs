using System;
using Script.CoreUObject;
using Interop;

namespace Script.Library
{
    public static partial class TScriptInterfaceImplementation
    {
        private static unsafe partial void __TScriptInterface_RegisterImplementation(nint A0, nint A1, nint A2);

        public static unsafe void TScriptInterface_RegisterImplementation<T>(TScriptInterface<T> InScriptInterface,
            nint InObject, Type InType) where T : IInterface
        {
            __TScriptInterface_RegisterImplementation(HandleData.Alloc(InScriptInterface), InObject,
                HandleData.Alloc(InType));
        }

        private static unsafe partial byte __TScriptInterface_IdenticalImplementation(nint A0, nint A1);

        public static unsafe bool TScriptInterface_IdenticalImplementation(nint InA, nint InB)
        {
            return __TScriptInterface_IdenticalImplementation(InA, InB) != 0;
        }

        private static unsafe partial void __TScriptInterface_UnRegisterImplementation(nint A0);

        public static unsafe void TScriptInterface_UnRegisterImplementation(nint InScriptInterface)
        {
            __TScriptInterface_UnRegisterImplementation(InScriptInterface);
        }

        private static unsafe partial nint __TScriptInterface_GetObjectImplementation(nint A0);

        public static unsafe T TScriptInterface_GetObjectImplementation<T>(nint InScriptInterface)
        {
            var Handle = __TScriptInterface_GetObjectImplementation(InScriptInterface);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }
    }
}