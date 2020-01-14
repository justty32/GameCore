using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;

namespace GameCore.Base
{
    public class RuleManager
    {
        public Dictionary<string, Rule> RuleDic {get; private set;}
        public bool Init()
        {
            Core.State.Log.Append("\n");
            Core.State.Log.Append("RuleManager initializing...");
            foreach (var rule_pair in RuleDic)
            {
                if (rule_pair.Value.Init())
                {
                    Core.State.Log.Append("\n");
                    Core.State.Log.Append(rule_pair.Key);
                    Core.State.Log.Append(" initialization failed");
                    return true;
                }
            }
            Core.State.Log.Append("\n");
            Core.State.Log.Append("RuleManager initialization finished");
            return false;
        }
    }
}
