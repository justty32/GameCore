namespace GameCore.Interface
{
    // static data
    // Can't be changed by outside
    // Difference with Config.cs, is these things shouldn't be change
    // Be separated by version, same version offer same member and same value.
    public static class CoreInfo
    {
        public static readonly string Version = "0.01";
        public static readonly string dependencies = "json.net" +
            ", unity2019.2.17f1";
        public static readonly int Card_amount_per_file = 2000;
        public static readonly int Bits = 32;
    }
}