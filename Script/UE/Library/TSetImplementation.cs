using System;
using Script.CoreUObject;
using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static unsafe class TSetImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TSet_RegisterImplementation(nint A0, nint A1);
#else
        private static nint __TSet_RegisterImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, nint, void> __TSet_RegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, void>)MethodBridge.Resolve(ref __TSet_RegisterImplementation_Slot,
                "Script.Library.TSetImplementation::TSet_RegisterImplementation");
#endif

        public static void TSet_RegisterImplementation<T>(TSet<T> InSet, Type InType)
        {
            __TSet_RegisterImplementation(HandleData.Alloc(InSet), HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TSet_UnRegisterImplementation(nint A0);
#else
        private static nint __TSet_UnRegisterImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, void> __TSet_UnRegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(ref __TSet_UnRegisterImplementation_Slot,
                "Script.Library.TSetImplementation::TSet_UnRegisterImplementation");
#endif

        public static void TSet_UnRegisterImplementation(nint InSet)
        {
            __TSet_UnRegisterImplementation(InSet);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TSet_EmptyImplementation(nint A0, int A1);
#else
        private static nint __TSet_EmptyImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, void> __TSet_EmptyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, void>)MethodBridge.Resolve(ref __TSet_EmptyImplementation_Slot,
                "Script.Library.TSetImplementation::TSet_EmptyImplementation");
#endif

        public static void TSet_EmptyImplementation(nint InSet, int InExpectedNumElements)
        {
            __TSet_EmptyImplementation(InSet, InExpectedNumElements);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TSet_NumImplementation(nint A0);
#else
        private static nint __TSet_NumImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int> __TSet_NumImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int>)MethodBridge.Resolve(ref __TSet_NumImplementation_Slot,
                "Script.Library.TSetImplementation::TSet_NumImplementation");
#endif

        public static int TSet_NumImplementation(nint InSet)
        {
            return __TSet_NumImplementation(InSet);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TSet_IsEmptyImplementation(nint A0);
#else
        private static nint __TSet_IsEmptyImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte> __TSet_IsEmptyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte>)MethodBridge.Resolve(ref __TSet_IsEmptyImplementation_Slot,
                "Script.Library.TSetImplementation::TSet_IsEmptyImplementation");
#endif

        public static bool TSet_IsEmptyImplementation(nint InSet)
        {
            return __TSet_IsEmptyImplementation(InSet) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TSet_GetMaxIndexImplementation(nint A0);
#else
        private static nint __TSet_GetMaxIndexImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int> __TSet_GetMaxIndexImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int>)MethodBridge.Resolve(ref __TSet_GetMaxIndexImplementation_Slot,
                "Script.Library.TSetImplementation::TSet_GetMaxIndexImplementation");
#endif

        public static int TSet_GetMaxIndexImplementation(nint InSet)
        {
            return __TSet_GetMaxIndexImplementation(InSet);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TSet_IsValidIndexImplementation(nint A0, int A1);
#else
        private static nint __TSet_IsValidIndexImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, byte> __TSet_IsValidIndexImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, byte>)MethodBridge.Resolve(
                ref __TSet_IsValidIndexImplementation_Slot,
                "Script.Library.TSetImplementation::TSet_IsValidIndexImplementation");
#endif

        public static bool TSet_IsValidIndexImplementation(nint InSet, int InIndex)
        {
            return __TSet_IsValidIndexImplementation(InSet, InIndex) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TSet_AddImplementation(nint A0, byte* A1);
#else
        private static nint __TSet_AddImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, void> __TSet_AddImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, void>)MethodBridge.Resolve(ref __TSet_AddImplementation_Slot,
                "Script.Library.TSetImplementation::TSet_AddImplementation");
#endif

        public static void TSet_AddImplementation(nint InSet, byte* InValueBuffer)
        {
            __TSet_AddImplementation(InSet, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TSet_RemoveImplementation(nint A0, byte* A1);
#else
        private static nint __TSet_RemoveImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TSet_RemoveImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, int>)MethodBridge.Resolve(ref __TSet_RemoveImplementation_Slot,
                "Script.Library.TSetImplementation::TSet_RemoveImplementation");
#endif

        public static int TSet_RemoveImplementation(nint InSet, byte* InValueBuffer)
        {
            return __TSet_RemoveImplementation(InSet, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TSet_ContainsImplementation(nint A0, byte* A1);
#else
        private static nint __TSet_ContainsImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte> __TSet_ContainsImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte>)MethodBridge.Resolve(ref __TSet_ContainsImplementation_Slot,
                "Script.Library.TSetImplementation::TSet_ContainsImplementation");
#endif

        public static bool TSet_ContainsImplementation(nint InSet, byte* InValueBuffer)
        {
            return __TSet_ContainsImplementation(InSet, InValueBuffer) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TSet_GetEnumeratorImplementation(nint A0, int A1, byte* A2);
#else
        private static nint __TSet_GetEnumeratorImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, byte*, void> __TSet_GetEnumeratorImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, byte*, void>)MethodBridge.Resolve(
                ref __TSet_GetEnumeratorImplementation_Slot,
                "Script.Library.TSetImplementation::TSet_GetEnumeratorImplementation");
#endif

        public static void TSet_GetEnumeratorImplementation(nint InSet, int InIndex, byte* ReturnBuffer)
        {
            __TSet_GetEnumeratorImplementation(InSet, InIndex, ReturnBuffer);
        }

        public static T TSet_GetEnumeratorCompoundImplementation<T>(nint InSet, int InIndex)
        {
            var ReturnBuffer = stackalloc byte[sizeof(nint)];

            __TSet_GetEnumeratorImplementation(InSet, InIndex, ReturnBuffer);

            var Handle = *(nint*)ReturnBuffer;

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }
    }
}