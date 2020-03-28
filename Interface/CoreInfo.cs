namespace GameCore.Interface
{
    // static data
    public static class CoreInfo
    {
        public static readonly string Version = "0.01";
        public static readonly string dependencies = "json.net" +
            ", unity2019.2.17f1" + ", XLua";
        public static readonly int Card_amount_per_file = 2000;
        public static readonly int Bits = 32;
    }
}