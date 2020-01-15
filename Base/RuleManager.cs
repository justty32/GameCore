using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using Newtonsoft.Json.Linq;

// TODO: make a init order list, include static rules and dynamic rules
//  then init by that order list

namespace GameCore.Base
{
    public class RuleManager
    {
        // rules
        public Dictionary<string, Rule> RuleDic { get; private set; } = new Dictionary<string, Rule>();
        public bool Init(List<string> order_list = null)
        {
            Core.State.Log.AppendLine("RuleManager initializing...");
            if (order_list == null || order_list.Count != RuleDic.Count)
            {
                Core.State.Log.AppendLine("rules' init order list error, using default init order");
                foreach (var rule_pair in RuleDic)
                {
                    if (rule_pair.Value.Init())
                    {
                        Core.State.Log.Append(rule_pair.Key);
                        Core.State.Log.AppendLine(" initialized failed");
                        return true;
                    }
                    Core.State.Log.Append(rule_pair.Key);
                    Core.State.Log.AppendLine(" intialized");
                }
            }
            else
            {
                for(int i = 0; i < order_list.Count; i++)
                {
                    if(RuleDic.ContainsKey(order_list[i]))
                    {
                        if (RuleDic[order_list[i]].Init())
                        {
                            Core.State.Log.Append(RuleDic[order_list[i]].GetType().ToString());
                            Core.State.Log.AppendLine(" initialized failed");
                            return true;
                        }
                        else
                        {
                            Core.State.Log.Append(RuleDic[order_list[i]].GetType().ToString());
                            Core.State.Log.AppendLine(" intialized");
                        }
                    }
                    else
                    {
                        Core.State.Log.Append(order_list[i]);
                        Core.State.Log.AppendLine(" not found!");
                    }
                }
            }
            Core.State.Log.AppendLine("RuleManager initialization finished");
            return false;
        }
        public bool FromJsonArray(JArray ja)
        {
            if (ja == null)
                return true;
            try
            {
                Core.State.Log.AppendLine("RuleManager doing FromJsonArray for rules...");
                for(int i = 0; i < ja.Count; i++)
                {
                    if(RuleDic.ContainsKey((string)ja[i]["RuleName"]))
                    {
                        if (RuleDic[(string)ja[i]["RuleName"]].FromJsonObject((JObject)ja[i]))
                            return true;
                        Core.State.Log.Append((string)ja[i]["RuleName"]);
                        Core.State.Log.AppendLine(" has from json object");
                    }
                    else
                    {
                        Core.State.Log.Append((string)ja[i]["RuleName"]);
                        Core.State.Log.AppendLine(" error from json object !!!");
                    }
                }
                Core.State.Log.AppendLine("RuleManager doing FromJsonArray done");
            }catch(Exception)
            {
                Core.State.Log.AppendLine("RuleManager doing FromJsonArray exception !!!");
                return true;
            }
            return false;
        }
        public JArray ToJsonArray()
        {
            JArray ja = new JArray();
            try
            {
                foreach(var rp in RuleDic)
                {
                    var j = rp.Value.ToJsonObject();
                    if (!j.ContainsKey("RuleName"))
                        j.Add("RuleName", rp.Key);
                    ja.Add(j);
                }
            }catch(Exception)
            {
                return null;
            }
            return ja;
        }
    }
}
