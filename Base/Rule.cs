using System;
using System.Collections.Generic;
using System.Text;

// Rule.Flag, can used to define a function's copy Foo(Flag f)
// by diff of parameter, can return needed result
// example : a function LandRule.MoveLand(Card cd), we make a copy MoveLand(Flag f, Card cd)
// if call the copy, and parameter f is Flag.NeedComponents, 
// it will return if the card has the needed components of MoveLand(Card cd)

namespace GameCore.Base
{
    public abstract class Rule : Util.INode
    {
        public TComponent AddComponent<TComponent>(Card card) where TComponent : Component, new()
        {
            if (card == null)
                return null;
            if (card.HasComponent<TComponent>())
                return null;
            if (card.AddComponent(Component.GetSpawner<TComponent>().SpawnBase()))
                return null;
            return card.GetComponent<TComponent>();
        }
        public bool RemoveComponent<TComponent>(Card card) where TComponent : Component, new()
        {
            if (card == null)
                return true;
            if (!card.HasComponent<TComponent>())
                return true;
            card.RemoveComponent<TComponent>();
            return false;
        }
        public bool HasComponent(Card card, params int[] component_type_numbers)
        {
            // also if check card is null
            // check if the card has all needed components
            if(card == null)
                return false;
            if(component_type_numbers != null){
                foreach(int i in component_type_numbers){
                    if(!card.HasComponent(i))
                        return false;                   
                }
            }
            return true;
        }
        public bool UnHasComponent(Card card, params int[] component_type_numbers)
        {
            // also if check card is null
            // check if the card don't has all specific components
            if(card == null)
                return false;
            if(component_type_numbers != null){
                foreach(int i in component_type_numbers){
                    if(card.HasComponent(i))
                        return false;                   
                }
            }
            return true;
        }
        public TComponent GetComponent<TComponent>(Card card) where TComponent : Component, new()
        {
            if (card == null)
                return null;
            if (card.HasComponent<TComponent>())
                return null;
            return card.GetComponent<TComponent>();
        }
        public virtual bool IsUsable() => true;
        public enum Flag{
            IsLegal, NeedComponents, UnNeedComponents
        }
    }
}
