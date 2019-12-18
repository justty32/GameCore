using System.Collections.Generic;

namespace GameCore.Map
{
    public class TerrainRule : Base.Rule
    {
        public class CTerrain : Base.Component
        {
            private string _type_name = "CTerrain";
            public override string TypeName => _type_name; 
        }
        private int _c_location_type_number = -1;
        private int _c_landform_type_number = -1;
        private int _c_terrain_type_number = -1;
        public bool Init()
        {
            _c_location_type_number = Base.Component.GetSpawner<Root.LocationRule.CLocation>().Type_Number;
            _c_landform_type_number = Base.Component.GetSpawner<LandformRule.CLandform>().Type_Number;
            _c_terrain_type_number = Base.Component.GetSpawner<CTerrain>().Type_Number;
            return false;
        }
    }
}