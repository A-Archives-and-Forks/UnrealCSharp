using System;
using Script.CoreUObject;
using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static unsafe class TArrayImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_RegisterImplementation(nint A0, nint A1);
#else
        private static nint __TArray_RegisterImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, nint, void> __TArray_RegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, void>)MethodBridge.Resolve(ref __TArray_RegisterImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_RegisterImplementation");
#endif

        public static void TArray_RegisterImplementation<T>(TArray<T> InArray, Type InType)
        {
            __TArray_RegisterImplementation(HandleData.Alloc(InArray), HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TArray_IdenticalImplementation(nint A0, nint A1);
#else
        private static nint __TArray_IdenticalImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, nint, byte> __TArray_IdenticalImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, byte>)MethodBridge.Resolve(
                ref __TArray_IdenticalImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_IdenticalImplementation");
#endif

        public static bool TArray_IdenticalImplementation(nint InA, nint InB)
        {
            return __TArray_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_UnRegisterImplementation(nint A0);
#else
        private static nint __TArray_UnRegisterImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, void> __TArray_UnRegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(ref __TArray_UnRegisterImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_UnRegisterImplementation");
#endif

        public static void TArray_UnRegisterImplementation(nint InArray)
        {
            __TArray_UnRegisterImplementation(InArray);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_GetTypeSizeImplementation(nint A0);
#else
        private static nint __TArray_GetTypeSizeImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int> __TArray_GetTypeSizeImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int>)MethodBridge.Resolve(ref __TArray_GetTypeSizeImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_GetTypeSizeImplementation");
#endif

        public static int TArray_GetTypeSizeImplementation(nint InArray)
        {
            return __TArray_GetTypeSizeImplementation(InArray);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_GetSlackImplementation(nint A0);
#else
        private static nint __TArray_GetSlackImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int> __TArray_GetSlackImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int>)MethodBridge.Resolve(ref __TArray_GetSlackImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_GetSlackImplementation");
#endif

        public static int TArray_GetSlackImplementation(nint InArray)
        {
            return __TArray_GetSlackImplementation(InArray);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TArray_IsValidIndexImplementation(nint A0, int A1);
#else
        private static nint __TArray_IsValidIndexImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, byte> __TArray_IsValidIndexImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, byte>)MethodBridge.Resolve(
                ref __TArray_IsValidIndexImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_IsValidIndexImplementation");
#endif

        public static bool TArray_IsValidIndexImplementation(nint InArray, int InIndex)
        {
            return __TArray_IsValidIndexImplementation(InArray, InIndex) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_NumImplementation(nint A0);
#else
        private static nint __TArray_NumImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int> __TArray_NumImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int>)MethodBridge.Resolve(ref __TArray_NumImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_NumImplementation");
#endif

        public static int TArray_NumImplementation(nint InArray)
        {
            return __TArray_NumImplementation(InArray);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TArray_IsEmptyImplementation(nint A0);
#else
        private static nint __TArray_IsEmptyImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte> __TArray_IsEmptyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte>)MethodBridge.Resolve(ref __TArray_IsEmptyImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_IsEmptyImplementation");
#endif

        public static bool TArray_IsEmptyImplementation(nint InArray)
        {
            return __TArray_IsEmptyImplementation(InArray) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_MaxImplementation(nint A0);
#else
        private static nint __TArray_MaxImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int> __TArray_MaxImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int>)MethodBridge.Resolve(ref __TArray_MaxImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_MaxImplementation");
#endif

        public static int TArray_MaxImplementation(nint InArray)
        {
            return __TArray_MaxImplementation(InArray);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_GetImplementation(nint A0, int A1, byte* A2);
#else
        private static nint __TArray_GetImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, byte*, void> __TArray_GetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, byte*, void>)MethodBridge.Resolve(
                ref __TArray_GetImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_GetImplementation");
#endif

        public static void TArray_GetImplementation(nint InArray, int InIndex, byte* ReturnBuffer)
        {
            __TArray_GetImplementation(InArray, InIndex, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_SetImplementation(nint A0, int A1, byte* A2);
#else
        private static nint __TArray_SetImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, byte*, void> __TArray_SetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, byte*, void>)MethodBridge.Resolve(
                ref __TArray_SetImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_SetImplementation");
#endif

        public static void TArray_SetImplementation(nint InArray, int InIndex, byte* InValueBuffer)
        {
            __TArray_SetImplementation(InArray, InIndex, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_FindImplementation(nint A0, byte* A1);
#else
        private static nint __TArray_FindImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TArray_FindImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, int>)MethodBridge.Resolve(ref __TArray_FindImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_FindImplementation");
#endif

        public static int TArray_FindImplementation(nint InArray, byte* InValueBuffer)
        {
            return __TArray_FindImplementation(InArray, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_FindLastImplementation(nint A0, byte* A1);
#else
        private static nint __TArray_FindLastImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TArray_FindLastImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, int>)MethodBridge.Resolve(ref __TArray_FindLastImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_FindLastImplementation");
#endif

        public static int TArray_FindLastImplementation(nint InArray, byte* InValueBuffer)
        {
            return __TArray_FindLastImplementation(InArray, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TArray_ContainsImplementation(nint A0, byte* A1);
#else
        private static nint __TArray_ContainsImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte> __TArray_ContainsImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte>)MethodBridge.Resolve(
                ref __TArray_ContainsImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_ContainsImplementation");
#endif

        public static bool TArray_ContainsImplementation(nint InArray, byte* InValueBuffer)
        {
            return __TArray_ContainsImplementation(InArray, InValueBuffer) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_AddUninitializedImplementation(nint A0, int A1);
#else
        private static nint __TArray_AddUninitializedImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, int> __TArray_AddUninitializedImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, int>)MethodBridge.Resolve(
                ref __TArray_AddUninitializedImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_AddUninitializedImplementation");
#endif

        public static int TArray_AddUninitializedImplementation(nint InArray, int InCount)
        {
            return __TArray_AddUninitializedImplementation(InArray, InCount);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_InsertZeroedImplementation(nint A0, int A1, int A2);
#else
        private static nint __TArray_InsertZeroedImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, int, void> __TArray_InsertZeroedImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, int, void>)MethodBridge.Resolve(
                ref __TArray_InsertZeroedImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_InsertZeroedImplementation");
#endif

        public static void TArray_InsertZeroedImplementation(nint InArray, int InIndex, int InCount)
        {
            __TArray_InsertZeroedImplementation(InArray, InIndex, InCount);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_InsertDefaultedImplementation(nint A0, int A1, int A2);
#else
        private static nint __TArray_InsertDefaultedImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, int, void> __TArray_InsertDefaultedImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, int, void>)MethodBridge.Resolve(
                ref __TArray_InsertDefaultedImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_InsertDefaultedImplementation");
#endif

        public static void TArray_InsertDefaultedImplementation(nint InArray, int InIndex, int InCount)
        {
            __TArray_InsertDefaultedImplementation(InArray, InIndex, InCount);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_RemoveAtImplementation(nint A0, int A1, int A2, byte A3);
#else
        private static nint __TArray_RemoveAtImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, int, byte, void> __TArray_RemoveAtImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, int, byte, void>)MethodBridge.Resolve(
                ref __TArray_RemoveAtImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_RemoveAtImplementation");
#endif

        public static void TArray_RemoveAtImplementation(nint InArray, int InIndex, int InCount,
            bool bAllowShrinking)
        {
            __TArray_RemoveAtImplementation(InArray, InIndex, InCount, (byte)(bAllowShrinking ? 1 : 0));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_ResetImplementation(nint A0, int A1);
#else
        private static nint __TArray_ResetImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, void> __TArray_ResetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, void>)MethodBridge.Resolve(ref __TArray_ResetImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_ResetImplementation");
#endif

        public static void TArray_ResetImplementation(nint InArray, int InNewSize)
        {
            __TArray_ResetImplementation(InArray, InNewSize);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_EmptyImplementation(nint A0, int A1);
#else
        private static nint __TArray_EmptyImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, void> __TArray_EmptyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, void>)MethodBridge.Resolve(ref __TArray_EmptyImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_EmptyImplementation");
#endif

        public static void TArray_EmptyImplementation(nint InArray, int InSlack)
        {
            __TArray_EmptyImplementation(InArray, InSlack);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_SetNumImplementation(nint A0, int A1, byte A2);
#else
        private static nint __TArray_SetNumImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, byte, void> __TArray_SetNumImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, byte, void>)MethodBridge.Resolve(
                ref __TArray_SetNumImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_SetNumImplementation");
#endif

        public static void TArray_SetNumImplementation(nint InArray, int InNewNum, bool bAllowShrinking)
        {
            __TArray_SetNumImplementation(InArray, InNewNum, (byte)(bAllowShrinking ? 1 : 0));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_AddImplementation(nint A0, byte* A1);
#else
        private static nint __TArray_AddImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TArray_AddImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, int>)MethodBridge.Resolve(ref __TArray_AddImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_AddImplementation");
#endif

        public static int TArray_AddImplementation(nint InArray, byte* InValueBuffer)
        {
            return __TArray_AddImplementation(InArray, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_AddZeroedImplementation(nint A0, int A1);
#else
        private static nint __TArray_AddZeroedImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, int> __TArray_AddZeroedImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, int>)MethodBridge.Resolve(ref __TArray_AddZeroedImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_AddZeroedImplementation");
#endif

        public static int TArray_AddZeroedImplementation(nint InArray, int InCount)
        {
            return __TArray_AddZeroedImplementation(InArray, InCount);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_AddUniqueImplementation(nint A0, byte* A1);
#else
        private static nint __TArray_AddUniqueImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TArray_AddUniqueImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, int>)MethodBridge.Resolve(
                ref __TArray_AddUniqueImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_AddUniqueImplementation");
#endif

        public static int TArray_AddUniqueImplementation(nint InArray, byte* InValueBuffer)
        {
            return __TArray_AddUniqueImplementation(InArray, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_RemoveSingleImplementation(nint A0, byte* A1);
#else
        private static nint __TArray_RemoveSingleImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TArray_RemoveSingleImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, int>)MethodBridge.Resolve(
                ref __TArray_RemoveSingleImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_RemoveSingleImplementation");
#endif

        public static int TArray_RemoveSingleImplementation(nint InArray, byte* InValueBuffer)
        {
            return __TArray_RemoveSingleImplementation(InArray, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_RemoveImplementation(nint A0, byte* A1);
#else
        private static nint __TArray_RemoveImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TArray_RemoveImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, int>)MethodBridge.Resolve(ref __TArray_RemoveImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_RemoveImplementation");
#endif

        public static int TArray_RemoveImplementation(nint InArray, byte* InValueBuffer)
        {
            return __TArray_RemoveImplementation(InArray, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_SwapMemoryImplementation(nint A0, int A1, int A2);
#else
        private static nint __TArray_SwapMemoryImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, int, void> __TArray_SwapMemoryImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, int, void>)MethodBridge.Resolve(
                ref __TArray_SwapMemoryImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_SwapMemoryImplementation");
#endif

        public static void TArray_SwapMemoryImplementation(nint InArray, int InFirstIndexToSwap,
            int InSecondIndexToSwap)
        {
            __TArray_SwapMemoryImplementation(InArray, InFirstIndexToSwap, InSecondIndexToSwap);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_SwapImplementation(nint A0, int A1, int A2);
#else
        private static nint __TArray_SwapImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, int, void> __TArray_SwapImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, int, void>)MethodBridge.Resolve(ref __TArray_SwapImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_SwapImplementation");
#endif

        public static void TArray_SwapImplementation(nint InArray, int InFirstIndexToSwap,
            int InSecondIndexToSwap)
        {
            __TArray_SwapImplementation(InArray, InFirstIndexToSwap, InSecondIndexToSwap);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_INDEX_NONEImplementation();
#else
        private static nint __TArray_INDEX_NONEImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<int> __TArray_INDEX_NONEImplementation =>
            (delegate* unmanaged[Cdecl]<int>)MethodBridge.Resolve(ref __TArray_INDEX_NONEImplementation_Slot,
                "Script.Library.TArrayImplementation::TArray_INDEX_NONEImplementation");
#endif

        public static int TArray_INDEX_NONEImplementation()
        {
            return __TArray_INDEX_NONEImplementation();
        }

        public static T TArray_GetCompoundImplementation<T>(nint InArray, int InIndex)
        {
            var ValueBuffer = stackalloc byte[sizeof(nint)];

            __TArray_GetImplementation(InArray, InIndex, ValueBuffer);

            var Handle = *(nint*)ValueBuffer;

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }
    }
}