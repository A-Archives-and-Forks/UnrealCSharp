using System;
using System.Reflection;
using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static unsafe class FMulticastDelegateImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FMulticastDelegate_RegisterImplementation(nint A0, nint A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, nint, void>
            __FMulticastDelegate_RegisterImplementation;
#endif

        public static void FMulticastDelegate_RegisterImplementation(object InMonoObject, Type InType)
        {
#if !LEANCLR
            if (__FMulticastDelegate_RegisterImplementation == null)
            {
                __FMulticastDelegate_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_RegisterImplementation");
            }
#endif

            __FMulticastDelegate_RegisterImplementation(HandleData.Alloc(InMonoObject), HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FMulticastDelegate_UnRegisterImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, void> __FMulticastDelegate_UnRegisterImplementation;
#endif

        public static void FMulticastDelegate_UnRegisterImplementation(nint InMonoObject)
        {
#if !LEANCLR
            if (__FMulticastDelegate_UnRegisterImplementation == null)
            {
                __FMulticastDelegate_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_UnRegisterImplementation");
            }
#endif

            __FMulticastDelegate_UnRegisterImplementation(InMonoObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __FMulticastDelegate_IsBoundImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte> __FMulticastDelegate_IsBoundImplementation;
#endif

        public static unsafe bool FMulticastDelegate_IsBoundImplementation(nint InMonoObject)
        {
#if !LEANCLR
            if (__FMulticastDelegate_IsBoundImplementation == null)
            {
                __FMulticastDelegate_IsBoundImplementation = (delegate* unmanaged[Cdecl]<nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_IsBoundImplementation");
            }
#endif

            return __FMulticastDelegate_IsBoundImplementation(InMonoObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __FMulticastDelegate_ContainsImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static delegate* unmanaged[Cdecl]<nint, nint, nint, nint, byte>
            __FMulticastDelegate_ContainsImplementation;
#endif

        public static bool FMulticastDelegate_ContainsImplementation(nint InMonoObject, nint InObject,
            Type InType, MethodInfo InMethodInfo)
        {
#if !LEANCLR
            if (__FMulticastDelegate_ContainsImplementation == null)
            {
                __FMulticastDelegate_ContainsImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_ContainsImplementation");
            }
#endif

            return __FMulticastDelegate_ContainsImplementation(InMonoObject, InObject,
                HandleData.Alloc(InType), HandleData.Alloc(InMethodInfo)) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FMulticastDelegate_AddImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __FMulticastDelegate_AddImplementation;
#endif

        public static void FMulticastDelegate_AddImplementation(nint InMonoObject, nint InObject, Type InType,
            MethodInfo InMethodInfo)
        {
#if !LEANCLR
            if (__FMulticastDelegate_AddImplementation == null)
            {
                __FMulticastDelegate_AddImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_AddImplementation");
            }
#endif

            __FMulticastDelegate_AddImplementation(InMonoObject, InObject, HandleData.Alloc(InType),
                HandleData.Alloc(InMethodInfo));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FMulticastDelegate_AddUniqueImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __FMulticastDelegate_AddUniqueImplementation;
#endif

        public static void FMulticastDelegate_AddUniqueImplementation(nint InMonoObject, nint InObject,
            Type InType, MethodInfo InMethodInfo)
        {
#if !LEANCLR
            if (__FMulticastDelegate_AddUniqueImplementation == null)
            {
                __FMulticastDelegate_AddUniqueImplementation =
                    (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_AddUniqueImplementation");
            }
#endif

            __FMulticastDelegate_AddUniqueImplementation(InMonoObject, InObject,
                HandleData.Alloc(InType), HandleData.Alloc(InMethodInfo));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FMulticastDelegate_RemoveImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>
            __FMulticastDelegate_RemoveImplementation;
#endif

        public static void FMulticastDelegate_RemoveImplementation(nint InMonoObject, nint InObject, Type InType,
            MethodInfo InMethodInfo)
        {
#if !LEANCLR
            if (__FMulticastDelegate_RemoveImplementation == null)
            {
                __FMulticastDelegate_RemoveImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_RemoveImplementation");
            }
#endif

            __FMulticastDelegate_RemoveImplementation(InMonoObject, InObject, HandleData.Alloc(InType),
                HandleData.Alloc(InMethodInfo));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FMulticastDelegate_RemoveAllImplementation(nint A0, nint A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, nint, void> __FMulticastDelegate_RemoveAllImplementation;
#endif

        public static void FMulticastDelegate_RemoveAllImplementation(nint InMonoObject, nint InObject)
        {
#if !LEANCLR
            if (__FMulticastDelegate_RemoveAllImplementation == null)
            {
                __FMulticastDelegate_RemoveAllImplementation = (delegate* unmanaged[Cdecl]<nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_RemoveAllImplementation");
            }
#endif

            __FMulticastDelegate_RemoveAllImplementation(InMonoObject, InObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FMulticastDelegate_ClearImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, void> __FMulticastDelegate_ClearImplementation;
#endif

        public static void FMulticastDelegate_ClearImplementation(nint InMonoObject)
        {
#if !LEANCLR
            if (__FMulticastDelegate_ClearImplementation == null)
            {
                __FMulticastDelegate_ClearImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_ClearImplementation");
            }
#endif

            __FMulticastDelegate_ClearImplementation(InMonoObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FMulticastDelegate_GenericBroadcast0Implementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, void>
            __FMulticastDelegate_GenericBroadcast0Implementation;
#endif

        public static void FMulticastDelegate_GenericBroadcast0Implementation(nint InMonoObject)
        {
#if !LEANCLR
            if (__FMulticastDelegate_GenericBroadcast0Implementation == null)
            {
                __FMulticastDelegate_GenericBroadcast0Implementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_GenericBroadcast0Implementation");
            }
#endif

            __FMulticastDelegate_GenericBroadcast0Implementation(InMonoObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FMulticastDelegate_GenericBroadcast2Implementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, void>
            __FMulticastDelegate_GenericBroadcast2Implementation;
#endif

        public static void FMulticastDelegate_GenericBroadcast2Implementation(nint InMonoObject, byte* InBuffer)
        {
#if !LEANCLR
            if (__FMulticastDelegate_GenericBroadcast2Implementation == null)
            {
                __FMulticastDelegate_GenericBroadcast2Implementation = (delegate* unmanaged[Cdecl]<nint, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_GenericBroadcast2Implementation");
            }
#endif

            __FMulticastDelegate_GenericBroadcast2Implementation(InMonoObject, InBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FMulticastDelegate_GenericBroadcast4Implementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, void>
            __FMulticastDelegate_GenericBroadcast4Implementation;
#endif

        public static void FMulticastDelegate_GenericBroadcast4Implementation(nint InMonoObject, byte* OutBuffer)
        {
#if !LEANCLR
            if (__FMulticastDelegate_GenericBroadcast4Implementation == null)
            {
                __FMulticastDelegate_GenericBroadcast4Implementation = (delegate* unmanaged[Cdecl]<nint, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_GenericBroadcast4Implementation");
            }
#endif

            __FMulticastDelegate_GenericBroadcast4Implementation(InMonoObject, OutBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FMulticastDelegate_GenericBroadcast6Implementation(nint A0, byte* A1, byte* A2);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>
            __FMulticastDelegate_GenericBroadcast6Implementation;
#endif

        public static void FMulticastDelegate_GenericBroadcast6Implementation(nint InMonoObject, byte* InBuffer,
            byte* OutBuffer)
        {
#if !LEANCLR
            if (__FMulticastDelegate_GenericBroadcast6Implementation == null)
            {
                __FMulticastDelegate_GenericBroadcast6Implementation =
                    (delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FMulticastDelegateImplementation::FMulticastDelegate_GenericBroadcast6Implementation");
            }
#endif

            __FMulticastDelegate_GenericBroadcast6Implementation(InMonoObject, InBuffer, OutBuffer);
        }
    }
}