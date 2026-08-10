using System.Text;
using Script.CoreUObject;
using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static partial class UStructImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __UStruct_StaticStructImplementation(byte* A0);
#else
        private static nint __UStruct_StaticStructImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<byte*, nint> __UStruct_StaticStructImplementation =>
            (delegate* unmanaged[Cdecl]<byte*, nint>)MethodBridge.Resolve(ref __UStruct_StaticStructImplementation_Slot,
                "Script.Library.UStructImplementation::UStruct_StaticStructImplementation");
#endif

        public static unsafe UScriptStruct UStruct_StaticStructImplementation(string InStructName)
        {
            nint Handle;

            var UTF8 = InStructName != null ? Encoding.UTF8.GetBytes(InStructName + '\0') : [0];

            fixed (byte* Ptr = UTF8)
            {
                Handle = __UStruct_StaticStructImplementation(Ptr);
            }

            return Handle != 0 ? (UScriptStruct)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UStruct_RegisterImplementation(nint A0, byte* A1);
#else
        private static nint __UStruct_RegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte*, void> __UStruct_RegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, void>)MethodBridge.Resolve(
                ref __UStruct_RegisterImplementation_Slot,
                "Script.Library.UStructImplementation::UStruct_RegisterImplementation");
#endif

        public static unsafe void UStruct_RegisterImplementation(object InMonoObject, string InStructName)
        {
            var UTF8 = InStructName != null ? Encoding.UTF8.GetBytes(InStructName + '\0') : [0];

            fixed (byte* Ptr = UTF8)
            {
                __UStruct_RegisterImplementation(HandleData.Alloc(InMonoObject), Ptr);
            }
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UStruct_IdenticalImplementation(nint A0, nint A1, nint A2);
#else
        private static nint __UStruct_IdenticalImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, byte> __UStruct_IdenticalImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, nint, byte>)MethodBridge.Resolve(
                ref __UStruct_IdenticalImplementation_Slot,
                "Script.Library.UStructImplementation::UStruct_IdenticalImplementation");
#endif

        public static unsafe bool UStruct_IdenticalImplementation(nint InScriptStruct, nint InA, nint InB)
        {
            return __UStruct_IdenticalImplementation(InScriptStruct, InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UStruct_UnRegisterImplementation(nint A0);
#else
        private static nint __UStruct_UnRegisterImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __UStruct_UnRegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(ref __UStruct_UnRegisterImplementation_Slot,
                "Script.Library.UStructImplementation::UStruct_UnRegisterImplementation");
#endif

        public static unsafe void UStruct_UnRegisterImplementation(nint InMonoObject)
        {
            __UStruct_UnRegisterImplementation(InMonoObject);
        }
    }
}