#include "Domain/LeanCLR/FLeanCLRDomain.h"
#if WITH_LEANCLR
#include "Domain/LeanCLR/FLeanCLRLog.h"
#include "Domain/LeanCLR/FLeanCLRMarshal.h"
#include "Domain/LeanCLR/FLeanCLRFunctionLibrary.h"
#include "Common/FUnrealCSharpFunctionLibrary.h"
#include "Reflection/FReflectionRegistry.h"
#include "Reflection/FClassReflection.h"
#include "Reflection/FMethodReflection.h"
#include "Binding/FBinding.h"
#include "Binding/Class/FBindingClass.h"
#include "Binding/Function/FBindingMethod.h"
#include "Domain/Script/FScriptLog.h"
#include "CoreMacro/Macro.h"
#include "CoreMacro/ClassMacro.h"
#include "CoreMacro/NamespaceMacro.h"
#include "CoreMacro/FunctionMacro.h"
#include "CoreMacro/PropertyMacro.h"
#include "Log/UnrealCSharpLog.h"
#include "HAL/FileManager.h"
#include "Misc/FileHelper.h"
#include "Misc/Paths.h"

// leanclr vm:: API driven by the runtime bring-up (P4). Wrapped in THIRD_PARTY_INCLUDES_* so leanclr's
// non-UE-clean headers don't trip warnings-as-errors (same idiom as FLeanCLRMarshal.cpp). The include
// root src/runtime comes from the LeanCLR External module when WITH_LEANCLR=1.
THIRD_PARTY_INCLUDES_START
#include "vm/settings.h"
#include "vm/runtime.h"
#include "vm/assembly.h"
#include "vm/rt_exception.h"
#include "vm/pinvoke.h"
#include "metadata/module_def.h"
#include "metadata/rt_metadata.h"
#include "interp/interp_defs.h"
#include "alloc/general_allocation.h"
#include "utils/string_builder.h"
#include "core/rt_result.h"
THIRD_PARTY_INCLUDES_END

// P4: runtime lifecycle (Initialize / assembly loading / bridge-handle resolution / native-diagnostics
// logging / Tick / Deinitialize). This is the FORWARD half of strategy A (native -> managed bridge via
// vm::Runtime::invoke, the path the P0/P1 spike round-tripped 20/20). The REVERSE half (C# -> UE native
// bindings) is wired via RegisterPInvokes: the LeanCLR-target generator emits [DllImport] stubs, and each
// UE binding is registered as a named P/Invoke bound to LeanCLRNativeInvoker (pure-C++ arity dispatch, no
// asm — args and returns are single-slot GP integer/pointer; no floats exist across the binding surface).
// SCOPE: this round covers the GENERATED bindings (Script/{UE,Game}/Proxy/Binding). The 28 hand-written
// core-library files (Script/UE/Library/*Implementation.cs) still use calli and are converted in a later
// round; the invoker already handles their non-void (nint/int/byte) returns. The per-method IScriptDomain
// bodies (P5) remain graceful stubs, so FReflectionRegistry stays empty until P5.1 lands.

namespace
{
	// leanclr's FileLoader is a context-free C function pointer, so the search directories are cached
	// in this TU-local static (seeded in Initialize before set_file_loader). Mirrors the P0/P1 spike's
	// spike_file_loader: search each directory for <name>.<ext>, read it, hand leanclr an owning buffer
	// allocated through its own allocator (leanclr frees it).
	TArray<FString> GSearchDirectories;

	bool LeanCLRFileLoader(const char* InAssemblyName, const char* InExtension, leanclr::vm::FileData& OutFileData)
	{
		const FString AssemblyName = UTF8_TO_TCHAR(InAssemblyName);

		const FString Extension = UTF8_TO_TCHAR(InExtension);

		for (const auto& Directory : GSearchDirectories)
		{
			const auto FilePath = FString::Printf(TEXT("%s/%s.%s"), *Directory, *AssemblyName, *Extension);

			// FILEREAD_Silent: a miss here is normal (leanclr probes every search dir, and requests .pdb
			// symbol files that are not shipped); without it every miss spams a LogStreaming warning.
			if (TArray<uint8> Data; FFileHelper::LoadFileToArray(Data, *FilePath, FILEREAD_Silent))
			{
				void* Buffer = leanclr::alloc::GeneralAllocation::malloc(Data.Num());

				if (Buffer == nullptr)
				{
					return false;
				}

				FMemory::Memcpy(Buffer, Data.GetData(), Data.Num());

				OutFileData.data = static_cast<const uint8_t*>(Buffer);

				OutFileData.length = static_cast<size_t>(Data.Num());

				OutFileData.shared = false;

				return true;
			}
		}

		// Diagnostics for packaged builds (iOS/Android): leanclr's own miss message is a std::printf to
		// stdout, which is invisible on device. Surface the actual attempted paths through UE_LOG so a
		// missing/mislocated assembly (the err=26 FileNotFound root cause) can be pinpointed. Skip .pdb —
		// symbol files are intentionally not shipped, so their misses are noise, not errors.
		if (Extension == TEXT("dll"))
		{
			UE_LOG(LogUnrealCSharp, Error,
			       TEXT("FLeanCLRDomain: file loader could not find %s.%s in %d search dir(s):"),
			       *AssemblyName, *Extension, GSearchDirectories.Num());

			for (const auto& Directory : GSearchDirectories)
			{
				const auto FilePath = FString::Printf(TEXT("%s/%s.%s"), *Directory, *AssemblyName, *Extension);

				UE_LOG(LogUnrealCSharp, Error,
				       TEXT("FLeanCLRDomain:   tried '%s' (dir exists=%d, file exists=%d)"),
				       *FilePath,
				       IFileManager::Get().DirectoryExists(*Directory) ? 1 : 0,
				       IFileManager::Get().FileExists(*FilePath) ? 1 : 0);
			}
		}

		return false;
	}

	// leanclr -> UE Output Log (P4.2). Native diagnostics only; the managed LogBridge.SetLog callback is
	// a C# -> native reverse call and belongs with the reverse channel (deferred).
	void LeanCLRReportUnhandledException(leanclr::vm::RtException* InException)
	{
		if (InException == nullptr)
		{
			return;
		}

		leanclr::utils::Utf8StringBuilder StringBuilder;

		leanclr::vm::Exception::format_exception(InException, StringBuilder);

		StringBuilder.sure_null_terminator_but_not_append();

		FLeanCLRLog::ErrorWriter(StringBuilder.get_const_chars());
	}

	void LeanCLRDebuggerLog(int32_t InLevel, const uint16_t* InCategory, size_t InCategoryLength,
	                        const uint16_t* InMessage, size_t InMessageLength)
	{
#if !NO_LOGGING
		const auto Message = StringCast<TCHAR>(reinterpret_cast<const UTF16CHAR*>(InMessage),
		                                       static_cast<int32>(InMessageLength));

		if (InCategory != nullptr && InCategoryLength > 0)
		{
			const auto Category = StringCast<TCHAR>(reinterpret_cast<const UTF16CHAR*>(InCategory),
			                                        static_cast<int32>(InCategoryLength));

			UE_LOG(LogUnrealCSharp, Log, TEXT("[LeanCLR][%s] %s"), Category.Get(), Message.Get());
		}
		else
		{
			UE_LOG(LogUnrealCSharp, Log, TEXT("[LeanCLR] %s"), Message.Get());
		}
#endif
	}

	// Resolve a static method by class-full-name + method-name inside a loaded module (UTF-8 boundary).
	const leanclr::metadata::RtMethodInfo* ResolveIn(leanclr::metadata::RtModuleDef* InModule,
	                                                 const FString& InFullClassName, const FString& InMethodName)
	{
		if (InModule == nullptr)
		{
			return nullptr;
		}

		return FLeanCLRMarshal::ResolveMethod(InModule, TCHAR_TO_UTF8(*InFullClassName), TCHAR_TO_UTF8(*InMethodName));
	}

	// Strategy A method-body helper: invoke a resolved bridge method and read its single return slot as a
	// managed handle. Mirrors the Mono/CoreCLR "call the native fn pointer, return the handle" pattern in
	// FScriptDomainImpl.inl, but routed through the interpreter (vm::Runtime::invoke) instead. Returns
	// InvalidManagedHandle when the handle is unresolved or the managed call raised (logged by the marshal).
	IManagedHandle InvokeForHandle(const leanclr::metadata::RtMethodInfo* InMethod,
	                               const FLeanCLRMarshal::FStackObject* InArgs, const int32 InArgCount)
	{
		FLeanCLRMarshal::FStackObject Return{};

		return FLeanCLRMarshal::Invoke(InMethod, InArgs, InArgCount, Return)
			       ? FLeanCLRMarshal::ToHandle(Return)
			       : InvalidManagedHandle;
	}

	// Universal reverse-channel invoker (P1.2/P4.3). leanclr can't do an unmanaged calli on a raw native
	// pointer (R5), so each UE binding is registered as a named P/Invoke; when C# calls the [DllImport]
	// stub, leanclr's Shim::get_invoker routes here. Args and returns across the whole binding surface are
	// single-slot GP integer/pointer values (handles, byte* buffers, ints, enums, bool) — there are NO
	// floats and NO multi-slot structs by value. So instead of the spike's general x64 marshaller + ml64
	// thunk, we gather the args and dispatch on the arity, letting the C++ compiler emit the Win64 ABI
	// call, and capture the return as intptr_t. Anything outside that (float/struct arg or return) is
	// refused loudly rather than mis-marshalled.
	leanclr::RtResultVoid LeanCLRNativeInvoker(leanclr::metadata::RtManagedMethodPointer /*InMethodPtr*/,
	                                           const leanclr::metadata::RtMethodInfo* InMethod,
	                                           const leanclr::interp::RtStackObject* InParams,
	                                           leanclr::interp::RtStackObject* OutReturn) noexcept
	{
		using namespace leanclr;

		// Recover the native address registered under this method's full name.
		// (RtResult::unwrap() is non-const, so these results must not be declared const.)
		auto Entry = vm::PInvokes::get_pinvoke_by_method(InMethod);

		if (Entry.is_err() || Entry.unwrap() == nullptr || Entry.unwrap()->func == nullptr)
		{
			UE_LOG(LogUnrealCSharp, Warning, TEXT("FLeanCLRDomain: no native binding registered for invoked P/Invoke"));

			return RtErr::ExecutionEngine;
		}

		const auto Func = reinterpret_cast<UPTRINT>(Entry.unwrap()->func);

		// Classify the return. UE bindings return only void or a single register-width integer/pointer:
		// generated bindings are void, the hand-written library uses nint/int/byte, and there are NO
		// float/double/long returns anywhere — so one intptr_t capture of RAX covers every case, keeping
		// this pure C++ (no ml64 thunk). Reject float (XMM) / struct-by-value (multi-slot) returns loudly.
		auto ReturnReduce = interp::InterpDefs::get_reduce_type_and_size_by_typesig(InMethod->return_type);

		if (ReturnReduce.is_err())
		{
			UE_LOG(LogUnrealCSharp, Warning, TEXT("FLeanCLRDomain: binding P/Invoke return type unresolved; not supported"));

			return RtErr::NotImplemented;
		}

		const auto ReturnType = ReturnReduce.unwrap().reduce_type;

		switch (ReturnType)
		{
		case metadata::RtArgOrLocOrFieldReduceType::Void:
		case metadata::RtArgOrLocOrFieldReduceType::I1:
		case metadata::RtArgOrLocOrFieldReduceType::U1:
		case metadata::RtArgOrLocOrFieldReduceType::I2:
		case metadata::RtArgOrLocOrFieldReduceType::U2:
		case metadata::RtArgOrLocOrFieldReduceType::I4:
		case metadata::RtArgOrLocOrFieldReduceType::I8:
		case metadata::RtArgOrLocOrFieldReduceType::I:
		case metadata::RtArgOrLocOrFieldReduceType::Ref:
			break;
		default:
			UE_LOG(LogUnrealCSharp, Warning,
			       TEXT("FLeanCLRDomain: binding P/Invoke has float/struct return; not supported"));

			return RtErr::NotImplemented;
		}

		void* Args[32] = {};

		const uint16 ParameterCount = InMethod->parameter_count; // static P/Invoke: no implicit 'this'

		if (ParameterCount > UE_ARRAY_COUNT(Args))
		{
			UE_LOG(LogUnrealCSharp, Warning, TEXT("FLeanCLRDomain: binding P/Invoke arity %d exceeds cap"),
			       static_cast<int32>(ParameterCount));

			return RtErr::NotImplemented;
		}

		size_t Slot = 0;

		for (uint16 Index = 0; Index < ParameterCount; ++Index)
		{
			const auto& ArgDesc = InMethod->arg_descs[Index];

			// v1: single-slot GP (pointer/integer/byref) args only. Reject multi-slot (struct by value)
			// and floating-point args (R4/R8 go in XMM registers, which this GP arity dispatch can't
			// place) rather than mis-marshal them. All real UE bindings are pointers, so this is a guard,
			// not a limitation exercised in practice.
			if (ArgDesc.stack_object_size != 1)
			{
				UE_LOG(LogUnrealCSharp, Warning, TEXT("FLeanCLRDomain: binding P/Invoke has multi-slot arg; not supported"));

				return RtErr::NotImplemented;
			}

			if (ArgDesc.reduce_type == metadata::RtArgOrLocOrFieldReduceType::R4 ||
				ArgDesc.reduce_type == metadata::RtArgOrLocOrFieldReduceType::R8)
			{
				UE_LOG(LogUnrealCSharp, Warning,
				       TEXT("FLeanCLRDomain: binding P/Invoke has floating-point arg; not supported"));

				return RtErr::NotImplemented;
			}

			Args[Index] = reinterpret_cast<void*>(InParams[Slot].u64);

			Slot += ArgDesc.stack_object_size;
		}

		// Dispatch on arity. Every arg is a single-slot GP value (pointer/handle/int/enum), so a
		// pointer-arg function-pointer cast places them per Win64 ABI (registers + stack spill) with no
		// asm. The return is captured as intptr_t (RAX); a void callee just leaves RAX unused.
		intptr_t ReturnValue = 0;

		switch (ParameterCount)
		{
		case 0:
			ReturnValue = reinterpret_cast<intptr_t(*)()>(Func)();
			break;
		case 1:
			ReturnValue = reinterpret_cast<intptr_t(*)(void*)>(Func)(Args[0]);
			break;
		case 2:
			ReturnValue = reinterpret_cast<intptr_t(*)(void*, void*)>(Func)(Args[0], Args[1]);
			break;
		case 3:
			ReturnValue = reinterpret_cast<intptr_t(*)(void*, void*, void*)>(Func)(Args[0], Args[1], Args[2]);
			break;
		case 4:
			ReturnValue = reinterpret_cast<intptr_t(*)(void*, void*, void*, void*)>(Func)(
				Args[0], Args[1], Args[2], Args[3]);
			break;
		case 5:
			ReturnValue = reinterpret_cast<intptr_t(*)(void*, void*, void*, void*, void*)>(Func)(
				Args[0], Args[1], Args[2], Args[3], Args[4]);
			break;
		case 6:
			ReturnValue = reinterpret_cast<intptr_t(*)(void*, void*, void*, void*, void*, void*)>(Func)(
				Args[0], Args[1], Args[2], Args[3], Args[4], Args[5]);
			break;
		case 7:
			ReturnValue = reinterpret_cast<intptr_t(*)(void*, void*, void*, void*, void*, void*, void*)>(Func)(
				Args[0], Args[1], Args[2], Args[3], Args[4], Args[5], Args[6]);
			break;
		case 8:
			ReturnValue = reinterpret_cast<intptr_t(*)(void*, void*, void*, void*, void*, void*, void*, void*)>(Func)(
				Args[0], Args[1], Args[2], Args[3], Args[4], Args[5], Args[6], Args[7]);
			break;
		default:
			// UE bindings never exceed a handful of args; widen this ladder if that ever changes.
			UE_LOG(LogUnrealCSharp, Warning, TEXT("FLeanCLRDomain: binding P/Invoke arity %d not dispatched"),
			       static_cast<int32>(ParameterCount));

			return RtErr::NotImplemented;
		}

		// Write the captured return back into the eval-stack slot. Sub-word integers are sign/zero
		// extended to the I4 stack width; pointer/int64/ref take the full 8 bytes; void writes nothing.
		OutReturn->u64 = 0;

		switch (ReturnType)
		{
		case metadata::RtArgOrLocOrFieldReduceType::Void:
			break;
		case metadata::RtArgOrLocOrFieldReduceType::I1:
			OutReturn->i32 = static_cast<int32>(static_cast<int8>(ReturnValue));
			break;
		case metadata::RtArgOrLocOrFieldReduceType::U1:
			OutReturn->i32 = static_cast<int32>(static_cast<uint8>(ReturnValue));
			break;
		case metadata::RtArgOrLocOrFieldReduceType::I2:
			OutReturn->i32 = static_cast<int32>(static_cast<int16>(ReturnValue));
			break;
		case metadata::RtArgOrLocOrFieldReduceType::U2:
			OutReturn->i32 = static_cast<int32>(static_cast<uint16>(ReturnValue));
			break;
		case metadata::RtArgOrLocOrFieldReduceType::I4:
			OutReturn->i32 = static_cast<int32>(ReturnValue);
			break;
		default: // I8 / I / Ref
			OutReturn->u64 = static_cast<uint64>(ReturnValue);
			break;
		}

		RET_VOID_OK();
	}
}

void FLeanCLRDomain::Initialize()
{
	if (bIsInitialized)
	{
		return;
	}

	// Seed the file-loader search set every time so a later domain picks up any changed search dirs
	// (the loader itself is process-global; see the once-guard below).
	GSearchDirectories = FLeanCLRFunctionLibrary::GetAssemblySearchDirectories();

	// leanclr's Runtime is a PROCESS-GLOBAL singleton and Runtime::shutdown() is a no-op (see
	// Deinitialize), so the heavy bring-up must run exactly once per process. The editor tears the
	// dynamic-class domain down (Deinitialize, which does NOT shut leanclr down) and PIE then creates a
	// fresh FLeanCLRDomain; calling Runtime::initialize() a second time re-runs corlib/metadata/GC init
	// on live global state and corrupts it, so managed execution afterwards throws BadImageFormatException.
	// Guard the one-time steps; a later domain reuses the still-initialized runtime and merely re-resolves
	// its own bridge handles + assemblies below (Assembly::load_by_name returns the cached module).
	static bool GRuntimeBroughtUp = false;

	if (!GRuntimeBroughtUp)
	{
		leanclr::vm::Settings::set_file_loader(&LeanCLRFileLoader);

		// Wire native diagnostics first so any failure below is visible in the Output Log.
		RegisterLog();

		if (auto InitializeResult = leanclr::vm::Runtime::initialize(); InitializeResult.is_err())
		{
			UE_LOG(LogUnrealCSharp, Error, TEXT("FLeanCLRDomain: vm::Runtime::initialize failed (err=%d)"),
			       static_cast<int32>(InitializeResult.unwrap_err()));

			return;
		}

		UE_LOG(LogUnrealCSharp, Log, TEXT("FLeanCLRDomain: runtime initialized"));

		// Reverse-channel registration must precede loading assemblies that bind those P/Invokes (matches
		// leanrun: register right after Runtime::initialize(), before the first assembly load). It writes
		// into leanclr's process-global P/Invoke map, so it is one-time too (re-running would double-
		// register and grow the name storage unboundedly across PIE sessions).
		RegisterPInvokes();

		GRuntimeBroughtUp = true;
	}
	else
	{
		UE_LOG(LogUnrealCSharp, Log,
		       TEXT("FLeanCLRDomain: reusing process-global leanclr runtime (re-init skipped)"));
	}

	auto InteropAssembly = leanclr::vm::Assembly::load_by_name("Interop");

	if (InteropAssembly.is_err() || InteropAssembly.unwrap() == nullptr)
	{
		UE_LOG(LogUnrealCSharp, Error, TEXT("FLeanCLRDomain: failed to load Interop assembly"));

		return;
	}

	InteropModule = InteropAssembly.unwrap()->mod;

	ResolveBridgeMethods();

	// Pre-flight diagnostic: InitializeLeanCLR throws TypeLoadException on Android.
	// Probe each BCL type the method body touches (Console, TextWriter, StringBuilder,
	// Encoding) so we know exactly which one leanclr cannot resolve.
	if (Bridge.TypeBridgeGetClass != nullptr)
	{
		static const TPair<const char*, const char*> TypeProbes[] = {
			{"System",             "Console"},
			{"System.IO",          "TextWriter"},
			{"System.Text",        "StringBuilder"},
			{"System.Text",        "Encoding"},
			{"System",             "Boolean"},       // LogBridge(bool) ctor param
		};
		for (const auto& [Ns, Name] : TypeProbes)
		{
			const auto ProbeHandle = GetClass(UTF8_TO_TCHAR(Ns), UTF8_TO_TCHAR(Name));
			UE_LOG(LogUnrealCSharp, Log,
			       TEXT("FLeanCLRDomain: type probe %s.%s -> %s"),
			       UTF8_TO_TCHAR(Ns), UTF8_TO_TCHAR(Name),
			       IManagedHandleIsValid(ProbeHandle) ? TEXT("OK") : TEXT("NOT FOUND"));
			if (IManagedHandleIsValid(ProbeHandle))
			{
				Free(ProbeHandle);
			}
		}
	}

	// Redirect C# Console.Out/Error into the UE log (LogBridge). LeanCLR variant only: it neither reads
	// Console.Out (that would materialize the OSEncoding console writer -> unimplemented Kernel32
	// P/Invokes -> leanclr fatal) nor uses the SetLog unmanaged-calli path (R5); LogBridge.Flush falls
	// through to the LogLeanCLR named P/Invoke registered in RegisterPInvokes. Without this redirect,
	// any C# Console.WriteLine (e.g. every unit test's output) crashes the process in leanclr.
	if (const auto InitializeLeanCLRLog = ResolveIn(InteropModule,
		COMBINE_FULL_NAME(NAMESPACE_INTEROP, CLASS_LOG_BRIDGE), FString(TEXT("InitializeLeanCLR"))))
	{
		FLeanCLRMarshal::FStackObject LogRet{};

		if (FLeanCLRMarshal::Invoke(InitializeLeanCLRLog, nullptr, 0, LogRet))
		{
			UE_LOG(LogUnrealCSharp, Log,
			       TEXT("FLeanCLRDomain: Console.Out/Error redirected to UE log via LogBridge"));
		}
		else
		{
			UE_LOG(LogUnrealCSharp, Warning,
			       TEXT("FLeanCLRDomain: LogBridge.InitializeLeanCLR invoke failed; C# Console.WriteLine ")
			       TEXT("will hit the unimplemented console-encoding path"));
		}
	}

	bIsInitialized = true;

	// Live self-test (= spike [3]): prove the invoke channel is actually alive inside the UE process,
	// not merely that handles resolved. StringBridge.NewString exercises GCHandle/Lock/CWT/Marshal;
	// GetString reads it back. Non-fatal on failure (logged), since P5 method bodies are still stubs.
	if (Bridge.StringBridgeNewString != nullptr && Bridge.StringBridgeGetString != nullptr &&
		Bridge.HandleDataFree != nullptr)
	{
		const char* Probe = "Hello LeanCLR";

		FLeanCLRMarshal::FStackObject NewArgs[1] = {FLeanCLRMarshal::FromPointer(Probe)};

		if (FLeanCLRMarshal::FStackObject NewRet{}; FLeanCLRMarshal::Invoke(Bridge.StringBridgeNewString, NewArgs, 1, NewRet))
		{
			const auto Handle = FLeanCLRMarshal::ToHandle(NewRet);

			char16_t Buffer[64] = {};

			FLeanCLRMarshal::FStackObject GetArgs[3] = {
				FLeanCLRMarshal::FromHandle(Handle),
				FLeanCLRMarshal::FromPointer(Buffer),
				FLeanCLRMarshal::FromInt32(UE_ARRAY_COUNT(Buffer))
			};

			if (FLeanCLRMarshal::FStackObject GetRet{}; FLeanCLRMarshal::Invoke(Bridge.StringBridgeGetString, GetArgs, 3, GetRet))
			{
				UE_LOG(LogUnrealCSharp, Log,
				       TEXT("FLeanCLRDomain: self-test StringBridge.NewString/GetString round-trip OK (len=%d)"),
				       FLeanCLRMarshal::ToInt32(GetRet));
			}
			else
			{
				UE_LOG(LogUnrealCSharp, Warning, TEXT("FLeanCLRDomain: self-test GetString failed"));
			}

			FLeanCLRMarshal::FStackObject FreeArgs[1] = {FLeanCLRMarshal::FromHandle(Handle)};

			FLeanCLRMarshal::FStackObject FreeRet{};

			FLeanCLRMarshal::Invoke(Bridge.HandleDataFree, FreeArgs, 1, FreeRet);
		}
		else
		{
			UE_LOG(LogUnrealCSharp, Warning, TEXT("FLeanCLRDomain: self-test NewString failed"));
		}
	}

	InitializeAssembly(FUnrealCSharpFunctionLibrary::GetFullAssemblyPublishPath());

	// Diagnostic (P5 override bring-up): the whole reflection layer + C#-override binding depend on
	// TypeBridge.GetClass resolving managed UE/Game types AND on the descriptor/method bridges parsing
	// that type. Every miss along this chain is SILENT (GetClass returns 0 / an attribute class stays
	// null / a method's IsOverride stays false — no exception), which is exactly why PIE showed "no crash,
	// no Test". So walk the same chain FCSharpBind::CanBind + BindImplementation walk, in-process, and log
	// each stage. This is the authoritative triage: it tells us precisely which link is broken.
	//
	// Chain (mirrors FCSharpBind):
	//   1. GetClass(UE Utils) / GetClass(Game override type)      -> TypeBridge.GetClass resolves the Type
	//   2. Override/UFunction attribute classes resolved          -> gate FMethodReflection::bIsOverride
	//   3. FReflectionRegistry class + class-level IsOverride      -> CanBind()'s decision
	//   4. GetClassDescriptor + GetClassMethods bridge parse       -> P5 bridges, first runtime exercise
	//   5. per-method {name, params, UFunction, IsOverride}        -> whether "Test" would bind
	DiagnoseOverrideBinding();
}

void FLeanCLRDomain::DiagnoseOverrideBinding()
{
	// [1] Raw TypeBridge.GetClass on a known UE type: proves managed UE-type resolution is alive after
	// the Interop load-context fix (AppDomain.CurrentDomain fallback in TypeBridge.GetTypeImplementation).
	const auto UtilsProbe = GetClass(
		COMBINE_NAMESPACE(NAMESPACE_ROOT, NAMESPACE_CORE_UOBJECT), CLASS_UTILS);

	UE_LOG(LogUnrealCSharp, Log,
	       TEXT("FLeanCLRDomain: [probe 1a] GetClass 'Script.CoreUObject.Utils' -> %s"),
	       IManagedHandleIsValid(UtilsProbe) ? TEXT("resolved") : TEXT("INVALID"));

	if (IManagedHandleIsValid(UtilsProbe))
	{
		Free(UtilsProbe);
	}

	// [2] The attribute classes that gate override detection. If GetClass fails for UE types these stay
	// null, and then EVERY method's bIsOverride is false even when the type itself parses — a silent
	// binding failure. Resolved during FReflectionRegistry::Initialize() just above.
	const auto* OverrideAttr = FReflectionRegistry::Get().GetOverrideAttributeClass();

	const auto* UFunctionAttr = FReflectionRegistry::Get().GetUFunctionAttributeClass();

	UE_LOG(LogUnrealCSharp, Log,
	       TEXT("FLeanCLRDomain: [probe 2] OverrideAttributeClass=%s UFunctionAttributeClass=%s"),
	       OverrideAttr != nullptr ? TEXT("resolved") : TEXT("NULL"),
	       UFunctionAttr != nullptr ? TEXT("resolved") : TEXT("NULL"));

	// [3] Drive the exact FReflectionRegistry path CanBind() uses. GetClass(ns,name) here mirrors what
	// GetClass(UStruct*) computes for the C++ UUnitTestSubsystem UCLASS (namespace "Script.<Module>",
	// name = class name), so a divergence would surface as a null here.
	auto* ClassReflection = FReflectionRegistry::Get().GetClass(
		TEXT("Script.UnrealCSharpTest"), TEXT("UUnitTestSubsystem"));

	if (ClassReflection == nullptr)
	{
		UE_LOG(LogUnrealCSharp, Warning,
		       TEXT("FLeanCLRDomain: [probe 3] FReflectionRegistry has NO 'Script.UnrealCSharpTest.")
		       TEXT("UUnitTestSubsystem' -> TypeBridge.GetClass returned 0; C#-override binding cannot ")
		       TEXT("proceed. Root cause is managed type resolution (Interop fallback / assembly load)."));

		return;
	}

	// [4] Class-level override flag = CanBind()'s gate. Then force the descriptor + method parse (the P5
	// GetClassDescriptor / GetClassMethods bridges' first real runtime exercise). Logged before the parse
	// so an interpreter fatal here is pinpointed to this exact call rather than looking like a silent hang.
	UE_LOG(LogUnrealCSharp, Log,
	       TEXT("FLeanCLRDomain: [probe 3] FReflectionRegistry resolved UUnitTestSubsystem; class-level ")
	       TEXT("IsOverride=%d (this is CanBind's gate)"), ClassReflection->IsOverride() ? 1 : 0);

	UE_LOG(LogUnrealCSharp, Log,
	       TEXT("FLeanCLRDomain: [probe 4] parsing methods via GetClassDescriptor/GetClassMethods bridges ..."));

	const auto& Methods = ClassReflection->GetMethods();

	UE_LOG(LogUnrealCSharp, Log,
	       TEXT("FLeanCLRDomain: [probe 4] parsed %d method(s) for UUnitTestSubsystem"), Methods.Num());

	// [5] Whether "Test" specifically is present and flagged as an override — the single fact that decides
	// if FCSharpBind binds it to execCallCSharp. If this shows Test/IsOverride=1, binding WILL happen and
	// the remaining question is purely the invoke path; if Test is absent or IsOverride=0, binding is the
	// break point.
	bool bFoundTest = false;

	for (const auto& [Key, Method] : Methods)
	{
		if (Method == nullptr)
		{
			continue;
		}

		const bool bIsTest = Key.Key == TEXT("Test");

		UE_LOG(LogUnrealCSharp, Log,
		       TEXT("FLeanCLRDomain: [probe 5] method '%s' params=%d UFunction=%d IsOverride=%d%s"),
		       *Key.Key, Key.Value, Method->IsUFunction() ? 1 : 0, Method->IsOverride() ? 1 : 0,
		       bIsTest ? TEXT("  <-- Test") : TEXT(""));

		bFoundTest |= bIsTest;
	}

	if (!bFoundTest)
	{
		UE_LOG(LogUnrealCSharp, Warning,
		       TEXT("FLeanCLRDomain: [probe 5] 'Test' NOT found among parsed methods; the C# override ")
		       TEXT("will not bind. Check the GetClassMethods bridge output / method name mangling."));
	}

	// [6] Raw method-level reflection truth for the exact methods that decide binding. Probe 5's
	// IsOverride=0 is three indirections from the metal (IsDefined / structural fallback / attribute-array
	// parse), and offline CoreCLR probing of the very same Game.dll proves its metadata is healthy
	// (Test = MethodDef 0x06001156, flags 0x00C6 = public virtual hidebysig non-newslot, [Override]
	// present). So SOME primitive in leanclr's method-level reflection must be reporting wrong values
	// in-process — this dump names which one. Controls: ToString/Equals (no attributes, virtual
	// non-newslot, declared on UObject in UE.dll) vs Test ([Override], declared in Game.dll).
	// Slot layout mirrors Utils.DumpMethodReflection; -2 means the managed call threw.
	if (Bridge.UtilsDumpMethodReflection == nullptr)
	{
		UE_LOG(LogUnrealCSharp, Warning,
		       TEXT("FLeanCLRDomain: [probe 6] Utils.DumpMethodReflection bridge unresolved (stale UE.dll?) ")
		       TEXT("- raw method-level reflection dump skipped."));

		return;
	}

	const auto ManagedClass = ClassReflection->GetManagedClass();

	// Lifetime counters from GetClassMethodsImplementation's LeanCLR workaround block (slots 13/14):
	// populated by probe 4's parse above; non-zero proves the block RAN in the loaded UE.dll (not merely
	// that it was compiled in — a stale UE.dll is a real failure mode here).
	int64 IsDefinedHitCounter = -1, StructuralOverrideCounter = -1;

	for (const char* ProbeName : {"Test", "ToString", "Equals"})
	{
		int64 Slots[16] = {};

		FLeanCLRMarshal::FStackObject Args[3] = {
			FLeanCLRMarshal::FromHandle(ManagedClass),
			FLeanCLRMarshal::FromPointer(ProbeName),
			FLeanCLRMarshal::FromPointer(Slots)
		};

		FLeanCLRMarshal::FStackObject Return{};

		if (!FLeanCLRMarshal::Invoke(Bridge.UtilsDumpMethodReflection, Args, 3, Return))
		{
			UE_LOG(LogUnrealCSharp, Warning,
			       TEXT("FLeanCLRDomain: [probe 6] DumpMethodReflection invoke failed for '%s'"),
			       UTF8_TO_TCHAR(ProbeName));

			continue;
		}

		UE_LOG(LogUnrealCSharp, Log,
		       TEXT("FLeanCLRDomain: [probe 6] '%s': matches=%lld isMethodInfo=%lld IsVirtual=%lld ")
		       TEXT("IsAbstract=%lld NewSlot=%lld Attributes=0x%llX token=0x%llX IsDefined(Override)=%lld ")
		       TEXT("CA.Count=%lld IsStatic=%lld declaredHere=%lld classIsDefined(Override)=%lld params=%lld ")
		       TEXT("totalMethods=%lld"),
		       UTF8_TO_TCHAR(ProbeName),
		       Slots[0], Slots[1], Slots[2], Slots[3], Slots[4],
		       static_cast<uint64>(Slots[5]) & 0xFFFF, static_cast<uint64>(Slots[6]),
		       Slots[7], Slots[8], Slots[9], Slots[10], Slots[11], Slots[12], Slots[15]);

		IsDefinedHitCounter = Slots[13];

		StructuralOverrideCounter = Slots[14];
	}

	UE_LOG(LogUnrealCSharp, Log,
	       TEXT("FLeanCLRDomain: [probe 6] workaround counters: IsDefinedHits=%lld ")
	       TEXT("StructuralOverrideFallback=%lld"), IsDefinedHitCounter, StructuralOverrideCounter);

	// [7] Probe 6 proved the managed side detects Test's [Override] (IsDefined=1, CA.Count=1) and the
	// workaround counters prove the LEANCLR attribute block ran — yet probe 5's FMethodReflection.IsOverride
	// is still 0. So the break sits between the C# bridge output and the C++ attribute-set match, a chain
	// FClassReflection::ParseMethods walks silently: attribute-count int[] -> attribute Type[] element
	// handle -> GetFullName(element) -> FReflectionRegistry::GetClass(handle) pointer identity vs
	// GetOverrideAttributeClass(). Re-run GetClassMethods raw (exactly like EnsureMethodsImplementation)
	// and log every link for 'Test' so the broken one names itself.
	{
		IManagedHandle RawParams[15] = {};

		RawParams[0] = ManagedClass;

		GetClassMethods(ManagedClass, reinterpret_cast<PTRINT*>(&RawParams[1]));

		const int32 RawMethodCount = static_cast<int32>(RawParams[1].Value);

		UE_LOG(LogUnrealCSharp, Log,
		       TEXT("FLeanCLRDomain: [probe 7] raw GetClassMethods: methods=%d countsArray=%d attrsArray=%d"),
		       RawMethodCount, IManagedHandleIsValid(RawParams[11]) ? 1 : 0,
		       IManagedHandleIsValid(RawParams[12]) ? 1 : 0);

		const auto* OverrideAttrClass = FReflectionRegistry::Get().GetOverrideAttributeClass();

		UE_LOG(LogUnrealCSharp, Log,
		       TEXT("FLeanCLRDomain: [probe 7] registry OverrideAttributeClass=%p itsManagedFullName='%s'"),
		       OverrideAttrClass,
		       OverrideAttrClass != nullptr
			       ? *GetFullName(OverrideAttrClass->GetManagedClass())
			       : TEXT(""));

		int32 RawAttrIndex = 0;

		bool bTestDumped = false;

		for (int32 MethodIndex = 0; MethodIndex < RawMethodCount; ++MethodIndex)
		{
			const auto NameHandle = ArrayGet(RawParams[2], MethodIndex);

			const FString MethodName = IManagedHandleIsValid(NameHandle) ? StringToFString(NameHandle) : FString{};

			if (IManagedHandleIsValid(NameHandle))
			{
				Free(NameHandle);
			}

			int32 AttrCount = 0;

			if (const auto CountHandle = ArrayGet(RawParams[11], MethodIndex); IManagedHandleIsValid(CountHandle))
			{
				if (void* CountPtr = UnboxValue(CountHandle); CountPtr != nullptr)
				{
					AttrCount = *static_cast<int32*>(CountPtr);
				}

				Free(CountHandle);
			}

			if (MethodName == TEXT("Test") && !bTestDumped)
			{
				bTestDumped = true;

				UE_LOG(LogUnrealCSharp, Log,
				       TEXT("FLeanCLRDomain: [probe 7] 'Test' at index=%d attrCount=%d (flat attr start=%d)"),
				       MethodIndex, AttrCount, RawAttrIndex);

				for (int32 AttrIndex = 0; AttrIndex < AttrCount; ++AttrIndex)
				{
					const auto Element = ArrayGet(RawParams[12], RawAttrIndex + AttrIndex);

					if (!IManagedHandleIsValid(Element))
					{
						UE_LOG(LogUnrealCSharp, Warning,
						       TEXT("FLeanCLRDomain: [probe 7] attr[%d]: element handle INVALID (ArrayGet on ")
						       TEXT("the attribute Type[] failed)"), AttrIndex);

						continue;
					}

					// NOTE: GetFullName/GetName run BEFORE GetClass transfers the handle's ownership.
					const FString ElementFullName = GetFullName(Element);

					const FString ElementName = GetName(Element);

					auto* ElementClass = FReflectionRegistry::Get().GetClass(Element);

					UE_LOG(LogUnrealCSharp, Log,
					       TEXT("FLeanCLRDomain: [probe 7] attr[%d]: fullName='%s' name='%s' regClass=%p ")
					       TEXT("vs OverrideAttributeClass=%p identical=%d"),
					       AttrIndex, *ElementFullName, *ElementName, ElementClass, OverrideAttrClass,
					       ElementClass != nullptr && ElementClass == OverrideAttrClass ? 1 : 0);
				}
			}

			RawAttrIndex += AttrCount;
		}

		// Mirror ParseMethods' cleanup: the array handles live in slots 2..14 (slot 1 is a raw count).
		for (int32 Slot = 2; Slot <= 14; ++Slot)
		{
			if (IManagedHandleIsValid(RawParams[Slot]))
			{
				Free(RawParams[Slot]);
			}
		}
	}
}

void FLeanCLRDomain::Deinitialize()
{
	if (!bIsInitialized)
	{
		return;
	}

	bIsInitialized = false;

	UnloadAssembly();

	// NOTE: vm::Runtime::shutdown() is intentionally NOT called this pass. The reference host (leanrun)
	// never calls it, and shutdown -> re-initialize reentrancy (hot reload re-inits the domain) is
	// unverified; calling it risks a crash on the next Initialize. Full teardown is deferred until
	// leanclr reentrancy is confirmed. UnloadAssembly clears our own handles/state.
}

void FLeanCLRDomain::Tick(const float InDeltaTime)
{
	if (!bIsInitialized || Bridge.SynchronizationContextTick == nullptr)
	{
		return;
	}

	FLeanCLRMarshal::FStackObject Args[1] = {FLeanCLRMarshal::FromFloat(InDeltaTime)};

	FLeanCLRMarshal::FStackObject Ret{};

	FLeanCLRMarshal::Invoke(Bridge.SynchronizationContextTick, Args, 1, Ret);
}

FString FLeanCLRDomain::GetNamespace(const IManagedHandle InManagedClass)
{
	if (IManagedHandleIsValid(InManagedClass) && Bridge.TypeBridgeGetNamespace != nullptr)
	{
		constexpr auto Size = 512;

		uint8 String[Size];

		FLeanCLRMarshal::FStackObject Args[3] = {
			FLeanCLRMarshal::FromHandle(InManagedClass),
			FLeanCLRMarshal::FromPointer(String),
			FLeanCLRMarshal::FromInt32(Size)
		};

		if (FLeanCLRMarshal::FStackObject Return{}; FLeanCLRMarshal::Invoke(Bridge.TypeBridgeGetNamespace, Args, 3, Return))
		{
			if (FLeanCLRMarshal::ToInt32(Return) > 0)
			{
				return FString(UTF8_TO_TCHAR(reinterpret_cast<const char*>(String)));
			}
		}
	}

	return {};
}

FString FLeanCLRDomain::GetName(const IManagedHandle InManagedClass)
{
	if (IManagedHandleIsValid(InManagedClass) && Bridge.TypeBridgeGetName != nullptr)
	{
		constexpr auto Size = 512;

		uint8 String[Size];

		FLeanCLRMarshal::FStackObject Args[3] = {
			FLeanCLRMarshal::FromHandle(InManagedClass),
			FLeanCLRMarshal::FromPointer(String),
			FLeanCLRMarshal::FromInt32(Size)
		};

		if (FLeanCLRMarshal::FStackObject Return{}; FLeanCLRMarshal::Invoke(Bridge.TypeBridgeGetName, Args, 3, Return))
		{
			if (FLeanCLRMarshal::ToInt32(Return) > 0)
			{
				return FString(UTF8_TO_TCHAR(reinterpret_cast<const char*>(String)));
			}
		}
	}

	return {};
}

FString FLeanCLRDomain::GetFullName(const IManagedHandle InManagedClass)
{
	if (IManagedHandleIsValid(InManagedClass) && Bridge.TypeBridgeGetFullName != nullptr)
	{
		constexpr auto Size = 512;

		uint8 String[Size];

		FLeanCLRMarshal::FStackObject Args[3] = {
			FLeanCLRMarshal::FromHandle(InManagedClass),
			FLeanCLRMarshal::FromPointer(String),
			FLeanCLRMarshal::FromInt32(Size)
		};

		if (FLeanCLRMarshal::FStackObject Return{}; FLeanCLRMarshal::Invoke(Bridge.TypeBridgeGetFullName, Args, 3, Return))
		{
			if (FLeanCLRMarshal::ToInt32(Return) > 0)
			{
				auto Result = FString(UTF8_TO_TCHAR(reinterpret_cast<const char*>(String)));

				// The managed full name can carry an assembly-qualified suffix ", Assembly, ..."; keep only
				// the type portion (mirrors FScriptDomainImpl.inl::GetFullName).
				if (int32 Index; Result.FindLastChar(TEXT(','), Index))
				{
					Result = Result.Left(Index).TrimEnd();
				}

				return Result;
			}
		}
	}

	return {};
}

IManagedHandle FLeanCLRDomain::NewObject(const IManagedHandle InManagedClass)
{
	if (Bridge.ObjectBridgeNewObject != nullptr)
	{
		FLeanCLRMarshal::FStackObject Args[1] = {FLeanCLRMarshal::FromHandle(InManagedClass)};

		return InvokeForHandle(Bridge.ObjectBridgeNewObject, Args, 1);
	}

	return InvalidManagedHandle;
}

IManagedHandle FLeanCLRDomain::BoxValue(const FString& InNamespace, const FString& InName, void* InValue)
{
	// Every TypeBridge.Box* entry takes a pointer to the raw value (e.g. BoxInt32(int* InValue)) and returns
	// the boxed handle, so the packing is identical across types — only the target bridge method differs by
	// the C# type name. Mirrors the name matching in FScriptDomainImpl.inl::BoxValue.
	if (InValue != nullptr)
	{
		const leanclr::metadata::RtMethodInfo* BoxMethod = nullptr;

		if (InName == TEXT("bool") || InName == TEXT("Boolean"))
		{
			BoxMethod = Bridge.TypeBridgeBoxBool;
		}
		else if (InName == TEXT("sbyte") || InName == TEXT("SByte") || InName == TEXT("int8"))
		{
			BoxMethod = Bridge.TypeBridgeBoxSByte;
		}
		else if (InName == TEXT("int16") || InName == TEXT("Int16") || InName == TEXT("short"))
		{
			BoxMethod = Bridge.TypeBridgeBoxInt16;
		}
		else if (InName == TEXT("int32") || InName == TEXT("Int32") || InName == TEXT("int"))
		{
			BoxMethod = Bridge.TypeBridgeBoxInt32;
		}
		else if (InName == TEXT("int64") || InName == TEXT("Int64") || InName == TEXT("long"))
		{
			BoxMethod = Bridge.TypeBridgeBoxInt64;
		}
		else if (InName == TEXT("byte") || InName == TEXT("Byte") || InName == TEXT("uint8"))
		{
			BoxMethod = Bridge.TypeBridgeBoxByte;
		}
		else if (InName == TEXT("uint16") || InName == TEXT("UInt16") || InName == TEXT("ushort"))
		{
			BoxMethod = Bridge.TypeBridgeBoxUInt16;
		}
		else if (InName == TEXT("uint32") || InName == TEXT("UInt32") || InName == TEXT("uint"))
		{
			BoxMethod = Bridge.TypeBridgeBoxUInt32;
		}
		else if (InName == TEXT("uint64") || InName == TEXT("UInt64") || InName == TEXT("ulong"))
		{
			BoxMethod = Bridge.TypeBridgeBoxUInt64;
		}
		else if (InName == TEXT("float") || InName == TEXT("Single"))
		{
			BoxMethod = Bridge.TypeBridgeBoxFloat;
		}
		else if (InName == TEXT("double") || InName == TEXT("Double"))
		{
			BoxMethod = Bridge.TypeBridgeBoxDouble;
		}

		if (BoxMethod != nullptr)
		{
			FLeanCLRMarshal::FStackObject Args[1] = {FLeanCLRMarshal::FromPointer(InValue)};

			return InvokeForHandle(BoxMethod, Args, 1);
		}
	}

	return InvalidManagedHandle;
}

void* FLeanCLRDomain::UnboxValue(const IManagedHandle InManagedHandle)
{
	// Shared scratch mirroring FScriptDomainImpl.inl::UnboxValue: try each TypeBridge.Unbox* in turn (each
	// returns non-zero only when the boxed value actually matches that type), writing the unboxed value into
	// Result and returning &Result. Every Unbox* takes (nint InHandle, T* OutValue) and returns int success.
	static uint64 Result{};

	if (IManagedHandleIsValid(InManagedHandle))
	{
		const auto TryUnbox = [InManagedHandle](const leanclr::metadata::RtMethodInfo* InMethod, void* OutValue) -> bool
		{
			if (InMethod == nullptr)
			{
				return false;
			}

			FLeanCLRMarshal::FStackObject Args[2] = {
				FLeanCLRMarshal::FromHandle(InManagedHandle),
				FLeanCLRMarshal::FromPointer(OutValue)
			};

			FLeanCLRMarshal::FStackObject Return{};

			return FLeanCLRMarshal::Invoke(InMethod, Args, 2, Return) && FLeanCLRMarshal::ToInt32(Return) != 0;
		};

		// bool is boxed/unboxed through an int* (nonzero = true); narrow it back into Result as a bool.
		if (int32 Value{}; TryUnbox(Bridge.TypeBridgeUnboxBool, &Value))
		{
			*static_cast<bool*>(static_cast<void*>(&Result)) = Value != 0;

			return &Result;
		}

		if (TryUnbox(Bridge.TypeBridgeUnboxSByte, &Result) ||
			TryUnbox(Bridge.TypeBridgeUnboxInt16, &Result) ||
			TryUnbox(Bridge.TypeBridgeUnboxInt32, &Result) ||
			TryUnbox(Bridge.TypeBridgeUnboxInt64, &Result) ||
			TryUnbox(Bridge.TypeBridgeUnboxByte, &Result) ||
			TryUnbox(Bridge.TypeBridgeUnboxUInt16, &Result) ||
			TryUnbox(Bridge.TypeBridgeUnboxUInt32, &Result) ||
			TryUnbox(Bridge.TypeBridgeUnboxUInt64, &Result) ||
			TryUnbox(Bridge.TypeBridgeUnboxFloat, &Result) ||
			TryUnbox(Bridge.TypeBridgeUnboxDouble, &Result))
		{
			return &Result;
		}
	}

	return nullptr;
}

IManagedHandle FLeanCLRDomain::NewString(const char* InText)
{
	if (Bridge.StringBridgeNewString != nullptr && InText != nullptr)
	{
		FLeanCLRMarshal::FStackObject Args[1] = {FLeanCLRMarshal::FromPointer(InText)};

		return InvokeForHandle(Bridge.StringBridgeNewString, Args, 1);
	}

	return InvalidManagedHandle;
}

FString FLeanCLRDomain::StringToFString(const IManagedHandle InManagedHandle)
{
	if (IManagedHandleIsValid(InManagedHandle) && Bridge.StringBridgeGetString != nullptr)
	{
		// StringBridge.GetString(nint, char* InBuffer, int InSize) writes UTF-16 and returns the length
		// (or <=0 on failure / empty). Try a stack buffer first, then a heap buffer for long strings —
		// mirrors FScriptDomainImpl.inl::StringToFString.
		constexpr auto Size = 1024;

		char16_t String[Size];

		{
			FLeanCLRMarshal::FStackObject Args[3] = {
				FLeanCLRMarshal::FromHandle(InManagedHandle),
				FLeanCLRMarshal::FromPointer(String),
				FLeanCLRMarshal::FromInt32(Size)
			};

			if (FLeanCLRMarshal::FStackObject Return{}; FLeanCLRMarshal::Invoke(Bridge.StringBridgeGetString, Args, 3, Return))
			{
				if (const auto Length = FLeanCLRMarshal::ToInt32(Return); Length > 0)
				{
					return FString(StringCast<TCHAR>(reinterpret_cast<const UTF16CHAR*>(String), Length).Get(), Length);
				}
			}
		}

		constexpr auto MaxSize = 65536;

		TArray<char16_t> StringArray;

		StringArray.SetNumUninitialized(MaxSize);

		FLeanCLRMarshal::FStackObject Args[3] = {
			FLeanCLRMarshal::FromHandle(InManagedHandle),
			FLeanCLRMarshal::FromPointer(StringArray.GetData()),
			FLeanCLRMarshal::FromInt32(MaxSize)
		};

		if (FLeanCLRMarshal::FStackObject Return{}; FLeanCLRMarshal::Invoke(Bridge.StringBridgeGetString, Args, 3, Return))
		{
			if (const auto Length = FLeanCLRMarshal::ToInt32(Return); Length > 0)
			{
				return FString(StringCast<TCHAR>(
					               reinterpret_cast<const UTF16CHAR*>(StringArray.GetData()), Length).Get(), Length);
			}
		}
	}

	return {};
}

void FLeanCLRDomain::Free(const IManagedHandle InManagedHandle)
{
	if (IManagedHandleIsValid(InManagedHandle) && Bridge.HandleDataFree != nullptr)
	{
		FLeanCLRMarshal::FStackObject Args[1] = {FLeanCLRMarshal::FromHandle(InManagedHandle)};

		FLeanCLRMarshal::FStackObject Return{};

		FLeanCLRMarshal::Invoke(Bridge.HandleDataFree, Args, 1, Return);
	}
}

IManagedHandle FLeanCLRDomain::NewArray(const FString& InNamespace, const FString& InName, const int32 InLength)
{
	if (InLength > 0 && Bridge.ArrayBridgeNewArray != nullptr)
	{
		const auto FullName = StringCast<UTF8CHAR>(*COMBINE_FULL_NAME(InNamespace, InName));

		FLeanCLRMarshal::FStackObject Args[2] = {
			FLeanCLRMarshal::FromPointer(FullName.Get()),
			FLeanCLRMarshal::FromInt32(InLength)
		};

		return InvokeForHandle(Bridge.ArrayBridgeNewArray, Args, 2);
	}

	return InvalidManagedHandle;
}

IManagedHandle FLeanCLRDomain::ArrayGet(const IManagedHandle InManagedArray, const int32 InIndex)
{
	if (IManagedHandleIsValid(InManagedArray) && Bridge.ArrayBridgeArrayGet != nullptr)
	{
		FLeanCLRMarshal::FStackObject Args[2] = {
			FLeanCLRMarshal::FromHandle(InManagedArray),
			FLeanCLRMarshal::FromInt32(InIndex)
		};

		return InvokeForHandle(Bridge.ArrayBridgeArrayGet, Args, 2);
	}

	return InvalidManagedHandle;
}

IManagedHandle FLeanCLRDomain::GetClass(const FString& InNamespace, const FString& InName)
{
	if (Bridge.TypeBridgeGetClass != nullptr)
	{
		// Bind the UTF-8 conversion to a named local so its buffer outlives the (synchronous) invoke —
		// TypeBridge.GetClass takes byte* InFullName and reads it during the call.
		const auto FullName = StringCast<UTF8CHAR>(*COMBINE_FULL_NAME(InNamespace, InName));

		FLeanCLRMarshal::FStackObject Args[1] = {FLeanCLRMarshal::FromPointer(FullName.Get())};

		return InvokeForHandle(Bridge.TypeBridgeGetClass, Args, 1);
	}

	return InvalidManagedHandle;
}

IManagedHandle FLeanCLRDomain::GetMethod(const IManagedHandle InManagedClass, const FString& InName,
                                         const int32 InParamCount)
{
	if (IManagedHandleIsValid(InManagedClass) && Bridge.TypeBridgeGetMethod != nullptr)
	{
		const auto Name = StringCast<UTF8CHAR>(*InName);

		FLeanCLRMarshal::FStackObject Args[3] = {
			FLeanCLRMarshal::FromHandle(InManagedClass),
			FLeanCLRMarshal::FromPointer(Name.Get()),
			FLeanCLRMarshal::FromInt32(InParamCount)
		};

		return InvokeForHandle(Bridge.TypeBridgeGetMethod, Args, 3);
	}

	return InvalidManagedHandle;
}

void FLeanCLRDomain::SetFieldStaticValue(const IManagedHandle InManagedClass, const FString& InName, void* InValue)
{
	// FieldBridge.SetStaticValue(nint InHandle, byte* InName, long InValue): the value is a handle stored as
	// a uint32 and widened to the long slot, matching FScriptDomainImpl.inl::SetFieldStaticValue.
	if (IManagedHandleIsValid(InManagedClass) && InValue != nullptr && Bridge.FieldBridgeSetStaticValue != nullptr)
	{
		const auto Name = StringCast<UTF8CHAR>(*InName);

		FLeanCLRMarshal::FStackObject Args[3] = {
			FLeanCLRMarshal::FromHandle(InManagedClass),
			FLeanCLRMarshal::FromPointer(Name.Get()),
			FLeanCLRMarshal::FromInt64(static_cast<int64>(*static_cast<uint32*>(InValue)))
		};

		FLeanCLRMarshal::FStackObject Return{};

		FLeanCLRMarshal::Invoke(Bridge.FieldBridgeSetStaticValue, Args, 3, Return);
	}
}

void* FLeanCLRDomain::GetFieldStaticValue(const IManagedHandle InManagedClass, const FString& InName)
{
	// FieldBridge.GetStaticValue(nint InHandle, byte* InName) -> long: the returned handle value is turned
	// back into an object pointer (mirrors FScriptDomainImpl.inl::GetFieldStaticValue).
	if (IManagedHandleIsValid(InManagedClass) && Bridge.FieldBridgeGetStaticValue != nullptr)
	{
		const auto Name = StringCast<UTF8CHAR>(*InName);

		FLeanCLRMarshal::FStackObject Args[2] = {
			FLeanCLRMarshal::FromHandle(InManagedClass),
			FLeanCLRMarshal::FromPointer(Name.Get())
		};

		if (FLeanCLRMarshal::FStackObject Return{}; FLeanCLRMarshal::Invoke(Bridge.FieldBridgeGetStaticValue, Args, 2, Return))
		{
			return IManagedHandleToObject(IManagedHandle{FLeanCLRMarshal::ToInt64(Return)});
		}
	}

	return nullptr;
}

void FLeanCLRDomain::SetPropertyValue(const IManagedHandle InManagedHandle, const FString& InName, void** InParams)
{
	// Resolve the runtime type of the instance, look up its "set_<Property>" accessor (1 param), and invoke
	// it with the incoming value. Mirrors FScriptDomainImpl.inl::SetPropertyValue.
	if (Bridge.TypeBridgeGetType != nullptr)
	{
		FLeanCLRMarshal::FStackObject TypeArgs[1] = {FLeanCLRMarshal::FromHandle(InManagedHandle)};

		if (const auto Class = InvokeForHandle(Bridge.TypeBridgeGetType, TypeArgs, 1); IManagedHandleIsValid(Class))
		{
			const auto Name = FString::Printf(TEXT("%s%s"), *PROPERTY_SET_PREFIX, *InName);

			if (const auto Method = GetMethod(Class, Name, 1); IManagedHandleIsValid(Method))
			{
				Invoke(InManagedHandle, Method, 1, InParams);
			}
		}
	}
}

FClassReflection* FLeanCLRDomain::MakeGenericType(const FClassReflection* InGeneric, const FClassReflection* InType)
{
	if (InGeneric != nullptr && InType != nullptr && Bridge.TypeBridgeMakeGenericType != nullptr)
	{
		FLeanCLRMarshal::FStackObject Args[2] = {
			FLeanCLRMarshal::FromHandle(InGeneric->GetManagedClass()),
			FLeanCLRMarshal::FromHandle(InType->GetManagedClass())
		};

		return FReflectionRegistry::Get().GetClass(InvokeForHandle(Bridge.TypeBridgeMakeGenericType, Args, 2));
	}

	return nullptr;
}

FClassReflection* FLeanCLRDomain::MakeGenericType(const FClassReflection* InGeneric,
                                                  const FClassReflection* InKeyType,
                                                  const FClassReflection* InValueType)
{
	if (InGeneric != nullptr && InKeyType != nullptr && InValueType != nullptr &&
		Bridge.TypeBridgeMakeGenericType2 != nullptr)
	{
		FLeanCLRMarshal::FStackObject Args[3] = {
			FLeanCLRMarshal::FromHandle(InGeneric->GetManagedClass()),
			FLeanCLRMarshal::FromHandle(InKeyType->GetManagedClass()),
			FLeanCLRMarshal::FromHandle(InValueType->GetManagedClass())
		};

		return FReflectionRegistry::Get().GetClass(InvokeForHandle(Bridge.TypeBridgeMakeGenericType2, Args, 3));
	}

	return nullptr;
}

IManagedHandle FLeanCLRDomain::Invoke(const IManagedHandle InManagedHandle, const IManagedHandle InManagedMethod,
                                      const int32 InParamCount, void** InParams)
{
	if (!IManagedHandleIsValid(InManagedMethod) || Bridge.MethodBridgeInvoke == nullptr)
	{
		return InvalidManagedHandle;
	}

	// Reverse-invoke fix (Option B): leanclr's reflection Method.Invoke (used by MethodBridge.Invoke)
	// drops writes to value-type `ref`/`out` parameters — it unboxes them into a throwaway temp and never
	// re-boxes. For methods that carry such a parameter (and no reference-type by-ref), invoke the C#
	// method through the host-built stackobject path instead, so the by-ref value slot points straight at
	// the caller's storage. Every other method keeps the untouched C# path. Classification is cached per
	// method handle (resolving it is itself a managed bridge call).
	if (Bridge.HandleDataGetObjectPointer != nullptr && Bridge.HandleDataAllocFromObject != nullptr)
	{
		const FReverseMethod* Cached = ReverseMethodCache.Find(InManagedMethod.Value);

		FReverseMethod Entry;

		if (Cached != nullptr)
		{
			Entry = *Cached;
		}
		else
		{
			Entry.Method = FLeanCLRMarshal::ResolveReverseMethod(Bridge.HandleDataGetObjectPointer, InManagedMethod);

			// Take the host-side stackobject path for two shapes: (1) value-type by-ref write-back (Option B,
			// P5 §3.17), and (2) reference-type instance constructors — leanclr's net10 ConstructorInfo.Invoke
			// (existingObject, ...) does not re-run the ctor body on the passed instance, so a C#-defined
			// dynamic class's constructor never populates its spawned/bound UObject; InvokeReverse binds `this`
			// to the existing object and runs the ctor body via invoke_stackobject_arguments (P6.1 §5.6).
			Entry.bUseCppPath = Entry.Method != nullptr &&
				(FLeanCLRMarshal::ReverseMethodNeedsCppPath(Entry.Method) ||
					FLeanCLRMarshal::IsReferenceConstructor(Entry.Method));

			// P6.1 diagnostic: dynamic-actor C# ctor values are not populating even after routing reference
			// ctors through InvokeReverse (CSV byte-identical). Log the classification for reference ctors and
			// for any method whose reverse-method resolution failed, so "ctor never took the C++ path" vs
			// "C++ path taken but ineffective" is disambiguated in one run. Cache-miss only (once per method).
			if (Entry.Method == nullptr || FLeanCLRMarshal::IsReferenceConstructor(Entry.Method))
			{
				UE_LOG(LogUnrealCSharp, Warning,
				       TEXT("FLeanCLRDomain [P6.1 ctor2] methodHandle=%lld resolved=%d isRefCtor=%d cppPath=%d"),
				       static_cast<long long>(InManagedMethod.Value), Entry.Method != nullptr ? 1 : 0,
				       Entry.Method != nullptr && FLeanCLRMarshal::IsReferenceConstructor(Entry.Method) ? 1 : 0,
				       Entry.bUseCppPath ? 1 : 0);
			}

			ReverseMethodCache.Add(InManagedMethod.Value, Entry);
		}

		if (Entry.bUseCppPath)
		{
			return FLeanCLRMarshal::InvokeReverse(Bridge.HandleDataGetObjectPointer, Bridge.HandleDataAllocFromObject,
			                                      Entry.Method, InManagedHandle, InParamCount, InParams);
		}
	}

	// MethodBridge.Invoke(nint InHandle, nint InMethod, int InParamCount, nint* InParams) -> nint. InParams
	// is an array of per-argument nint (pointer/handle) slots; pass its address straight through.
	FLeanCLRMarshal::FStackObject Args[4] = {
		FLeanCLRMarshal::FromHandle(InManagedHandle),
		FLeanCLRMarshal::FromHandle(InManagedMethod),
		FLeanCLRMarshal::FromInt32(InParamCount),
		FLeanCLRMarshal::FromPointer(InParams)
	};

	return InvokeForHandle(Bridge.MethodBridgeInvoke, Args, 4);
}

bool FLeanCLRDomain::IsOverride(const IManagedHandle InManagedClass)
{
	if (Bridge.UtilsIsOverride != nullptr)
	{
		FLeanCLRMarshal::FStackObject Args[1] = {FLeanCLRMarshal::FromHandle(InManagedClass)};

		if (FLeanCLRMarshal::FStackObject Return{}; FLeanCLRMarshal::Invoke(Bridge.UtilsIsOverride, Args, 1, Return))
		{
			return FLeanCLRMarshal::ToInt32(Return) != 0;
		}
	}

	return false;
}

void FLeanCLRDomain::GetClassDescriptor(const IManagedHandle InManagedClass, PTRINT* OutParams)
{
	// Utils.GetClassDescriptor(nint InTypeHandle, nint* OutBuffer): the caller-provided OutParams array is
	// filled with handles/counts by the managed side; pass its address straight through.
	if (Bridge.UtilsGetClassDescriptor != nullptr)
	{
		FLeanCLRMarshal::FStackObject Args[2] = {
			FLeanCLRMarshal::FromHandle(InManagedClass),
			FLeanCLRMarshal::FromPointer(OutParams)
		};

		FLeanCLRMarshal::FStackObject Return{};

		FLeanCLRMarshal::Invoke(Bridge.UtilsGetClassDescriptor, Args, 2, Return);
	}
}

void FLeanCLRDomain::GetClassProperties(const IManagedHandle InManagedClass, PTRINT* OutParams)
{
	if (Bridge.UtilsGetClassProperties != nullptr)
	{
		FLeanCLRMarshal::FStackObject Args[2] = {
			FLeanCLRMarshal::FromHandle(InManagedClass),
			FLeanCLRMarshal::FromPointer(OutParams)
		};

		FLeanCLRMarshal::FStackObject Return{};

		FLeanCLRMarshal::Invoke(Bridge.UtilsGetClassProperties, Args, 2, Return);
	}
}

void FLeanCLRDomain::GetClassFields(const IManagedHandle InManagedClass, PTRINT* OutParams)
{
	if (Bridge.UtilsGetClassFields != nullptr)
	{
		FLeanCLRMarshal::FStackObject Args[2] = {
			FLeanCLRMarshal::FromHandle(InManagedClass),
			FLeanCLRMarshal::FromPointer(OutParams)
		};

		FLeanCLRMarshal::FStackObject Return{};

		FLeanCLRMarshal::Invoke(Bridge.UtilsGetClassFields, Args, 2, Return);
	}
}

void FLeanCLRDomain::GetClassMethods(const IManagedHandle InManagedClass, PTRINT* OutParams)
{
	if (Bridge.UtilsGetClassMethods != nullptr)
	{
		FLeanCLRMarshal::FStackObject Args[2] = {
			FLeanCLRMarshal::FromHandle(InManagedClass),
			FLeanCLRMarshal::FromPointer(OutParams)
		};

		FLeanCLRMarshal::FStackObject Return{};

		FLeanCLRMarshal::Invoke(Bridge.UtilsGetClassMethods, Args, 2, Return);
	}
}

bool FLeanCLRDomain::IsInitialized() const
{
	return bIsInitialized;
}

TArray<IManagedHandle> FLeanCLRDomain::GetAssemblies() const
{
	return Assemblies;
}

TArray<FClassReflection*> FLeanCLRDomain::GetClassesWithAttribute(const FClassReflection* InClass,
                                                                  const IManagedHandle InManagedHandle)
{
	// Attribute-based type discovery drives dynamic generation of C#-DEFINED UClass/UEnum/UStruct. It
	// reflection-invokes Utils.GetTypesWithAttribute(Type InAttributeType, Assembly InAssembly, out int
	// OutLength) -> Type[] and turns each returned Type into an FClassReflection (mirrors the shared
	// FScriptDomainImpl.inl path). Two LeanCLR-specific gaps had to be closed to make this work:
	//   1. out int OutLength is a value-type by-ref. leanclr's reflection Method.Invoke drops writes to
	//      such params, but FLeanCLRDomain::Invoke now routes any "value-type by-ref, no reference by-ref"
	//      method through the Option B host-side stackobject path (P5 §3.17) — so routing this call through
	//      Invoke() rather than the leanclr reflection path makes the out param write back correctly.
	//   2. Assembly InAssembly needs a managed System.Reflection.Assembly handle, but Assemblies stores
	//      raw RtAssembly* pointers (LoadAssembly). AllocAssemblyHandle converts that native pointer into a
	//      HandleData handle so the reverse-invoke path can resolve it like any other reference argument.
	TArray<FClassReflection*> Result;

	if (InClass == nullptr || !IManagedHandleIsValid(InManagedHandle) ||
		Bridge.HandleDataGetObjectPointer == nullptr || Bridge.HandleDataAllocFromObject == nullptr)
	{
		return Result;
	}

	const auto UtilsClass = FReflectionRegistry::Get().GetUtilsClass();

	if (UtilsClass == nullptr)
	{
		return Result;
	}

	const IManagedHandle MethodHandle = GetMethod(UtilsClass->GetManagedClass(),
	                                              FUNCTION_UTILS_GET_TYPES_WITH_ATTRIBUTE, 3);

	if (!IManagedHandleIsValid(MethodHandle))
	{
		return Result;
	}

	// The stored handle is a raw RtAssembly* (see LoadAssembly); turn it into a managed Assembly handle.
	auto* Assembly = reinterpret_cast<leanclr::metadata::RtAssembly*>(
		static_cast<UPTRINT>(InManagedHandle.Value));

	const IManagedHandle AssemblyHandle = FLeanCLRMarshal::AllocAssemblyHandle(
		Bridge.HandleDataAllocFromObject, Assembly);

	if (!IManagedHandleIsValid(AssemblyHandle))
	{
		Free(MethodHandle);

		return Result;
	}

	// InParams layout for the reverse-invoke path: reference-type-by-value args (attribute Type, Assembly)
	// are read as slots holding a HandleData key; the value-type out param points straight at OutLength.
	int64 AttributeKey = InClass->GetManagedClass().Value;

	int64 AssemblyKey = AssemblyHandle.Value;

	int32 OutLength = 0;

	void* InParams[3] = {&AttributeKey, &AssemblyKey, &OutLength};

	if (const IManagedHandle Types = Invoke(InvalidManagedHandle, MethodHandle, 3, InParams);
		IManagedHandleIsValid(Types))
	{
		for (int32 Index = 0; Index < OutLength; ++Index)
		{
			if (const IManagedHandle Element = ArrayGet(Types, Index); IManagedHandleIsValid(Element))
			{
				Result.Add(FReflectionRegistry::Get().GetClass(GetNamespace(Element), GetName(Element)));

				Free(Element);
			}
		}

		Free(Types);
	}

	Free(AssemblyHandle);

	Free(MethodHandle);

	return Result;
}

void FLeanCLRDomain::InitializeAssembly(const TArray<FString>& InAssemblies)
{
	LoadAssembly(InAssemblies);

	// Utils.* and SynchronizationContext.Tick live in the UE assembly (not Interop). Resolve their
	// method handles once the UE module is loaded (mirrors FCoreCLRDomain::InitializeAssembly).
	UEModule = nullptr;

	if (const auto UEAssembly = leanclr::vm::Assembly::find_by_name(
		TCHAR_TO_UTF8(*FUnrealCSharpFunctionLibrary::GetUEName())))
	{
		UEModule = UEAssembly->mod;
	}

	if (UEModule != nullptr)
	{
		const auto UtilsFullName = COMBINE_FULL_NAME(
			COMBINE_NAMESPACE(NAMESPACE_ROOT, NAMESPACE_CORE_UOBJECT), CLASS_UTILS);

		Bridge.UtilsIsOverride = ResolveIn(UEModule, UtilsFullName, FUNCTION_UTILS_IS_OVERRIDE);

		Bridge.UtilsGetClassDescriptor = ResolveIn(UEModule, UtilsFullName, FUNCTION_UTILS_GET_CLASS_DESCRIPTOR);

		Bridge.UtilsGetClassProperties = ResolveIn(UEModule, UtilsFullName, FUNCTION_UTILS_GET_CLASS_PROPERTIES);

		Bridge.UtilsGetClassFields = ResolveIn(UEModule, UtilsFullName, FUNCTION_UTILS_GET_CLASS_FIELDS);

		Bridge.UtilsGetClassMethods = ResolveIn(UEModule, UtilsFullName, FUNCTION_UTILS_GET_CLASS_METHODS);

		// Diagnostic-only (not part of the IScriptDomain surface): the name is local to the LeanCLR
		// backend, so no shared FunctionMacro entry. May stay null against an older UE.dll.
		Bridge.UtilsDumpMethodReflection = ResolveIn(UEModule, UtilsFullName, FString(TEXT("DumpMethodReflection")));

		RegisterSynchronizationContextTick();

		FReflectionRegistry::Get().Initialize();

		// RegisterBinding() is deliberately NOT called: it is the reverse channel (C# -> UE native
		// bindings) and depends on the P1.1 generator change + the P/Invoke invoker. Deferred pass.
	}
	else
	{
		UE_LOG(LogUnrealCSharp, Warning,
		       TEXT("FLeanCLRDomain: UE assembly '%s' not loaded; Utils.*/Tick handles unresolved, ")
		       TEXT("reflection registry not initialized."), *FUnrealCSharpFunctionLibrary::GetUEName());
	}
}

void FLeanCLRDomain::LoadAssembly(const TArray<FString>& InAssemblies)
{
	Assemblies.Empty();

	const auto InteropPath = FUnrealCSharpFunctionLibrary::GetFullInteropPublishPath();

	for (const auto& AssemblyPath : InAssemblies)
	{
		// Interop is already loaded during Initialize; skip it here.
		if (AssemblyPath == InteropPath)
		{
			continue;
		}

		const auto AssemblyName = FPaths::GetBaseFilename(AssemblyPath);

		if (auto LoadedAssembly = leanclr::vm::Assembly::load_by_name(TCHAR_TO_UTF8(*AssemblyName));
			!LoadedAssembly.is_err() && LoadedAssembly.unwrap() != nullptr)
		{
			// Store the RtAssembly* as an opaque handle so GetAssemblies() is non-empty and identifiable.
			Assemblies.Add(IManagedHandle{static_cast<int64>(reinterpret_cast<UPTRINT>(LoadedAssembly.unwrap()))});

			UE_LOG(LogUnrealCSharp, Log, TEXT("FLeanCLRDomain: loaded assembly '%s'"), *AssemblyName);
		}
		else
		{
			UE_LOG(LogUnrealCSharp, Warning, TEXT("FLeanCLRDomain: failed to load assembly '%s'"), *AssemblyName);
		}
	}
}

void FLeanCLRDomain::UnloadAssembly()
{
	FReflectionRegistry::Get().Deinitialize();

	// Clear managed-side static caches (HandleData, TypeBridge).
	// Without this, StaticClassSingleton cached UClass wrappers survive across PIE
	// teardown/restart with stale (zero) handles, causing SpawnActor to fail on the second PIE
	// because the native ObjectRegistry is freshly re-created and doesn't contain the old handle.
	if (Bridge.AssemblyLoaderUnload != nullptr)
	{
		FLeanCLRMarshal::FStackObject Ret{};

		FLeanCLRMarshal::Invoke(Bridge.AssemblyLoaderUnload, nullptr, 0, Ret);
	}

	Assemblies.Empty();

	UEModule = nullptr;

	InteropModule = nullptr;

	Bridge = FLeanCLRBridge{};
}

void FLeanCLRDomain::RegisterPInvokes()
{
	// Reverse channel (P1.2/P4.3): register every UE binding as a named P/Invoke keyed by the C# declaring
	// method full name. FBindingMethod::GetMethod() is already that exact "Namespace.Class::Method" string
	// (same macros the generator baked into the [DllImport] metadata — see FBindingClassGenerator), so no
	// reconstruction is needed. Called from Initialize() after Runtime::initialize() and before any
	// assembly loads, matching leanrun. When C# later calls a binding, Shim::get_invoker routes to
	// LeanCLRNativeInvoker. Unlike Mono/CoreCLR, we do NOT populate the C# StringToMethod dictionary
	// (MethodBridge.RegisterBinding) — the [DllImport] stubs don't call MethodBridge.GetMethod.
	static TArray<TArray<ANSICHAR>> PInvokeNames;

	// vm::PInvokes stores the key by const char*, so each UTF-8 buffer must outlive registration; hold
	// owning copies for the process lifetime (mirrors FScriptDomainImpl.inl's Names buffer).
	auto RegisterName = [](const FString& InName, const void* InFunc)
	{
		const auto Utf8 = StringCast<UTF8CHAR>(*InName);

		const auto Length = Utf8.Length() + 1;

		auto& Storage = PInvokeNames.AddDefaulted_GetRef();

		Storage.SetNumUninitialized(Length);

		FMemory::Memcpy(Storage.GetData(), Utf8.Get(), Length);

		leanclr::vm::PInvokes::register_pinvoke(
			reinterpret_cast<const char*>(Storage.GetData()),
			reinterpret_cast<leanclr::vm::PInvokeFunction>(reinterpret_cast<UPTRINT>(const_cast<void*>(InFunc))),
			&LeanCLRNativeInvoker);
	};

	int32 Registered = 0;

	int32 Aliases = 0;

	for (const auto& Class : FBinding::Get().Register().GetClasses())
	{
		for (const auto& Method : Class->GetMethods())
		{
			const auto& Canonical = Method.GetMethod();

			RegisterName(Canonical, Method.GetFunction());

			++Registered;

			// Hand-written core library (Script/UE/Library/*Implementation.cs) can't make its public
			// managed wrapper the [DllImport] extern (the wrapper marshals args/returns, and shapes like
			// nint->string differ only by return type, which C# can't overload). So on LeanCLR that extern
			// reuses the existing "__"-prefixed field name (e.g. __FName_ToStringImplementation), whose full
			// name is "Ns.Class::__Method". Register that alias (same native func) so those externs resolve.
			// Generated bindings keep the canonical name and never touch the alias, so this is harmless for
			// them (an unreferenced map entry); "__"-names never collide with a real method name.
			int32 SeparatorIndex = INDEX_NONE;

			if (Canonical.FindLastChar(TEXT(':'), SeparatorIndex) && SeparatorIndex != INDEX_NONE)
			{
				const auto Alias = Canonical.Left(SeparatorIndex + 1) + TEXT("__") +
					Canonical.RightChop(SeparatorIndex + 1);

				RegisterName(Alias, Method.GetFunction());

				++Aliases;
			}
		}
	}

	UE_LOG(LogUnrealCSharp, Log,
	       TEXT("FLeanCLRDomain: registered %d UE binding P/Invokes (+%d '__' aliases for hand-written library)"),
	       Registered, Aliases);

	// LogBridge reverse channel: C# Console.Out/Error are redirected to LogBridge (InitializeLeanCLR,
	// invoked in Initialize), whose Flush calls the [DllImport] LogLeanCLR extern — leanclr resolves it
	// by managed full name, same as every binding above. Sink = the same FScriptLog::Log Mono/CoreCLR
	// hand to LogBridge.SetLog, so C# Console output lands in the UE log identically across backends.
	RegisterName(COMBINE_FULL_NAME(NAMESPACE_INTEROP, CLASS_LOG_BRIDGE) + TEXT("::LogLeanCLR"),
	             reinterpret_cast<const void*>(&FScriptLog::Log));
}

void FLeanCLRDomain::ResolveBridgeMethods()
{
	if (InteropModule == nullptr)
	{
		return;
	}

	int32 Resolved = 0;

	int32 Total = 0;

#define RESOLVE_INTEROP_BRIDGE(Member, ClassNameMacro, MethodNameMacro) \
	{ \
		++Total; \
		Bridge.Member = ResolveIn(InteropModule, COMBINE_FULL_NAME(NAMESPACE_INTEROP, ClassNameMacro), \
		                          MethodNameMacro); \
		if (Bridge.Member != nullptr) \
		{ \
			++Resolved; \
		} \
	}

	RESOLVE_INTEROP_BRIDGE(AssemblyLoaderLoadFromStream, CLASS_ASSEMBLY_LOADER, FUNCTION_ASSEMBLY_LOADER_LOAD_FROM_STREAM)
	RESOLVE_INTEROP_BRIDGE(AssemblyLoaderUnload, CLASS_ASSEMBLY_LOADER, FUNCTION_ASSEMBLY_LOADER_UNLOAD)

	RESOLVE_INTEROP_BRIDGE(HandleDataFree, CLASS_HANDLE_DATA, FUNCTION_HANDLE_DATA_FREE)
	RESOLVE_INTEROP_BRIDGE(HandleDataGetObjectPointer, CLASS_HANDLE_DATA, FUNCTION_HANDLE_DATA_GET_OBJECT_POINTER)
	RESOLVE_INTEROP_BRIDGE(HandleDataAllocFromObject, CLASS_HANDLE_DATA, FUNCTION_HANDLE_DATA_ALLOC_FROM_OBJECT)

	RESOLVE_INTEROP_BRIDGE(LogBridgeSetLog, CLASS_LOG_BRIDGE, FUNCTION_LOG_BRIDGE_SET_LOG)
	RESOLVE_INTEROP_BRIDGE(LogBridgeInitialize, CLASS_LOG_BRIDGE, FUNCTION_LOG_BRIDGE_INITIALIZE)

	RESOLVE_INTEROP_BRIDGE(TypeBridgeGetClass, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_GET_CLASS)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeGetType, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_GET_TYPE)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeGetMethod, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_GET_METHOD)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeGetFunctionPointer, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_GET_FUNCTION_POINTER)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeGetNamespace, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_GET_NAMESPACE)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeGetName, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_GET_NAME)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeGetFullName, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_GET_FULL_NAME)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeMakeGenericType, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_MAKE_GENERIC_TYPE)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeMakeGenericType2, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_MAKE_GENERIC_TYPE2)

	RESOLVE_INTEROP_BRIDGE(TypeBridgeBoxBool, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_BOX_BOOL)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeBoxSByte, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_BOX_SBYTE)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeBoxInt16, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_BOX_INT16)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeBoxInt32, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_BOX_INT32)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeBoxInt64, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_BOX_INT64)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeBoxByte, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_BOX_BYTE)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeBoxUInt16, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_BOX_UINT16)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeBoxUInt32, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_BOX_UINT32)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeBoxUInt64, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_BOX_UINT64)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeBoxFloat, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_BOX_FLOAT)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeBoxDouble, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_BOX_DOUBLE)

	RESOLVE_INTEROP_BRIDGE(TypeBridgeUnboxBool, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_UNBOX_BOOL)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeUnboxSByte, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_UNBOX_SBYTE)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeUnboxInt16, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_UNBOX_INT16)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeUnboxInt32, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_UNBOX_INT32)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeUnboxInt64, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_UNBOX_INT64)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeUnboxByte, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_UNBOX_BYTE)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeUnboxUInt16, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_UNBOX_UINT16)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeUnboxUInt32, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_UNBOX_UINT32)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeUnboxUInt64, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_UNBOX_UINT64)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeUnboxFloat, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_UNBOX_FLOAT)
	RESOLVE_INTEROP_BRIDGE(TypeBridgeUnboxDouble, CLASS_TYPE_BRIDGE, FUNCTION_TYPE_BRIDGE_UNBOX_DOUBLE)

	RESOLVE_INTEROP_BRIDGE(ObjectBridgeNewObject, CLASS_OBJECT_BRIDGE, FUNCTION_OBJECT_BRIDGE_NEW_OBJECT)

	RESOLVE_INTEROP_BRIDGE(FieldBridgeSetStaticValue, CLASS_FIELD_BRIDGE, FUNCTION_FIELD_BRIDGE_SET_STATIC_VALUE)
	RESOLVE_INTEROP_BRIDGE(FieldBridgeGetStaticValue, CLASS_FIELD_BRIDGE, FUNCTION_FIELD_BRIDGE_GET_STATIC_VALUE)

	RESOLVE_INTEROP_BRIDGE(MethodBridgeRegisterBinding, CLASS_METHOD_BRIDGE, FUNCTION_METHOD_BRIDGE_REGISTER_BINDING)
	RESOLVE_INTEROP_BRIDGE(MethodBridgeInvoke, CLASS_METHOD_BRIDGE, FUNCTION_METHOD_BRIDGE_INVOKE)

	RESOLVE_INTEROP_BRIDGE(StringBridgeNewString, CLASS_STRING_BRIDGE, FUNCTION_STRING_BRIDGE_NEW_STRING)
	RESOLVE_INTEROP_BRIDGE(StringBridgeGetString, CLASS_STRING_BRIDGE, FUNCTION_STRING_BRIDGE_GET_STRING)

	RESOLVE_INTEROP_BRIDGE(ArrayBridgeNewArray, CLASS_ARRAY_BRIDGE, FUNCTION_ARRAY_BRIDGE_ARRAY_NEW)
	RESOLVE_INTEROP_BRIDGE(ArrayBridgeArrayGet, CLASS_ARRAY_BRIDGE, FUNCTION_ARRAY_BRIDGE_ARRAY_GET)

#undef RESOLVE_INTEROP_BRIDGE

	if (Resolved == Total)
	{
		UE_LOG(LogUnrealCSharp, Log, TEXT("FLeanCLRDomain: resolved %d/%d Interop bridge handles"),
		       Resolved, Total);
	}
	else
	{
		UE_LOG(LogUnrealCSharp, Warning, TEXT("FLeanCLRDomain: resolved %d/%d Interop bridge handles (some missing)"),
		       Resolved, Total);
	}
}

void FLeanCLRDomain::RegisterLog()
{
	leanclr::vm::Settings::set_report_unhandled_exception_function(&LeanCLRReportUnhandledException);

	leanclr::vm::Settings::set_debugger_log_function(&LeanCLRDebuggerLog);
}

void FLeanCLRDomain::RegisterBinding() const
{
	// LeanCLR reverse channel goes through named P/Invokes (see RegisterPInvokes), not the C#
	// StringToMethod dictionary: the LeanCLR-target generator emits [DllImport] stubs that never call
	// MethodBridge.GetMethod, so populating that dictionary via Bridge.MethodBridgeRegisterBinding is
	// unnecessary here. Kept as an intentional no-op; the cached handle stays resolved but unused.
}

void FLeanCLRDomain::RegisterSynchronizationContextTick()
{
	Bridge.SynchronizationContextTick = ResolveIn(UEModule,
		COMBINE_FULL_NAME(COMBINE_NAMESPACE(NAMESPACE_ROOT, NAMESPACE_CORE_UOBJECT), CLASS_SYNCHRONIZATION_CONTEXT),
		FUNCTION_SYNCHRONIZATION_CONTEXT_TICK);
}
#endif
