﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    public abstract class Concept : Util.INode
    {
        /*
         * Every Derives should implement TypeName{get;}, Which is the basis to distinguish type of it
         * TypeNumber is also auto-distributed by TypeName, one associated to one.
         * 
         * To create an entity, use GetSpawner<>().Spawn() first, instead of default constructor.
         * Only if after it, the TypeNumber of the concept-type is effective.
         */
        public int TypeNumber { get; set; } = -1; // Which is auto distributed by GameCore, Don't set it Directly ! 
        public abstract string TypeName { get; }
        public virtual JObject ToJsonObject()
        {
            JObject js = null;
            try{
                js = new JObject(new JProperty("TypeName", TypeName));
                if (js == null)
                    return null;
            }catch(Exception){
                return null;
            }
            return js;
        }
        public virtual Concept FromJsonObject(JObject js)
        {
            if(js == null)
                return null;
            try{
                if(!((string)js["TypeName"]).Equals(TypeName))
                    return null;
            }catch(Exception e){
                return Core.State.WriteException<Concept>(e);
            }
            return null; 
        }
        public static JObject AlignJsonOjbect(JObject js)
        {
            if (js == null)
                return null;
            try{
                // check is legal
                if (!js.ContainsKey("TypeName"))
                    return null;
                // remove redundantion
                if (js.ContainsKey("Card"))
                    js.Remove("Card");
                // reset type number
                int tn = ConceptManager.GetTypeNumber((string)js["TypeName"]);
                if (tn < 0)
                    return null;
                if (js.ContainsKey("TypeNumber"))
                    js.Remove("TypeNumber");
                js.Add("TypeNumber", tn);
            }catch(Exception e){
                return Core.State.WriteException<JObject>(e);
            }
            return js;
        }
        public Card Card { get; set; } = null;
        public virtual bool IsUsable()
        {
            if (TypeNumber >= 0)
                return true;
            return false;
        }
        public int AutoSetTypeNumber()
        {
            /*
             * Set TypeNumber by TypeName, then return TypeNumber.
             * If there isn't have specific spawner yet, TypeNumber not change.
             * 
             * type_number setting while spawner be create.
             * use Concept.GetSpawner<Type>() to create spawner
             */
            if (Core.ConceptManager.SpawnerTypeNameSet.ContainsKey(TypeName))
                TypeNumber = Core.ConceptManager.SpawnerTypeNameSet[TypeName];
            return TypeNumber;
        }
        protected Concept() {
            /*
             * default create, not recommend. please use Concept.GetSpawner().Spawn().
             * Should use TypeNumberAutoSet() After, to set the TypeNumber, or still be -1.
             * 
             * If there isn't have specific spawner yet, TypeNumber not change.
             * TypeNumber setting while spawner be create.
             * use Concept.GetSpawner<Type>() to create spawner
             */
        }
    } 
}
