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
        private static nint __UObject_IdenticalImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __UObject_IdenticalImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, byte>)MethodBridge.Resolve(
                ref __UObject_IdenticalImplementation_Slot,
                "Script.Library.UObjectImplementation::UObject_IdenticalImplementation");
#endif

        public static unsafe bool UObject_IdenticalImplementation(nint InA, nint InB)
        {
            return __UObject_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __UObject_StaticClassImplementation(byte* A0);
#else
        private static nint __UObject_StaticClassImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<byte*, nint> __UObject_StaticClassImplementation =>
            (delegate* unmanaged[Cdecl]<byte*, nint>)MethodBridge.Resolve(ref __UObject_StaticClassImplementation_Slot,
                "Script.Library.UObjectImplementation::UObject_StaticClassImplementation");
#endif

        public static unsafe UClass UObject_StaticClassImplementation(string InClassName)
        {
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
        private static nint __UObject_GetClassImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __UObject_GetClassImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint>)MethodBridge.Resolve(ref __UObject_GetClassImplementation_Slot,
                "Script.Library.UObjectImplementation::UObject_GetClassImplementation");
#endif

        public static unsafe UClass UObject_GetClassImplementation(nint InObject)
        {
            var Handle = __UObject_GetClassImplementation(InObject);

            return Handle != 0 ? (UClass)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __UObject_GetNameImplementation(nint A0);
#else
        private static nint __UObject_GetNameImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint> __UObject_GetNameImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint>)MethodBridge.Resolve(ref __UObject_GetNameImplementation_Slot,
                "Script.Library.UObjectImplementation::UObject_GetNameImplementation");
#endif

        public static unsafe string UObject_GetNameImplementation(nint InObject)
        {
            var Handle = __UObject_GetNameImplementation(InObject);

            return Handle != 0 ? (string)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UObject_IsValidImplementation(nint A0);
#else
        private static nint __UObject_IsValidImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte> __UObject_IsValidImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte>)MethodBridge.Resolve(ref __UObject_IsValidImplementation_Slot,
                "Script.Library.UObjectImplementation::UObject_IsValidImplementation");
#endif

        public static unsafe bool UObject_IsValidImplementation(nint InObject)
        {
            return __UObject_IsValidImplementation(InObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UObject_IsAImplementation(nint A0, nint A1);
#else
        private static nint __UObject_IsAImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, byte> __UObject_IsAImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, byte>)MethodBridge.Resolve(ref __UObject_IsAImplementation_Slot,
                "Script.Library.UObjectImplementation::UObject_IsAImplementation");
#endif

        public static unsafe bool UObject_IsAImplementation(nint InObject, nint SomeBase)
        {
            return __UObject_IsAImplementation(InObject, SomeBase) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UObject_AddToRootImplementation(nint A0);
#else
        private static nint __UObject_AddToRootImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __UObject_AddToRootImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(ref __UObject_AddToRootImplementation_Slot,
                "Script.Library.UObjectImplementation::UObject_AddToRootImplementation");
#endif

        public static unsafe void UObject_AddToRootImplementation(nint InObject)
        {
            __UObject_AddToRootImplementation(InObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe void __UObject_RemoveFromRootImplementation(nint A0);
#else
        private static nint __UObject_RemoveFromRootImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, void> __UObject_RemoveFromRootImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(
                ref __UObject_RemoveFromRootImplementation_Slot,
                "Script.Library.UObjectImplementation::UObject_RemoveFromRootImplementation");
#endif

        public static unsafe void UObject_RemoveFromRootImplementation(nint InObject)
        {
            __UObject_RemoveFromRootImplementation(InObject);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UObject_IsRootedImplementation(nint A0);
#else
        private static nint __UObject_IsRootedImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte> __UObject_IsRootedImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte>)MethodBridge.Resolve(ref __UObject_IsRootedImplementation_Slot,
                "Script.Library.UObjectImplementation::UObject_IsRootedImplementation");
#endif

        public static unsafe bool UObject_IsRootedImplementation(nint InObject)
        {
            return __UObject_IsRootedImplementation(InObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UObject_AddReferenceImplementation(nint A0);
#else
        private static nint __UObject_AddReferenceImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte> __UObject_AddReferenceImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte>)MethodBridge.Resolve(ref __UObject_AddReferenceImplementation_Slot,
                "Script.Library.UObjectImplementation::UObject_AddReferenceImplementation");
#endif

        public static unsafe bool UObject_AddReferenceImplementation(nint InObject)
        {
            return __UObject_AddReferenceImplementation(InObject) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UObject_RemoveReferenceImplementation(nint A0);
#else
        private static nint __UObject_RemoveReferenceImplementation_Slot;
        private static unsafe delegate* unmanaged[Cdecl]<nint, byte> __UObject_RemoveReferenceImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte>)MethodBridge.Resolve(
                ref __UObject_RemoveReferenceImplementation_Slot,
                "Script.Library.UObjectImplementation::UObject_RemoveReferenceImplementation");
#endif

        public static unsafe bool UObject_RemoveReferenceImplementation(nint InObject)
        {
            return __UObject_RemoveReferenceImplementation(InObject) != 0;
        }
    }
}