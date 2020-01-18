﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

// there is a template in Base.Util.RRule, copy that by ctrl-c and ctrl-v

// TODO: add RuleName like concept's TypeName, and adjust RuleManager and its from json.

// make concept's number in default constructor

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
            }catch(Exception)
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
            }catch(Exception)
            {
                return null;
            }
            return json;
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
        public static bool AddConcept(Card card, IList<int> c_type_numbers)
        {
            if (!Card.IsUsable(card))
                return true;
            foreach(int t in c_type_numbers)
            {
                var c = ConceptManager.GetSpawner(t).SpawnBase();
                if (c != null)
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
        public static int GetConceptTypeNumber<TConcept>() where TConcept : Concept, new()
        {
            return ConceptSpawner<TConcept>.GetSpawner().TypeNumber;
        }
        public static int GetDynamicConceptTypeNumber(string type_name)
        {
            return ConceptSpawner<CDynamic>.GetDynamicSpawner(type_name).TypeNumber;
        }
        public static TConcept GetConcept<TConcept>(Card card) where TConcept : Concept, new()
        {
            if (card == null)
                return null;
            if (card.Has<TConcept>())
                return null;
            return card.Get<TConcept>();
        }
    }
}
