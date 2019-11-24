using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    public abstract class Rule
    {
        public bool AddComponent<TComponent>(Card card) where TComponent : Component, new()
        {
            if (card == null)
                return true;
            if (card.HasComponent<TComponent>())
                return true;
            return card.AddComponent(Component.GetSpawner<TComponent>().SpawnBase());
        }
    }
}
