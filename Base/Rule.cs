using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

// every rule, should register in Interface.Rules, using _NewAndRegist()
// if there are some data need to save to ruledata.json file, implement FromJson and ToJson
// there are template in Base.Util.RRule, copy that by ctrl-c and ctrl-v

// Rule.Flag, can used to define a function's copy Foo(Flag f)
// by diff of parameter, can return needed result
// example : a function LandRule.MoveLand(Card cd), we make a copy MoveLand(Flag f, Card cd)
// if call the copy, and parameter f is Flag.NeedConcepts, 
// it will return if the card has the needed concepts of MoveLand(Card cd)

namespace GameCore.Base
{
    public abstract class Rule : Util.INode
    {
        public Rule()
        {
            Core.Rules.RuleDic.Add(this.ToString(), this);
        }
        public static TConcept AddConcept<TConcept>(Card card) where TConcept : Concept, new()
        {
            if (card == null)
                return null;
            if (card.Has<TConcept>())
                return null;
            if (card.Add(ConceptManager.GetSpawner<TConcept>().SpawnBase()))
                return null;
            return card.Get<TConcept>();
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
            if(card == null)
                return false;
            if(concept_type_numbers != null){
                foreach(int i in concept_type_numbers){
                    if(card.Has(i))
                        return true;                   
                }
            }
            else
                return false;
            return false;
        }
        public static bool HasConcept(Card card, params int[] concept_type_numbers)
        {
            // also if check card is null
            // check if the card has all needed concepts
            if(card == null)
                return false;
            if(concept_type_numbers != null){
                foreach(int i in concept_type_numbers){
                    if(!card.Has(i))
                        return false;                   
                }
            }
            else
                return false;
            return true;
        }
        public static bool UnHasConcept(Card card, params int[] concept_type_numbers)
        {
            // also if check card is null
            // check if the card don't has all specific concepts
            if(card == null)
                return false;
            if(concept_type_numbers != null){
                foreach(int i in concept_type_numbers){
                    if(card.Has(i))
                        return false;                   
                }
            }
            else
                return true;
            return true;
        }
        public static TConcept GetConcept<TConcept>(Card card) where TConcept : Concept, new()
        {
            if (card == null)
                return null;
            if (card.Has<TConcept>())
                return null;
            return card.Get<TConcept>();
        }
        public virtual JObject ToJsonObject()
        {
            return new JObject();
        }
        public virtual bool FromJsonObject(JObject js)
        {
            if(js == null) 
                return true;
            return false;
        }
        public virtual bool IsUsable() => true;
    }
}
