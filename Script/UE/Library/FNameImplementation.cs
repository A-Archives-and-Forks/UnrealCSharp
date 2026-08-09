using System.Text;
using Script.CoreUObject;
using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static class FNameImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FName_RegisterImplementation(nint A0, byte* A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte*, void> __FName_RegisterImplementation;
#endif

        public static unsafe void FName_RegisterImplementation(FName InName, string InValue)
        {
#if !LEANCLR
            if (__FName_RegisterImplementation == null)
            {
                __FName_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, void>)
                    MethodBridge.GetMethod("Script.Library.FNameImplementation::FName_RegisterImplementation");
            }
#endif

            var UTF8 = InValue != null ? Encoding.UTF8.GetBytes(InValue + '\0') : [0];

            fixed (byte* Ptr = UTF8)
            {
                __FName_RegisterImplementation(HandleData.Alloc(InName), Ptr);
            }
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __FName_IdenticalImplementation(nint A0, nint A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __FName_IdenticalImplementation;
#endif

        public static unsafe bool FName_IdenticalImplementation(nint InA, nint InB)
        {
#if !LEANCLR
            if (__FName_IdenticalImplementation == null)
            {
                __FName_IdenticalImplementation = (delegate* unmanaged[Cdecl]<nint, nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.FNameImplementation::FName_IdenticalImplementation");
            }
#endif

            return __FName_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __FName_UnRegisterImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __FName_UnRegisterImplementation;
#endif

        public static unsafe void FName_UnRegisterImplementation(nint InName)
        {
#if !LEANCLR
            if (__FName_UnRegisterImplementation == null)
            {
                __FName_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.FNameImplementation::FName_UnRegisterImplementation");
            }
#endif

            __FName_UnRegisterImplementation(InName);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __FName_ToStringImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __FName_ToStringImplementation;
#endif

        public static unsafe string FName_ToStringImplementation(nint InName)
        {
#if !LEANCLR
            if (__FName_ToStringImplementation == null)
            {
                __FName_ToStringImplementation = (delegate* unmanaged[Cdecl]<nint, nint>)
                    MethodBridge.GetMethod("Script.Library.FNameImplementation::FName_ToStringImplementation");
            }
#endif

            var Handle = __FName_ToStringImplementation(InName);

            return Handle != 0 ? (string)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __FName_NAME_NoneImplementation();
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint> __FName_NAME_NoneImplementation;
#endif

        public static unsafe FName FName_NAME_NoneImplementation()
        {
#if !LEANCLR
            if (__FName_NAME_NoneImplementation == null)
            {
                __FName_NAME_NoneImplementation = (delegate* unmanaged[Cdecl]<nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.FNameImplementation::FName_NAME_NoneImplementation");
            }
#endif

            var Handle = __FName_NAME_NoneImplementation();

            return Handle != 0 ? (FName)HandleData.GetObject(Handle) : null;
        }
    }
}