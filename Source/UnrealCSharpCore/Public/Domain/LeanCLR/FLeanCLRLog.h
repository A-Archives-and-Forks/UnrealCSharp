#pragma once

#if WITH_LEANCLR
#include "CoreMinimal.h"

// Redirects leanclr runtime diagnostics into the UE Output Log (LogUnrealCSharp).
//
// P3 scope: only the plain char* writers are provided. Wiring these into the leanclr runtime
// (Settings::set_report_unhandled_exception_function / set_debugger_log_function and the managed
// LogBridge) is P4.2 (RegisterLog) — see TODO markers there. Kept header-light on purpose: no
// leanclr headers here, so consumers of the log do not pull the vm:: API (design R6).
class FLeanCLRLog
{
public:
	static void Log(const char* InMessage);

	static void ErrorWriter(const char* InMessage);

private:
	FLeanCLRLog() = delete;
};
#endif
