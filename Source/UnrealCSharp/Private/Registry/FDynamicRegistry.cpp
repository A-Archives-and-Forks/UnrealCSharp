#include "Registry/FDynamicRegistry.h"
#include "Delegate/FUnrealCSharpModuleDelegates.h"
#include "Dynamic/FDynamicClassGenerator.h"
#include "Environment/FCSharpEnvironment.h"
#include "Reflection/FReflectionRegistry.h"
#if WITH_LEANCLR
#include "Log/UnrealCSharpLog.h"
#include "UObject/UnrealType.h"
#endif

FDynamicRegistry::FDynamicRegistry()
{
	Initialize();
}

FDynamicRegistry::~FDynamicRegistry()
{
	Deinitialize();
}

void FDynamicRegistry::Initialize()
{
	FDynamicClassGenerator::OnPostClassConstructor = [](UObject* InObject)
	{
		if (IsInGameThread())
		{
			FCSharpEnvironment::GetEnvironment().Bind<true>(InObject);

			if (const auto FoundManagedHandle = FCSharpEnvironment::GetEnvironment().GetObject(InObject);
				IManagedHandleIsValid(FoundManagedHandle))
			{
				if (const auto FoundClass = FReflectionRegistry::Get().GetClass(InObject->GetClass()))
				{
#if WITH_LEANCLR
					// [P6.1 ctor-readback] discriminator: does the C# ctor's property setter write land on
					// THIS native instance? Read Int32Value off the UObject before/after running the ctor.
					// No class-name filter (that missed last round) — fire whenever the class actually has an
					// FIntProperty named "Int32Value", which uniquely identifies the dynamic test actor. Also
					// log the class name so we learn the generated UClass's real name.
					const FIntProperty* Int32Probe = CastField<FIntProperty>(
						InObject->GetClass()->FindPropertyByName(TEXT("Int32Value")));

					if (Int32Probe != nullptr)
					{
						UE_LOG(LogUnrealCSharp, Warning,
						       TEXT("[P6.1 ctor-readback] class=%s obj=%s BEFORE Int32Value=%d"),
						       *InObject->GetClass()->GetName(), *InObject->GetName(),
						       Int32Probe->GetPropertyValue_InContainer(InObject));
					}
#endif

					FoundClass->ConstructorObject(FoundManagedHandle);

#if WITH_LEANCLR
					if (Int32Probe != nullptr)
					{
						UE_LOG(LogUnrealCSharp, Warning,
						       TEXT("[P6.1 ctor-readback] class=%s obj=%s AFTER  Int32Value=%d"),
						       *InObject->GetClass()->GetName(), *InObject->GetName(),
						       Int32Probe->GetPropertyValue_InContainer(InObject));
					}
#endif
				}
			}
		}
	};

	OnCSharpEnvironmentInitializeDelegateHandle = FUnrealCSharpModuleDelegates::OnCSharpEnvironmentInitialize.AddRaw(
		this, &FDynamicRegistry::OnCSharpEnvironmentInitialize);
}

void FDynamicRegistry::Deinitialize()
{
	if (OnCSharpEnvironmentInitializeDelegateHandle.IsValid())
	{
		FUnrealCSharpModuleDelegates::OnCSharpEnvironmentInitialize.Remove(OnCSharpEnvironmentInitializeDelegateHandle);
	}

	FDynamicClassGenerator::OnPostClassConstructor = nullptr;
}

void FDynamicRegistry::OnCSharpEnvironmentInitialize() const
{
	RegisterDynamic();
}

void FDynamicRegistry::RegisterDynamic() const
{
	if (FDomain::IsLoadSucceed())
	{
		FDynamicGeneratorCore::Generator(FReflectionRegistry::Get().GetUClassAttributeClass(),
		                                 [](FClassReflection* InClass)
		                                 {
			                                 if (const auto DynamicClass = FDynamicClassGenerator::GetDynamicClass(
				                                 InClass))
			                                 {
				                                 FCSharpEnvironment::GetEnvironment().Bind<true>(DynamicClass);
			                                 }
		                                 }
		);
	}
}
