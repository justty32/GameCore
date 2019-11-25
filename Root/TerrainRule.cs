namespace GameCore.Root
{
    public class TerrainRule : Base.Rule
    {
        public class CTerrain : Base.Component
        {
            private string _type_name = "CTerrain";
            public override string TypeName => _type_name; 
        }
    }
}