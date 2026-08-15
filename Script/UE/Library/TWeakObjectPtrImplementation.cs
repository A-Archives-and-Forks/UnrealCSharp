using System;
using Script.CoreUObject;
using Interop;

namespace Script.Library
{
    public static partial class TWeakObjectPtrImplementation
    {
        private static unsafe partial void __TWeakObjectPtr_RegisterImplementation(nint A0, nint A1, nint A2);

        public static unsafe void TWeakObjectPtr_RegisterImplementation<T>(TWeakObjectPtr<T> InWeakObjectPtr,
            nint InObject, Type InType) where T : UObject
        {
            __TWeakObjectPtr_RegisterImplementation(HandleData.Alloc(InWeakObjectPtr), InObject,
                HandleData.Alloc(InType));
        }

        private static unsafe partial byte __TWeakObjectPtr_IdenticalImplementation(nint A0, nint A1);

        public static unsafe bool TWeakObjectPtr_IdenticalImplementation(nint InA, nint InB)
        {
            return __TWeakObjectPtr_IdenticalImplementation(InA, InB) != 0;
        }

        private static unsafe partial void __TWeakObjectPtr_UnRegisterImplementation(nint A0);

        public static unsafe void TWeakObjectPtr_UnRegisterImplementation(nint InWeakObjectPtr)
        {
            __TWeakObjectPtr_UnRegisterImplementation(InWeakObjectPtr);
        }

        private static unsafe partial nint __TWeakObjectPtr_GetImplementation(nint A0);

        public static unsafe T TWeakObjectPtr_GetImplementation<T>(nint InWeakObjectPtr)
        {
            var Handle = __TWeakObjectPtr_GetImplementation(InWeakObjectPtr);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }
    }
}