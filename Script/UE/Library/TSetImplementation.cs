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
        private static delegate* unmanaged[Cdecl]<nint, nint, void> __TSet_RegisterImplementation;
#endif

        public static void TSet_RegisterImplementation<T>(TSet<T> InSet, Type InType)
        {
#if !LEANCLR
            if (__TSet_RegisterImplementation == null)
            {
                __TSet_RegisterImplementation = (delegate* unmanaged[Cdecl]<nint, nint, void>)
                    MethodBridge.GetMethod("Script.Library.TSetImplementation::TSet_RegisterImplementation");
            }
#endif

            __TSet_RegisterImplementation(HandleData.Alloc(InSet), HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TSet_UnRegisterImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, void> __TSet_UnRegisterImplementation;
#endif

        public static void TSet_UnRegisterImplementation(nint InSet)
        {
#if !LEANCLR
            if (__TSet_UnRegisterImplementation == null)
            {
                __TSet_UnRegisterImplementation = (delegate* unmanaged[Cdecl]<nint, void>)
                    MethodBridge.GetMethod("Script.Library.TSetImplementation::TSet_UnRegisterImplementation");
            }
#endif

            __TSet_UnRegisterImplementation(InSet);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TSet_EmptyImplementation(nint A0, int A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, void> __TSet_EmptyImplementation;
#endif

        public static void TSet_EmptyImplementation(nint InSet, int InExpectedNumElements)
        {
#if !LEANCLR
            if (__TSet_EmptyImplementation == null)
            {
                __TSet_EmptyImplementation = (delegate* unmanaged[Cdecl]<nint, int, void>)
                    MethodBridge.GetMethod("Script.Library.TSetImplementation::TSet_EmptyImplementation");
            }
#endif

            __TSet_EmptyImplementation(InSet, InExpectedNumElements);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TSet_NumImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, int> __TSet_NumImplementation;
#endif

        public static int TSet_NumImplementation(nint InSet)
        {
#if !LEANCLR
            if (__TSet_NumImplementation == null)
            {
                __TSet_NumImplementation = (delegate* unmanaged[Cdecl]<nint, int>)
                    MethodBridge.GetMethod("Script.Library.TSetImplementation::TSet_NumImplementation");
            }
#endif

            return __TSet_NumImplementation(InSet);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TSet_IsEmptyImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte> __TSet_IsEmptyImplementation;
#endif

        public static bool TSet_IsEmptyImplementation(nint InSet)
        {
#if !LEANCLR
            if (__TSet_IsEmptyImplementation == null)
            {
                __TSet_IsEmptyImplementation = (delegate* unmanaged[Cdecl]<nint, byte>)
                    MethodBridge.GetMethod("Script.Library.TSetImplementation::TSet_IsEmptyImplementation");
            }
#endif

            return __TSet_IsEmptyImplementation(InSet) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TSet_GetMaxIndexImplementation(nint A0);
#else
        private static delegate* unmanaged[Cdecl]<nint, int> __TSet_GetMaxIndexImplementation;
#endif

        public static int TSet_GetMaxIndexImplementation(nint InSet)
        {
#if !LEANCLR
            if (__TSet_GetMaxIndexImplementation == null)
            {
                __TSet_GetMaxIndexImplementation = (delegate* unmanaged[Cdecl]<nint, int>)
                    MethodBridge.GetMethod(
                        "Script.Library.TSetImplementation::TSet_GetMaxIndexImplementation");
            }
#endif

            return __TSet_GetMaxIndexImplementation(InSet);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TSet_IsValidIndexImplementation(nint A0, int A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, byte> __TSet_IsValidIndexImplementation;
#endif

        public static bool TSet_IsValidIndexImplementation(nint InSet, int InIndex)
        {
#if !LEANCLR
            if (__TSet_IsValidIndexImplementation == null)
            {
                __TSet_IsValidIndexImplementation = (delegate* unmanaged[Cdecl]<nint, int, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.TSetImplementation::TSet_IsValidIndexImplementation");
            }
#endif

            return __TSet_IsValidIndexImplementation(InSet, InIndex) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TSet_AddImplementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, void> __TSet_AddImplementation;
#endif

        public static void TSet_AddImplementation(nint InSet, byte* InValueBuffer)
        {
#if !LEANCLR
            if (__TSet_AddImplementation == null)
            {
                __TSet_AddImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, void>)
                    MethodBridge.GetMethod("Script.Library.TSetImplementation::TSet_AddImplementation");
            }
#endif

            __TSet_AddImplementation(InSet, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TSet_RemoveImplementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TSet_RemoveImplementation;
#endif

        public static int TSet_RemoveImplementation(nint InSet, byte* InValueBuffer)
        {
#if !LEANCLR
            if (__TSet_RemoveImplementation == null)
            {
                __TSet_RemoveImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, int>)
                    MethodBridge.GetMethod("Script.Library.TSetImplementation::TSet_RemoveImplementation");
            }
#endif

            return __TSet_RemoveImplementation(InSet, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TSet_ContainsImplementation(nint A0, byte* A1);
#else
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte> __TSet_ContainsImplementation;
#endif

        public static bool TSet_ContainsImplementation(nint InSet, byte* InValueBuffer)
        {
#if !LEANCLR
            if (__TSet_ContainsImplementation == null)
            {
                __TSet_ContainsImplementation = (delegate* unmanaged[Cdecl]<nint, byte*, byte>)
                    MethodBridge.GetMethod("Script.Library.TSetImplementation::TSet_ContainsImplementation");
            }
#endif

            return __TSet_ContainsImplementation(InSet, InValueBuffer) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TSet_GetEnumeratorImplementation(nint A0, int A1, byte* A2);
#else
        private static delegate* unmanaged[Cdecl]<nint, int, byte*, void> __TSet_GetEnumeratorImplementation;
#endif

        public static void TSet_GetEnumeratorImplementation(nint InSet, int InIndex, byte* ReturnBuffer)
        {
#if !LEANCLR
            if (__TSet_GetEnumeratorImplementation == null)
            {
                __TSet_GetEnumeratorImplementation = (delegate* unmanaged[Cdecl]<nint, int, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TSetImplementation::TSet_GetEnumeratorImplementation");
            }
#endif

            __TSet_GetEnumeratorImplementation(InSet, InIndex, ReturnBuffer);
        }

        public static T TSet_GetEnumeratorCompoundImplementation<T>(nint InSet, int InIndex)
        {
#if !LEANCLR
            if (__TSet_GetEnumeratorImplementation == null)
            {
                __TSet_GetEnumeratorImplementation = (delegate* unmanaged[Cdecl]<nint, int, byte*, void>)
                    MethodBridge.GetMethod(
                        "Script.Library.TSetImplementation::TSet_GetEnumeratorImplementation");
            }
#endif

            var ReturnBuffer = stackalloc byte[sizeof(nint)];

            __TSet_GetEnumeratorImplementation(InSet, InIndex, ReturnBuffer);

            var Handle = *(nint*)ReturnBuffer;

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }
    }
}