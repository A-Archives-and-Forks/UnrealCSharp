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
        private static delegate* unmanaged[Cdecl]<nint, nint, void> __TArray_RegisterImplementation;
#endif

        public static void TArray_RegisterImplementation<T>(TArray<T> InArray, Type InType)
        {
#if !LEANCLR
            if (__TArray_RegisterImplementation == null)
            {
                __TArray_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_RegisterImplementation");
            }
#endif

            __TArray_RegisterImplementation(HandleData.Alloc(InArray), HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TArray_IdenticalImplementation(nint A0, nint A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, nint, byte> __TArray_IdenticalImplementation;
#endif

        public static bool TArray_IdenticalImplementation(nint InA, nint InB)
        {
#if !LEANCLR
            if (__TArray_IdenticalImplementation == null)
            {
                __TArray_IdenticalImplementation = (delegate* unmanaged[Cdecl]<nint, nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_IdenticalImplementation");
            }
#endif

            return __TArray_IdenticalImplementation(InA, InB) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_UnRegisterImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, void> __TArray_UnRegisterImplementation;
#endif

        public static void TArray_UnRegisterImplementation(nint InArray)
        {
#if !LEANCLR
            if (__TArray_UnRegisterImplementation == null)
            {
                __TArray_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_UnRegisterImplementation");
            }
#endif

            __TArray_UnRegisterImplementation(InArray);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_GetTypeSizeImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, int> __TArray_GetTypeSizeImplementation;
#endif

        public static int TArray_GetTypeSizeImplementation(nint InArray)
        {
#if !LEANCLR
            if (__TArray_GetTypeSizeImplementation == null)
            {
                __TArray_GetTypeSizeImplementation = (delegate* unmanaged[Cdecl]<nint, int>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_GetTypeSizeImplementation");
            }
#endif

            return __TArray_GetTypeSizeImplementation(InArray);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_GetSlackImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, int> __TArray_GetSlackImplementation;
#endif

        public static int TArray_GetSlackImplementation(nint InArray)
        {
#if !LEANCLR
            if (__TArray_GetSlackImplementation == null)
            {
                __TArray_GetSlackImplementation = (delegate* unmanaged[Cdecl]<nint, int>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_GetSlackImplementation");
            }
#endif

            return __TArray_GetSlackImplementation(InArray);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TArray_IsValidIndexImplementation(nint A0, int A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, byte> __TArray_IsValidIndexImplementation;
#endif

        public static bool TArray_IsValidIndexImplementation(nint InArray, int InIndex)
        {
#if !LEANCLR
            if (__TArray_IsValidIndexImplementation == null)
            {
                __TArray_IsValidIndexImplementation = (delegate* unmanaged[Cdecl]<nint, int, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_IsValidIndexImplementation");
            }
#endif

            return __TArray_IsValidIndexImplementation(InArray, InIndex) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_NumImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, int> __TArray_NumImplementation;
#endif

        public static int TArray_NumImplementation(nint InArray)
        {
#if !LEANCLR
            if (__TArray_NumImplementation == null)
            {
                __TArray_NumImplementation = (delegate* unmanaged[Cdecl]<nint, int>)
                    MethodBridge.GetMethod("Script.Library.TArrayImplementation::TArray_NumImplementation");
            }
#endif

            return __TArray_NumImplementation(InArray);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TArray_IsEmptyImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte> __TArray_IsEmptyImplementation;
#endif

        public static bool TArray_IsEmptyImplementation(nint InArray)
        {
#if !LEANCLR
            if (__TArray_IsEmptyImplementation == null)
            {
                __TArray_IsEmptyImplementation = (delegate* unmanaged[Cdecl]<nint, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_IsEmptyImplementation");
            }
#endif

            return __TArray_IsEmptyImplementation(InArray) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_MaxImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, int> __TArray_MaxImplementation;
#endif

        public static int TArray_MaxImplementation(nint InArray)
        {
#if !LEANCLR
            if (__TArray_MaxImplementation == null)
            {
                __TArray_MaxImplementation = (delegate* unmanaged[Cdecl]<nint, int>)
                    MethodBridge.GetMethod("Script.Library.TArrayImplementation::TArray_MaxImplementation");
            }
#endif

            return __TArray_MaxImplementation(InArray);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_GetImplementation(nint A0, int A1, byte* A2);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, byte*, void> __TArray_GetImplementation;
#endif

        public static void TArray_GetImplementation(nint InArray, int InIndex, byte* ReturnBuffer)
        {
#if !LEANCLR
            if (__TArray_GetImplementation == null)
            {
                __TArray_GetImplementation = (delegate* unmanaged[Cdecl]<nint, int, byte*, void>)
                    MethodBridge.GetMethod("Script.Library.TArrayImplementation::TArray_GetImplementation");
            }
#endif

            __TArray_GetImplementation(InArray, InIndex, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_SetImplementation(nint A0, int A1, byte* A2);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, byte*, void> __TArray_SetImplementation;
#endif

        public static void TArray_SetImplementation(nint InArray, int InIndex, byte* InValueBuffer)
        {
#if !LEANCLR
            if (__TArray_SetImplementation == null)
            {
                __TArray_SetImplementation = (delegate* unmanaged[Cdecl]<nint, int, byte*, void>)
                    MethodBridge.GetMethod("Script.Library.TArrayImplementation::TArray_SetImplementation");
            }
#endif

            __TArray_SetImplementation(InArray, InIndex, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_FindImplementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TArray_FindImplementation;
#endif

        public static int TArray_FindImplementation(nint InArray, byte* InValueBuffer)
        {
#if !LEANCLR
            if (__TArray_FindImplementation == null)
            {
                __TArray_FindImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, int>)
                    MethodBridge.GetMethod("Script.Library.TArrayImplementation::TArray_FindImplementation");
            }
#endif

            return __TArray_FindImplementation(InArray, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_FindLastImplementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TArray_FindLastImplementation;
#endif

        public static int TArray_FindLastImplementation(nint InArray, byte* InValueBuffer)
        {
#if !LEANCLR
            if (__TArray_FindLastImplementation == null)
            {
                __TArray_FindLastImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, int>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_FindLastImplementation");
            }
#endif

            return __TArray_FindLastImplementation(InArray, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TArray_ContainsImplementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte> __TArray_ContainsImplementation;
#endif

        public static bool TArray_ContainsImplementation(nint InArray, byte* InValueBuffer)
        {
#if !LEANCLR
            if (__TArray_ContainsImplementation == null)
            {
                __TArray_ContainsImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_ContainsImplementation");
            }
#endif

            return __TArray_ContainsImplementation(InArray, InValueBuffer) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_AddUninitializedImplementation(nint A0, int A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, int> __TArray_AddUninitializedImplementation;
#endif

        public static int TArray_AddUninitializedImplementation(nint InArray, int InCount)
        {
#if !LEANCLR
            if (__TArray_AddUninitializedImplementation == null)
            {
                __TArray_AddUninitializedImplementation = (delegate* unmanaged[Cdecl]<nint, int, int>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_AddUninitializedImplementation");
            }
#endif

            return __TArray_AddUninitializedImplementation(InArray, InCount);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_InsertZeroedImplementation(nint A0, int A1, int A2);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, int, void> __TArray_InsertZeroedImplementation;
#endif

        public static void TArray_InsertZeroedImplementation(nint InArray, int InIndex, int InCount)
        {
#if !LEANCLR
            if (__TArray_InsertZeroedImplementation == null)
            {
                __TArray_InsertZeroedImplementation = (delegate* unmanaged[Cdecl]<nint, int, int, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_InsertZeroedImplementation");
            }
#endif

            __TArray_InsertZeroedImplementation(InArray, InIndex, InCount);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_InsertDefaultedImplementation(nint A0, int A1, int A2);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, int, void> __TArray_InsertDefaultedImplementation;
#endif

        public static void TArray_InsertDefaultedImplementation(nint InArray, int InIndex, int InCount)
        {
#if !LEANCLR
            if (__TArray_InsertDefaultedImplementation == null)
            {
                __TArray_InsertDefaultedImplementation = (delegate* unmanaged[Cdecl]<nint, int, int, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_InsertDefaultedImplementation");
            }
#endif

            __TArray_InsertDefaultedImplementation(InArray, InIndex, InCount);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_RemoveAtImplementation(nint A0, int A1, int A2, byte A3);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, int, byte, void> __TArray_RemoveAtImplementation;
#endif

        public static void TArray_RemoveAtImplementation(nint InArray, int InIndex, int InCount,
            bool bAllowShrinking)
        {
#if !LEANCLR
            if (__TArray_RemoveAtImplementation == null)
            {
                __TArray_RemoveAtImplementation = (delegate* unmanaged[Cdecl]<nint, int, int, byte, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_RemoveAtImplementation");
            }
#endif

            __TArray_RemoveAtImplementation(InArray, InIndex, InCount, (byte)(bAllowShrinking ? 1 : 0));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_ResetImplementation(nint A0, int A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, void> __TArray_ResetImplementation;
#endif

        public static void TArray_ResetImplementation(nint InArray, int InNewSize)
        {
#if !LEANCLR
            if (__TArray_ResetImplementation == null)
            {
                __TArray_ResetImplementation = (delegate* unmanaged[Cdecl]<nint, int, void>)
                    MethodBridge.GetMethod("Script.Library.TArrayImplementation::TArray_ResetImplementation");
            }
#endif

            __TArray_ResetImplementation(InArray, InNewSize);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_EmptyImplementation(nint A0, int A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, void> __TArray_EmptyImplementation;
#endif

        public static void TArray_EmptyImplementation(nint InArray, int InSlack)
        {
#if !LEANCLR
            if (__TArray_EmptyImplementation == null)
            {
                __TArray_EmptyImplementation = (delegate* unmanaged[Cdecl]<nint, int, void>)
                    MethodBridge.GetMethod("Script.Library.TArrayImplementation::TArray_EmptyImplementation");
            }
#endif

            __TArray_EmptyImplementation(InArray, InSlack);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_SetNumImplementation(nint A0, int A1, byte A2);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, byte, void> __TArray_SetNumImplementation;
#endif

        public static void TArray_SetNumImplementation(nint InArray, int InNewNum, bool bAllowShrinking)
        {
#if !LEANCLR
            if (__TArray_SetNumImplementation == null)
            {
                __TArray_SetNumImplementation = (delegate* unmanaged[Cdecl]<nint, int, byte, void>)
                    MethodBridge.GetMethod("Script.Library.TArrayImplementation::TArray_SetNumImplementation");
            }
#endif

            __TArray_SetNumImplementation(InArray, InNewNum, (byte)(bAllowShrinking ? 1 : 0));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_AddImplementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TArray_AddImplementation;
#endif

        public static int TArray_AddImplementation(nint InArray, byte* InValueBuffer)
        {
#if !LEANCLR
            if (__TArray_AddImplementation == null)
            {
                __TArray_AddImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, int>)
                    MethodBridge.GetMethod("Script.Library.TArrayImplementation::TArray_AddImplementation");
            }
#endif

            return __TArray_AddImplementation(InArray, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_AddZeroedImplementation(nint A0, int A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, int> __TArray_AddZeroedImplementation;
#endif

        public static int TArray_AddZeroedImplementation(nint InArray, int InCount)
        {
#if !LEANCLR
            if (__TArray_AddZeroedImplementation == null)
            {
                __TArray_AddZeroedImplementation = (delegate* unmanaged[Cdecl]<nint, int, int>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_AddZeroedImplementation");
            }
#endif

            return __TArray_AddZeroedImplementation(InArray, InCount);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_AddUniqueImplementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TArray_AddUniqueImplementation;
#endif

        public static int TArray_AddUniqueImplementation(nint InArray, byte* InValueBuffer)
        {
#if !LEANCLR
            if (__TArray_AddUniqueImplementation == null)
            {
                __TArray_AddUniqueImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, int>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_AddUniqueImplementation");
            }
#endif

            return __TArray_AddUniqueImplementation(InArray, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_RemoveSingleImplementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TArray_RemoveSingleImplementation;
#endif

        public static int TArray_RemoveSingleImplementation(nint InArray, byte* InValueBuffer)
        {
#if !LEANCLR
            if (__TArray_RemoveSingleImplementation == null)
            {
                __TArray_RemoveSingleImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, int>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_RemoveSingleImplementation");
            }
#endif

            return __TArray_RemoveSingleImplementation(InArray, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_RemoveImplementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TArray_RemoveImplementation;
#endif

        public static int TArray_RemoveImplementation(nint InArray, byte* InValueBuffer)
        {
#if !LEANCLR
            if (__TArray_RemoveImplementation == null)
            {
                __TArray_RemoveImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, int>)
                    MethodBridge.GetMethod("Script.Library.TArrayImplementation::TArray_RemoveImplementation");
            }
#endif

            return __TArray_RemoveImplementation(InArray, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_SwapMemoryImplementation(nint A0, int A1, int A2);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, int, void> __TArray_SwapMemoryImplementation;
#endif

        public static void TArray_SwapMemoryImplementation(nint InArray, int InFirstIndexToSwap,
            int InSecondIndexToSwap)
        {
#if !LEANCLR
            if (__TArray_SwapMemoryImplementation == null)
            {
                __TArray_SwapMemoryImplementation = (delegate* unmanaged[Cdecl]<nint, int, int, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_SwapMemoryImplementation");
            }
#endif

            __TArray_SwapMemoryImplementation(InArray, InFirstIndexToSwap, InSecondIndexToSwap);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TArray_SwapImplementation(nint A0, int A1, int A2);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, int, void> __TArray_SwapImplementation;
#endif

        public static void TArray_SwapImplementation(nint InArray, int InFirstIndexToSwap,
            int InSecondIndexToSwap)
        {
#if !LEANCLR
            if (__TArray_SwapImplementation == null)
            {
                __TArray_SwapImplementation = (delegate* unmanaged[Cdecl]<nint, int, int, void>)
                    MethodBridge.GetMethod("Script.Library.TArrayImplementation::TArray_SwapImplementation");
            }
#endif

            __TArray_SwapImplementation(InArray, InFirstIndexToSwap, InSecondIndexToSwap);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TArray_INDEX_NONEImplementation();
#else
        private static delegate* unmanaged[Cdecl]<int> __TArray_INDEX_NONEImplementation;
#endif

        public static int TArray_INDEX_NONEImplementation()
        {
#if !LEANCLR
            if (__TArray_INDEX_NONEImplementation == null)
            {
                __TArray_INDEX_NONEImplementation = (delegate* unmanaged[Cdecl]<int>)
                    MethodBridge.GetMethod(
                        "Script.Library.TArrayImplementation::TArray_INDEX_NONEImplementation");
            }
#endif

            return __TArray_INDEX_NONEImplementation();
        }

        public static T TArray_GetCompoundImplementation<T>(nint InArray, int InIndex)
        {
#if !LEANCLR
            if (__TArray_GetImplementation == null)
            {
                __TArray_GetImplementation = (delegate* unmanaged[Cdecl]<nint, int, byte*, void>)
                    MethodBridge.GetMethod("Script.Library.TArrayImplementation::TArray_GetImplementation");
            }
#endif

            var ValueBuffer = stackalloc byte[sizeof(nint)];

            __TArray_GetImplementation(InArray, InIndex, ValueBuffer);

            var Handle = *(nint*)ValueBuffer;

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }
    }
}