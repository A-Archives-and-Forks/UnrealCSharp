#include "Reflection/FPropertyReflection.h"
#include "Reflection/FReflectionRegistry.h"
#include "Domain/Script/IManagedHandle.h"
#include "Domain/Script/IScriptDomain.h"
#if WITH_LEANCLR
#include "Log/UnrealCSharpLog.h"
#endif

FPropertyReflection::FPropertyReflection(const FString& InName,
                                         const IManagedHandle InManagedProperty,
                                         FClassReflection* InReflectionType,
                                         const TSet<FClassReflection*>& InAttributes,
                                         const TMap<FClassReflection*, TArray<FString>>& InAttributeValues):
	FReflection(InName, InAttributes, InAttributeValues),
	ManagedProperty(InManagedProperty),
	ReflectionType(InReflectionType)
{
	bIsUProperty = Attributes.Contains(FReflectionRegistry::Get().GetUPropertyAttributeClass());

#if WITH_LEANCLR
	if (InName == TEXT("Int32Value") || InName == TEXT("BoolValue"))
	{
		const auto* UPropAttr = FReflectionRegistry::Get().GetUPropertyAttributeClass();

		UE_LOG(LogUnrealCSharp, Warning,
		       TEXT("[P6.1 attr] prop=%s attrs=%d UPropAttrClass=%p (%s) bIsUProperty=%d"),
		       *InName, Attributes.Num(), UPropAttr,
		       UPropAttr != nullptr ? *UPropAttr->GetName() : TEXT("<null>"), bIsUProperty ? 1 : 0);

		for (const auto* Attribute : Attributes)
		{
			UE_LOG(LogUnrealCSharp, Warning, TEXT("[P6.1 attr]   have attr ptr=%p name=%s"),
			       Attribute, Attribute != nullptr ? *Attribute->GetName() : TEXT("<null>"));
		}
	}
#endif
}

FPropertyReflection::~FPropertyReflection()
{
	if (IManagedHandleIsValid(ManagedProperty))
	{
#if WITH_CORECLR
		if (const auto ScriptDomain = IScriptDomain::Get())
		{
			ScriptDomain->Free(ManagedProperty);
		}
#endif

		ManagedProperty = InvalidManagedHandle;
	}
}

FClassReflection* FPropertyReflection::GetReflectionType() const
{
	return ReflectionType;
}

bool FPropertyReflection::IsUProperty() const
{
	return bIsUProperty;
}

void FPropertyReflection::SetValue(const IManagedHandle InManagedHandle, void** InParams) const
{
	if (const auto ScriptDomain = IScriptDomain::Get())
	{
		ScriptDomain->SetPropertyValue(InManagedHandle, Name, InParams);
	}
}
