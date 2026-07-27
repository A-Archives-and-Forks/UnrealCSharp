using System.Text;
using Script.CoreUObject;
using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static partial class UObjectImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UObject_IdenticalImplementation(nint A0, nint A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __UObject_IdenticalImplementation;
#endif

        public static unsafe bool UObject_IdenticalImplementation(nint InA, nint InB)
        {
#if !LEANCLR
            if (__UObject_IdenticalImplementation == null)
            {
                __UObject_IdenticalImplementation = (delegate* unmanaged[Cdecl]<nint, nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.UObjectImplementation::UObject_IdenticalImplementation");
            }
#endif

            return __UObject_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __UObject_StaticClassImplementation(byte* A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<byte*, nint> __UObject_StaticClassImplementation;
#endif

        public static unsafe UClass UObject_StaticClassImplementation(string InClassName)
        {
#if !LEANCLR
            if (__UObject_StaticClassImplementation == null)
            {
                __UObject_StaticClassImplementation = (delegate* unmanaged[Cdecl]<byte*, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.UObjectImplementation::UObject_StaticClassImplementation");
            }
#endif

            nint Handle;

            var UTF8 = InClassName != null ? Encoding.UTF8.GetBytes(InClassName + '\0') : [0];

            fixed (byte* Ptr = UTF8)
            {
                Handle = __UObject_StaticClassImplementation(Ptr);
            }

            return Handle != 0 ? (UClass)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __UObject_GetClassImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __UObject_GetClassImplementation;
#endif

        public static unsafe UClass UObject_GetClassImplementation(nint InObject)
        {
#if !LEANCLR
            if (__UObject_GetClassImplementation == null)
            {
                __UObject_GetClassImplementation = (delegate* unmanaged[Cdecl]<nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.UObjectImplementation::UObject_GetClassImplementation");
            }
#endif

            var Handle = __UObject_GetClassImplementation(InObject);

            return Handle != 0 ? (UClass)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __UObject_GetNameImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __UObject_GetNameImplementation;
#endif

        public static unsafe string UObject_GetNameImplementation(nint InObject)
        {
#if !LEANCLR
            if (__UObject_GetNameImplementation == null)
            {
                __UObject_GetNameImplementation = (delegate* unmanaged[Cdecl]<nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.UObjectImplementation::UObject_GetNameImplementation");
            }
#endif

            var Handle = __UObject_GetNameImplementation(InObject);

            return Handle != 0 ? (string)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UObject_IsValidImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte> __UObject_IsValidImplementation;
#endif

        public static unsafe bool UObject_IsValidImplementation(nint InObject)
        {
#if !LEANCLR
            if (__UObject_IsValidImplementation == null)
            {
                __UObject_IsValidImplementation = (delegate* unmanaged[Cdecl]<nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.UObjectImplementation::UObject_IsValidImplementation");
            }
#endif

            return __UObject_IsValidImplementation(InObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UObject_IsAImplementation(nint A0, nint A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __UObject_IsAImplementation;
#endif

        public static unsafe bool UObject_IsAImplementation(nint InObject, nint SomeBase)
        {
#if !LEANCLR
            if (__UObject_IsAImplementation == null)
            {
                __UObject_IsAImplementation = (delegate* unmanaged[Cdecl]<nint, nint, byte>)
                    MethodBridge.GetMethod("Script.Library.UObjectImplementation::UObject_IsAImplementation");
            }
#endif

            return __UObject_IsAImplementation(InObject, SomeBase) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UObject_AddToRootImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __UObject_AddToRootImplementation;
#endif

        public static unsafe void UObject_AddToRootImplementation(nint InObject)
        {
#if !LEANCLR
            if (__UObject_AddToRootImplementation == null)
            {
                __UObject_AddToRootImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.UObjectImplementation::UObject_AddToRootImplementation");
            }
#endif

            __UObject_AddToRootImplementation(InObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UObject_RemoveFromRootImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __UObject_RemoveFromRootImplementation;
#endif

        public static unsafe void UObject_RemoveFromRootImplementation(nint InObject)
        {
#if !LEANCLR
            if (__UObject_RemoveFromRootImplementation == null)
            {
                __UObject_RemoveFromRootImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.UObjectImplementation::UObject_RemoveFromRootImplementation");
            }
#endif

            __UObject_RemoveFromRootImplementation(InObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UObject_IsRootedImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte> __UObject_IsRootedImplementation;
#endif

        public static unsafe bool UObject_IsRootedImplementation(nint InObject)
        {
#if !LEANCLR
            if (__UObject_IsRootedImplementation == null)
            {
                __UObject_IsRootedImplementation = (delegate* unmanaged[Cdecl]<nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.UObjectImplementation::UObject_IsRootedImplementation");
            }
#endif

            return __UObject_IsRootedImplementation(InObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UObject_AddReferenceImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte> __UObject_AddReferenceImplementation;
#endif

        public static unsafe bool UObject_AddReferenceImplementation(nint InObject)
        {
#if !LEANCLR
            if (__UObject_AddReferenceImplementation == null)
            {
                __UObject_AddReferenceImplementation = (delegate* unmanaged[Cdecl]<nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.UObjectImplementation::UObject_AddReferenceImplementation");
            }
#endif

            return __UObject_AddReferenceImplementation(InObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UObject_RemoveReferenceImplementation(nint A0);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte> __UObject_RemoveReferenceImplementation;
#endif

        public static unsafe bool UObject_RemoveReferenceImplementation(nint InObject)
        {
#if !LEANCLR
            if (__UObject_RemoveReferenceImplementation == null)
            {
                __UObject_RemoveReferenceImplementation = (delegate* unmanaged[Cdecl]<nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.UObjectImplementation::UObject_RemoveReferenceImplementation");
            }
#endif

            return __UObject_RemoveReferenceImplementation(InObject) != 0;
        }
    }
}