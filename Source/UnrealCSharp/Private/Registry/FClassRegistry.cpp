#include "Registry/FClassRegistry.h"
#include "Domain/FDomain.h"
#include "Dynamic/FDynamicClassGenerator.h"
#include "Environment/FCSharpEnvironment.h"
#if WITH_LEANCLR
#include "Log/UnrealCSharpLog.h"
#include "UObject/UnrealType.h"
#endif

TMap<TWeakObjectPtr<UClass>, UClass::ClassConstructorType> FClassRegistry::ClassConstructorMap;

FClassRegistry::FClassRegistry()
{
	Initialize();
}

FClassRegistry::~FClassRegistry()
{
	Deinitialize();
}

void FClassRegistry::Initialize()
{
	FDynamicClassGenerator::ClassConstructorSet.Add(&FClassRegistry::ClassConstructor);
}

void FClassRegistry::Deinitialize()
{
	FDynamicClassGenerator::ClassConstructorSet.Remove(&FClassRegistry::ClassConstructor);

	for (const auto& [Key, Value] : ClassConstructorMap)
	{
		if (Key.IsValid())
		{
			if (Key->ClassConstructor == &FClassRegistry::ClassConstructor)
			{
				Key->ClassConstructor = Value;
			}
		}
	}

	ClassConstructorMap.Empty();

	for (auto& [Key, Value] : ClassDescriptorMap)
	{
		delete Value;

		Value = nullptr;
	}

	ClassDescriptorMap.Empty();

	PropertyHashMap.Empty();

	for (auto& [Key, Value] : PropertyDescriptorMap)
	{
		delete Value;

		Value = nullptr;
	}

	PropertyDescriptorMap.Empty();

	CSharpFunctionHashMap.Empty();

	UnrealFunctionHashMap.Empty();

	for (auto& [Key, Value] : CSharpFunctionDescriptorMap)
	{
		delete Value;

		Value = nullptr;
	}

	CSharpFunctionDescriptorMap.Empty();

	for (auto& [Key, Value] : UnrealFunctionDescriptorMap)
	{
		delete Value;

		Value = nullptr;
	}

	UnrealFunctionDescriptorMap.Empty();
}

FClassDescriptor* FClassRegistry::GetClassDescriptor(const UStruct* InStruct) const
{
	const auto FoundClassDescriptor = ClassDescriptorMap.Find(InStruct);

	return FoundClassDescriptor != nullptr ? *FoundClassDescriptor : nullptr;
}

FClassDescriptor* FClassRegistry::GetClassDescriptor(const FName& InClassName) const
{
	const auto InClass = LoadObject<UStruct>(nullptr, *InClassName.ToString());

	return InClass != nullptr ? GetClassDescriptor(InClass) : nullptr;
}

FClassDescriptor* FClassRegistry::AddClassDescriptor(UStruct* InStruct)
{
	if (const auto FoundClassDescriptor = ClassDescriptorMap.Find(InStruct))
	{
		return *FoundClassDescriptor;
	}

	const auto ClassDescriptor = new FClassDescriptor(InStruct);

	ClassDescriptorMap.Add(InStruct, ClassDescriptor);

	return ClassDescriptor;
}

void FClassRegistry::AddClassConstructor(UClass* InClass)
{
	if (!ClassConstructorMap.Contains(InClass))
	{
		ClassConstructorMap.Add(InClass, InClass->ClassConstructor);

		InClass->ClassConstructor = &FClassRegistry::ClassConstructor;
	}
}

void FClassRegistry::RemoveClassDescriptor(const UStruct* InStruct)
{
	if (const auto FoundClassDescriptor = ClassDescriptorMap.Find(InStruct))
	{
		if (const auto Class = Cast<UClass>(const_cast<UStruct*>(InStruct)))
		{
			if (const auto FoundClassConstructor = ClassConstructorMap.Find(Class))
			{
				Class->ClassConstructor = *FoundClassConstructor;

				ClassConstructorMap.Remove(Class);
			}
		}

		delete *FoundClassDescriptor;

		ClassDescriptorMap.Remove(InStruct);
	}
}

FPropertyDescriptor* FClassRegistry::GetOrAddPropertyDescriptor(const uint32 InPropertyHash)
{
	if (const auto FoundPropertyDescriptor = PropertyDescriptorMap.Find(InPropertyHash))
	{
		return *FoundPropertyDescriptor;
	}

	if (const auto FoundPropertyHash = PropertyHashMap.Find(InPropertyHash))
	{
		if (const auto FoundPropertyDescriptor = std::get<0>(*FoundPropertyHash)->AddPropertyDescriptor(
			std::get<1>(*FoundPropertyHash)))
		{
			PropertyHashMap.Remove(InPropertyHash);

			PropertyDescriptorMap.Add(InPropertyHash, FoundPropertyDescriptor);

			return FoundPropertyDescriptor;
		}
	}

	return nullptr;
}

void FClassRegistry::RemoveFunctionDescriptor(const uint32 InFunctionHash)
{
	if (const auto FoundCSharpFunctionDescriptor = CSharpFunctionDescriptorMap.Find(InFunctionHash))
	{
		delete *FoundCSharpFunctionDescriptor;

		CSharpFunctionDescriptorMap.Remove(InFunctionHash);

		CSharpFunctionHashMap.Remove(InFunctionHash);
	}

	if (const auto FoundUnrealFunctionDescriptor = UnrealFunctionDescriptorMap.Find(InFunctionHash))
	{
		delete *FoundUnrealFunctionDescriptor;

		UnrealFunctionDescriptorMap.Remove(InFunctionHash);

		UnrealFunctionHashMap.Remove(InFunctionHash);
	}
}

void FClassRegistry::AddPropertyHash(const uint32 InPropertyHash, FClassDescriptor* InClassDescriptor,
                                     FProperty* InProperty)
{
	PropertyHashMap.Add(InPropertyHash, std::make_tuple(InClassDescriptor, InProperty));
}

void FClassRegistry::RemovePropertyDescriptor(const uint32 InPropertyHash)
{
	if (const auto FoundPropertyDescriptor = PropertyDescriptorMap.Find(InPropertyHash))
	{
		delete *FoundPropertyDescriptor;

		PropertyDescriptorMap.Remove(InPropertyHash);
	}
}

void FClassRegistry::ClassConstructor(const FObjectInitializer& InObjectInitializer)
{
	auto Class = InObjectInitializer.GetClass();

	while (Class != nullptr)
	{
		if (ClassConstructorMap.Contains(Class) && ClassConstructorMap[Class] != &FClassRegistry::ClassConstructor)
		{
			ClassConstructorMap[Class](InObjectInitializer);

			break;
		}

		Class = Class->GetSuperClass();
	}

	if (IsInGameThread())
	{
		if (FDomain::IsLoadSucceed())
		{
			const auto Object = InObjectInitializer.GetObj();

			if (const auto FoundManagedHandle = FCSharpEnvironment::GetEnvironment().GetObject(Object);
				IManagedHandleIsValid(FoundManagedHandle))
			{
				FDynamicClassGenerator::ObjectDeferredInitializer(InObjectInitializer);

				if (const auto FoundClass = FReflectionRegistry::Get().GetClass(Object->GetClass()))
				{
#if WITH_LEANCLR
					// [P6.1 ctor-readback @ClassRegistry] The spawn-instance ctor may funnel here rather than
					// through OnPostClassConstructor. Read Int32Value off the native UObject before/after the
					// ctor to see whether the C# ctor's setter write reaches this instance. If Int32Value
					// isn't found, dump the class + property names so we learn the real generated naming.
					const bool bLeanCLRProbe = Object->GetClass()->GetName().Contains(TEXT("RawDynamic"));

					const FIntProperty* Int32Probe = bLeanCLRProbe
						                                  ? CastField<FIntProperty>(
							                                  Object->GetClass()->FindPropertyByName(TEXT("Int32Value")))
						                                  : nullptr;

					if (bLeanCLRProbe && Int32Probe == nullptr)
					{
						FString PropNames;

						for (TFieldIterator<FProperty> It(Object->GetClass()); It; ++It)
						{
							PropNames += It->GetName() + TEXT(",");
						}

						UE_LOG(LogUnrealCSharp, Warning,
						       TEXT("[P6.1 ctor-readback @ClassRegistry] class=%s NO Int32Value; props=[%s]"),
						       *Object->GetClass()->GetName(), *PropNames);
					}

					if (Int32Probe != nullptr)
					{
						UE_LOG(LogUnrealCSharp, Warning,
						       TEXT("[P6.1 ctor-readback @ClassRegistry] class=%s BEFORE Int32Value=%d"),
						       *Object->GetClass()->GetName(), Int32Probe->GetPropertyValue_InContainer(Object));
					}
#endif

					FoundClass->ConstructorObject(FoundManagedHandle);

#if WITH_LEANCLR
					if (Int32Probe != nullptr)
					{
						UE_LOG(LogUnrealCSharp, Warning,
						       TEXT("[P6.1 ctor-readback @ClassRegistry] class=%s AFTER  Int32Value=%d"),
						       *Object->GetClass()->GetName(), Int32Probe->GetPropertyValue_InContainer(Object));
					}
#endif
				}
			}
		}
	}
}
