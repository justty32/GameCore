using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Root
{
    public class TileRule : Base.Rule
    {
        public class CTile : Base.Component
        {
            private const string _type_name = "CTile";
            public override string TypeName => _type_name;
            public int PositionX { get; set; } = -1;
            public int PositionY { get; set; } = -1;
        }
        private int _c_location_type_number = -1;
        private int _c_tile_type_number = -1;
        public int CTileTypeNumber { get => _c_tile_type_number; }
        public bool Init()
        {
            _c_location_type_number = Base.Component.GetSpawner<LocationRule.CLocation>().Type_Number;
            _c_tile_type_number = Base.Component.GetSpawner<CTile>().Type_Number;
            return false;
        }
        public bool AddCTile(Base.Card card)
        {
            return AddComponent<CTile>(card);
        }
    }
}
