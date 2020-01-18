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
        public int MaxNumber { get; set; } = -1;
        public List<int> ChangedCards { get; set; } = new List<int>();
        public Dictionary<int, Card> cards { get; private set; } = new Dictionary<int, Card>();
        public void ResetChangedCards()
        {
            ChangedCards = new List<int>();
        }
        public bool Add(Card card)
        {
            if (!Card.IsUsable(card))
                return true;
            if (cards.ContainsKey(card.Number))
                return true;
            cards.Add(card.Number, card);
            if(!ChangedCards.Contains(card.Number))
            ChangedCards.Add(card.Number);
            return false;
        }
        public bool Remove(int number)
        {
            if (!cards.ContainsKey(number))
                return true;
            cards.Remove(number);
            if(ChangedCards.Contains(number))
                ChangedCards.Remove(number);
            return false;
        }
        public bool Contains(int number) => cards.ContainsKey(number);
        public Card this[int number]
        {
            get
            {
                if(!ChangedCards.Contains(number))
                    ChangedCards.Add(number);
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
