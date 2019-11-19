using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Root
{
    public class LocationRule 
    {
        public class CLocation : Base.Component
        {
            // Level: Small can't contains Big. Box can contains Box
            public enum TypeLevel
            {
                Box, Room, Building, Village, City, Tile, Region, Land, World, Planet, Root
            }
            private const string type_name = "CLocation";
            public override string TypeName => type_name;
            private int _number = -1; private int _upper_number = 0; private TypeLevel _level = TypeLevel.Box;
            private List<int> _below_numbers;
            public int Number { get => _number; }
            public int UpperNumber { get => _upper_number; }
            public TypeLevel Level { get => _level; }
            public List<int> BelowNumbers { get => _below_numbers; } // most time not be used
            public CLocation() {}
            public static bool operator ==(CLocation a, CLocation b)
            {
                return a.Number == b.Number;
            }
            public static bool operator !=(CLocation a, CLocation b)
            {
                return a.Number != b.Number;
            }
            public override bool Equals(object obj)
            {
                return obj is CLocation location &&
                       Number == location.Number &&
                       _upper_number == location._upper_number &&
                       _level == location._level;
            }
            public void BeNew(TypeLevel level = TypeLevel.Box, CLocation upper_location = null)
            {
                // If has any illegal, set level to box and upper_number be zero
                if (SetLevel(level, true))
                    SetLevel(TypeLevel.Box);
                if (SetUpperNumber(upper_location))
                    SetUpperNumber(0);
                // distribute its number
                Core.Instance.rules.LocationRule._locations_number_distribute_reference++;
                _number = Core.Instance.rules.LocationRule._locations_number_distribute_reference;
                // refresh below numbers
                _below_numbers = new List<int>();
            }
            public void BeNew(int upper_one_number = 0, TypeLevel level = TypeLevel.Box)
            {
                // If has any illegal, set level to box and upper_number be zero
                if (SetLevel(level, true))
                    SetLevel(TypeLevel.Box);
                if (SetUpperNumber(upper_one_number, true))
                    SetUpperNumber(0);
                // distribute its number
                Core.Instance.rules.LocationRule._locations_number_distribute_reference++;
                _number = Core.Instance.rules.LocationRule._locations_number_distribute_reference;
                // refresh below numbers
                _below_numbers = new List<int>();
            }
            public bool SetLevel(TypeLevel level, bool check_is_legal = false)
            {
                // box must contains box
                if (!check_is_legal || level == TypeLevel.Box)
                {
                    _level = level;
                    return false;
                }
                else
                {
                    // find instance of this upper one, check it
                    if (Core.Instance.rules.LocationRule.locations.ContainsKey(UpperNumber))
                        if (Core.Instance.rules.LocationRule.locations[UpperNumber].Level > level)
                        {
                            _level = level;
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
                    && (upper_one.Level <= _level || Level == TypeLevel.Box))
                    return true;
                _upper_number = upper_one.Number;
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
                    _upper_number = upper_one_number;
                    return false;
                }
                else
                {
                    // find instance of the upper_number
                    // check upper's upper is this
                    // check upper's level is bigger than this, or this level is box
                    if (Core.Instance.rules.LocationRule.locations.ContainsKey(UpperNumber))
                    {
                        if (Core.Instance.rules.LocationRule.locations[UpperNumber].UpperNumber != Number
                             && (Core.Instance.rules.LocationRule.locations[UpperNumber].Level > Level
                                || Level == TypeLevel.Box)
                        )
                        {
                            _upper_number = upper_one_number;
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        private int _locations_number_distribute_reference = -1; // How many location number has been distributed. Used for distribute the location's numbers.
        public SortedList<int, CLocation> locations { get; } = new SortedList<int, CLocation>();
        public LocationRule(int locations_number_distribute_reference)
        {
            _locations_number_distribute_reference = locations_number_distribute_reference;
            CLocation root_location = Base.Component.GetSpawner<CLocation>().Spawn();
            root_location.BeNew(0);
            root_location.SetUpperNumber(0);
            root_location.SetLevel(CLocation.TypeLevel.Root);
            locations.Add(root_location.Number, root_location);
        }
    }
}
