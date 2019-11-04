using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    public class Card
    {
        /*
         * Name will be "none" initially
         * Number will be -1 initially
         */
        public int Number { get; set; } = -1;
        public string Name { get; set; } = null;
        public Card()
        {
            Name = "none";
        }
        public ComponentSet Components
        {
            get => Components;
            set
            {
                if (Components != null)
                    Components.Owners.Remove(this);
                Components = value;
                Components.Owners.Add(this);
            }
        }
    }
}
