#if UE_5_6_OR_LATER
using Script.CoreUObject;
using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static class FUtf8StringImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FUtf8String_RegisterImplementation(nint A0, byte* A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte*, void> __FUtf8String_RegisterImplementation;
#endif

        public static unsafe void FUtf8String_RegisterImplementation(FUtf8String InString, string InValue)
        {
#if !LEANCLR
            if (__FUtf8String_RegisterImplementation == null)
            {
                __FUtf8String_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FUtf8StringImplementation::FUtf8String_RegisterImplementation");
            }
#endif

            var UTF8 = InValue != null ? System.Text.Encoding.UTF8.GetBytes(InValue + '\0') : [0];

            fixed (byte* Ptr = UTF8)
            {
                __FUtf8String_RegisterImplementation(HandleData.Alloc(InString), Ptr);
            }
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __FUtf8String_IdenticalImplementation(nint A0, nint A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __FUtf8String_IdenticalImplementation;
#endif

        public static unsafe bool FUtf8String_IdenticalImplementation(nint InA, nint InB)
        {
#if !LEANCLR
            if (__FUtf8String_IdenticalImplementation == null)
            {
                __FUtf8String_IdenticalImplementation = (delegate* unmanaged[Cdecl]<nint, nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.FUtf8StringImplementation::FUtf8String_IdenticalImplementation");
            }
#endif

            return __FUtf8String_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FUtf8String_UnRegisterImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __FUtf8String_UnRegisterImplementation;
#endif

        public static unsafe void FUtf8String_UnRegisterImplementation(nint InString)
        {
#if !LEANCLR
            if (__FUtf8String_UnRegisterImplementation == null)
            {
                __FUtf8String_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FUtf8StringImplementation::FUtf8String_UnRegisterImplementation");
            }
#endif

            __FUtf8String_UnRegisterImplementation(InString);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __FUtf8String_ToStringImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __FUtf8String_ToStringImplementation;
#endif

        public static unsafe string FUtf8String_ToStringImplementation(nint InString)
        {
#if !LEANCLR
            if (__FUtf8String_ToStringImplementation == null)
            {
                __FUtf8String_ToStringImplementation = (delegate* unmanaged[Cdecl]<nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.FUtf8StringImplementation::FUtf8String_ToStringImplementation");
            }
#endif

            var Handle = __FUtf8String_ToStringImplementation(InString);

            return Handle != 0 ? (string)HandleData.GetObject(Handle) : null;
        }
    }
}
#endif