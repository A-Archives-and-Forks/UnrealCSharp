#pragma once

#if WITH_LEANCLR
#include "CoreMinimal.h"

// Redirects leanclr runtime diagnostics into the UE Output Log (LogUnrealCSharp).
//
// Wired into the runtime by FLeanCLRDomain::RegisterLog (Settings::set_report_unhandled_exception_function
// / set_debugger_log_function). Kept header-light on purpose: no leanclr headers here, so consumers of
// the log do not pull the vm:: API (design R6).
class FLeanCLRLog
{
public:
	static void ErrorWriter(const char* InMessage);

private:
	FLeanCLRLog() = delete;
};
#endif
