using System;
using System.Reflection;
using Interop;

namespace Script.Library
{
    public static unsafe partial class FDelegateImplementation
    {
        private static unsafe partial void __FDelegate_RegisterImplementation(nint A0, nint A1);

        public static void FDelegate_RegisterImplementation(object InMonoObject, Type InType)
        {
            __FDelegate_RegisterImplementation(HandleData.Alloc(InMonoObject), HandleData.Alloc(InType));
        }

        private static unsafe partial void __FDelegate_UnRegisterImplementation(nint A0);

        public static void FDelegate_UnRegisterImplementation(nint InMonoObject)
        {
            __FDelegate_UnRegisterImplementation(InMonoObject);
        }

        private static unsafe partial void __FDelegate_BindImplementation(nint A0, nint A1, nint A2, nint A3);

        public static void FDelegate_BindImplementation(nint InMonoObject, nint InObject, Type InType,
            MethodInfo InMethodInfo)
        {
            __FDelegate_BindImplementation(InMonoObject, InObject, HandleData.Alloc(InType),
                HandleData.Alloc(InMethodInfo));
        }

        private static unsafe partial byte __FDelegate_IsBoundImplementation(nint A0);

        public static bool FDelegate_IsBoundImplementation(nint InMonoObject)
        {
            return __FDelegate_IsBoundImplementation(InMonoObject) != 0;
        }

        private static unsafe partial void __FDelegate_UnBindImplementation(nint A0);

        public static void FDelegate_UnBindImplementation(nint InMonoObject)
        {
            __FDelegate_UnBindImplementation(InMonoObject);
        }

        private static unsafe partial void __FDelegate_ClearImplementation(nint A0);

        public static void FDelegate_ClearImplementation(nint InMonoObject)
        {
            __FDelegate_ClearImplementation(InMonoObject);
        }

        private static unsafe partial void __FDelegate_GenericExecute0Implementation(nint A0);

        public static void FDelegate_GenericExecute0Implementation(nint InMonoObject)
        {
            __FDelegate_GenericExecute0Implementation(InMonoObject);
        }

        private static unsafe partial void __FDelegate_PrimitiveExecute1Implementation(nint A0, byte* A1);

        public static void FDelegate_PrimitiveExecute1Implementation(nint InMonoObject, byte* ReturnBuffer)
        {
            __FDelegate_PrimitiveExecute1Implementation(InMonoObject, ReturnBuffer);
        }

        private static unsafe partial void __FDelegate_CompoundExecute1Implementation(nint A0, byte* A1);

        public static void FDelegate_CompoundExecute1Implementation(nint InMonoObject, byte* ReturnBuffer)
        {
            __FDelegate_CompoundExecute1Implementation(InMonoObject, ReturnBuffer);
        }

        private static unsafe partial void __FDelegate_GenericExecute2Implementation(nint A0, byte* A1);

        public static void FDelegate_GenericExecute2Implementation(nint InMonoObject, byte* InBuffer)
        {
            __FDelegate_GenericExecute2Implementation(InMonoObject, InBuffer);
        }

        private static unsafe partial void __FDelegate_PrimitiveExecute3Implementation(nint A0, byte* A1, byte* A2);

        public static void FDelegate_PrimitiveExecute3Implementation(nint InMonoObject, byte* InBuffer,
            byte* ReturnBuffer)
        {
            __FDelegate_PrimitiveExecute3Implementation(InMonoObject, InBuffer, ReturnBuffer);
        }

        private static unsafe partial void __FDelegate_CompoundExecute3Implementation(nint A0, byte* A1, byte* A2);

        public static void FDelegate_CompoundExecute3Implementation(nint InMonoObject, byte* InBuffer,
            byte* ReturnBuffer)
        {
            __FDelegate_CompoundExecute3Implementation(InMonoObject, InBuffer, ReturnBuffer);
        }

        private static unsafe partial void __FDelegate_GenericExecute4Implementation(nint A0, byte* A1);

        public static void FDelegate_GenericExecute4Implementation(nint InMonoObject, byte* OutBuffer)
        {
            __FDelegate_GenericExecute4Implementation(InMonoObject, OutBuffer);
        }

        private static unsafe partial void __FDelegate_GenericExecute6Implementation(nint A0, byte* A1, byte* A2);

        public static void FDelegate_GenericExecute6Implementation(nint InMonoObject, byte* InBuffer,
            byte* OutBuffer)
        {
            __FDelegate_GenericExecute6Implementation(InMonoObject, InBuffer, OutBuffer);
        }

        private static unsafe partial void __FDelegate_PrimitiveExecute7Implementation(nint A0, byte* A1, byte* A2, byte* A3);

        public static void FDelegate_PrimitiveExecute7Implementation(nint InMonoObject, byte* InBuffer,
            byte* OutBuffer, byte* ReturnBuffer)
        {
            __FDelegate_PrimitiveExecute7Implementation(InMonoObject, InBuffer, OutBuffer, ReturnBuffer);
        }

        private static unsafe partial void __FDelegate_CompoundExecute7Implementation(nint A0, byte* A1, byte* A2, byte* A3);

        public static void FDelegate_CompoundExecute7Implementation(nint InMonoObject, byte* InBuffer,
            byte* OutBuffer, byte* ReturnBuffer)
        {
            __FDelegate_CompoundExecute7Implementation(InMonoObject, InBuffer, OutBuffer, ReturnBuffer);
        }
    }
}