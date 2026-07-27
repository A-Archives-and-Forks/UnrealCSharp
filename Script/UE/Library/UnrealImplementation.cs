using Script.CoreUObject;
using Script.Engine;
using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static class UnrealImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __Unreal_NewObjectImplementation(nint A0, nint A1, nint A2, EObjectFlags A3, nint A4, byte A5);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, EObjectFlags, nint, byte, nint>
            __Unreal_NewObjectImplementation;
#endif

        public static unsafe T Unreal_NewObjectImplementation<T>(nint Outer, nint Class, nint Name, EObjectFlags Flags,
            nint Template, bool bCopyTransientsFromClassDefaults)
        {
#if !LEANCLR
            if (__Unreal_NewObjectImplementation == null)
            {
                __Unreal_NewObjectImplementation =
                    (delegate* unmanaged[Cdecl]<nint, nint, nint, EObjectFlags, nint, byte, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.UnrealImplementation::Unreal_NewObjectImplementation");
            }
#endif

            var Handle = __Unreal_NewObjectImplementation(Outer, Class, Name, Flags, Template,
                (byte)(bCopyTransientsFromClassDefaults ? 1 : 0));

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __Unreal_DuplicateObjectImplementation(nint A0, nint A1, nint A2);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, nint> __Unreal_DuplicateObjectImplementation;
#endif

        public static unsafe T Unreal_DuplicateObjectImplementation<T>(nint SourceObject, nint Outer, nint Name)
        {
#if !LEANCLR
            if (__Unreal_DuplicateObjectImplementation == null)
            {
                __Unreal_DuplicateObjectImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.UnrealImplementation::Unreal_DuplicateObjectImplementation");
            }
#endif

            var Handle = __Unreal_DuplicateObjectImplementation(SourceObject, Outer, Name);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __Unreal_LoadObjectImplementation(nint A0, nint A1, nint A2, ELoadFlags A3, nint A4);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, ELoadFlags, nint, nint>
            __Unreal_LoadObjectImplementation;
#endif

        public static unsafe T Unreal_LoadObjectImplementation<T>(nint Outer, nint Name, nint Filename,
            ELoadFlags LoadFlags, nint Sandbox)
        {
#if !LEANCLR
            if (__Unreal_LoadObjectImplementation == null)
            {
                __Unreal_LoadObjectImplementation =
                    (delegate* unmanaged[Cdecl]<nint, nint, nint, ELoadFlags, nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.UnrealImplementation::Unreal_LoadObjectImplementation");
            }
#endif

            var Handle = __Unreal_LoadObjectImplementation(Outer, Name, Filename, LoadFlags, Sandbox);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __Unreal_LoadClassImplementation(nint A0, nint A1, nint A2, ELoadFlags A3, nint A4);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint, ELoadFlags, nint, nint>
            __Unreal_LoadClassImplementation;
#endif

        public static unsafe UClass Unreal_LoadClassImplementation(nint Outer, nint Name, nint Filename,
            ELoadFlags LoadFlags, nint Sandbox)
        {
#if !LEANCLR
            if (__Unreal_LoadClassImplementation == null)
            {
                __Unreal_LoadClassImplementation =
                    (delegate* unmanaged[Cdecl]<nint, nint, nint, ELoadFlags, nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.UnrealImplementation::Unreal_LoadClassImplementation");
            }
#endif

            var Handle = __Unreal_LoadClassImplementation(Outer, Name, Filename, LoadFlags, Sandbox);

            return Handle != 0 ? (UClass)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __Unreal_CreateWidgetImplementation(nint A0, nint A1);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint> __Unreal_CreateWidgetImplementation;
#endif

        public static unsafe T Unreal_CreateWidgetImplementation<T>(nint OwningObject, nint UserWidgetClass)
        {
#if !LEANCLR
            if (__Unreal_CreateWidgetImplementation == null)
            {
                __Unreal_CreateWidgetImplementation = (delegate* unmanaged[Cdecl]<nint, nint, nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.UnrealImplementation::Unreal_CreateWidgetImplementation");
            }
#endif

            var Handle = __Unreal_CreateWidgetImplementation(OwningObject, UserWidgetClass);

            return Handle != 0 ? (T)HandleData.GetObject(Handle) : default;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __Unreal_GWorldImplementation();
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint> __Unreal_GWorldImplementation;
#endif

        public static unsafe UWorld Unreal_GWorldImplementation()
        {
#if !LEANCLR
            if (__Unreal_GWorldImplementation == null)
            {
                __Unreal_GWorldImplementation = (delegate* unmanaged[Cdecl]<nint>)
                    MethodBridge.GetMethod("Script.Library.UnrealImplementation::Unreal_GWorldImplementation");
            }
#endif

            var Handle = __Unreal_GWorldImplementation();

            return Handle != 0 ? (UWorld)HandleData.GetObject(Handle) : null;
        }

#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe nint __Unreal_GetTransientPackageImplementation();
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint> __Unreal_GetTransientPackageImplementation;
#endif

        public static unsafe UPackage Unreal_GetTransientPackageImplementation()
        {
#if !LEANCLR
            if (__Unreal_GetTransientPackageImplementation == null)
            {
                __Unreal_GetTransientPackageImplementation = (delegate* unmanaged[Cdecl]<nint>)
                    MethodBridge.GetMethod(
                        "Script.Library.UnrealImplementation::Unreal_GetTransientPackageImplementation");
            }
#endif

            var Handle = __Unreal_GetTransientPackageImplementation();

            return Handle != 0 ? (UPackage)HandleData.GetObject(Handle) : null;
        }
    }
}