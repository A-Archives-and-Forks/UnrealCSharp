#include "Domain/LeanCLR/FLeanCLRLog.h"
#if WITH_LEANCLR
#include "Log/UnrealCSharpLog.h"

void FLeanCLRLog::Log(const char* InMessage)
{
#if !NO_LOGGING
	if (InMessage != nullptr && UE_LOG_ACTIVE(LogUnrealCSharp, Log))
	{
		UE_LOG(LogUnrealCSharp, Log, TEXT("%s"), UTF8_TO_TCHAR(InMessage));
	}
#endif
}

void FLeanCLRLog::ErrorWriter(const char* InMessage)
{
#if !NO_LOGGING
	if (InMessage != nullptr)
	{
		UE_LOG(LogUnrealCSharp, Error, TEXT("%s"), UTF8_TO_TCHAR(InMessage));
	}
#endif
}
#endif
