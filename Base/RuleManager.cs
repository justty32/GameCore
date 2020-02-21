using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace GameCore.Base
{
    public class RulesCollection
    {
        public Root.EntityRule EntityRule = new Root.EntityRule();
        public Root.TimeRule TimeRule = new Root.TimeRule();
        public Root.LocationRule LocationRule = new Root.LocationRule();
        public Thing.ItemRule ItemRule = new Thing.ItemRule();
        public Thing.CraftRule CraftRule = new Thing.CraftRule();
        public Thing.StoreRule StoreRule = new Thing.StoreRule();
        public Thing.MaterialRule MaterialRule = new Thing.MaterialRule();
    }

    public class RuleManager
    {
        // rules
        public Dictionary<string, Rule> RuleDic { get; private set; }
        public RulesCollection Rules = null;
        public List<string> InitOrderList = null;
        public RuleManager()
        {
            RuleDic = new Dictionary<string, Rule>();
        }
        private List<string> _AlignInitOrderListByRuleDic(List<string> order_list, bool log = true)
        {
            if (RuleDic == null)
                return null;
            if (order_list == null)
            {
                order_list = new List<string>(RuleDic.Keys);
                Core.State.Log.AppendLine("initial order list not found");
            }
            if (log)
            {
                Core.State.Log.AppendLine("down below rules are not in order list");
                Core.State.Log.AppendLine("it will still be worked, but the order will be randomly");
            }
            foreach (var rpair in RuleDic)
            {
                if (!order_list.Contains(rpair.Key))
                {
                    order_list.Add(rpair.Key);
                    Core.State.Log.AppendLine(rpair.Key);
                }
            }
            return order_list;
        }
        public bool Init(List<string> initial_order_list = null)
        {
            var order_list = initial_order_list;
            Core.State.Log.AppendLine("...RuleManager start initializing by initial order list...");
            order_list = _AlignInitOrderListByRuleDic(order_list);
            Core.State.Log.AppendLine("...Rules are initializing...");
            for (int i = 0; i < order_list.Count; i++)
            {
                if (RuleDic.ContainsKey(order_list[i]))
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
                    Core.State.Log.AppendLine(" rule not found in RuleDictionary!");
                }
            }
            Core.State.Log.AppendLine("...Rules' initialization finished...");
            InitOrderList = order_list;
            return false;
        }
        public bool FromJsonArray(JArray ja)
        {
            //try to do FromJsonArray by order in InitOrderList
            // if InitOrderList is null or error, the order will be default
            if (ja == null)
                return true;
            try
            {
                Core.State.Log.AppendLine("...RuleManager doing FromJsonArray for rules...");
                InitOrderList = _AlignInitOrderListByRuleDic(InitOrderList, false);
                for (int i = 0; i < InitOrderList.Count; i++)
                {
                    // for every rules in jarray
                    for (int j = 0; j < ja.Count; j++)
                    {
                        // if rule name equals to target
                        if (Util.JObjectContainsKey((JObject)ja[j], "RuleName"))
                        {
                            if (((string)ja[j]["RuleName"]).Equals(InitOrderList[i]))
                            {
                                // and rule dic contains it, init it
                                if (RuleDic.ContainsKey(InitOrderList[i]))
                                {
                                    if (RuleDic[InitOrderList[i]].FromJsonObject((JObject)ja[j]))
                                        return true;
                                    Core.State.Log.Append(InitOrderList[i]);
                                    Core.State.Log.AppendLine(" has from json object");
                                }
                                else
                                {
                                    Core.State.Log.Append(InitOrderList[i]);
                                    Core.State.Log.AppendLine(" error from json object !!!");
                                }
                            }
                        }
                        else
                        {
                            Core.State.Log.Append(InitOrderList[i]);
                            Core.State.Log.AppendLine(" 's data not in save file ");
                        }
                        // if not find, do nothing
                    }
                }
                Core.State.Log.AppendLine("...RuleManager doing FromJsonArray done");
            }
            catch (Exception e)
            {
                return Core.State.WriteException(e);
            }
            return false;
        }
        public JArray ToJsonArray()
        {
            //try to do ToJsonArray by reverse of InitOrderList
            // if InitOrderList is null or error, the order will be default
            JArray ja = new JArray();
            try
            {
                Core.State.Log.AppendLine("...RuleManager doing ToJsonArray...");
                InitOrderList = _AlignInitOrderListByRuleDic(InitOrderList, false);
                for (int i = InitOrderList.Count - 1; i >= 0; i--)
                {
                    if (RuleDic.ContainsKey(InitOrderList[i]))
                    {
                        var rule_json = RuleDic[InitOrderList[i]].ToJsonObject();
                        if (rule_json == null)
                            return null;
                        if (!Util.JObjectContainsKey(rule_json, "RuleName"))
                            rule_json.Add("RuleName", RuleDic[InitOrderList[i]].RuleName);
                        ja.Insert(0, rule_json);
                        Core.State.Log.Append(InitOrderList[i]);
                        Core.State.Log.AppendLine(" has been to json object");
                    }
                }
                Core.State.Log.AppendLine("...RuleManager doing ToJsonArray done...");
            }
            catch (Exception e)
            {
                return Core.State.WriteException<JArray>(e);
            }
            return ja;
        }
    }
}