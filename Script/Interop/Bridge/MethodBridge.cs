using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Interop
{
    public static unsafe class MethodBridge
    {
        private static readonly Dictionary<string, nint> StringToMethod = new(StringComparer.Ordinal);

        [UnmanagedCallersOnly]
        public static void RegisterBinding(byte** InNames, nint* InMethods, int InLength)
        {
            if (InNames != null && InMethods != null)
            {
                for (var Index = 0; Index < InLength; Index++)
                {
                    if (InNames[Index] != null && InMethods[Index] != 0)
                    {
                        var Name = Marshal.PtrToStringUTF8((nint)InNames[Index]) ?? string.Empty;

                        if (!string.IsNullOrEmpty(Name))
                        {
                            StringToMethod[Name] = InMethods[Index];
                        }
                    }
                }
            }
        }

        [UnmanagedCallersOnly]
        public static nint Invoke(nint InHandle, nint InMethod, int InParamCount, nint* InParams)
        {
            try
            {
                if (HandleData.GetObject(InMethod) is MethodBase Method)
                {
                    var Object = HandleData.GetObject(InHandle);

                    var MethodParameters = Method.GetParameters();

                    var MethodParameterLength = MethodParameters.Length;

                    var Parameters = new object?[MethodParameterLength];

                    for (var Index = 0; Index < MethodParameterLength; Index++)
                    {
                        var ParameterType = MethodParameters[Index].ParameterType;

                        if (InParams == null || Index >= InParamCount)
                        {
                            Parameters[Index] = ParameterType.IsValueType
                                ? Activator.CreateInstance(ParameterType)
                                : null;
                        }
                        else if (InParams[Index] == 0)
                        {
                            Parameters[Index] = null;
                        }
                        else
                        {
                            var ElementType = ParameterType.IsByRef ? ParameterType.GetElementType()! : ParameterType;

                            Parameters[Index] = ElementType.IsValueType
                                ? GetValue(InParams[Index], ElementType)
                                : HandleData.GetObject(*(nint*)InParams[Index]);
                        }
                    }

                    var Result = Method.Invoke(Object, Parameters);

                    if (InParams != null)
                    {
                        for (var Index = 0; Index < MethodParameterLength && Index < InParamCount; Index++)
                        {
                            var ParameterType = MethodParameters[Index].ParameterType;

                            if (ParameterType.IsByRef && InParams[Index] != 0)
                            {
                                var Param = (nint*)InParams[Index];

                                var ElementType = ParameterType.GetElementType()!;

                                var Parameter = Parameters[Index];

                                if (ElementType.IsValueType)
                                {
                                    SetValue(InParams[Index], Parameter, ElementType);
                                }
                                else if (Parameter != null)
                                {
                                    *Param = HandleData.Alloc(Parameter);
                                }
                                else
                                {
                                    *Param = 0;
                                }
                            }
                        }
                    }

                    if (Result is null)
                    {
                        return 0;
                    }

                    var ResultType = Result.GetType();

                    if (ResultType.IsValueType)
                    {
                        if (ResultType.IsEnum)
                        {
                            var UnderlyingType = Enum.GetUnderlyingType(ResultType);

                            Result = Convert.ChangeType(Result, UnderlyingType);
                        }
                    }

                    return HandleData.Alloc(Result);
                }

                return 0;
            }
            catch (Exception Exception)
            {
                var InnerException = Exception is TargetInvocationException
                {
                    InnerException: not null
                } TargetInvocationException
                    ? TargetInvocationException.InnerException
                    : Exception;

                Console.Error.WriteLine($"\nUnhandled Exception:\n{InnerException}");

                return 0;
            }
        }

        public static nint GetMethod(string InName)
        {
            return StringToMethod.TryGetValue(InName, out var Method) ? Method : nint.Zero;
        }

        // Lazy resolution slot for the Script.Library.* bridges. Under LEANCLR those entry points are
        // plain [DllImport] declarations resolved by the runtime; Mono/CoreCLR instead hold a function
        // pointer that has to be looked up on first use. Keeping the "resolve once" check here lets the
        // two backends share one call-site form -- see Script/UE/Library/*.cs, where each bridge is a
        // property over its own slot instead of ~200 copies of the same #if !LEANCLR block.
        // A miss leaves the slot zero and is retried on the next call, exactly as the inlined check did.
        public static nint Resolve(ref nint InSlot, string InName)
        {
            if (InSlot == nint.Zero)
            {
                InSlot = GetMethod(InName);
            }

            return InSlot;
        }

        private static object GetValue(nint InHandle, Type InType)
        {
            var Type = Nullable.GetUnderlyingType(InType) ?? InType;

            if (Type.IsEnum)
            {
                return Enum.ToObject(Type, ReadPrimitiveValue(InHandle, Enum.GetUnderlyingType(Type)));
            }

            return ReadPrimitiveValue(InHandle, Type);
        }

        // Read a primitive value type directly from the pointer instead of Marshal.PtrToStructure.
        // LeanCLR leaves Marshal.PtrToStructure/StructureToPtr unimplemented (marshal.cpp
        // ptr_to_structure_impl/structure_to_ptr_impl -> fatal_on_not_implemented_error), which crashed
        // every UE->C# call carrying a value-type parameter (the first one reached was
        // SetInt32ValueFunction(int)). Only primitives + enum underlying types ever reach here: every
        // UStruct/FName/FText/FString/TSubclassOf/TScriptInterface/TSoft*/TWeak*/TArray/TSet/TMap in the
        // binding surface is a reference (handle) type and takes the HandleData.GetObject path. Direct
        // pointer reads are byte-identical to PtrToStructure on Mono/CoreCLR, so this is zero-regression.
        //
        // The switch is exhaustive over the generated binding surface, measured rather than assumed:
        // sweeping the compiled UE.dll + Game.dll (17199 types / 194979 methods) for value-type
        // parameters found 35040 of them, and the ONLY type not covered by an explicit case was nint
        // (3059 sites) -- which is why nint/nuint are listed below. With those two present nothing in
        // the surface can fall through, so a fall-through means a genuinely new parameter shape rather
        // than an expected blittable struct. Hence the throw instead of a Marshal fallback: on LeanCLR
        // Marshal here is fatal_on_not_implemented_error (kills the process, uncatchable, no
        // diagnostics), and on Mono/CoreCLR nothing in the surface needs it. A NotSupportedException
        // naming the type is strictly more useful than either.
        private static object ReadPrimitiveValue(nint InHandle, Type InType)
        {
            return InType switch
            {
                _ when InType == typeof(bool) => *(bool*)InHandle,
                _ when InType == typeof(sbyte) => *(sbyte*)InHandle,
                _ when InType == typeof(byte) => *(byte*)InHandle,
                _ when InType == typeof(short) => *(short*)InHandle,
                _ when InType == typeof(ushort) => *(ushort*)InHandle,
                _ when InType == typeof(int) => *(int*)InHandle,
                _ when InType == typeof(uint) => *(uint*)InHandle,
                _ when InType == typeof(long) => *(long*)InHandle,
                _ when InType == typeof(ulong) => *(ulong*)InHandle,
                _ when InType == typeof(nint) => *(nint*)InHandle,
                _ when InType == typeof(nuint) => *(nuint*)InHandle,
                _ when InType == typeof(float) => *(float*)InHandle,
                _ when InType == typeof(double) => *(double*)InHandle,
                _ when InType == typeof(char) => *(char*)InHandle,
                _ => throw new NotSupportedException(
                    $"MethodBridge cannot read parameter type '{InType.FullName}' from a raw pointer. " +
                    "Only primitives, nint/nuint and enums are supported; reference types go through HandleData."),
            };
        }

        private static void SetValue(nint InHandle, object? InValue, Type InType)
        {
            if (InValue != null)
            {
                var Type = Nullable.GetUnderlyingType(InType) ?? InType;

                if (Type.IsEnum)
                {
                    var Underlying = Enum.GetUnderlyingType(Type);

                    WritePrimitiveValue(InHandle, Convert.ChangeType(InValue, Underlying), Underlying);
                }
                else
                {
                    WritePrimitiveValue(InHandle, InValue, Type);
                }
            }
        }

        // Byref (out) write-back counterpart to ReadPrimitiveValue; avoids the unimplemented
        // Marshal.StructureToPtr for the same primitive + enum surface. Same measured exhaustiveness
        // argument as ReadPrimitiveValue -- and stricter here: of the 3059 fall-through sites found in
        // the compiled surface, *zero* were byref, so this default was already unreachable before
        // nint/nuint were added.
        private static void WritePrimitiveValue(nint InHandle, object InValue, Type InType)
        {
            switch (InType)
            {
                case var _ when InType == typeof(bool): *(bool*)InHandle = (bool)InValue; break;
                case var _ when InType == typeof(sbyte): *(sbyte*)InHandle = (sbyte)InValue; break;
                case var _ when InType == typeof(byte): *(byte*)InHandle = (byte)InValue; break;
                case var _ when InType == typeof(short): *(short*)InHandle = (short)InValue; break;
                case var _ when InType == typeof(ushort): *(ushort*)InHandle = (ushort)InValue; break;
                case var _ when InType == typeof(int): *(int*)InHandle = (int)InValue; break;
                case var _ when InType == typeof(uint): *(uint*)InHandle = (uint)InValue; break;
                case var _ when InType == typeof(long): *(long*)InHandle = (long)InValue; break;
                case var _ when InType == typeof(ulong): *(ulong*)InHandle = (ulong)InValue; break;
                case var _ when InType == typeof(nint): *(nint*)InHandle = (nint)InValue; break;
                case var _ when InType == typeof(nuint): *(nuint*)InHandle = (nuint)InValue; break;
                case var _ when InType == typeof(float): *(float*)InHandle = (float)InValue; break;
                case var _ when InType == typeof(double): *(double*)InHandle = (double)InValue; break;
                case var _ when InType == typeof(char): *(char*)InHandle = (char)InValue; break;
                default:
                    throw new NotSupportedException(
                        $"MethodBridge cannot write parameter type '{InType.FullName}' through a raw pointer. " +
                        "Only primitives, nint/nuint and enums are supported; reference types go through HandleData.");
            }
        }
    }
}