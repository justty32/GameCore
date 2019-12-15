using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    public abstract class Rule : Util.INode
    {
        public bool AddComponent<TComponent>(Card card) where TComponent : Component, new()
        {
            if (card == null)
                return true;
            if (card.HasComponent<TComponent>())
                return true;
            return card.AddComponent(Component.GetSpawner<TComponent>().SpawnBase());
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
        public TComponent GetComponent<TComponent>(Card card) where TComponent : Component, new()
        {
            if (card == null)
                return null;
            if (card.HasComponent<TComponent>())
                return null;
            return card.GetComponent<TComponent>();
        }
        public abstract bool IsUsable();
    }
}
