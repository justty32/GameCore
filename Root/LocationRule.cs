using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Root
{
    public class LocationRule
    {
        // Level: Small can't contains Big. Box can contains Box
        /*
         * The default constructor do nothing, 
         * but Init() import the location components number's distribute reference
         */
        public class CLocation : Base.Component
        {
            /*
             * BeNew() will distribute a number, after reference++
             * 
             * BelowNumbers generaly not be used, that isn't be set on these functions,
             * the BelowNumbers is the additive way to increase the order,
             * need to be filled by outsider, ex. build a tree while loading data
             */
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
            // only add the CLocation
            if (card == null)
                return true;
            if (card.AddComponent(Base.Component.GetSpawner<LocationRule.CLocation>().Spawn()))
                return true;
            return false;
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
