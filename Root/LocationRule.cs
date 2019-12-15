using System;
using System.Collections.Generic;

namespace GameCore.Root
{
    public class LocationRule : Base.Rule
    {
        public class CLocation : Base.Component
        {
            private string _type_name = "CLocation";
            public override string TypeName => _type_name;
            public int UpperCard { get; private set; } = -1;
            public List<int> BelowCards { get; private set; }
            public bool Init(int upper_card, params int[] below_cards)
            {
                if(upper_card < 0)
                    return true;
                UpperCard = upper_card;
                if(below_cards != null)
                    BelowCards = new List<int>(below_cards);
                else
                    BelowCards = new List<int>();
                if(Core.Instance.Rules.LocationRule.LocationCards.Contains(Card.Number))
                    return true;
                Core.Instance.Rules.LocationRule.LocationCards.Add(Card.Number);
                return false;
            }
            public override bool IsUsable()
            {
                if (TypeNumber < 0 || BelowCards == null)
                    return false;
                return true;
            }
        }
        private int _ctn_location = -1;
        public List<int> LocationCards {get; private set;} = null;
        public bool Init()
        {
            _ctn_location = Base.Component.GetSpawner<CLocation>().Type_Number;
            LocationCards = new List<int>();
            return false; 
        }
        public bool AddCLocation(Base.Card card, int upper_card, params int[] below_cards)
        {
            if(AddComponent<CLocation>(card))
                return true;
            var c_location = card.GetComponent<CLocation>();
            if(c_location == null)
                return true;
            return c_location.Init(upper_card, below_cards);
        }
        public bool RemoveCLocation(Base.Card card)
        {
            if(!HasComponent(card, _ctn_location))
                return true;
            card.RemoveComponent(_ctn_location);
            LocationCards.Remove(card.Number);
            return false;
        }
        public override bool IsUsable()
        {
            if (_ctn_location >= 0
                && LocationCards != null)
                return true;
            return false;
        }
    }
}