using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Interop
{
    public static class HandleData
    {
        private static readonly System.Threading.Lock Lock = new();

        private static readonly Dictionary<nint, GCHandle> Handles = new();

        private static nint Handle;

        private sealed class HandleReference
        {
            public nint Value;

            public int Count;
        }

        private static readonly ConditionalWeakTable<object, HandleReference> ObjectToHandleReference = new();

        [UnmanagedCallersOnly]
        public static void Free(nint InHandle) => FreeImplementation(InHandle);

        public static nint Alloc(object InObject, bool bPinned = false)
        {
            lock (Lock)
            {
                var HandleReference = ObjectToHandleReference.GetValue(InObject, _ =>
                    new HandleReference { Value = ++Handle, Count = 0 });

                HandleReference.Count++;

                if (!Handles.ContainsKey(HandleReference.Value))
                {
                    Handles[HandleReference.Value] =
                        GCHandle.Alloc(InObject, bPinned ? GCHandleType.Pinned : GCHandleType.Normal);
                }

                return HandleReference.Value;
            }
        }

        public static void FreeImplementation(nint InHandle)
        {
            if (InHandle != 0)
            {
                lock (Lock)
                {
                    if (Handles.TryGetValue(InHandle, out var OutHandle) && OutHandle.IsAllocated)
                    {
                        var Target = OutHandle.Target;

                        if (Target != null)
                        {
                            if (ObjectToHandleReference.TryGetValue(Target, out var OutHandleReference))
                            {
                                if (OutHandleReference.Value == InHandle)
                                {
                                    OutHandleReference.Count--;

                                    if (OutHandleReference.Count > 0)
                                    {
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    if (Handles.Remove(InHandle, out var RemovedHandle))
                    {
                        if (RemovedHandle.IsAllocated)
                        {
                            RemovedHandle.Free();
                        }
                    }
                }
            }
        }

        public static nint GetHandle(object? InObject)
        {
            if (InObject != null)
            {
                lock (Lock)
                {
                    return ObjectToHandleReference.TryGetValue(InObject, out var OutHandleReference)
                        ? OutHandleReference.Value
                        : 0;
                }
            }

            return 0;
        }

        public static object? GetObject(nint InHandle)
        {
            if (InHandle != 0)
            {
                lock (Lock)
                {
                    return Handles.TryGetValue(InHandle, out var OutHandle) ? OutHandle.Target : null;
                }
            }

            return null;
        }

        // LeanCLR-only reverse-invoke support (host-side stackobject invoke). These let the native side
        // resolve a HandleData key to the underlying runtime object pointer (to build an argument buffer)
        // and register a native object pointer as a new handle (for a returned/out object). Runtime-neutral
        // — Interop.dll has no LEANCLR macro — and never called by Mono/CoreCLR, which invoke C# functions
        // through their own reflection that already writes back value-type by-ref parameters. Resolved and
        // invoked through FLeanCLRMarshal like the other Interop bridges (not [UnmanagedCallersOnly]).
        public static nint GetObjectPointer(nint InHandle)
        {
            // Reinterpret the object reference's storage as an nint: under leanclr an object reference is the
            // RtObject* address, and the object stays pinned-in-place (non-moving GC) and rooted by its
            // HandleData GCHandle for as long as the handle lives, so the returned pointer is stable.
            if (GetObject(InHandle) is { } Object)
            {
                return Unsafe.As<object, nint>(ref Object);
            }

            return 0;
        }

        public static nint AllocFromObject(object InObject)
        {
            return Alloc(InObject);
        }

        internal static void Clear()
        {
            lock (Lock)
            {
                foreach (var Handle in Handles)
                {
                    if (Handle.Value.IsAllocated)
                    {
                        Handle.Value.Free();
                    }
                }

                Handles.Clear();

                ObjectToHandleReference.Clear();
            }
        }
    }
}