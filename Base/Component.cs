﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    public abstract class Component : Util.INode
    {
        /*
         * Every Derives should implement TypeName{get;}, Which is the basis to distinguish type of it
         * TypeNumber is also auto-distributed by TypeName, one associated to one.
         * 
         * To create an entity, use GetSpawner<>().Spawn() first, instead of default constructor.
         * Only if after it, the TypeNumber of the component-type is effective.
         */
        public int TypeNumber { get; set; } = -1; // Which is auto distributed by GameCore, Don't set it Directly ! 
        public abstract string TypeName { get; }
        public virtual JObject ToJsonObject()
        {
            JObject js = null;
            try{
                js = new JObject(new JProperty("TypeName", TypeName));
            }catch(Exception){
                return null;
            }
            return js;
        }
        public virtual bool FromJsonObject(JObject js)
        {
            if(js == null)
                return true;
            try{
                if(!((string)js[TypeName]).Equals(TypeName))
                    return true;
            }catch(Exception){
                return true;
            }
            return false; 
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
             * use Component.GetSpawner<Type>() to create spawner
             */
            if (Core.ComponentManager.SpawnerTypeNameSet.ContainsKey(TypeName))
                TypeNumber = Core.ComponentManager.SpawnerTypeNameSet[TypeName];
            return TypeNumber;
        }
        private Component() {
            /*
             * default create, not recommend. please use Component.GetSpawner().Spawn().
             * Should use TypeNumberAutoSet() After, to set the TypeNumber, or still be -1.
             * 
             * If there isn't have specific spawner yet, TypeNumber not change.
             * TypeNumber setting while spawner be create.
             * use Component.GetSpawner<Type>() to create spawner
             */
        }
    } 
}

