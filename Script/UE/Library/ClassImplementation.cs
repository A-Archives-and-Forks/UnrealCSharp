using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static partial class UClassImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UClass_RemoveFunctionImplementation(nint A0, nint A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, void> __UClass_RemoveFunctionImplementation;
#endif

        public static unsafe void UClass_RemoveFunctionImplementation(nint InClass, nint InName)
        {
#if !LEANCLR
            if (__UClass_RemoveFunctionImplementation == null)
            {
                __UClass_RemoveFunctionImplementation = (delegate* unmanaged[Cdecl]<nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.UClassImplementation::UClass_RemoveFunctionImplementation");
            }
#endif

            __UClass_RemoveFunctionImplementation(InClass, InName);
        }
    }
}