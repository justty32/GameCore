using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Map
{
    public class TileRule : Base.Rule
    {
        public class CTile : Base.Component
        {
            private const string _type_name = "CTile";
            public override string TypeName => _type_name;
            public int PositionX { get; internal set; } = -1;
            public int PositionY { get; internal set; } = -1;
        }
        private int _ctn_location = -1;
        private int _ctn_tile = -1;
        public bool Init()
        {
            _ctn_location = Base.Component.GetSpawner<Root.LocationRule.CLocation>().Type_Number;
            _ctn_tile = Base.Component.GetSpawner<CTile>().Type_Number;
            return false;
        }
        public bool AddCTile(Base.Card card)
        {
            return AddComponent<CTile>(card);
        }
    }
}
