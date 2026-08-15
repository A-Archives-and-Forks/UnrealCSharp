using Script.CoreUObject;
using Interop;

namespace Script.Library
{
    public static partial class FTextImplementation
    {
        private static unsafe partial void __FText_RegisterImplementation(nint A0, byte* A1, byte* A2, byte* A3, byte A4);

        public static unsafe void FText_RegisterImplementation(FText InText, string InBuffer, string InTextNamespace,
            string InPackageNamespace, bool bRequiresQuotes)
        {
            var Buffer = InBuffer != null ? System.Text.Encoding.UTF8.GetBytes(InBuffer + '\0') : [0];

            var TextNamespace = InTextNamespace != null
                ? System.Text.Encoding.UTF8.GetBytes(InTextNamespace + '\0')
                : [0];

            var PackageNamespace = InPackageNamespace != null
                ? System.Text.Encoding.UTF8.GetBytes(InPackageNamespace + '\0')
                : [0];

            fixed (byte* BufferPtr = Buffer, TextNamespacePtr = TextNamespace, PackageNamespacePtr = PackageNamespace)
            {
                __FText_RegisterImplementation(HandleData.Alloc(InText), BufferPtr, TextNamespacePtr,
                    PackageNamespacePtr, (byte)(bRequiresQuotes ? 1 : 0));
            }
        }

        private static unsafe partial byte __FText_IdenticalImplementation(nint A0, nint A1);

        public static unsafe bool FText_IdenticalImplementation(nint InA, nint InB)
        {
            return __FText_IdenticalImplementation(InA, InB) != 0;
        }

        private static unsafe partial void __FText_UnRegisterImplementation(nint A0);

        public static unsafe void FText_UnRegisterImplementation(nint InText)
        {
            __FText_UnRegisterImplementation(InText);
        }

        private static unsafe partial nint __FText_ToStringImplementation(nint A0);

        public static unsafe string FText_ToStringImplementation(nint InText)
        {
            var Handle = __FText_ToStringImplementation(InText);

            return Handle != 0 ? (string)HandleData.GetObject(Handle) : null;
        }
    }
}