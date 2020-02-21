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
            Core.RuleManager.RuleDic.Add(RuleName, this);
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
}