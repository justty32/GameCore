using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    public class CardList
    {
        // wrap the card list
        public int MaxNumber {get; set;}
        public Dictionary<int, Card> cards { get; private set; } = new Dictionary<int, Card>();
        public bool Add(Card card)
        {
            if (cards.ContainsKey(card.Number))
                return true;
            cards.Add(card.Number, card);
            return false;
        }
        public bool Remove(int number)
        {
            if (!cards.ContainsKey(number))
                return true;
            cards.Remove(number);
            return false;
        }
        public bool Contains(int number) => cards.ContainsKey(number);
        public Card this[int number]
        {
            get
            {
                if (!cards.ContainsKey(number))
                {
                    if (Core.Load.Card(number))
                        return null;
                    if (!cards.ContainsKey(number))
                        return null;
                    return cards[number];
                }
                else
                    return cards[number];
            }
        }
    }
}
