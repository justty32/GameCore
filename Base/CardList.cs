using System;
using System.Collections.Generic;

namespace GameCore.Base
{
    public class CardList
    {
        // wrap the card list
        public int MaxNumber { get; set; } = -1;
        public HashSet<int> ChangedCards { get; set; } = new HashSet<int>();
        public SortedDictionary<int, Card> cards { get; private set; } = new SortedDictionary<int, Card>();
        public void ResetChangedCards()
        {
            ChangedCards = new HashSet<int>();
        }
        public Card NewCard(string name = null)
        {
            Card cd = new Card();
            if (cd.InitBeNew(name))
                return null;
            return cd;
        }
        public bool Add(Card card)
        {
            if (!Card.IsUsable(card))
                return true;
            if (cards.ContainsKey(card.Number))
                return true;
            cards.Add(card.Number, card);
            ChangedCards.Add(card.Number);
            return false;
        }
        public bool Remove(int number, bool remove_changed_card = true)
        {
            if (!cards.ContainsKey(number))
                return true;
            cards.Remove(number);
            if(remove_changed_card)
                ChangedCards.Remove(number);
            return false;
        }
        public bool Release(int count = 0, bool save_cards = false, bool from_last_addition = false)
        {
            if (count < 0)
                return true;
            if (from_last_addition)
            {
                for(int i = 1; i <= count; i++)
                {
                    cards.Remove(cards.Count - i);
                    if (save_cards)
                    {
                        Core.Save.Card(cards.Count - i);
                        ChangedCards.Remove(cards.Count - i);
                    }
                }
            }
            else
            {
                int i = 0;
                foreach (var cdn in cards.Keys)
                {
                    cards.Remove(cdn);
                    if (save_cards) { 
                        Core.Save.Card(cdn);
                        ChangedCards.Remove(cdn);
                    }
                    i++;
                    if (i > count)
                        break;
                }
            }
            return false;
        }
        public bool Contains(int number) => cards.ContainsKey(number);
        public Card this[int number]
        {
            get
            {
                ChangedCards.Add(number);
                Card cd;
                if (!cards.TryGetValue(number, out cd))
                {
                    if (Core.Load.Card(number))
                        return null;
                    if (!cards.TryGetValue(number, out cd))
                        return null;
                    else
                        return cd;
                }else
                    return cd;
            }
        }
    }
}