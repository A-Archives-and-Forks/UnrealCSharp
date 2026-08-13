#pragma once

#if WITH_LEANCLR
#include "CoreMinimal.h"
#include "Domain/Script/IManagedHandle.h"

// leanclr vm:: API. This header (and only it, plus FLeanCLRDomain.cpp) pulls the leanclr headers,
// keeping the interpreter surface isolated per design R6. The include root src/runtime comes from
// the LeanCLR External module (LeanCLR.Build.cs), available to UnrealCSharpCore when WITH_LEANCLR=1.
// Wrapped in THIRD_PARTY_INCLUDES_* so leanclr's non-UE-clean headers don't trip warnings-as-errors.
THIRD_PARTY_INCLUDES_START
#include "interp/interp_defs.h"
THIRD_PARTY_INCLUDES_END

namespace leanclr
{
namespace metadata
{
struct RtMethodInfo;
class RtModuleDef;
struct RtAssembly;
}
}

// invoke encode/decode for strategy A: instead of calling managed bridge methods through native
// function pointers (which the leanclr IL interpreter cannot hand out), FLeanCLRDomain resolves each
// Interop bridge method to an RtMethodInfo* and calls it via vm::Runtime::invoke_stackobject_*.
// FLeanCLRMarshal owns the RtStackObject packing/unpacking and the resolve+invoke primitives — the
// exact logic proven green in the P1 spike (Intermediate/LeanCLRSpike/spike.cpp resolve()/invoke()).
class FLeanCLRMarshal
{
public:
	using FStackObject = leanclr::interp::RtStackObject;

	// Resolve a static bridge method by class full name + method name inside a loaded module.
	// Initializes the class (Class::initialize_all) so the method is callable. Returns nullptr on miss.
	static const leanclr::metadata::RtMethodInfo* ResolveMethod(
		leanclr::metadata::RtModuleDef* InModule, const char* InFullClassName, const char* InMethodName);

	// Invoke a resolved method with InArgCount packed argument slots. Sizes the arg/return buffers
	// from the method's declared stack-object sizes, runs the static ctor if needed, logs and returns
	// false on a managed exception. The single return slot is written to OutReturn on success.
	static bool Invoke(const leanclr::metadata::RtMethodInfo* InMethod,
	                   const FStackObject* InArgs, int32 InArgCount, FStackObject& OutReturn);

	// ---- Reverse invoke (Option B): value-type by-ref write-back --------------------------------
	// leanclr's reflection Method.Invoke unboxes a by-ref VALUE-type argument into a throwaway temp and
	// never re-boxes it back, so a C# function's write to a `ref int`/`out int` is lost (a `ref` of a
	// reference type survives because leanclr points it at the managed array slot). To make value-type
	// out params write back, invoke the target C# method through the stackobject path with a host-built
	// argument buffer whose by-ref value slot points straight at the caller's storage.
	//
	// ResolveReverseMethod turns a managed MethodInfo handle into the concrete RtMethodInfo* (via the
	// GetObjectPointer bridge + Reflection::get_method_info_from_handle_arg). ReverseMethodNeedsCppPath
	// returns true iff the method has >=1 value-type by-ref param, no reference-type by-ref param, and a
	// reference-type (or static) `this` — the shape this path handles; everything else stays on the
	// existing C# MethodBridge.Invoke path (which already handles it). InGetObjectPointer /
	// InAllocFromObject are the resolved HandleData bridge methods; the object pointers they yield are
	// HandleData-rooted and leanclr's GC is non-moving, so holding them in the host buffer is safe.
	static const leanclr::metadata::RtMethodInfo* ResolveReverseMethod(
		const leanclr::metadata::RtMethodInfo* InGetObjectPointer, IManagedHandle InMethodHandle);

	static bool ReverseMethodNeedsCppPath(const leanclr::metadata::RtMethodInfo* InMethod);

	// Returns true for an instance constructor on a reference type. leanclr's net10 BCL routes
	// ConstructorInfo.Invoke(existingObject, ...) through a path that does NOT re-run the ctor body on the
	// passed instance (it behaves like an allocating invoke), so a C#-defined dynamic class's constructor
	// never populates the spawned/bound UObject's properties. Routing such ctors through InvokeReverse
	// instead invokes the ctor body via the stackobject path with `this` bound to the existing object,
	// matching CoreCLR/Mono ConstructorInfo.Invoke(obj) semantics. Value-type (struct) ctors are excluded
	// because the obj+1 value-type `this` layout is not built here.
	static bool IsReferenceConstructor(const leanclr::metadata::RtMethodInfo* InMethod);

	static IManagedHandle InvokeReverse(const leanclr::metadata::RtMethodInfo* InGetObjectPointer,
	                                    const leanclr::metadata::RtMethodInfo* InAllocFromObject,
	                                    const leanclr::metadata::RtMethodInfo* InMethod,
	                                    IManagedHandle InThis, int32 InParamCount, void** InParams);

	// ---- Managed Assembly handle from a raw RtAssembly* (P6.1, blocker 2) -----------------------
	// FLeanCLRDomain stores each loaded assembly as an opaque native RtAssembly* (Assembly::load_by_name),
	// not a HandleData GCHandle — so it cannot be passed to a C# method expecting a System.Reflection
	// .Assembly. This turns that native pointer into a managed Assembly reflection object (via
	// Reflection::get_assembly_reflection_object) and registers it as a HandleData handle through the
	// AllocFromObject bridge, matching how CoreCLR's GetAssemblies() hands out managed assembly handles.
	// The returned handle owns a GCHandle and must be Free()d by the caller.
	static IManagedHandle AllocAssemblyHandle(const leanclr::metadata::RtMethodInfo* InAllocFromObject,
	                                          leanclr::metadata::RtAssembly* InAssembly);

	// ---- Scalar / handle <-> RtStackObject slot conversions -------------------------------------
	// IManagedHandle is an opaque int64 (a managed GCHandle) shuttled through as u64, matching how
	// Mono/CoreCLR treat it. Pointers cover byte*/char16_t* bridge arguments.
	static FStackObject FromHandle(IManagedHandle InHandle);
	static FStackObject FromInt32(int32 InValue);
	static FStackObject FromInt64(int64 InValue);
	static FStackObject FromFloat(float InValue);
	static FStackObject FromPointer(const void* InPointer);

	static IManagedHandle ToHandle(const FStackObject& InSlot);
	static int32 ToInt32(const FStackObject& InSlot);
	static int64 ToInt64(const FStackObject& InSlot);
	static void* ToPointer(const FStackObject& InSlot);

private:
	FLeanCLRMarshal() = delete;
};
#endif
