#if UE_5_6_OR_LATER
using Script.CoreUObject;
using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static class FAnsiStringImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FAnsiString_RegisterImplementation(nint A0, byte* A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte*, void> __FAnsiString_RegisterImplementation;
#endif

        public static unsafe void FAnsiString_RegisterImplementation(FAnsiString InString, string InValue)
        {
#if !LEANCLR
            if (__FAnsiString_RegisterImplementation == null)
            {
                __FAnsiString_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FAnsiStringImplementation::FAnsiString_RegisterImplementation");
            }
#endif

            var UTF8 = InValue != null ? System.Text.Encoding.UTF8.GetBytes(InValue + '\0') : [0];

            fixed (byte* Ptr = UTF8)
            {
                __FAnsiString_RegisterImplementation(HandleData.Alloc(InString), Ptr);
            }
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __FAnsiString_IdenticalImplementation(nint A0, nint A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __FAnsiString_IdenticalImplementation;
#endif

        public static unsafe bool FAnsiString_IdenticalImplementation(nint InA, nint InB)
        {
#if !LEANCLR
            if (__FAnsiString_IdenticalImplementation == null)
            {
                __FAnsiString_IdenticalImplementation = (delegate* unmanaged[Cdecl]<nint, nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.FAnsiStringImplementation::FAnsiString_IdenticalImplementation");
            }
#endif

            return __FAnsiString_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FAnsiString_UnRegisterImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __FAnsiString_UnRegisterImplementation;
#endif

        public static unsafe void FAnsiString_UnRegisterImplementation(nint InString)
        {
#if !LEANCLR
            if (__FAnsiString_UnRegisterImplementation == null)
            {
                __FAnsiString_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FAnsiStringImplementation::FAnsiString_UnRegisterImplementation");
            }
#endif

            __FAnsiString_UnRegisterImplementation(InString);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __FAnsiString_ToStringImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __FAnsiString_ToStringImplementation;
#endif

        public static unsafe string FAnsiString_ToStringImplementation(nint InString)
        {
#if !LEANCLR
            if (__FAnsiString_ToStringImplementation == null)
            {
                __FAnsiString_ToStringImplementation = (delegate* unmanaged[Cdecl]<nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.FAnsiStringImplementation::FAnsiString_ToStringImplementation");
            }
#endif

            var Handle = __FAnsiString_ToStringImplementation(InString);

            return Handle != 0 ? (string)HandleData.GetObject(Handle) : null;
        }
    }
}
#endif