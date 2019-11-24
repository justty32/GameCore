using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Root
{
    class WorldRule
    {
        class CWorld : Base.Component
        {
            private const string _type_name = "CWorld";
            public override string TypeName => _type_name;
            public int FillingTypeNumber { get; private set; } // things filling between lands
            public bool SetFillingTypeNumber(int filled_thing)
            {
                FillingTypeNumber = filled_thing;
                return false;
            }

        }
    }
}
