using System.Text;
using Script.CoreUObject;
using Interop;

namespace Script.Library
{
    public static partial class UStructImplementation
    {
        private static unsafe partial nint __UStruct_StaticStructImplementation(byte* A0);

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

        private static unsafe partial void __UStruct_RegisterImplementation(nint A0, byte* A1);

        public static unsafe void UStruct_RegisterImplementation(object InMonoObject, string InStructName)
        {
            var UTF8 = InStructName != null ? Encoding.UTF8.GetBytes(InStructName + '\0') : [0];

            fixed (byte* Ptr = UTF8)
            {
                __UStruct_RegisterImplementation(HandleData.Alloc(InMonoObject), Ptr);
            }
        }

        private static unsafe partial byte __UStruct_IdenticalImplementation(nint A0, nint A1, nint A2);

        public static unsafe bool UStruct_IdenticalImplementation(nint InScriptStruct, nint InA, nint InB)
        {
            return __UStruct_IdenticalImplementation(InScriptStruct, InA, InB) != 0;
        }

        private static unsafe partial void __UStruct_UnRegisterImplementation(nint A0);

        public static unsafe void UStruct_UnRegisterImplementation(nint InMonoObject)
        {
            __UStruct_UnRegisterImplementation(InMonoObject);
        }
    }
}