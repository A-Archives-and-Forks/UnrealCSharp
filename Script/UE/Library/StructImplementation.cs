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
        private static unsafe delegate* unmanaged[Cdecl]<byte*, nint> __UStruct_StaticStructImplementation;
#endif

        public static unsafe UScriptStruct UStruct_StaticStructImplementation(string InStructName)
        {
#if !LEANCLR
            if (__UStruct_StaticStructImplementation == null)
            {
                __UStruct_StaticStructImplementation = (delegate* unmanaged[Cdecl]<byte*, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.UStructImplementation::UStruct_StaticStructImplementation");
            }
#endif

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
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte*, void> __UStruct_RegisterImplementation;
#endif

        public static unsafe void UStruct_RegisterImplementation(object InMonoObject, string InStructName)
        {
#if !LEANCLR
            if (__UStruct_RegisterImplementation == null)
            {
                __UStruct_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.UStructImplementation::UStruct_RegisterImplementation");
            }
#endif

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
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, byte> __UStruct_IdenticalImplementation;
#endif

        public static unsafe bool UStruct_IdenticalImplementation(nint InScriptStruct, nint InA, nint InB)
        {
#if !LEANCLR
            if (__UStruct_IdenticalImplementation == null)
            {
                __UStruct_IdenticalImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.UStructImplementation::UStruct_IdenticalImplementation");
            }
#endif

            return __UStruct_IdenticalImplementation(InScriptStruct, InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UStruct_UnRegisterImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __UStruct_UnRegisterImplementation;
#endif

        public static unsafe void UStruct_UnRegisterImplementation(nint InMonoObject)
        {
#if !LEANCLR
            if (__UStruct_UnRegisterImplementation == null)
            {
                __UStruct_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.UStructImplementation::UStruct_UnRegisterImplementation");
            }
#endif

            __UStruct_UnRegisterImplementation(InMonoObject);
        }
    }
}