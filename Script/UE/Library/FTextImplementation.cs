using Script.CoreUObject;
using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static class FTextImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FText_RegisterImplementation(nint A0, byte* A1, byte* A2, byte* A3, byte A4);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, byte, void>
            __FText_RegisterImplementation;
#endif

        public static unsafe void FText_RegisterImplementation(FText InText, string InBuffer, string InTextNamespace,
            string InPackageNamespace, bool bRequiresQuotes)
        {
#if !LEANCLR
            if (__FText_RegisterImplementation == null)
            {
                __FText_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, byte*, byte*, byte, void>)
                    MethodBridge.GetMethod("Script.Library.FTextImplementation::FText_RegisterImplementation");
            }
#endif

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

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __FText_IdenticalImplementation(nint A0, nint A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __FText_IdenticalImplementation;
#endif

        public static unsafe bool FText_IdenticalImplementation(nint InA, nint InB)
        {
#if !LEANCLR
            if (__FText_IdenticalImplementation == null)
            {
                __FText_IdenticalImplementation = (delegate* unmanaged[Cdecl]<nint, nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.FTextImplementation::FText_IdenticalImplementation");
            }
#endif

            return __FText_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FText_UnRegisterImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __FText_UnRegisterImplementation;
#endif

        public static unsafe void FText_UnRegisterImplementation(nint InText)
        {
#if !LEANCLR
            if (__FText_UnRegisterImplementation == null)
            {
                __FText_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FTextImplementation::FText_UnRegisterImplementation");
            }
#endif

            __FText_UnRegisterImplementation(InText);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __FText_ToStringImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __FText_ToStringImplementation;
#endif

        public static unsafe string FText_ToStringImplementation(nint InText)
        {
#if !LEANCLR
            if (__FText_ToStringImplementation == null)
            {
                __FText_ToStringImplementation = (delegate* unmanaged[Cdecl]<nint, nint>)
                    MethodBridge.GetMethod("Script.Library.FTextImplementation::FText_ToStringImplementation");
            }
#endif

            var Handle = __FText_ToStringImplementation(InText);

            return Handle != 0 ? (string)HandleData.GetObject(Handle) : null;
        }
    }
}