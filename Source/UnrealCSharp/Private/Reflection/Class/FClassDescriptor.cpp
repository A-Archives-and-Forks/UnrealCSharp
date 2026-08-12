#include "Reflection/Class/FClassDescriptor.h"
#include "CoreMacro/PropertyMacro.h"
#include "Domain/Script/IScriptDomain.h"
#include "Environment/FCSharpEnvironment.h"
#include "Reflection/FReflectionRegistry.h"

FClassDescriptor::FClassDescriptor(UStruct* InStruct):
	Struct(InStruct)
{
	Initialize();
}

FClassDescriptor::~FClassDescriptor()
{
	Deinitialize();
}

void FClassDescriptor::Initialize()
{
	if (const auto FoundClass = Cast<UClass>(Struct))
	{
		FoundClass->ClearFunctionMapsCaches();
	}

	Class = FReflectionRegistry::Get().GetClass(Struct);

	Class->ConstructorClass();
}

void FClassDescriptor::Deinitialize()
{
	const auto FoundClass = Cast<UClass>(Struct);

	if (FoundClass != nullptr)
	{
		FoundClass->ClearFunctionMapsCaches();
	}

	if (Class != nullptr && IManagedHandleIsValid(Class->GetManagedClass()))
	{
		if (const auto ScriptDomain = IScriptDomain::Get())
		{
			// Drop this one type's managed StaticClass() / StaticStruct() cache: the UStruct it wraps is going
			// away, so the cached wrapper would keep handing out a dead UClass / UScriptStruct (e.g. to C# code
			// that calls StaticClass() again after a Blueprint recompile). Written through the field path
			// because the generators emit the singleton as a static auto-property and Interop.FieldBridge
			// resolves that to its backing field. The property path used before could never write:
			// IScriptDomain::SetPropertyValue derives the type from an *instance* handle, and a static reset
			// has no instance to hand it (Interop.TypeBridge.GetType(0) returns 0, so the call bailed out).
			// Domain teardown is covered separately by Interop.AssemblyLoader.Unload's registered singletons.
			uint32 Value{};

			ScriptDomain->SetFieldStaticValue(Class->GetManagedClass(),
			                                  FoundClass != nullptr
				                                  ? PROPERTY_STATIC_CLASS_SINGLETON
				                                  : PROPERTY_STATIC_STRUCT_SINGLETON,
			                                  &Value);
		}
	}

	for (const auto& FunctionHash : FunctionHashSet)
	{
		FCSharpEnvironment::GetEnvironment().RemoveFunctionDescriptor(FunctionHash);
	}

	FunctionHashSet.Empty();

	for (const auto& PropertyHash : PropertyHashSet)
	{
		FCSharpEnvironment::GetEnvironment().RemovePropertyDescriptor(PropertyHash);
	}

	PropertyHashSet.Empty();
}

FClassReflection* FClassDescriptor::GetClass() const
{
	return Class;
}

FFunctionDescriptor* FClassDescriptor::GetFunctionDescriptor(const FString& InFunctionName)
{
	for (const auto FunctionHash : FunctionHashSet)
	{
		if (const auto FunctionDescriptor = FCSharpEnvironment::GetEnvironment().GetFunctionDescriptor<
			FCSharpFunctionDescriptor>(FunctionHash))
		{
			if (FunctionDescriptor->GetName() == InFunctionName)
			{
				return FunctionDescriptor;
			}
		}
	}

	return nullptr;
}

FPropertyDescriptor* FClassDescriptor::AddPropertyDescriptor(FProperty* InProperty)
{
	if (InProperty != nullptr)
	{
		const auto NewPropertyDescriptor = FPropertyDescriptor::Factory(InProperty);

		PropertyHashSet.Add(GetTypeHash(InProperty));

		return NewPropertyDescriptor;
	}

	return nullptr;
}

bool FClassDescriptor::HasFunctionDescriptor(const uint32 InFunctionHash) const
{
	return FunctionHashSet.Contains(InFunctionHash);
}
