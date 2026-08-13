#include "Domain/LeanCLR/FLeanCLRMarshal.h"
#if WITH_LEANCLR
#include "Domain/LeanCLR/FLeanCLRLog.h"

// leanclr internal API consumed by the invoke path (mirrors the P1 spike include set). This TU is
// the one that references real leanclr symbols, so it doubles as the P2.1 link check: building the
// LeanCLR target must resolve these against leanclr.lib with no unresolved externals.
// Wrapped in THIRD_PARTY_INCLUDES_* so leanclr's non-UE-clean headers don't trip warnings-as-errors.
THIRD_PARTY_INCLUDES_START
#include "metadata/module_def.h"
#include "metadata/rt_metadata.h"
#include "vm/assembly.h"
#include "vm/class.h"
#include "vm/method.h"
#include "vm/object.h"
#include "vm/reflection.h"
#include "vm/runtime.h"
#include "vm/rt_exception.h"
#include "utils/string_builder.h"
THIRD_PARTY_INCLUDES_END

using namespace leanclr;

namespace
{
	// format_exception produces multi-line text (type + message + stack trace) and UE_LOG's %s stops at
	// the first newline, so the text has to be split and logged line by line. All three invoke paths
	// (forward invoke, reverse ctor, reverse method) need exactly this, so it lives here once.
	void LogManagedException(vm::RtException* InException)
	{
		if (InException == nullptr)
		{
			return;
		}

		utils::Utf8StringBuilder StringBuilder;

		vm::Exception::format_exception(InException, StringBuilder);

		StringBuilder.sure_null_terminator_but_not_append();

		const FString FullLog = UTF8_TO_TCHAR(StringBuilder.get_const_chars());

		TArray<FString> Lines;

		FullLog.ParseIntoArrayLines(Lines);

		for (const FString& Line : Lines)
		{
			UE_LOG(LogUnrealCSharp, Error, TEXT("[LeanCLR] %s"), *Line);
		}
	}

	// Run the interpreter and report a failure the one way both invoke paths need: a managed exception in
	// full, or InFailMessage when the invoke errored without producing one (which would otherwise be
	// silent). Returns true on success. The early return itself stays at each call site because the two
	// callers have different return types (bool vs IManagedHandle).
	bool InvokeRaw(const metadata::RtMethodInfo* InMethod, FLeanCLRMarshal::FStackObject* InArgs,
	               FLeanCLRMarshal::FStackObject* OutReturn, const char* InFailMessage)
	{
		const auto Result = vm::Runtime::invoke_stackobject_arguments_with_run_cctor(InMethod, InArgs, OutReturn);

		if (Result.is_err())
		{
			if (vm::RtException* Exception = vm::Exception::get_and_clear_current_exception())
			{
				LogManagedException(Exception);
			}
			else
			{
				FLeanCLRLog::ErrorWriter(InFailMessage);
			}

			return false;
		}

		return true;
	}
}

const metadata::RtMethodInfo* FLeanCLRMarshal::ResolveMethod(
	metadata::RtModuleDef* InModule, const char* InFullClassName, const char* InMethodName)
{
	if (InModule == nullptr || InFullClassName == nullptr || InMethodName == nullptr)
	{
		return nullptr;
	}

	auto ClassResult = InModule->get_class_by_nested_full_name(InFullClassName, /*ignore_case*/ false,
	                                                           /*throw_exception_when_not_found*/ false);
	if (ClassResult.is_err() || ClassResult.unwrap() == nullptr)
	{
		FLeanCLRLog::ErrorWriter("LeanCLR: bridge class not found");

		return nullptr;
	}

	metadata::RtClass* Class = ClassResult.unwrap();
	if (vm::Class::initialize_all(Class).is_err())
	{
		FLeanCLRLog::ErrorWriter("LeanCLR: bridge class initialize_all failed");

		return nullptr;
	}

	return vm::Method::find_matched_method_in_class_by_name(Class, InMethodName);
}

bool FLeanCLRMarshal::Invoke(const metadata::RtMethodInfo* InMethod,
                             const FStackObject* InArgs, int32 InArgCount, FStackObject& OutReturn)
{
	if (InMethod == nullptr)
	{
		return false;
	}

	// Return buffer sized from the method's declared return stack-object size (at least one slot).
	const size_t ReturnSlots = vm::Method::get_return_value_stack_object_size(InMethod);

	TArray<FStackObject> ReturnBuffer;
	ReturnBuffer.SetNumZeroed(ReturnSlots == 0 ? 1 : static_cast<int32>(ReturnSlots));

	// Argument buffer must be at least the method's declared total arg size; copy caller slots in.
	const size_t ArgSlots = vm::Method::get_total_arg_stack_object_size(InMethod);

	TArray<FStackObject> ArgBuffer;
	ArgBuffer.SetNumZeroed(ArgSlots == 0 ? 1 : static_cast<int32>(ArgSlots));

	if (InArgs != nullptr)
	{
		const int32 CopyCount = FMath::Min(InArgCount, ArgBuffer.Num());
		for (int32 Index = 0; Index < CopyCount; ++Index)
		{
			ArgBuffer[Index] = InArgs[Index];
		}
	}

	if (!InvokeRaw(InMethod, ArgBuffer.GetData(), ReturnBuffer.GetData(), "LeanCLR: bridge invoke failed"))
	{
		return false;
	}

	OutReturn = ReturnBuffer[0];

	return true;
}

FLeanCLRMarshal::FStackObject FLeanCLRMarshal::FromHandle(const IManagedHandle InHandle)
{
	FStackObject Slot{};

	Slot.i64 = InHandle.Value;

	return Slot;
}

FLeanCLRMarshal::FStackObject FLeanCLRMarshal::FromInt32(const int32 InValue)
{
	FStackObject Slot{};

	Slot.i32 = InValue;

	return Slot;
}

FLeanCLRMarshal::FStackObject FLeanCLRMarshal::FromInt64(const int64 InValue)
{
	FStackObject Slot{};

	Slot.i64 = InValue;

	return Slot;
}

FLeanCLRMarshal::FStackObject FLeanCLRMarshal::FromFloat(const float InValue)
{
	FStackObject Slot{};

	Slot.f32 = InValue;

	return Slot;
}

FLeanCLRMarshal::FStackObject FLeanCLRMarshal::FromPointer(const void* InPointer)
{
	FStackObject Slot{};

	Slot.cptr = InPointer;

	return Slot;
}

IManagedHandle FLeanCLRMarshal::ToHandle(const FStackObject& InSlot)
{
	return IManagedHandle{InSlot.i64};
}

int32 FLeanCLRMarshal::ToInt32(const FStackObject& InSlot)
{
	return InSlot.i32;
}

int64 FLeanCLRMarshal::ToInt64(const FStackObject& InSlot)
{
	return InSlot.i64;
}

void* FLeanCLRMarshal::ToPointer(const FStackObject& InSlot)
{
	return InSlot.ptr;
}

namespace
{
	// Invoke the GetObjectPointer bridge: a HandleData counter key -> the underlying runtime RtObject*
	// (returned as a raw pointer). The object stays rooted by its HandleData GCHandle and leanclr's GC is
	// non-moving, so the pointer is stable for as long as the handle lives.
	void* ResolveObjectPointer(const metadata::RtMethodInfo* InBridge, const IManagedHandle InHandle)
	{
		if (InBridge == nullptr || !IManagedHandleIsValid(InHandle))
		{
			return nullptr;
		}

		const FLeanCLRMarshal::FStackObject Arg = FLeanCLRMarshal::FromHandle(InHandle);

		if (FLeanCLRMarshal::FStackObject Return{}; FLeanCLRMarshal::Invoke(InBridge, &Arg, 1, Return))
		{
			return FLeanCLRMarshal::ToPointer(Return);
		}

		return nullptr;
	}

	// Invoke the AllocFromObject bridge: a runtime RtObject* -> a fresh HandleData handle. The object is
	// passed in an obj slot so the interpreter frame roots it during the alloc call (it is not yet in the
	// handle table, and only reachable from this native frame until then).
	IManagedHandle AllocHandleFromObject(const metadata::RtMethodInfo* InBridge, vm::RtObject* InObject)
	{
		if (InBridge == nullptr || InObject == nullptr)
		{
			return InvalidManagedHandle;
		}

		FLeanCLRMarshal::FStackObject Arg{};

		Arg.obj = InObject;

		if (FLeanCLRMarshal::FStackObject Return{}; FLeanCLRMarshal::Invoke(InBridge, &Arg, 1, Return))
		{
			return FLeanCLRMarshal::ToHandle(Return);
		}

		return InvalidManagedHandle;
	}
}

const metadata::RtMethodInfo* FLeanCLRMarshal::ResolveReverseMethod(
	const metadata::RtMethodInfo* InGetObjectPointer, const IManagedHandle InMethodHandle)
{
	void* MethodObject = ResolveObjectPointer(InGetObjectPointer, InMethodHandle);

	if (MethodObject == nullptr)
	{
		return nullptr;
	}

	// (RtResult::unwrap() is non-const, so this must not be declared const.)
	auto Result = vm::Reflection::get_method_info_from_handle_arg(MethodObject);

	return Result.is_err() ? nullptr : Result.unwrap();
}

bool FLeanCLRMarshal::ReverseMethodNeedsCppPath(const metadata::RtMethodInfo* InMethod)
{
	if (InMethod == nullptr)
	{
		return false;
	}

	// Value-type `this` uses the obj+1 layout, which this path does not build — keep it on the C# path.
	if (vm::Method::is_instance(InMethod) && vm::Class::is_value_type(InMethod->parent))
	{
		return false;
	}

	const size_t ParameterCount = vm::Method::get_param_count_exclude_this(InMethod);

	bool bHasValueByRef = false;

	for (size_t Index = 0; Index < ParameterCount; ++Index)
	{
		const metadata::RtTypeSig* ParameterType = InMethod->parameters[Index];

		if (!ParameterType->is_by_ref())
		{
			continue;
		}

		const metadata::RtTypeSig ValueType = ParameterType->to_canonized_without_byref();

		auto ClassResult = vm::Class::get_class_from_typesig(&ValueType);

		if (ClassResult.is_err())
		{
			return false;
		}

		if (vm::Class::is_value_type(ClassResult.unwrap()))
		{
			bHasValueByRef = true;
		}
		else
		{
			// A reference-type by-ref is already written back correctly by the C# path, and external host
			// storage for a reference by-ref is not GC-traced by the interpreted-frame scanner — so leave
			// any method carrying one on the C# path.
			return false;
		}
	}

	return bHasValueByRef;
}

bool FLeanCLRMarshal::IsReferenceConstructor(const metadata::RtMethodInfo* InMethod)
{
	return InMethod != nullptr && vm::Method::is_ctor(InMethod) && vm::Method::is_instance(InMethod) &&
		!vm::Class::is_value_type(InMethod->parent);
}

IManagedHandle FLeanCLRMarshal::InvokeReverse(const metadata::RtMethodInfo* InGetObjectPointer,
                                              const metadata::RtMethodInfo* InAllocFromObject,
                                              const metadata::RtMethodInfo* InMethod, const IManagedHandle InThis,
                                              const int32 InParamCount, void** InParams)
{
	if (InMethod == nullptr)
	{
		return InvalidManagedHandle;
	}

	// P6.1 §5.6/§5.8 root-cause fix: a reference constructor must run its body on the EXISTING bound
	// `this`, but invoke_stackobject_arguments_with_run_cctor treats a .ctor as newobj — it does not
	// execute the body against slot 0, so every property setter in the ctor writes to a throwaway object
	// and never reaches the spawned instance (PIE readback proved write-then-read of Int32Value returns 0
	// inside the ctor). leanclr's Reflection::invoke_method(ctor, obj != null, ...) runs the ctor body on
	// `obj` directly (reflection.cpp:2776-2807). The dynamic object-construction path only ever invokes
	// the parameterless .ctor, so route that here; leave any (currently non-existent) parameterised
	// reference ctor on the legacy stackobject path below.
	if (vm::Method::is_ctor(InMethod) && vm::Method::get_param_count_exclude_this(InMethod) == 0)
	{
		const auto ThisObject = static_cast<vm::RtObject*>(ResolveObjectPointer(InGetObjectPointer, InThis));

		if (ThisObject == nullptr)
		{
			FLeanCLRLog::ErrorWriter("LeanCLR: constructor invoke could not resolve 'this'");

			return InvalidManagedHandle;
		}

		vm::RtObject* CtorException = nullptr;

		const auto CtorResult = vm::Reflection::invoke_method(InMethod, ThisObject, /*params*/ nullptr,
		                                                      &CtorException);

		// A managed exception is reported in full just below; this covers the other failure mode
		// (invoke returned an error with no exception object), which would otherwise be silent.
		if (CtorResult.is_err() && CtorException == nullptr)
		{
			FLeanCLRLog::ErrorWriter("LeanCLR: constructor invoke failed");
		}

		if (CtorException != nullptr)
		{
			LogManagedException(reinterpret_cast<vm::RtException*>(CtorException));
		}

		// A constructor is void: nothing to marshal back to the caller.
		return InvalidManagedHandle;
	}

	const size_t ArgSlots = vm::Method::get_total_arg_stack_object_size(InMethod);

	TArray<FStackObject> ArgBuffer;
	ArgBuffer.SetNumZeroed(ArgSlots == 0 ? 1 : static_cast<int32>(ArgSlots));

	size_t SlotIndex = 0;

	if (vm::Method::is_instance(InMethod))
	{
		ArgBuffer[SlotIndex].obj = static_cast<vm::RtObject*>(ResolveObjectPointer(InGetObjectPointer, InThis));

		++SlotIndex;
	}

	const size_t ParameterCount = vm::Method::get_param_count_exclude_this(InMethod);

	for (size_t Index = 0; Index < ParameterCount && SlotIndex < static_cast<size_t>(ArgBuffer.Num()); ++Index)
	{
		const metadata::RtTypeSig* ParameterType = InMethod->parameters[Index];

		const metadata::RtTypeSig ValueType = ParameterType->is_by_ref()
			                                      ? ParameterType->to_canonized_without_byref()
			                                      : *ParameterType;

		auto ClassResult = vm::Class::get_class_from_typesig(&ValueType);

		if (ClassResult.is_err())
		{
			return InvalidManagedHandle;
		}

		metadata::RtClass* ParameterClass = ClassResult.unwrap();

		(void)vm::Class::initialize_all(ParameterClass);

		auto ReduceResult = interp::InterpDefs::get_reduce_type_and_size_by_typesig(ParameterType);

		if (ReduceResult.is_err())
		{
			return InvalidManagedHandle;
		}

		const size_t ByteSize = ReduceResult.unwrap().byte_size;

		FStackObject& Slot = ArgBuffer[SlotIndex];

		SlotIndex += interp::InterpDefs::get_stack_object_size_by_byte_size(ByteSize);

		void* Argument = (InParams != nullptr && Index < static_cast<size_t>(InParamCount)) ? InParams[Index] : nullptr;

		if (ParameterType->is_by_ref())
		{
			// Classification guarantees a value-type by-ref here: point the slot straight at the caller's
			// own storage so the callee's write to the out param lands there directly. This is the fix —
			// leanclr's reflection path would have unboxed into a temp and dropped the write.
			Slot.ptr = Argument;
		}
		else if (vm::Class::is_value_type(ParameterClass))
		{
			if (Argument != nullptr && ByteSize > 0)
			{
				FMemory::Memcpy(&Slot, Argument, ByteSize);
			}
		}
		else
		{
			// Reference type by value: InParams[Index] points at a slot holding a HandleData key.
			const IManagedHandle ArgumentHandle = Argument != nullptr
				                                       ? IManagedHandle{*static_cast<int64*>(Argument)}
				                                       : InvalidManagedHandle;

			Slot.obj = static_cast<vm::RtObject*>(ResolveObjectPointer(InGetObjectPointer, ArgumentHandle));
		}
	}

	const size_t ReturnSlots = vm::Method::get_return_value_stack_object_size(InMethod);

	TArray<FStackObject> ReturnBuffer;
	ReturnBuffer.SetNumZeroed(ReturnSlots == 0 ? 1 : static_cast<int32>(ReturnSlots));

	if (!InvokeRaw(InMethod, ArgBuffer.GetData(), ReturnBuffer.GetData(), "LeanCLR: reverse invoke failed"))
	{
		return InvalidManagedHandle;
	}

	if (vm::Method::is_void_return(InMethod))
	{
		return InvalidManagedHandle;
	}

	auto ReturnClassResult = vm::Class::get_class_from_typesig(InMethod->return_type);

	if (ReturnClassResult.is_err())
	{
		return InvalidManagedHandle;
	}

	metadata::RtClass* ReturnClass = ReturnClassResult.unwrap();

	if (vm::Class::is_value_type(ReturnClass))
	{
		// Box the raw return bytes so the caller receives a handle, matching MethodBridge.Invoke's return.
		auto BoxResult = LEANCLR_BOX_OBJECT_INTERNAL(ReturnClass, ReturnBuffer.GetData(), "FLeanCLRMarshal::InvokeReverse");

		if (BoxResult.is_err())
		{
			return InvalidManagedHandle;
		}

		return AllocHandleFromObject(InAllocFromObject, BoxResult.unwrap());
	}

	return AllocHandleFromObject(InAllocFromObject, ReturnBuffer[0].obj);
}

IManagedHandle FLeanCLRMarshal::AllocAssemblyHandle(const metadata::RtMethodInfo* InAllocFromObject,
                                                    metadata::RtAssembly* InAssembly)
{
	if (InAllocFromObject == nullptr || InAssembly == nullptr)
	{
		return InvalidManagedHandle;
	}

	// Materialize the System.Reflection.Assembly reflection object for this native assembly, then root it
	// as a HandleData handle. RtReflectionAssembly derives from RtObject, so the alloc bridge (obj slot)
	// pins it during registration exactly like any other reference return.
	auto Result = vm::Reflection::get_assembly_reflection_object(InAssembly);

	if (Result.is_err())
	{
		FLeanCLRLog::ErrorWriter("LeanCLR: get_assembly_reflection_object failed");

		return InvalidManagedHandle;
	}

	return AllocHandleFromObject(InAllocFromObject, static_cast<vm::RtObject*>(Result.unwrap()));
}
#endif
