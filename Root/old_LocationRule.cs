using System;
using System.Collections.Generic;
using System.Text;

/*
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
        private int _ctn_location = -1;
        public List<int> LocationCards {get; private set;} = null;
        public bool Init()
        {
            _ctn_location = Base.Component.GetSpawner<CLocation>().Type_Number;
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
    */
/*
namespace GameCore.Root
{
    public class LocationRule : Base.Rule
    {
        // Level: Small can't contains Big. Box can contains Box
        //
        // The default constructor do nothing, 
        //
        public class CLocation : Base.Component
        {
            //
            // BelowNumbers generaly not be used, that isn't be set on these functions,
            // the BelowNumbers is the additive way to increase the order,
            // need to be filled by outsider, ex. build a tree while loading data
            //
            private const string _type_name = "CLocation";
            public override string TypeName => _type_name;
            public int Number { get; private set; }
            public int UpperNumber { get; private set; }
            public LocationRule.Level Level { get; private set; }
            public List<int> BelowNumbers { get; private set; } // most time not be used
            public CLocation() {}
            public static bool operator ==(CLocation a, CLocation b)
            {
                return a.Number == b.Number;
            }
            public static bool operator !=(CLocation a, CLocation b)
            {
                return a.Number != b.Number;
            }
            public void BeNew(LocationRule.Level level = LocationRule.Level.Box, CLocation upper_location = null)
            {
                // If has any illegal, set level to box and upper_number be zero
                if (SetLevel(level, true))
                    SetLevel(LocationRule.Level.Box);
                if (SetUpperNumber(upper_location))
                    SetUpperNumber(0);
                // distribute its number
                if (Core.Instance.Rules != null)
                {
                    Core.Instance.Rules.LocationRule._locations_number_distribute_reference++;
                    Number = Core.Instance.Rules.LocationRule._locations_number_distribute_reference;
                }
                else
                    Number = 0;
                // refresh below numbers
                BelowNumbers = new List<int>();
            }
            public void BeNew(int upper_one_number = 0, LocationRule.Level level = LocationRule.Level.Box)
            {
                // If has any illegal, set level to box and upper_number be zero
                if (SetLevel(level, true))
                    SetLevel(LocationRule.Level.Box);
                if (SetUpperNumber(upper_one_number, true))
                    SetUpperNumber(0);
                // distribute its number
                if (Core.Instance.Rules != null)
                {
                    Core.Instance.Rules.LocationRule._locations_number_distribute_reference++;
                    Number = Core.Instance.Rules.LocationRule._locations_number_distribute_reference;
                }
                else
                    Number = 0;
                // refresh below numbers
                BelowNumbers = new List<int>();
            }
            public bool SetLevel(LocationRule.Level level, bool check_is_legal = false)
            {
                // box must contains box
                if (!check_is_legal || level == LocationRule.Level.Box)
                {
                    Level = level;
                    return false;
                }
                else
                {
                    // find instance of this upper one, check it
                    if (Core.Instance.Rules.LocationRule.locations.ContainsKey(UpperNumber))
                        if (Core.Instance.Rules.LocationRule.locations[UpperNumber].Level > level)
                        {
                            Level = level;
                            return false;
                        }
                }
                return true;
            }
            public bool SetUpperNumber(CLocation upper_one)
            {
                // always check is legal
                // target's upper can't be this
                // target's level can't smaller than, or equal to this. (level <= this)
                // If that, do nothing, return true.
                if (upper_one == null)
                    return true;
                if (upper_one.UpperNumber == Number
                    && (upper_one.Level <= Level || Level == LocationRule.Level.Box))
                    return true;
                UpperNumber = upper_one.Number;
                return false;
            }
            public bool SetUpperNumber(int upper_one_number, bool check_is_legal = false)
            {
                // always check is legal
                // target's upper can't be this
                // target's level can't smaller than, or equal to this. (level <= this)
                // If that, do nothing, return true.
                if (!check_is_legal || upper_one_number == 0)
                {
                    UpperNumber = upper_one_number;
                    return false;
                }
                else
                {
                    // find instance of the upper_number
                    // check upper's upper is this
                    // check upper's level is bigger than this, or this level is box
                    if (Core.Instance.Rules.LocationRule.locations.ContainsKey(UpperNumber))
                    {
                        if (Core.Instance.Rules.LocationRule.locations[UpperNumber].UpperNumber != Number
                             && (Core.Instance.Rules.LocationRule.locations[UpperNumber].Level > Level
                                || Level == LocationRule.Level.Box)
                        )
                        {
                            UpperNumber = upper_one_number;
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        public enum Level
        {
            Box, Room, Building, Village, City, Tile, Region, Land, World, Planet, Root
        }
        private int _locations_number_distribute_reference = -1; // How many location number has been distributed. Used for distribute the location's numbers.
        public SortedList<int, CLocation> locations { get; } = new SortedList<int, CLocation>();
        private int _c_location_type_number = 0; // store CLocation's TypeNumber, at Init()
        public int CLocationTypeNumber { get => _c_location_type_number; }
        public LocationRule(){}
        public void Init(int locations_number_distribute_reference)
        {
            _locations_number_distribute_reference = locations_number_distribute_reference;
            CLocation root_location = Base.Component.GetSpawner<CLocation>().Spawn();
            root_location.BeNew(0);
            root_location.SetUpperNumber(0);
            root_location.SetLevel(Level.Root);
            locations.Add(root_location.Number, root_location);
            _c_location_type_number = root_location.TypeNumber;
        }
        public bool AddCLocation(Base.Card card)
        {
            return AddComponent<CLocation>(card);
        }
        public bool BeNewCLocation(Base.Card card, int upper_number = 0, Level level = Level.Box)
        {
            if (card == null)
                return true;
            if (!card.HasComponent(_c_location_type_number))
                return true;
            CLocation component_location = card.Components[_c_location_type_number] as CLocation;
            if (component_location == null)
                return true;
            component_location.BeNew(upper_number, level);
            if (component_location.UpperNumber != upper_number)
                return true;
            return false;
        }
    }
}
*/