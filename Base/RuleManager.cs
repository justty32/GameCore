using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;

namespace GameCore.Base
{
    public class RuleManager
    {
        public Dictionary<string, Rule> RuleDic {get; private set;}
        public Root.TimeRule TimeRule { get; private set; } = new Root.TimeRule();
        public Root.LocationRule LocationRule { get; private set; } = new Root.LocationRule();
    }
}
