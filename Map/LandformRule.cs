using System;
using System.Collections.Generic;

namespace GameCore.Map
{
    public class LandformRule : Base.Rule
    {
        public class CLandform : Base.Concept
        {
            private string _type_name = "CLandform";
            public override string TypeName => _type_name;
            public bool Init()
            {
                return false;
            }
            public bool Init(Base.Card card)
            {
                return false;
            }
        }
        private int _c_location_type_number = -1;
        private int _c_landform_type_number = -1;
        private int _c_terrain_type_number = -1;
        public override bool Init()
        {
            _c_location_type_number = Base.ConceptManager.GetSpawner<Root.LocationRule.CLocation>().TypeNumber;
            _c_landform_type_number = Base.ConceptManager.GetSpawner<CLandform>().TypeNumber;
            _c_terrain_type_number = Base.ConceptManager.GetSpawner<TerrainRule.CTerrain>().TypeNumber;
            return false;
        }
    }
}