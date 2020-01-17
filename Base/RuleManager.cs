using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using Newtonsoft.Json.Linq;

// TODO: make a init order list, include static rules and dynamic rules
//  then init by that order list

namespace GameCore.Base
{
    public class RulesCollection
    {
        public Root.TimeRule TimeRule = new Root.TimeRule();
        public Root.LocationRule LocationRule = new Root.LocationRule();
        public Map.TileRule TileRule = new Map.TileRule();
        public Map.TerrainRule TerrainRule = new Map.TerrainRule();
        public Map.LandformRule LandformRule = new Map.LandformRule();
        public Map.LandRule LandRule = new Map.LandRule();
        public Map.WorldRule WorldRule = new Map.WorldRule();
        public Map.PlanetRule PlanetRule = new Map.PlanetRule();
    }
    public class RuleManager
    {
        // rules
        public Dictionary<string, Rule> RuleDic { get; private set; }
        public RulesCollection Rules = null;
        public RuleManager()
        {
            RuleDic = new Dictionary<string, Rule>();
        }
        public bool Init(List<string> initial_order_list = null)
        {
            var order_list = initial_order_list;
            List<string> d_list = new List<string>();
            Core.State.Log.AppendLine("...RuleManager start initializing by initial order list...");
            if (order_list == null )
            {
                Core.State.Log.AppendLine("init order list not found, the order will be random");
                order_list = new List<string>(RuleDic.Keys);
            }
            else if (order_list.Count < RuleDic.Count)
            {
                Core.State.Log.AppendLine("rules down below are not in init order list");
                Core.State.Log.AppendLine("these rules will be Initialized randomly after init order list's initialization...");
                foreach(var rs in RuleDic.Keys)
                {
                    if (!order_list.Contains(rs))
                        d_list.Add(rs);
                }
                foreach (var s in d_list)
                    Core.State.Log.AppendLine(s);
            }
            Core.State.Log.AppendLine("...Rules are initializing...");
            for(int i = 0; i < order_list.Count; i++)
            {
                if(RuleDic.ContainsKey(order_list[i]))
                {
                    if (RuleDic[order_list[i]].Init())
                    {
                        Core.State.Log.Append(order_list[i]);
                        Core.State.Log.AppendLine(" initialized failed");
                        return true;
                    }
                    else
                    {
                        Core.State.Log.Append(order_list[i]);
                        Core.State.Log.AppendLine(" intialized");
                    }
                }
                else
                {
                    Core.State.Log.Append(order_list[i]);
                    Core.State.Log.AppendLine(" not found!");
                }
            }
            for(int i = 0; i < d_list.Count; i++)
            {
                if (RuleDic[d_list[i]].Init())
                {
                    Core.State.Log.Append(d_list[i]);
                    Core.State.Log.AppendLine(" initialized failed");
                    return true;
                }
                else
                {
                    Core.State.Log.Append(d_list[i]);
                    Core.State.Log.AppendLine(" intialized");
                }
            }
            Core.State.Log.AppendLine("...Rules' initialization finished...");
            return false;
        }
        public bool FromJsonArray(JArray ja)
        {
            if (ja == null)
                return true;
            try
            {
                Core.State.Log.AppendLine("...RuleManager doing FromJsonArray for rules...");
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
                Core.State.Log.AppendLine("...RuleManager doing FromJsonArray done");
            }catch(Exception e)
            {
                return Core.State.WriteException(e);
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
