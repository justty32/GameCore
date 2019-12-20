using System;
using System.Collections.Generic;
using GameCore.Base;

namespace GameCore.Root
{
    public class RootRule : Rule
    {
        public TimeRule TimeRule {get; private set;}
        public LocationRule LocationRule {get; private set;}
        public bool Init()
        {
            TimeRule = new TimeRule();
            LocationRule = new LocationRule();
            if(Util.HasAnyNull(TimeRule, LocationRule))
                return true;
            if(Util.HasAnyTrue(
                TimeRule.Init(), LocationRule.Init()
            ))
                return true;
            return false;
        }
        public override bool IsUsable()
        {
            if(Util.HasAnyNull(TimeRule, LocationRule))
                return false;
            if(Util.HasAnyFalse(
                TimeRule.IsUsable(), LocationRule.IsUsable()
            ))
                return false;
            return true;
        }
    }
}