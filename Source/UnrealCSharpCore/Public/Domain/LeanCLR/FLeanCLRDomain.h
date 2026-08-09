#pragma once

#if WITH_LEANCLR
#include "CoreMinimal.h"
#include "Domain/Script/IManagedHandle.h"
#include "Domain/Script/IScriptDomain.h"

class FClassReflection;

// Forward-declared so this header stays free of leanclr includes (design R6): the interpreter
// surface is pulled only by FLeanCLRDomain.cpp / FLeanCLRMarshal. The bridge holds opaque method
// handles; their definition lives in the leanclr metadata headers.
namespace leanclr
{
namespace metadata
{
struct RtMethodInfo;
class RtModuleDef;
}
}

// Resolved handles to the C# Interop bridge methods, aggregated in one place. Semantically the
// leanclr counterpart of SCRIPT_TYPES (IScriptTypes.h): where Mono/CoreCLR cache native function
// pointers, strategy A caches RtMethodInfo* and calls them through vm::Runtime::invoke (an IL
// interpreter cannot hand out callable native pointers). Populated by ResolveBridgeMethods (P4).
struct FLeanCLRBridge
{
#define LEANCLR_BRIDGE_METHOD(Name) const leanclr::metadata::RtMethodInfo* Name{};

	LEANCLR_BRIDGE_METHOD(AssemblyLoaderLoadFromStream)
	LEANCLR_BRIDGE_METHOD(AssemblyLoaderUnload)

	LEANCLR_BRIDGE_METHOD(HandleDataFree)

	// Reverse-invoke support (Option B): resolve a HandleData key to a runtime RtObject* and register a
	// runtime object pointer as a new handle, so the C++ stackobject invoke path can marshal a C#
	// function's `this`/reference args and its returned/out object without going through C# reflection.
	LEANCLR_BRIDGE_METHOD(HandleDataGetObjectPointer)
	LEANCLR_BRIDGE_METHOD(HandleDataAllocFromObject)

	LEANCLR_BRIDGE_METHOD(LogBridgeSetLog)
	LEANCLR_BRIDGE_METHOD(LogBridgeInitialize)

	LEANCLR_BRIDGE_METHOD(TypeBridgeGetClass)
	LEANCLR_BRIDGE_METHOD(TypeBridgeGetType)
	LEANCLR_BRIDGE_METHOD(TypeBridgeGetMethod)
	LEANCLR_BRIDGE_METHOD(TypeBridgeGetFunctionPointer)
	LEANCLR_BRIDGE_METHOD(TypeBridgeGetNamespace)
	LEANCLR_BRIDGE_METHOD(TypeBridgeGetName)
	LEANCLR_BRIDGE_METHOD(TypeBridgeGetFullName)
	LEANCLR_BRIDGE_METHOD(TypeBridgeMakeGenericType)
	LEANCLR_BRIDGE_METHOD(TypeBridgeMakeGenericType2)

	LEANCLR_BRIDGE_METHOD(TypeBridgeBoxBool)
	LEANCLR_BRIDGE_METHOD(TypeBridgeBoxSByte)
	LEANCLR_BRIDGE_METHOD(TypeBridgeBoxInt16)
	LEANCLR_BRIDGE_METHOD(TypeBridgeBoxInt32)
	LEANCLR_BRIDGE_METHOD(TypeBridgeBoxInt64)
	LEANCLR_BRIDGE_METHOD(TypeBridgeBoxByte)
	LEANCLR_BRIDGE_METHOD(TypeBridgeBoxUInt16)
	LEANCLR_BRIDGE_METHOD(TypeBridgeBoxUInt32)
	LEANCLR_BRIDGE_METHOD(TypeBridgeBoxUInt64)
	LEANCLR_BRIDGE_METHOD(TypeBridgeBoxFloat)
	LEANCLR_BRIDGE_METHOD(TypeBridgeBoxDouble)

	LEANCLR_BRIDGE_METHOD(TypeBridgeUnboxBool)
	LEANCLR_BRIDGE_METHOD(TypeBridgeUnboxSByte)
	LEANCLR_BRIDGE_METHOD(TypeBridgeUnboxInt16)
	LEANCLR_BRIDGE_METHOD(TypeBridgeUnboxInt32)
	LEANCLR_BRIDGE_METHOD(TypeBridgeUnboxInt64)
	LEANCLR_BRIDGE_METHOD(TypeBridgeUnboxByte)
	LEANCLR_BRIDGE_METHOD(TypeBridgeUnboxUInt16)
	LEANCLR_BRIDGE_METHOD(TypeBridgeUnboxUInt32)
	LEANCLR_BRIDGE_METHOD(TypeBridgeUnboxUInt64)
	LEANCLR_BRIDGE_METHOD(TypeBridgeUnboxFloat)
	LEANCLR_BRIDGE_METHOD(TypeBridgeUnboxDouble)

	LEANCLR_BRIDGE_METHOD(ObjectBridgeNewObject)

	LEANCLR_BRIDGE_METHOD(FieldBridgeSetStaticValue)
	LEANCLR_BRIDGE_METHOD(FieldBridgeGetStaticValue)

	LEANCLR_BRIDGE_METHOD(MethodBridgeRegisterBinding)
	LEANCLR_BRIDGE_METHOD(MethodBridgeInvoke)

	LEANCLR_BRIDGE_METHOD(StringBridgeNewString)
	LEANCLR_BRIDGE_METHOD(StringBridgeGetString)

	LEANCLR_BRIDGE_METHOD(ArrayBridgeNewArray)
	LEANCLR_BRIDGE_METHOD(ArrayBridgeArrayGet)

	LEANCLR_BRIDGE_METHOD(UtilsIsOverride)
	LEANCLR_BRIDGE_METHOD(UtilsGetClassDescriptor)
	LEANCLR_BRIDGE_METHOD(UtilsGetClassProperties)
	LEANCLR_BRIDGE_METHOD(UtilsGetClassFields)
	LEANCLR_BRIDGE_METHOD(UtilsGetClassMethods)

	LEANCLR_BRIDGE_METHOD(SynchronizationContextTick)

#undef LEANCLR_BRIDGE_METHOD
};

// Third script backend: a from-scratch IL interpreter (leanclr) driven through its vm:: API.
// Unlike Mono/CoreCLR this does NOT reuse FScriptDomainImpl.inl — that .inl dispatches through
// native function pointers, which leanclr cannot provide. Each IScriptDomain method is implemented
// in the .cpp by invoking the corresponding cached bridge method via FLeanCLRMarshal (strategy A).
//
// P3 status: skeleton. Lifecycle (P4) and the per-method invoke bodies (P5) are graceful stubs that
// return empty/invalid so a LeanCLR launch constructs and initializes without crashing.
class UNREALCSHARPCORE_API FLeanCLRDomain final : public IScriptDomain
{
public:
	FLeanCLRDomain() = default;

	virtual ~FLeanCLRDomain() override = default;

public:
	virtual void Initialize() override;

	virtual void Tick(float InDeltaTime) override;

	virtual void Deinitialize() override;

public:
	virtual FString GetNamespace(const IManagedHandle InManagedClass) override;

	virtual FString GetName(const IManagedHandle InManagedClass) override;

	virtual FString GetFullName(const IManagedHandle InManagedClass) override;

	virtual IManagedHandle NewObject(const IManagedHandle InManagedClass) override;

	virtual IManagedHandle BoxValue(const FString& InNamespace, const FString& InName, void* InValue) override;

	virtual void* UnboxValue(const IManagedHandle InManagedHandle) override;

	virtual IManagedHandle NewString(const char* InText) override;

	virtual FString StringToFString(const IManagedHandle InManagedHandle) override;

	virtual void Free(const IManagedHandle InManagedHandle) override;

	virtual IManagedHandle NewArray(const FString& InNamespace, const FString& InName, int32 InLength) override;

	virtual IManagedHandle ArrayGet(const IManagedHandle InManagedArray, int32 InIndex) override;

	virtual IManagedHandle GetClass(const FString& InNamespace, const FString& InName) override;

	virtual IManagedHandle GetMethod(const IManagedHandle InManagedClass, const FString& InName,
	                                 int32 InParamCount) override;

	virtual void SetFieldStaticValue(const IManagedHandle InManagedClass, const FString& InName,
	                                 void* InValue) override;

	virtual void* GetFieldStaticValue(const IManagedHandle InManagedClass, const FString& InName) override;

	virtual void SetPropertyValue(const IManagedHandle InManagedHandle, const FString& InName,
	                              void** InParams) override;

	virtual FClassReflection* MakeGenericType(const FClassReflection* InGeneric,
	                                          const FClassReflection* InType) override;

	virtual FClassReflection* MakeGenericType(const FClassReflection* InGeneric,
	                                          const FClassReflection* InKeyType,
	                                          const FClassReflection* InValueType) override;

	virtual IManagedHandle Invoke(const IManagedHandle InManagedHandle, const IManagedHandle InManagedMethod,
	                              int32 InParamCount = 0, void** InParams = nullptr) override;

	virtual bool IsOverride(const IManagedHandle InManagedClass) override;

	virtual void GetClassDescriptor(const IManagedHandle InManagedClass, PTRINT* OutParams) override;

	virtual void GetClassProperties(const IManagedHandle InManagedClass, PTRINT* OutParams) override;

	virtual void GetClassFields(const IManagedHandle InManagedClass, PTRINT* OutParams) override;

	virtual void GetClassMethods(const IManagedHandle InManagedClass, PTRINT* OutParams) override;

public:
	virtual bool IsInitialized() const override;

	virtual TArray<IManagedHandle> GetAssemblies() const override;

	virtual TArray<FClassReflection*> GetClassesWithAttribute(const FClassReflection* InClass,
	                                                          const IManagedHandle InManagedHandle) override;

private:
	// Lifecycle sub-steps (P4). Declared now to fix the class shape; skeleton bodies live in the .cpp.
	void InitializeAssembly(const TArray<FString>& InAssemblies);

	void LoadAssembly(const TArray<FString>& InAssemblies);

	void UnloadAssembly();

	void RegisterPInvokes();

	void ResolveBridgeMethods();

	void RegisterLog();

	void RegisterBinding() const;

	void RegisterSynchronizationContextTick();

private:
	FLeanCLRBridge Bridge;

	// Reverse-invoke classification cache (Option B, value-type by-ref write-back). Maps a managed
	// MethodInfo handle to its resolved concrete RtMethodInfo* and whether the call takes the host-side
	// stackobject path. Resolving is itself a managed bridge call, so it is done once per method here;
	// RtMethodInfo* is permanent metadata and leanclr's GC is non-moving, so caching the pointer is safe.
	struct FReverseMethod
	{
		const leanclr::metadata::RtMethodInfo* Method{};

		bool bUseCppPath{};
	};

	TMap<int64, FReverseMethod> ReverseMethodCache;

	// Loaded module handles resolved during bring-up (P4). Interop hosts the ~45 bridge methods; the
	// UE assembly hosts Utils.* and SynchronizationContext.Tick. Forward-declared, so this header stays
	// leanclr-free (design R6); the definitions are pulled only in FLeanCLRDomain.cpp / FLeanCLRMarshal.
	leanclr::metadata::RtModuleDef* InteropModule{};

	leanclr::metadata::RtModuleDef* UEModule{};

	TArray<IManagedHandle> Assemblies;

	bool bIsInitialized{};
};
#endif
