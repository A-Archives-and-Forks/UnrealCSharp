using System;
using Script.CoreUObject;
using Interop;

namespace Script.Library
{
    public static partial class TLazyObjectPtrImplementation
    {
        private static unsafe partial void __TLazyObjectPtr_RegisterImplementation(nint A0, nint A1, nint A2);

        public static unsafe void TLazyObjectPtr_RegisterImplementation<T>(TLazyObjectPtr<T> InLazyObjectPtr,
            nint InObject, Type InType) where T : UObject
        {
            __TLazyObjectPtr_RegisterImplementation(HandleData.Alloc(InLazyObjectPtr), InObject,
                HandleData.Alloc(InType));
        }

        private static unsafe partial byte __TLazyObjectPtr_IdenticalImplementation(nint A0, nint A1);

        public static unsafe bool TLazyObjectPtr_IdenticalImplementation(nint InA, nint InB)
        {
            return __TLazyObjectPtr_IdenticalImplementation(InA, InB) != 0;
        }

        private static unsafe partial void __TLazyObjectPtr_UnRegisterImplementation(nint A0);

        public static unsafe void TLazyObjectPtr_UnRegisterImplementation(nint InLazyObjectPtr)
        {
            __TLazyObjectPtr_UnRegisterImplementation(InLazyObjectPtr);
        }

        private static unsafe partial nint __TLazyObjectPtr_GetImplementation(nint A0);

        public static unsafe T TLazyObjectPtr_GetImplementation<T>(nint InLazyObjectPtr)
        {
            var Handle = __TLazyObjectPtr_GetImplementation(InLazyObjectPtr);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }
    }
}