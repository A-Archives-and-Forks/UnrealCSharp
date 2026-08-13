using System;
using Script.CoreUObject;
using Interop;
#if LEANCLR
using System.Runtime.InteropServices;
#endif

namespace Script.Library
{
    public static unsafe class TMapImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TMap_RegisterImplementation(nint A0, nint A1);
#else
        private static nint __TMap_RegisterImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, nint, void> __TMap_RegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, nint, void>)MethodBridge.Resolve(ref __TMap_RegisterImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_RegisterImplementation");
#endif

        public static void TMap_RegisterImplementation<TKey, TValue>(TMap<TKey, TValue> InMap, Type InType)
        {
            __TMap_RegisterImplementation(HandleData.Alloc(InMap), HandleData.Alloc(InType));
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TMap_UnRegisterImplementation(nint A0);
#else
        private static nint __TMap_UnRegisterImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, void> __TMap_UnRegisterImplementation =>
            (delegate* unmanaged[Cdecl]<nint, void>)MethodBridge.Resolve(ref __TMap_UnRegisterImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_UnRegisterImplementation");
#endif

        public static void TMap_UnRegisterImplementation(nint InMap)
        {
            __TMap_UnRegisterImplementation(InMap);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TMap_EmptyImplementation(nint A0, int A1);
#else
        private static nint __TMap_EmptyImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, void> __TMap_EmptyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, void>)MethodBridge.Resolve(ref __TMap_EmptyImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_EmptyImplementation");
#endif

        public static void TMap_EmptyImplementation(nint InMap, int InExpectedNumElements)
        {
            __TMap_EmptyImplementation(InMap, InExpectedNumElements);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TMap_NumImplementation(nint A0);
#else
        private static nint __TMap_NumImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int> __TMap_NumImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int>)MethodBridge.Resolve(ref __TMap_NumImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_NumImplementation");
#endif

        public static int TMap_NumImplementation(nint InMap)
        {
            return __TMap_NumImplementation(InMap);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TMap_IsEmptyImplementation(nint A0);
#else
        private static nint __TMap_IsEmptyImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte> __TMap_IsEmptyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte>)MethodBridge.Resolve(ref __TMap_IsEmptyImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_IsEmptyImplementation");
#endif

        public static bool TMap_IsEmptyImplementation(nint InMap)
        {
            return __TMap_IsEmptyImplementation(InMap) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TMap_GetMaxIndexImplementation(nint A0);
#else
        private static nint __TMap_GetMaxIndexImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int> __TMap_GetMaxIndexImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int>)MethodBridge.Resolve(ref __TMap_GetMaxIndexImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_GetMaxIndexImplementation");
#endif

        public static int TMap_GetMaxIndexImplementation(nint InMap)
        {
            return __TMap_GetMaxIndexImplementation(InMap);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TMap_IsValidIndexImplementation(nint A0, int A1);
#else
        private static nint __TMap_IsValidIndexImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, byte> __TMap_IsValidIndexImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, byte>)MethodBridge.Resolve(
                ref __TMap_IsValidIndexImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_IsValidIndexImplementation");
#endif

        public static bool TMap_IsValidIndexImplementation(nint InMap, int InIndex)
        {
            return __TMap_IsValidIndexImplementation(InMap, InIndex) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TMap_AddImplementation(nint A0, byte* A1, byte* A2);
#else
        private static nint __TMap_AddImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, void> __TMap_AddImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __TMap_AddImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_AddImplementation");
#endif

        public static void TMap_AddImplementation(nint InMap, byte* InKeyBuffer, byte* InValueBuffer)
        {
            __TMap_AddImplementation(InMap, InKeyBuffer, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern int __TMap_RemoveImplementation(nint A0, byte* A1);
#else
        private static nint __TMap_RemoveImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, int> __TMap_RemoveImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, int>)MethodBridge.Resolve(ref __TMap_RemoveImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_RemoveImplementation");
#endif

        public static int TMap_RemoveImplementation(nint InMap, byte* InKeyBuffer)
        {
            return __TMap_RemoveImplementation(InMap, InKeyBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte __TMap_ContainsImplementation(nint A0, byte* A1);
#else
        private static nint __TMap_ContainsImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte> __TMap_ContainsImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte>)MethodBridge.Resolve(ref __TMap_ContainsImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_ContainsImplementation");
#endif

        public static bool TMap_ContainsImplementation(nint InMap, byte* InKeyBuffer)
        {
            return __TMap_ContainsImplementation(InMap, InKeyBuffer) != 0;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TMap_FindKeyImplementation(nint A0, byte* A1, byte* A2);
#else
        private static nint __TMap_FindKeyImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, void> __TMap_FindKeyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __TMap_FindKeyImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_FindKeyImplementation");
#endif

        public static void TMap_FindKeyImplementation(nint InMap, byte* InValueBuffer, byte* ReturnBuffer)
        {
            __TMap_FindKeyImplementation(InMap, InValueBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TMap_FindImplementation(nint A0, byte* A1, byte* A2);
#else
        private static nint __TMap_FindImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, void> __TMap_FindImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __TMap_FindImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_FindImplementation");
#endif

        public static void TMap_FindImplementation(nint InMap, byte* InKeyBuffer, byte* ReturnBuffer)
        {
            __TMap_FindImplementation(InMap, InKeyBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TMap_GetImplementation(nint A0, byte* A1, byte* A2);
#else
        private static nint __TMap_GetImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, void> __TMap_GetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __TMap_GetImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_GetImplementation");
#endif

        public static void TMap_GetImplementation(nint InMap, byte* InKeyBuffer, byte* ReturnBuffer)
        {
            __TMap_GetImplementation(InMap, InKeyBuffer, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TMap_SetImplementation(nint A0, byte* A1, byte* A2);
#else
        private static nint __TMap_SetImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, byte*, byte*, void> __TMap_SetImplementation =>
            (delegate* unmanaged[Cdecl]<nint, byte*, byte*, void>)MethodBridge.Resolve(
                ref __TMap_SetImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_SetImplementation");
#endif

        public static void TMap_SetImplementation(nint InMap, byte* InKeyBuffer, byte* InValueBuffer)
        {
            __TMap_SetImplementation(InMap, InKeyBuffer, InValueBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TMap_GetEnumeratorKeyImplementation(nint A0, int A1, byte* A2);
#else
        private static nint __TMap_GetEnumeratorKeyImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, byte*, void> __TMap_GetEnumeratorKeyImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, byte*, void>)MethodBridge.Resolve(
                ref __TMap_GetEnumeratorKeyImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_GetEnumeratorKeyImplementation");
#endif

        public static void TMap_GetEnumeratorKeyImplementation(nint InMap, int InIndex, byte* ReturnBuffer)
        {
            __TMap_GetEnumeratorKeyImplementation(InMap, InIndex, ReturnBuffer);
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern void __TMap_GetEnumeratorValueImplementation(nint A0, int A1, byte* A2);
#else
        private static nint __TMap_GetEnumeratorValueImplementation_Slot;
        private static delegate* unmanaged[Cdecl]<nint, int, byte*, void> __TMap_GetEnumeratorValueImplementation =>
            (delegate* unmanaged[Cdecl]<nint, int, byte*, void>)MethodBridge.Resolve(
                ref __TMap_GetEnumeratorValueImplementation_Slot,
                "Script.Library.TMapImplementation::TMap_GetEnumeratorValueImplementation");
#endif

        public static void TMap_GetEnumeratorValueImplementation(nint InMap, int InIndex, byte* ReturnBuffer)
        {
            __TMap_GetEnumeratorValueImplementation(InMap, InIndex, ReturnBuffer);
        }

        public static TKey TMap_FindKeyCompoundImplementation<TKey>(nint InMap, byte* InValueBuffer)
        {
            var ReturnBuffer = stackalloc byte[sizeof(nint)];

            __TMap_FindKeyImplementation(InMap, InValueBuffer, ReturnBuffer);

            var Handle = *(nint*)ReturnBuffer;

            return Handle != 0 ? (TKey)HandleData.GetObject(Handle) : default;
        }

        public static TValue TMap_FindCompoundImplementation<TValue>(nint InMap, byte* InKeyBuffer)
        {
            var ReturnBuffer = stackalloc byte[sizeof(nint)];

            __TMap_FindImplementation(InMap, InKeyBuffer, ReturnBuffer);

            var Handle = *(nint*)ReturnBuffer;

            return Handle != 0 ? (TValue)HandleData.GetObject(Handle) : default;
        }

        public static TValue TMap_GetCompoundImplementation<TValue>(nint InMap, byte* InKeyBuffer)
        {
            var ReturnBuffer = stackalloc byte[sizeof(nint)];

            __TMap_GetImplementation(InMap, InKeyBuffer, ReturnBuffer);

            var Handle = *(nint*)ReturnBuffer;

            return Handle != 0 ? (TValue)HandleData.GetObject(Handle) : default;
        }

        public static TKey TMap_GetEnumeratorKeyCompoundImplementation<TKey>(nint InMap, int InIndex)
        {
            var ReturnBuffer = stackalloc byte[sizeof(nint)];

            __TMap_GetEnumeratorKeyImplementation(InMap, InIndex, ReturnBuffer);

            var Handle = *(nint*)ReturnBuffer;

            return Handle != 0 ? (TKey)HandleData.GetObject(Handle) : default;
        }

        public static TValue TMap_GetEnumeratorValueCompoundImplementation<TValue>(nint InMap, int InIndex)
        {
            var ReturnBuffer = stackalloc byte[sizeof(nint)];

            __TMap_GetEnumeratorValueImplementation(InMap, InIndex, ReturnBuffer);

            var Handle = *(nint*)ReturnBuffer;

            return Handle != 0 ? (TValue)HandleData.GetObject(Handle) : default;
        }
    }
}