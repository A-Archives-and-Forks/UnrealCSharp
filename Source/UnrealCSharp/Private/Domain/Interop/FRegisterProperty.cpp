#include "Binding/Class/FClassBuilder.h"
#include "Environment/FCSharpEnvironment.h"
#include "CoreMacro/BufferMacro.h"
#include "CoreMacro/NamespaceMacro.h"
#if WITH_LEANCLR
#include "Log/UnrealCSharpLog.h"
#endif

namespace
{
	struct FRegisterProperty
	{
		static void GetObjectPropertyImplementation(const IManagedHandle InManagedHandle,
		                                            const uint32 InPropertyHash, RETURN_BUFFER_SIGNATURE)
		{
			if (const auto FoundAddress = FCSharpEnvironment::GetEnvironment().GetAddress<
				UObject, void*>(InManagedHandle))
			{
				if (const auto PropertyDescriptor = FCSharpEnvironment::GetEnvironment().
					GetOrAddPropertyDescriptor(InPropertyHash))
				{
					PropertyDescriptor->Get(
						PropertyDescriptor->ContainerPtrToValuePtr<void>(FoundAddress),
						RETURN_BUFFER);
				}
			}
		}

		static void SetObjectPropertyImplementation(const IManagedHandle InManagedHandle,
		                                            const uint32 InPropertyHash, IN_BUFFER_SIGNATURE)
		{
			const auto FoundAddress = FCSharpEnvironment::GetEnvironment().GetAddress<UObject, void*>(InManagedHandle);

#if WITH_LEANCLR
			// [P6.1 propset] The exact write path. GetHandle(this) is valid (=44638), so the ctor-time write
			// loss is one of these two resolutions failing. Log addr (handle->UObject) and desc (hash->FProperty)
			// whenever the target is the dynamic test actor, or whenever addr is null. Compare ctor-time vs
			// test-time sets for the SAME hash to see which resolution differs.
			{
				const auto ProbeObject = reinterpret_cast<UObject*>(FoundAddress);

				if (FoundAddress == nullptr ||
					(ProbeObject != nullptr && ProbeObject->GetClass()->GetName().Contains(TEXT("Dynamic"))))
				{
					const auto ProbeDesc = FCSharpEnvironment::GetEnvironment().GetOrAddPropertyDescriptor(
						InPropertyHash);

					UE_LOG(LogUnrealCSharp, Warning, TEXT("[P6.1 propset] hash=%u addr=%p class=%s desc=%p"),
					       InPropertyHash, FoundAddress,
					       ProbeObject != nullptr ? *ProbeObject->GetClass()->GetName() : TEXT("<null>"), ProbeDesc);
				}
			}
#endif

			if (FoundAddress)
			{
				if (const auto PropertyDescriptor = FCSharpEnvironment::GetEnvironment().
					GetOrAddPropertyDescriptor(InPropertyHash))
				{
					PropertyDescriptor->Set(IN_BUFFER, PropertyDescriptor->ContainerPtrToValuePtr<void>(FoundAddress));
				}
			}
		}

		static void GetStructPropertyImplementation(const IManagedHandle InManagedHandle,
		                                            const uint32 InPropertyHash, RETURN_BUFFER_SIGNATURE)
		{
			if (const auto FoundAddress = FCSharpEnvironment::GetEnvironment().GetAddress<
				UScriptStruct, void*>(InManagedHandle))
			{
				if (const auto PropertyDescriptor = FCSharpEnvironment::GetEnvironment().
					GetOrAddPropertyDescriptor(InPropertyHash))
				{
					PropertyDescriptor->Get(PropertyDescriptor->ContainerPtrToValuePtr<void>(FoundAddress),
					                        RETURN_BUFFER);
				}
			}
		}

		static void SetStructPropertyImplementation(const IManagedHandle InManagedHandle,
		                                            const uint32 InPropertyHash, IN_BUFFER_SIGNATURE)
		{
			if (const auto FoundAddress = FCSharpEnvironment::GetEnvironment().GetAddress<
				UScriptStruct, void*>(InManagedHandle))
			{
				if (const auto PropertyDescriptor = FCSharpEnvironment::GetEnvironment().
					GetOrAddPropertyDescriptor(InPropertyHash))
				{
					PropertyDescriptor->Set(IN_BUFFER,
					                        PropertyDescriptor->ContainerPtrToValuePtr<void>(FoundAddress));
				}
			}
		}

		FRegisterProperty()
		{
			FClassBuilder(TEXT("FProperty"), NAMESPACE_LIBRARY)
				.Function("GetObjectProperty", GetObjectPropertyImplementation)
				.Function("SetObjectProperty", SetObjectPropertyImplementation)
				.Function("GetStructProperty", GetStructPropertyImplementation)
				.Function("SetStructProperty", SetStructPropertyImplementation);
		}
	};

	[[maybe_unused]] FRegisterProperty RegisterProperty;
}
