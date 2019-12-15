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
            public LocationRule.Level Level { get; private set; } = LocationRule.Level.Box;
            public bool Init(int upper_card, LocationRule.Level level)
            {
                if(upper_card < 0)
                    return true;
                UpperCard = upper_card;
                Level = level;
                if(Core.Instance.Rules.LocationRule.LocationCards.Contains(Card.Number))
                    return true;
                Core.Instance.Rules.LocationRule.LocationCards.Add(Card.Number);
                return false;
            }
            public void SetUpperCard(int upper_card = -1)
            {
                if(upper_card < 0)
                    UpperCard = -1;
                else
                    UpperCard = upper_card;      
            }
            public void SetLevel(LocationRule.Level level)
            {
                Level = level;
            }
        }
        public enum Level
        {
            Box, Room, Building, City, Tile, Region, Land, World, Planet
        }
        private int _c_location_type_number = -1;
        public List<int> LocationCards {get; private set;} = null;
        public bool Init()
        {
            _c_location_type_number = Base.Component.GetSpawner<CLocation>().Type_Number;
            LocationCards = new List<int>();
            return false; 
        }
        public bool AddCLocation(Base.Card card, int upper_card, Level level)
        {
            if(AddComponent<CLocation>(card))
                return true;
            var c_location = card.GetComponent<CLocation>();
            if(c_location == null)
                return true;
            return c_location.Init(upper_card, level);
        }
        public bool RemoveCLocation(Base.Card card)
        {
            if(!HasComponent(card, _c_location_type_number))
                return true;
            card.RemoveComponent(_c_location_type_number);
            LocationCards.Remove(card.Number);
            return false;
        }
        public override bool IsUsable()
        {
            if (_c_location_type_number >= 0
                && LocationCards != null)
                return true;
            return false;
        }
    }
}