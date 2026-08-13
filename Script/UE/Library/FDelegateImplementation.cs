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
        private static nint __FDelegate_RegisterImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, nint, void> __FDelegate_RegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, void>)MethodBridge.Resolve(
                ref __FDelegate_RegisterImplementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_RegisterImplementation");
#endif

        public static void FDelegate_RegisterImplementation(object InMonoObject, Type InType)
        {
            __FDelegate_RegisterImplementation(HandleData.Alloc(InMonoObject), HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_UnRegisterImplementation(nint A0);
#else
        private static nint __FDelegate_UnRegisterImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, void> __FDelegate_UnRegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(ref __FDelegate_UnRegisterImplementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_UnRegisterImplementation");
#endif

        public static void FDelegate_UnRegisterImplementation(nint InMonoObject)
        {
            __FDelegate_UnRegisterImplementation(InMonoObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_BindImplementation(nint A0, nint A1, nint A2, nint A3);
#else
        private static nint __FDelegate_BindImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void> __FDelegate_BindImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, nint, void>)MethodBridge.Resolve(
                ref __FDelegate_BindImplementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_BindImplementation");
#endif

        public static void FDelegate_BindImplementation(nint InMonoObject, nint InObject, Type InType,
            MethodInfo InMethodInfo)
        {
            __FDelegate_BindImplementation(InMonoObject, InObject, HandleData.Alloc(InType),
                HandleData.Alloc(InMethodInfo));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __FDelegate_IsBoundImplementation(nint A0);
#else
        private static nint __FDelegate_IsBoundImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte> __FDelegate_IsBoundImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte>)MethodBridge.Resolve(ref __FDelegate_IsBoundImplementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_IsBoundImplementation");
#endif

        public static bool FDelegate_IsBoundImplementation(nint InMonoObject)
        {
            return __FDelegate_IsBoundImplementation(InMonoObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_UnBindImplementation(nint A0);
#else
        private static nint __FDelegate_UnBindImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, void> __FDelegate_UnBindImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(ref __FDelegate_UnBindImplementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_UnBindImplementation");
#endif

        public static void FDelegate_UnBindImplementation(nint InMonoObject)
        {
            __FDelegate_UnBindImplementation(InMonoObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_ClearImplementation(nint A0);
#else
        private static nint __FDelegate_ClearImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, void> __FDelegate_ClearImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(ref __FDelegate_ClearImplementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_ClearImplementation");
#endif

        public static void FDelegate_ClearImplementation(nint InMonoObject)
        {
            __FDelegate_ClearImplementation(InMonoObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_GenericExecute0Implementation(nint A0);
#else
        private static nint __FDelegate_GenericExecute0Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, void> __FDelegate_GenericExecute0Implementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(
                ref __FDelegate_GenericExecute0Implementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_GenericExecute0Implementation");
#endif

        public static void FDelegate_GenericExecute0Implementation(nint InMonoObject)
        {
            __FDelegate_GenericExecute0Implementation(InMonoObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_PrimitiveExecute1Implementation(nint A0, byte* A1);
#else
        private static nint __FDelegate_PrimitiveExecute1Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, void> __FDelegate_PrimitiveExecute1Implementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, void>)MethodBridge.Resolve(
                ref __FDelegate_PrimitiveExecute1Implementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_PrimitiveExecute1Implementation");
#endif

        public static void FDelegate_PrimitiveExecute1Implementation(nint InMonoObject, byte* ReturnBuffer)
        {
            __FDelegate_PrimitiveExecute1Implementation(InMonoObject, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_CompoundExecute1Implementation(nint A0, byte* A1);
#else
        private static nint __FDelegate_CompoundExecute1Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, void> __FDelegate_CompoundExecute1Implementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, void>)MethodBridge.Resolve(
                ref __FDelegate_CompoundExecute1Implementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_CompoundExecute1Implementation");
#endif

        public static void FDelegate_CompoundExecute1Implementation(nint InMonoObject, byte* ReturnBuffer)
        {
            __FDelegate_CompoundExecute1Implementation(InMonoObject, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_GenericExecute2Implementation(nint A0, byte* A1);
#else
        private static nint __FDelegate_GenericExecute2Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, void> __FDelegate_GenericExecute2Implementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, void>)MethodBridge.Resolve(
                ref __FDelegate_GenericExecute2Implementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_GenericExecute2Implementation");
#endif

        public static void FDelegate_GenericExecute2Implementation(nint InMonoObject, byte* InBuffer)
        {
            __FDelegate_GenericExecute2Implementation(InMonoObject, InBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_PrimitiveExecute3Implementation(nint A0, byte* A1, byte* A2);
#else
        private static nint __FDelegate_PrimitiveExecute3Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>
            __FDelegate_PrimitiveExecute3Implementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FDelegate_PrimitiveExecute3Implementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_PrimitiveExecute3Implementation");
#endif

        public static void FDelegate_PrimitiveExecute3Implementation(nint InMonoObject, byte* InBuffer,
            byte* ReturnBuffer)
        {
            __FDelegate_PrimitiveExecute3Implementation(InMonoObject, InBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_CompoundExecute3Implementation(nint A0, byte* A1, byte* A2);
#else
        private static nint __FDelegate_CompoundExecute3Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>
            __FDelegate_CompoundExecute3Implementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FDelegate_CompoundExecute3Implementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_CompoundExecute3Implementation");
#endif

        public static void FDelegate_CompoundExecute3Implementation(nint InMonoObject, byte* InBuffer,
            byte* ReturnBuffer)
        {
            __FDelegate_CompoundExecute3Implementation(InMonoObject, InBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_GenericExecute4Implementation(nint A0, byte* A1);
#else
        private static nint __FDelegate_GenericExecute4Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, void> __FDelegate_GenericExecute4Implementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, void>)MethodBridge.Resolve(
                ref __FDelegate_GenericExecute4Implementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_GenericExecute4Implementation");
#endif

        public static void FDelegate_GenericExecute4Implementation(nint InMonoObject, byte* OutBuffer)
        {
            __FDelegate_GenericExecute4Implementation(InMonoObject, OutBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_GenericExecute6Implementation(nint A0, byte* A1, byte* A2);
#else
        private static nint __FDelegate_GenericExecute6Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, void> __FDelegate_GenericExecute6Implementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FDelegate_GenericExecute6Implementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_GenericExecute6Implementation");
#endif

        public static void FDelegate_GenericExecute6Implementation(nint InMonoObject, byte* InBuffer,
            byte* OutBuffer)
        {
            __FDelegate_GenericExecute6Implementation(InMonoObject, InBuffer, OutBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_PrimitiveExecute7Implementation(nint A0, byte* A1, byte* A2, byte* A3);
#else
        private static nint __FDelegate_PrimitiveExecute7Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, void>
            __FDelegate_PrimitiveExecute7Implementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FDelegate_PrimitiveExecute7Implementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_PrimitiveExecute7Implementation");
#endif

        public static void FDelegate_PrimitiveExecute7Implementation(nint InMonoObject, byte* InBuffer,
            byte* OutBuffer, byte* ReturnBuffer)
        {
            __FDelegate_PrimitiveExecute7Implementation(InMonoObject, InBuffer, OutBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FDelegate_CompoundExecute7Implementation(nint A0, byte* A1, byte* A2, byte* A3);
#else
        private static nint __FDelegate_CompoundExecute7Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, void>
            __FDelegate_CompoundExecute7Implementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FDelegate_CompoundExecute7Implementation_Slot,
                "Script.Library.FDelegateImplementation::FDelegate_CompoundExecute7Implementation");
#endif

        public static void FDelegate_CompoundExecute7Implementation(nint InMonoObject, byte* InBuffer,
            byte* OutBuffer, byte* ReturnBuffer)
        {
            __FDelegate_CompoundExecute7Implementation(InMonoObject, InBuffer, OutBuffer, ReturnBuffer);
        }
    }
}