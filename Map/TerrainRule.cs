using System.Collections.Generic;
using GameCore.Base;

namespace GameCore.Map
{
    public class TerrainRule : Base.Rule
    {
        public class CTerrain : Base.Concept
        {
            private string _type_name = "CTerrain";
            public override string TypeName => _type_name; 
        }
        private int _c_location_type_number = -1;
        private int _c_landform_type_number = -1;
        private int _c_terrain_type_number = -1;
        public override bool Init()
        {
            _c_location_type_number = ConceptManager.GetSpawner<Root.LocationRule.CLocation>().TypeNumber;
            _c_landform_type_number = ConceptManager.GetSpawner<LandformRule.CLandform>().TypeNumber;
            _c_terrain_type_number = ConceptManager.GetSpawner<CTerrain>().TypeNumber;
            return false;
        }
    }
}