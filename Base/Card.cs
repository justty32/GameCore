using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    public partial class Card
    {
        /*
         * Name be "none", and Number be -1 initially
         * If Components being null while getting it, new() and return it.
         *
         * BeNewCard() just distribute a number, after reference++.
         */
        public int Number { get; set; } = -1;
        public string Name { get; set; } = null;
        public void BeNewCard()
        {
            Core.Instance._card_number_distribute_reference++;
            Number = Core.Instance._card_number_distribute_reference;
        }
    }
}
