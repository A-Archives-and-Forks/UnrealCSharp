using System.Text;
using Script.CoreUObject;
using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static class FStringImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FString_RegisterImplementation(nint A0, byte* A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte*, void> __FString_RegisterImplementation;
#endif

        public static unsafe void FString_RegisterImplementation(FString InString, string InValue)
        {
#if !LEANCLR
            if (__FString_RegisterImplementation == null)
            {
                __FString_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FStringImplementation::FString_RegisterImplementation");
            }
#endif

            var UTF8 = InValue != null ? Encoding.UTF8.GetBytes(InValue + '\0') : [0];

            fixed (byte* Ptr = UTF8)
            {
                __FString_RegisterImplementation(HandleData.Alloc(InString), Ptr);
            }
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __FString_IdenticalImplementation(nint A0, nint A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __FString_IdenticalImplementation;
#endif

        public static unsafe bool FString_IdenticalImplementation(nint InA, nint InB)
        {
#if !LEANCLR
            if (__FString_IdenticalImplementation == null)
            {
                __FString_IdenticalImplementation = (delegate* unmanaged[Cdecl]<nint, nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.FStringImplementation::FString_IdenticalImplementation");
            }
#endif

            return __FString_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FString_UnRegisterImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __FString_UnRegisterImplementation;
#endif

        public static unsafe void FString_UnRegisterImplementation(nint InString)
        {
#if !LEANCLR
            if (__FString_UnRegisterImplementation == null)
            {
                __FString_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FStringImplementation::FString_UnRegisterImplementation");
            }
#endif

            __FString_UnRegisterImplementation(InString);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __FString_ToStringImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __FString_ToStringImplementation;
#endif

        public static unsafe string FString_ToStringImplementation(nint InString)
        {
#if !LEANCLR
            if (__FString_ToStringImplementation == null)
            {
                __FString_ToStringImplementation = (delegate* unmanaged[Cdecl]<nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.FStringImplementation::FString_ToStringImplementation");
            }
#endif

            var Handle = __FString_ToStringImplementation(InString);

            return Handle != 0 ? (string)HandleData.GetObject(Handle) : null;
        }
    }
}