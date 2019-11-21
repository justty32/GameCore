using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Root
{
    public class TileRule
    {
        public class CTile : Base.Component
        {
            private string _type_name = "CTile";
            public override string TypeName => _type_name;
            public int PositionX { get; set; } = -1;
            public int PositionY { get; set; } = -1;
        }
        public bool Init()
        {
            return false;
        }
    }
}
