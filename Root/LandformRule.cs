using System;
using System.Collections.Generic;

namespace GameCore.Root
{
    public class LandformRule : Base.Rule
    {
        public class CLandform : Base.Component
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
        public bool Init()
        {
            _c_location_type_number = Base.Component.GetSpawner<LocationRule.CLocation>().Type_Number;
            _c_landform_type_number = Base.Component.GetSpawner<CLandform>().Type_Number;
            _c_terrain_type_number = Base.Component.GetSpawner<TerrainRule.CTerrain>().Type_Number;
            return false;
        }
    }
}