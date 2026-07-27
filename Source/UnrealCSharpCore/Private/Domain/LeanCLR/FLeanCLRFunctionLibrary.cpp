#include "Domain/LeanCLR/FLeanCLRFunctionLibrary.h"
#if WITH_LEANCLR
#include "Common/FUnrealCSharpFunctionLibrary.h"
#include "HAL/PlatformProcess.h"
#include "Misc/Paths.h"

FString FLeanCLRFunctionLibrary::GetPublishDirectory()
{
	return FUnrealCSharpFunctionLibrary::GetFullPublishDirectory();
}

FString FLeanCLRFunctionLibrary::GetCorlibDirectory()
{
	// Binary output directory: plugin Binaries/<subdir> in editor, the executable dir otherwise —
	// same layout CoreCLR uses for its runtime. LeanCLR.Build.cs stages corlib under
	// LeanCLR/<Platform>/net beneath it. GetBinariesSubdirectory() ("Win64", ...) is the single
	// runtime source for the <Platform> segment; keep it aligned with LeanCLR.Build.cs's PlatformDir.
#if PLATFORM_ANDROID && !WITH_EDITOR
	// FPlatformProcess::ExecutablePath() is not implemented on Android and fatal-errors, and
	// GetBinariesSubdirectory() returns "" there. Mirror Mono's fixed layout
	// (FMonoFunctionLibrary): resolve from the project dir and hard-code the "Android" segment,
	// matching how LeanCLR.Build.cs stages the corlib net dir.
	return FString::Printf(
		TEXT("%s/Binaries/Android/LeanCLR/Android/net"),
		*FPaths::ProjectDir());
#else
#if WITH_EDITOR
	const auto BinaryOutputDirectory = FString::Printf(
		TEXT("%s/Binaries/%s"),
		*FUnrealCSharpFunctionLibrary::GetPluginDirectory(),
		FPlatformProcess::GetBinariesSubdirectory());
#else
	const auto BinaryOutputDirectory = FPaths::ConvertRelativePathToFull(
		FPaths::GetPath(FPlatformProcess::ExecutablePath()));
#endif

	return FString::Printf(
		TEXT("%s/LeanCLR/%s/net"),
		*BinaryOutputDirectory,
		FPlatformProcess::GetBinariesSubdirectory());
#endif
}

TArray<FString> FLeanCLRFunctionLibrary::GetAssemblySearchDirectories()
{
	// corlib first so System.Private.CoreLib / System.Runtime resolve before the UnrealCSharp
	// assemblies (same ordering the P1 spike used).
	return {GetCorlibDirectory(), GetPublishDirectory()};
}
#endif
