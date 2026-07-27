using System;
using System.Reflection;
using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static unsafe class FDelegateImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_RegisterImplementation(nint A0, nint A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, nint, void> __FDelegate_RegisterImplementation;
#endif

        public static void FDelegate_RegisterImplementation(object InMonoObject, Type InType)
        {
#if !LEANCLR
            if (__FDelegate_RegisterImplementation == null)
            {
                __FDelegate_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_RegisterImplementation");
            }
#endif

            __FDelegate_RegisterImplementation(HandleData.Alloc(InMonoObject), HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_UnRegisterImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, void> __FDelegate_UnRegisterImplementation;
#endif

        public static void FDelegate_UnRegisterImplementation(nint InMonoObject)
        {
#if !LEANCLR
            if (__FDelegate_UnRegisterImplementation == null)
            {
                __FDelegate_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_UnRegisterImplementation");
            }
#endif

            __FDelegate_UnRegisterImplementation(InMonoObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_BindImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void> __FDelegate_BindImplementation;
#endif

        public static void FDelegate_BindImplementation(nint InMonoObject, nint InObject, Type InType,
            MethodInfo InMethodInfo)
        {
#if !LEANCLR
            if (__FDelegate_BindImplementation == null)
            {
                __FDelegate_BindImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_BindImplementation");
            }
#endif

            __FDelegate_BindImplementation(InMonoObject, InObject, HandleData.Alloc(InType),
                HandleData.Alloc(InMethodInfo));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __FDelegate_IsBoundImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte> __FDelegate_IsBoundImplementation;
#endif

        public static bool FDelegate_IsBoundImplementation(nint InMonoObject)
        {
#if !LEANCLR
            if (__FDelegate_IsBoundImplementation == null)
            {
                __FDelegate_IsBoundImplementation = (delegate* unmanaged[Cdecl]<nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_IsBoundImplementation");
            }
#endif

            return __FDelegate_IsBoundImplementation(InMonoObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_UnBindImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, void> __FDelegate_UnBindImplementation;
#endif

        public static void FDelegate_UnBindImplementation(nint InMonoObject)
        {
#if !LEANCLR
            if (__FDelegate_UnBindImplementation == null)
            {
                __FDelegate_UnBindImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_UnBindImplementation");
            }
#endif

            __FDelegate_UnBindImplementation(InMonoObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_ClearImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, void> __FDelegate_ClearImplementation;
#endif

        public static void FDelegate_ClearImplementation(nint InMonoObject)
        {
#if !LEANCLR
            if (__FDelegate_ClearImplementation == null)
            {
                __FDelegate_ClearImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_ClearImplementation");
            }
#endif

            __FDelegate_ClearImplementation(InMonoObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_GenericExecute0Implementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, void> __FDelegate_GenericExecute0Implementation;
#endif

        public static void FDelegate_GenericExecute0Implementation(nint InMonoObject)
        {
#if !LEANCLR
            if (__FDelegate_GenericExecute0Implementation == null)
            {
                __FDelegate_GenericExecute0Implementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_GenericExecute0Implementation");
            }
#endif

            __FDelegate_GenericExecute0Implementation(InMonoObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_PrimitiveExecute1Implementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, void> __FDelegate_PrimitiveExecute1Implementation;
#endif

        public static void FDelegate_PrimitiveExecute1Implementation(nint InMonoObject, byte* ReturnBuffer)
        {
#if !LEANCLR
            if (__FDelegate_PrimitiveExecute1Implementation == null)
            {
                __FDelegate_PrimitiveExecute1Implementation = (delegate* unmanaged[Cdecl]<nint, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_PrimitiveExecute1Implementation");
            }
#endif

            __FDelegate_PrimitiveExecute1Implementation(InMonoObject, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_CompoundExecute1Implementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, void> __FDelegate_CompoundExecute1Implementation;
#endif

        public static void FDelegate_CompoundExecute1Implementation(nint InMonoObject, byte* ReturnBuffer)
        {
#if !LEANCLR
            if (__FDelegate_CompoundExecute1Implementation == null)
            {
                __FDelegate_CompoundExecute1Implementation = (delegate* unmanaged[Cdecl]<nint, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_CompoundExecute1Implementation");
            }
#endif

            __FDelegate_CompoundExecute1Implementation(InMonoObject, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_GenericExecute2Implementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, void> __FDelegate_GenericExecute2Implementation;
#endif

        public static void FDelegate_GenericExecute2Implementation(nint InMonoObject, byte* InBuffer)
        {
#if !LEANCLR
            if (__FDelegate_GenericExecute2Implementation == null)
            {
                __FDelegate_GenericExecute2Implementation = (delegate* unmanaged[Cdecl]<nint, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_GenericExecute2Implementation");
            }
#endif

            __FDelegate_GenericExecute2Implementation(InMonoObject, InBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_PrimitiveExecute3Implementation(nint A0, byte* A1, byte* A2);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>
            __FDelegate_PrimitiveExecute3Implementation;
#endif

        public static void FDelegate_PrimitiveExecute3Implementation(nint InMonoObject, byte* InBuffer,
            byte* ReturnBuffer)
        {
#if !LEANCLR
            if (__FDelegate_PrimitiveExecute3Implementation == null)
            {
                __FDelegate_PrimitiveExecute3Implementation = (delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_PrimitiveExecute3Implementation");
            }
#endif

            __FDelegate_PrimitiveExecute3Implementation(InMonoObject, InBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_CompoundExecute3Implementation(nint A0, byte* A1, byte* A2);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>
            __FDelegate_CompoundExecute3Implementation;
#endif

        public static void FDelegate_CompoundExecute3Implementation(nint InMonoObject, byte* InBuffer,
            byte* ReturnBuffer)
        {
#if !LEANCLR
            if (__FDelegate_CompoundExecute3Implementation == null)
            {
                __FDelegate_CompoundExecute3Implementation = (delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_CompoundExecute3Implementation");
            }
#endif

            __FDelegate_CompoundExecute3Implementation(InMonoObject, InBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_GenericExecute4Implementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, void> __FDelegate_GenericExecute4Implementation;
#endif

        public static void FDelegate_GenericExecute4Implementation(nint InMonoObject, byte* OutBuffer)
        {
#if !LEANCLR
            if (__FDelegate_GenericExecute4Implementation == null)
            {
                __FDelegate_GenericExecute4Implementation = (delegate* unmanaged[Cdecl]<nint, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_GenericExecute4Implementation");
            }
#endif

            __FDelegate_GenericExecute4Implementation(InMonoObject, OutBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_GenericExecute6Implementation(nint A0, byte* A1, byte* A2);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>
            __FDelegate_GenericExecute6Implementation;
#endif

        public static void FDelegate_GenericExecute6Implementation(nint InMonoObject, byte* InBuffer,
            byte* OutBuffer)
        {
#if !LEANCLR
            if (__FDelegate_GenericExecute6Implementation == null)
            {
                __FDelegate_GenericExecute6Implementation = (delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_GenericExecute6Implementation");
            }
#endif

            __FDelegate_GenericExecute6Implementation(InMonoObject, InBuffer, OutBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_PrimitiveExecute7Implementation(nint A0, byte* A1, byte* A2, byte* A3);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, void>
            __FDelegate_PrimitiveExecute7Implementation;
#endif

        public static void FDelegate_PrimitiveExecute7Implementation(nint InMonoObject, byte* InBuffer,
            byte* OutBuffer, byte* ReturnBuffer)
        {
#if !LEANCLR
            if (__FDelegate_PrimitiveExecute7Implementation == null)
            {
                __FDelegate_PrimitiveExecute7Implementation =
                    (delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_PrimitiveExecute7Implementation");
            }
#endif

            __FDelegate_PrimitiveExecute7Implementation(InMonoObject, InBuffer, OutBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_CompoundExecute7Implementation(nint A0, byte* A1, byte* A2, byte* A3);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, void>
            __FDelegate_CompoundExecute7Implementation;
#endif

        public static void FDelegate_CompoundExecute7Implementation(nint InMonoObject, byte* InBuffer,
            byte* OutBuffer, byte* ReturnBuffer)
        {
#if !LEANCLR
            if (__FDelegate_CompoundExecute7Implementation == null)
            {
                __FDelegate_CompoundExecute7Implementation =
                    (delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FDelegateImplementation::FDelegate_CompoundExecute7Implementation");
            }
#endif

            __FDelegate_CompoundExecute7Implementation(InMonoObject, InBuffer, OutBuffer, ReturnBuffer);
        }
    }
}