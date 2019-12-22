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
// if call the copy, and parameter f is Flag.NeedComponents, 
// it will return if the card has the needed components of MoveLand(Card cd)

namespace GameCore.Base
{
    public abstract class Rule : Util.INode
    {
        public static TComponent AddComponent<TComponent>(Card card) where TComponent : Component, new()
        {
            if (card == null)
                return null;
            if (card.Has<TComponent>())
                return null;
            if (card.Add(Component.GetSpawner<TComponent>().SpawnBase()))
                return null;
            return card.Get<TComponent>();
        }
        public static bool RemoveComponent<TComponent>(Card card) where TComponent : Component, new()
        {
            if (card == null)
                return true;
            if (!card.Has<TComponent>())
                return true;
            card.Remove<TComponent>();
            return false;
        }
        public static bool HasAnyComponent(Card card, params int[] component_type_numbers)
        {
            // also if check card is null
            // if the card has at least one of needed components, return true
            if(card == null)
                return false;
            if(component_type_numbers != null){
                foreach(int i in component_type_numbers){
                    if(card.Has(i))
                        return true;                   
                }
            }
            else
                return false;
            return false;
        }
        public static bool HasComponent(Card card, params int[] component_type_numbers)
        {
            // also if check card is null
            // check if the card has all needed components
            if(card == null)
                return false;
            if(component_type_numbers != null){
                foreach(int i in component_type_numbers){
                    if(!card.Has(i))
                        return false;                   
                }
            }
            else
                return false;
            return true;
        }
        public static bool UnHasComponent(Card card, params int[] component_type_numbers)
        {
            // also if check card is null
            // check if the card don't has all specific components
            if(card == null)
                return false;
            if(component_type_numbers != null){
                foreach(int i in component_type_numbers){
                    if(card.Has(i))
                        return false;                   
                }
            }
            else
                return true;
            return true;
        }
        public static TComponent GetComponent<TComponent>(Card card) where TComponent : Component, new()
        {
            if (card == null)
                return null;
            if (card.Has<TComponent>())
                return null;
            return card.Get<TComponent>();
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
        public enum Flag{
            IsLegal, NeedComponents, UnNeedComponents
        }
    }
}
