using System;
using System.Reflection;
using Interop;

namespace Script.Library
{
    public static unsafe partial class FMulticastDelegateImplementation
    {
        private static unsafe partial void __FMulticastDelegate_RegisterImplementation(nint A0, nint A1);

        public static void FMulticastDelegate_RegisterImplementation(object InMonoObject, Type InType)
        {
            __FMulticastDelegate_RegisterImplementation(HandleData.Alloc(InMonoObject), HandleData.Alloc(InType));
        }

        private static unsafe partial void __FMulticastDelegate_UnRegisterImplementation(nint A0);

        public static void FMulticastDelegate_UnRegisterImplementation(nint InMonoObject)
        {
            __FMulticastDelegate_UnRegisterImplementation(InMonoObject);
        }

        private static unsafe partial byte __FMulticastDelegate_IsBoundImplementation(nint A0);

        public static unsafe bool FMulticastDelegate_IsBoundImplementation(nint InMonoObject)
        {
            return __FMulticastDelegate_IsBoundImplementation(InMonoObject) != 0;
        }

        private static unsafe partial byte __FMulticastDelegate_ContainsImplementation(nint A0, nint A1, nint A2, nint A3);

        public static bool FMulticastDelegate_ContainsImplementation(nint InMonoObject, nint InObject,
            Type InType, MethodInfo InMethodInfo)
        {
            return __FMulticastDelegate_ContainsImplementation(InMonoObject, InObject,
                HandleData.Alloc(InType), HandleData.Alloc(InMethodInfo)) != 0;
        }

        private static unsafe partial void __FMulticastDelegate_AddImplementation(nint A0, nint A1, nint A2, nint A3);

        public static void FMulticastDelegate_AddImplementation(nint InMonoObject, nint InObject, Type InType,
            MethodInfo InMethodInfo)
        {
            __FMulticastDelegate_AddImplementation(InMonoObject, InObject, HandleData.Alloc(InType),
                HandleData.Alloc(InMethodInfo));
        }

        private static unsafe partial void __FMulticastDelegate_AddUniqueImplementation(nint A0, nint A1, nint A2, nint A3);

        public static void FMulticastDelegate_AddUniqueImplementation(nint InMonoObject, nint InObject,
            Type InType, MethodInfo InMethodInfo)
        {
            __FMulticastDelegate_AddUniqueImplementation(InMonoObject, InObject,
                HandleData.Alloc(InType), HandleData.Alloc(InMethodInfo));
        }

        private static unsafe partial void __FMulticastDelegate_RemoveImplementation(nint A0, nint A1, nint A2, nint A3);

        public static void FMulticastDelegate_RemoveImplementation(nint InMonoObject, nint InObject, Type InType,
            MethodInfo InMethodInfo)
        {
            __FMulticastDelegate_RemoveImplementation(InMonoObject, InObject, HandleData.Alloc(InType),
                HandleData.Alloc(InMethodInfo));
        }

        private static unsafe partial void __FMulticastDelegate_RemoveAllImplementation(nint A0, nint A1);

        public static void FMulticastDelegate_RemoveAllImplementation(nint InMonoObject, nint InObject)
        {
            __FMulticastDelegate_RemoveAllImplementation(InMonoObject, InObject);
        }

        private static unsafe partial void __FMulticastDelegate_ClearImplementation(nint A0);

        public static void FMulticastDelegate_ClearImplementation(nint InMonoObject)
        {
            __FMulticastDelegate_ClearImplementation(InMonoObject);
        }

        private static unsafe partial void __FMulticastDelegate_GenericBroadcast0Implementation(nint A0);

        public static void FMulticastDelegate_GenericBroadcast0Implementation(nint InMonoObject)
        {
            __FMulticastDelegate_GenericBroadcast0Implementation(InMonoObject);
        }

        private static unsafe partial void __FMulticastDelegate_GenericBroadcast2Implementation(nint A0, byte* A1);

        public static void FMulticastDelegate_GenericBroadcast2Implementation(nint InMonoObject, byte* InBuffer)
        {
            __FMulticastDelegate_GenericBroadcast2Implementation(InMonoObject, InBuffer);
        }

        private static unsafe partial void __FMulticastDelegate_GenericBroadcast4Implementation(nint A0, byte* A1);

        public static void FMulticastDelegate_GenericBroadcast4Implementation(nint InMonoObject, byte* OutBuffer)
        {
            __FMulticastDelegate_GenericBroadcast4Implementation(InMonoObject, OutBuffer);
        }

        private static unsafe partial void __FMulticastDelegate_GenericBroadcast6Implementation(nint A0, byte* A1, byte* A2);

        public static void FMulticastDelegate_GenericBroadcast6Implementation(nint InMonoObject, byte* InBuffer,
            byte* OutBuffer)
        {
            __FMulticastDelegate_GenericBroadcast6Implementation(InMonoObject, InBuffer, OutBuffer);
        }
    }
}