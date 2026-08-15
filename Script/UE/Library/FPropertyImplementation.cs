using Interop;

namespace Script.Library
{
    public static unsafe partial class FPropertyImplementation
    {
        private static unsafe partial void __FProperty_GetObjectPropertyImplementation(nint A0, uint A1, byte* A2);

        public static void FProperty_GetObjectPropertyImplementation(nint InMonoObject, uint InPropertyHash,
            byte* ReturnBuffer)
        {
            __FProperty_GetObjectPropertyImplementation(InMonoObject, InPropertyHash, ReturnBuffer);
        }

        private static unsafe partial void __FProperty_SetObjectPropertyImplementation(nint A0, uint A1, byte* A2);

        public static void FProperty_SetObjectPropertyImplementation(nint InMonoObject, uint InPropertyHash,
            byte* InBuffer)
        {
            __FProperty_SetObjectPropertyImplementation(InMonoObject, InPropertyHash, InBuffer);
        }

        private static unsafe partial void __FProperty_GetStructPropertyImplementation(nint A0, uint A1, byte* A2);

        public static void FProperty_GetStructPropertyImplementation(nint InMonoObject, uint InPropertyHash,
            byte* ReturnBuffer)
        {
            __FProperty_GetStructPropertyImplementation(InMonoObject, InPropertyHash, ReturnBuffer);
        }

        private static unsafe partial void __FProperty_SetStructPropertyImplementation(nint A0, uint A1, byte* A2);

        public static void FProperty_SetStructPropertyImplementation(nint InMonoObject, uint InPropertyHash,
            byte* InBuffer)
        {
            __FProperty_SetStructPropertyImplementation(InMonoObject, InPropertyHash, InBuffer);
        }
    }
}