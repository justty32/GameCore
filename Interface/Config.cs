using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;

namespace GameCore.Interface
{
    public class Config
    {
        // used in Core, not Data
        internal int card_amount_per_file = -1;
        public Config()
        {
            BeDefault();
        }
        public void BeDefault()
        {
            card_amount_per_file = 2000;
        }
        public bool IsUsable()
        {
            if (Util.HasAnyNegative(
                card_amount_per_file
                ))
                return false;
            return true;
        }
    }
}
