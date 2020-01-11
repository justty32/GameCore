using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;

namespace GameCore.Base
{
    public class RuleManager
    {
        public Dictionary<string, Rule> RuleDic {get; private set;}
        public Root.TimeRule TimeRule {get; private set;}
        public Root.LocationRule LocationRule {get; private set;}
        private void _NewAndRegist<TRule>(Rule rule) where TRule : Rule, new()
        {
            rule = new TRule();
            RuleDic.Add(rule.GetType().ToString(), rule);
        }
        public RuleManager()
        {
            // make instances
            _NewAndRegist<Root.TimeRule>(TimeRule);
            _NewAndRegist<Root.LocationRule>(LocationRule);
        }
        public bool Init( // many parameters
        )
        {
            // do Init
            if(Base.Util.HasAnyFalse(
                TimeRule.Init(),
                LocationRule.Init()
            ))
                return true;
            return false;
        }
    }
}
