using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static unsafe class FPropertyImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FProperty_GetObjectPropertyImplementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FProperty_GetObjectPropertyImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void>
            __FProperty_GetObjectPropertyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FProperty_GetObjectPropertyImplementation_Slot,
                "Script.Library.FPropertyImplementation::FProperty_GetObjectPropertyImplementation");
#endif

        public static void FProperty_GetObjectPropertyImplementation(nint InMonoObject, uint InPropertyHash,
            byte* ReturnBuffer)
        {
            __FProperty_GetObjectPropertyImplementation(InMonoObject, InPropertyHash, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FProperty_SetObjectPropertyImplementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FProperty_SetObjectPropertyImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void>
            __FProperty_SetObjectPropertyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FProperty_SetObjectPropertyImplementation_Slot,
                "Script.Library.FPropertyImplementation::FProperty_SetObjectPropertyImplementation");
#endif

        public static void FProperty_SetObjectPropertyImplementation(nint InMonoObject, uint InPropertyHash,
            byte* InBuffer)
        {
            __FProperty_SetObjectPropertyImplementation(InMonoObject, InPropertyHash, InBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FProperty_GetStructPropertyImplementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FProperty_GetStructPropertyImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void>
            __FProperty_GetStructPropertyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FProperty_GetStructPropertyImplementation_Slot,
                "Script.Library.FPropertyImplementation::FProperty_GetStructPropertyImplementation");
#endif

        public static void FProperty_GetStructPropertyImplementation(nint InMonoObject, uint InPropertyHash,
            byte* ReturnBuffer)
        {
            __FProperty_GetStructPropertyImplementation(InMonoObject, InPropertyHash, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __FProperty_SetStructPropertyImplementation(nint A0, uint A1, byte* A2);
#else
        private static nint __FProperty_SetStructPropertyImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, uint, byte*, void>
            __FProperty_SetStructPropertyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, uint, byte*, void>)MethodBridge.Resolve(
                ref __FProperty_SetStructPropertyImplementation_Slot,
                "Script.Library.FPropertyImplementation::FProperty_SetStructPropertyImplementation");
#endif

        public static void FProperty_SetStructPropertyImplementation(nint InMonoObject, uint InPropertyHash,
            byte* InBuffer)
        {
            __FProperty_SetStructPropertyImplementation(InMonoObject, InPropertyHash, InBuffer);
        }
    }
}