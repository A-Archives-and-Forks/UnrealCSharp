using Interop;

namespace Script.Library
{
    public static partial class FActorSpawnParametersImplementation
    {
        private static unsafe partial byte __FActorSpawnParameters_GetbNoFailImplementation(nint A0);

        public static unsafe bool FActorSpawnParameters_GetbNoFailImplementation(nint InObject)
        {
            return __FActorSpawnParameters_GetbNoFailImplementation(InObject) != 0;
        }

        private static unsafe partial void __FActorSpawnParameters_SetbNoFailImplementation(nint A0, byte A1);

        public static unsafe void FActorSpawnParameters_SetbNoFailImplementation(nint InObject, bool InValue)
        {
            __FActorSpawnParameters_SetbNoFailImplementation(InObject, (byte)(InValue ? 1 : 0));
        }

        private static unsafe partial byte __FActorSpawnParameters_GetbDeferConstructionImplementation(nint A0);

        public static unsafe bool FActorSpawnParameters_GetbDeferConstructionImplementation(nint InObject)
        {
            return __FActorSpawnParameters_GetbDeferConstructionImplementation(InObject) != 0;
        }

        private static unsafe partial void __FActorSpawnParameters_SetbDeferConstructionImplementation(nint A0, byte A1);

        public static unsafe void FActorSpawnParameters_SetbDeferConstructionImplementation(nint InObject, bool InValue)
        {
            __FActorSpawnParameters_SetbDeferConstructionImplementation(InObject, (byte)(InValue ? 1 : 0));
        }

        private static unsafe partial byte __FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation(nint A0);

        public static unsafe bool FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation(nint InObject)
        {
            return __FActorSpawnParameters_GetbAllowDuringConstructionScriptImplementation(InObject) != 0;
        }

        private static unsafe partial void __FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation(nint A0, byte A1);

        public static unsafe void FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation(nint InObject,
            bool InValue)
        {
            __FActorSpawnParameters_SetbAllowDuringConstructionScriptImplementation(InObject, (byte)(InValue ? 1 : 0));
        }
    }
}