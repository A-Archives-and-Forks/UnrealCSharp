using Interop;

#if LEANCLR
using System.Runtime.InteropServices;

#endif
namespace Script.Library
{
    public static partial class UDataTableFunctionLibraryImplementation
    {
#if LEANCLR
        [DllImport("__UnrealCSharpLeanCLR", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe byte __UDataTableFunctionLibrary_GetDataTableRowFromNameImplementation(nint A0, nint A1, nint* A2);
#else
        private static unsafe delegate* unmanaged[Cdecl]<nint, nint, nint*, byte>
            __UDataTableFunctionLibrary_GetDataTableRowFromNameImplementation;
#endif

        public static unsafe bool UDataTableFunctionLibrary_GetDataTableRowFromNameImplementation<T>(
            nint Table, nint RowName, out T OutRow)
        {
#if !LEANCLR
            if (__UDataTableFunctionLibrary_GetDataTableRowFromNameImplementation == null)
            {
                __UDataTableFunctionLibrary_GetDataTableRowFromNameImplementation =
                    (delegate* unmanaged[Cdecl]<nint, nint, nint*, byte>)
                    MethodBridge.GetMethod(
                        "Script.Library.UDataTableFunctionLibraryImplementation::UDataTableFunctionLibrary_GetDataTableRowFromNameImplementation");
            }
#endif

            nint OutHandle = 0;

            var Result = __UDataTableFunctionLibrary_GetDataTableRowFromNameImplementation(Table, RowName, &OutHandle);

            OutRow = OutHandle != 0 ? (T)HandleData.GetObject(OutHandle) : default;

            return Result != 0;
        }
    }
}