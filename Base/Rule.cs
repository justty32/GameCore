using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

// there is a template in Base.Util.RRule, copy that by ctrl-c and ctrl-v
// make concept's number in default constructor
// TODO : make dynamic rules, which change rule name

namespace GameCore.Base
{
    public abstract class Rule : Util.INode
    {
        public Rule()
        {
            Core._RuleManager.RuleDic.Add(RuleName, this);
        }
        public virtual string RuleName { get => GetType().Name; }
        public virtual bool Init() => false;
        public virtual bool IsUsable() => true;
        public virtual bool FromJsonObject(JObject json)
        {
            if (json == null)
                return true;
            try
            {
                if (!((string)json["RuleName"]).Equals(RuleName))
                    return true;
            }
            catch (Exception)
            {
                return true;
            }
            return false;
        }
        public virtual JObject ToJsonObject()
        {
            JObject json = null;
            try
            {
                json = new JObject();
                json.Add("RuleName", RuleName);
            }
            catch (Exception)
            {
                return null;
            }
            return json;
        }
        public static Card ToCard(int card_number)
        {
            return Core.Cards[card_number];
        }
        public static bool CUsable(Concept c)
        {
            if (c == null)
                return false;
            return c.IsUsable();
        }
        public static bool CUsable(params Concept[] concepts)
        {
            if (concepts == null)
                return false;
            foreach (var c in concepts)
                if (!c.IsUsable())
                    return false;
            return true;
        }
        public static TConcept AddConcept<TConcept>(Card card) where TConcept : Concept, new()
        {
            if (card == null)
                return null;
            if (card.Has<TConcept>())
                return null;
            var c = Concept.Spawn<TConcept>();
            if (c == null)
                return null;
            if (!c.IsUsable())
                return null;
            if (card.Add(c))
                return null;
            return c;
        }
        public static bool AddConcept(Card card, IList<int> c_type_numbers)
        {
            if (!Card.IsUsable(card))
                return true;
            foreach (int t in c_type_numbers)
            {
                var c = ConceptManager.GetSpawner(t).SpawnBase();
                if (c != null && c.IsUsable())
                    card.Add(c);
            }
            return false;
        }
        public static bool RemoveConcept<TConcept>(Card card) where TConcept : Concept, new()
        {
            if (card == null)
                return true;
            if (!card.Has<TConcept>())
                return true;
            card.Remove<TConcept>();
            return false;
        }
        public static bool HasAnyConcept(Card card, params int[] concept_type_numbers)
        {
            // also if check card is null
            // if the card has at least one of needed concepts, return true
            if (card == null)
                return false;
            if (concept_type_numbers != null)
            {
                foreach (int i in concept_type_numbers)
                {
                    if (card.Has(i))
                    {
                        if (card.Get(i).IsUsable())
                            return true;
                        else
                            card.Remove(i);
                    }
                }
            }
            return false;
        }
        public static bool HasConcept(Card card, params int[] concept_type_numbers)
        {
            // also if check card is null
            // check if the card has all needed concepts
            if (card == null)
                return false;
            if (concept_type_numbers != null)
            {
                foreach (int i in concept_type_numbers)
                {
                    if (!card.Has(i))
                    {
                        if (card.Get(i).IsUsable())
                            return true;
                        else
                            card.Remove(i);
                    }
                }
            }
            return true;
        }
        public static bool UnHasConcept(Card card, params int[] concept_type_numbers)
        {
            // also if check card is null
            // check if the card don't has all specific concepts
            if (card == null)
                return false;
            if (concept_type_numbers != null)
            {
                foreach (int i in concept_type_numbers)
                {
                    if (card.Has(i))
                    {
                        if (card.Get(i).IsUsable())
                            return true;
                        else
                            card.Remove(i);
                    }
                }
            }
            return true;
        }
        public static int GetConceptTypeNumber<TConcept>() where TConcept : Concept, new()
        {
            return ConceptSpawner<TConcept>.GetSpawner().TypeNumber;
        }
        public static int GetDynamicConceptTypeNumber(string type_name)
        {
            return ConceptSpawner<CDynamic>.GetCDynamicSpawner(type_name).TypeNumber;
        }
        public static TConcept GetConcept<TConcept>(Card card) where TConcept : Concept, new()
        {
            if (card == null)
                return null;
            var c = card.Get<TConcept>();
            if (c == null)
                return null;
            if (!c.IsUsable())
                return null;
            return c;
        }
    }
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