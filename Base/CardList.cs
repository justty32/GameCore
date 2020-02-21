using System.Collections.Generic;

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
        public bool NewCard(string name = null)
        {
            Card cd = new Card();
            return cd.InitBeNew(name);
        }
        public bool Add(Card card)
        {
            if (!Card.IsUsable(card))
                return true;
            if (cards.ContainsKey(card.Number))
                return true;
            cards.Add(card.Number, card);
            if (!ChangedCards.Contains(card.Number))
                ChangedCards.Add(card.Number);
            return false;
        }
        public bool Remove(int number, bool remove_changed_card = true)
        {
            if (!cards.ContainsKey(number))
                return true;
            cards.Remove(number);
            if(remove_changed_card)
                if (ChangedCards.Contains(number))
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
                        if (ChangedCards.Contains(cards.Count - i))
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
                    if (save_cards)
                        if (ChangedCards.Contains(cdn))
                        {
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
                if (!ChangedCards.Contains(number))
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