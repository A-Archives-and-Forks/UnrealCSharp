using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static unsafe class FFunctionImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_GenericCall0Implementation(nint A0, uint A1);
#else
        private static nint __FFunction_GenericCall0Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, void> __FFunction_GenericCall0Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, void>)MethodBridge.Resolve(
                ref __FFunction_GenericCall0Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_GenericCall0Implementation");
#endif

        public static void FFunction_GenericCall0Implementation(nint InMonoObject, uint InFunctionHash)
        {
            __FFunction_GenericCall0Implementation(InMonoObject, InFunctionHash);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_PrimitiveCall1Implementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FFunction_PrimitiveCall1Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void> __FFunction_PrimitiveCall1Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_PrimitiveCall1Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_PrimitiveCall1Implementation");
#endif

        public static void FFunction_PrimitiveCall1Implementation(nint InMonoObject, uint InFunctionHash,
            byte* ReturnBuffer)
        {
            __FFunction_PrimitiveCall1Implementation(InMonoObject, InFunctionHash, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_CompoundCall1Implementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FFunction_CompoundCall1Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void> __FFunction_CompoundCall1Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_CompoundCall1Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_CompoundCall1Implementation");
#endif

        public static void FFunction_CompoundCall1Implementation(nint InMonoObject, uint InFunctionHash,
            byte* ReturnBuffer)
        {
            __FFunction_CompoundCall1Implementation(InMonoObject, InFunctionHash, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_GenericCall2Implementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FFunction_GenericCall2Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void> __FFunction_GenericCall2Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_GenericCall2Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_GenericCall2Implementation");
#endif

        public static void FFunction_GenericCall2Implementation(nint InMonoObject, uint InFunctionHash, byte* InBuffer)
        {
            __FFunction_GenericCall2Implementation(InMonoObject, InFunctionHash, InBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_PrimitiveCall3Implementation(nint A0, uint A1, byte* A2, byte* A3);
#else
        private static nint __FFunction_PrimitiveCall3Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void>
            __FFunction_PrimitiveCall3Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_PrimitiveCall3Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_PrimitiveCall3Implementation");
#endif

        public static void FFunction_PrimitiveCall3Implementation(nint InMonoObject, uint InFunctionHash,
            byte* InBuffer, byte* ReturnBuffer)
        {
            __FFunction_PrimitiveCall3Implementation(InMonoObject, InFunctionHash, InBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_CompoundCall3Implementation(nint A0, uint A1, byte* A2, byte* A3);
#else
        private static nint __FFunction_CompoundCall3Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void>
            __FFunction_CompoundCall3Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_CompoundCall3Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_CompoundCall3Implementation");
#endif

        public static void FFunction_CompoundCall3Implementation(nint InMonoObject, uint InFunctionHash, byte* InBuffer,
            byte* ReturnBuffer)
        {
            __FFunction_CompoundCall3Implementation(InMonoObject, InFunctionHash, InBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_GenericCall4Implementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FFunction_GenericCall4Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void> __FFunction_GenericCall4Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_GenericCall4Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_GenericCall4Implementation");
#endif

        public static void FFunction_GenericCall4Implementation(nint InMonoObject, uint InFunctionHash, byte* OutBuffer)
        {
            __FFunction_GenericCall4Implementation(InMonoObject, InFunctionHash, OutBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_GenericCall6Implementation(nint A0, uint A1, byte* A2, byte* A3);
#else
        private static nint __FFunction_GenericCall6Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void>
            __FFunction_GenericCall6Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_GenericCall6Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_GenericCall6Implementation");
#endif

        public static void FFunction_GenericCall6Implementation(nint InMonoObject, uint InFunctionHash, byte* InBuffer,
            byte* OutBuffer)
        {
            __FFunction_GenericCall6Implementation(InMonoObject, InFunctionHash, InBuffer, OutBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_PrimitiveCall7Implementation(nint A0, uint A1, byte* A2, byte* A3, byte* A4);
#else
        private static nint __FFunction_PrimitiveCall7Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, byte*, void>
            __FFunction_PrimitiveCall7Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_PrimitiveCall7Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_PrimitiveCall7Implementation");
#endif

        public static void FFunction_PrimitiveCall7Implementation(nint InMonoObject, uint InFunctionHash,
            byte* InBuffer, byte* OutBuffer, byte* ReturnBuffer)
        {
            __FFunction_PrimitiveCall7Implementation(InMonoObject, InFunctionHash, InBuffer, OutBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_CompoundCall7Implementation(nint A0, uint A1, byte* A2, byte* A3, byte* A4);
#else
        private static nint __FFunction_CompoundCall7Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, byte*, void>
            __FFunction_CompoundCall7Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_CompoundCall7Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_CompoundCall7Implementation");
#endif

        public static void FFunction_CompoundCall7Implementation(nint InMonoObject, uint InFunctionHash, byte* InBuffer,
            byte* OutBuffer, byte* ReturnBuffer)
        {
            __FFunction_CompoundCall7Implementation(InMonoObject, InFunctionHash, InBuffer, OutBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_GenericCall8Implementation(nint A0, uint A1);
#else
        private static nint __FFunction_GenericCall8Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, void> __FFunction_GenericCall8Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, void>)MethodBridge.Resolve(
                ref __FFunction_GenericCall8Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_GenericCall8Implementation");
#endif

        public static void FFunction_GenericCall8Implementation(nint InMonoObject, uint InFunctionHash)
        {
            __FFunction_GenericCall8Implementation(InMonoObject, InFunctionHash);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_PrimitiveCall9Implementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FFunction_PrimitiveCall9Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void> __FFunction_PrimitiveCall9Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_PrimitiveCall9Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_PrimitiveCall9Implementation");
#endif

        public static void FFunction_PrimitiveCall9Implementation(nint InMonoObject, uint InFunctionHash,
            byte* ReturnBuffer)
        {
            __FFunction_PrimitiveCall9Implementation(InMonoObject, InFunctionHash, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_CompoundCall9Implementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FFunction_CompoundCall9Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void> __FFunction_CompoundCall9Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_CompoundCall9Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_CompoundCall9Implementation");
#endif

        public static void FFunction_CompoundCall9Implementation(nint InMonoObject, uint InFunctionHash,
            byte* ReturnBuffer)
        {
            __FFunction_CompoundCall9Implementation(InMonoObject, InFunctionHash, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_GenericCall10Implementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FFunction_GenericCall10Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void> __FFunction_GenericCall10Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_GenericCall10Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_GenericCall10Implementation");
#endif

        public static void FFunction_GenericCall10Implementation(nint InMonoObject, uint InFunctionHash, byte* InBuffer)
        {
            __FFunction_GenericCall10Implementation(InMonoObject, InFunctionHash, InBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_PrimitiveCall11Implementation(nint A0, uint A1, byte* A2, byte* A3);
#else
        private static nint __FFunction_PrimitiveCall11Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void>
            __FFunction_PrimitiveCall11Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_PrimitiveCall11Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_PrimitiveCall11Implementation");
#endif

        public static void FFunction_PrimitiveCall11Implementation(nint InMonoObject, uint InFunctionHash,
            byte* InBuffer, byte* ReturnBuffer)
        {
            __FFunction_PrimitiveCall11Implementation(InMonoObject, InFunctionHash, InBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_CompoundCall11Implementation(nint A0, uint A1, byte* A2, byte* A3);
#else
        private static nint __FFunction_CompoundCall11Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void>
            __FFunction_CompoundCall11Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_CompoundCall11Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_CompoundCall11Implementation");
#endif

        public static void FFunction_CompoundCall11Implementation(nint InMonoObject, uint InFunctionHash,
            byte* InBuffer, byte* ReturnBuffer)
        {
            __FFunction_CompoundCall11Implementation(InMonoObject, InFunctionHash, InBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_GenericCall14Implementation(nint A0, uint A1, byte* A2, byte* A3);
#else
        private static nint __FFunction_GenericCall14Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void>
            __FFunction_GenericCall14Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_GenericCall14Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_GenericCall14Implementation");
#endif

        public static void FFunction_GenericCall14Implementation(nint InMonoObject, uint InFunctionHash, byte* InBuffer,
            byte* OutBuffer)
        {
            __FFunction_GenericCall14Implementation(InMonoObject, InFunctionHash, InBuffer, OutBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_PrimitiveCall15Implementation(nint A0, uint A1, byte* A2, byte* A3, byte* A4);
#else
        private static nint __FFunction_PrimitiveCall15Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, byte*, void>
            __FFunction_PrimitiveCall15Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_PrimitiveCall15Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_PrimitiveCall15Implementation");
#endif

        public static void FFunction_PrimitiveCall15Implementation(nint InMonoObject, uint InFunctionHash,
            byte* InBuffer, byte* OutBuffer, byte* ReturnBuffer)
        {
            __FFunction_PrimitiveCall15Implementation(InMonoObject, InFunctionHash, InBuffer, OutBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_CompoundCall15Implementation(nint A0, uint A1, byte* A2, byte* A3, byte* A4);
#else
        private static nint __FFunction_CompoundCall15Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, byte*, void>
            __FFunction_CompoundCall15Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, byte*, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_CompoundCall15Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_CompoundCall15Implementation");
#endif

        public static void FFunction_CompoundCall15Implementation(nint InMonoObject, uint InFunctionHash,
            byte* InBuffer, byte* OutBuffer, byte* ReturnBuffer)
        {
            __FFunction_CompoundCall15Implementation(InMonoObject, InFunctionHash, InBuffer, OutBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_GenericCall16Implementation(nint A0, uint A1);
#else
        private static nint __FFunction_GenericCall16Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, void> __FFunction_GenericCall16Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, void>)MethodBridge.Resolve(
                ref __FFunction_GenericCall16Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_GenericCall16Implementation");
#endif

        public static void FFunction_GenericCall16Implementation(nint InMonoObject, uint InFunctionHash)
        {
            __FFunction_GenericCall16Implementation(InMonoObject, InFunctionHash);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_GenericCall18Implementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FFunction_GenericCall18Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void> __FFunction_GenericCall18Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_GenericCall18Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_GenericCall18Implementation");
#endif

        public static void FFunction_GenericCall18Implementation(nint InMonoObject, uint InFunctionHash, byte* InBuffer)
        {
            __FFunction_GenericCall18Implementation(InMonoObject, InFunctionHash, InBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_GenericCall24Implementation(nint A0, uint A1);
#else
        private static nint __FFunction_GenericCall24Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, void> __FFunction_GenericCall24Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, void>)MethodBridge.Resolve(
                ref __FFunction_GenericCall24Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_GenericCall24Implementation");
#endif

        public static void FFunction_GenericCall24Implementation(nint InMonoObject, uint InFunctionHash)
        {
            __FFunction_GenericCall24Implementation(InMonoObject, InFunctionHash);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FFunction_GenericCall26Implementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FFunction_GenericCall26Implementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void> __FFunction_GenericCall26Implementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FFunction_GenericCall26Implementation_Slot,
                "Script.Library.FFunctionImplementation::FFunction_GenericCall26Implementation");
#endif

        public static void FFunction_GenericCall26Implementation(nint InMonoObject, uint InFunctionHash, byte* InBuffer)
        {
            __FFunction_GenericCall26Implementation(InMonoObject, InFunctionHash, InBuffer);
        }
    }
}