﻿using System.Collections.Generic;

namespace GameCore.Interface
{
    // this is using json.serialize to make json string output
    // so variables in it should be careful
    public class Config
    {
        // used in Core, not Data
        // Difference with CoreInfo, these configurations are changeable every time
        // need to be offered a instance in Core, while Core.Init()
        public List<string> RulesInitOrder;
        public Config()
        {
            RulesInitOrder = new List<string>();
        }
    }
}