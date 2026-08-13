#pragma once

#include "CoreMinimal.h"

#if WITH_LEANCLR
// Path / directory helpers for the LeanCLR backend.
//
// Per design §4.8.4 ALL platform-specific path differences must live here (and in LeanCLR.Build.cs);
// FLeanCLRDomain.cpp itself must stay platform-branch-free. leanclr is fed assemblies through a
// file loader (Settings::set_file_loader) rather than loading from a fixed layout, so these helpers
// just enumerate the directories that loader searches: the UnrealCSharp publish output (Interop / UE
// / Game) and the corlib (.NET 10 BCL IL) staged next to the binary by LeanCLR.Build.cs::StageCorlib.
class UNREALCSHARPCORE_API FLeanCLRFunctionLibrary
{
public:
	// Directory holding the published Interop / UE / Game assemblies. Reuses the shared publish path.
	static FString GetPublishDirectory();

	// Directory holding the corlib (System.Private.CoreLib + BCL IL) that leanclr loads at startup.
	// Mirrors LeanCLR.Build.cs staging: <BinaryOutputDir>/LeanCLR/<Platform>/net.
	static FString GetCorlibDirectory();

	// Ordered directories the leanclr file loader should search (corlib first, then publish).
	static TArray<FString> GetAssemblySearchDirectories();
};
#endif
