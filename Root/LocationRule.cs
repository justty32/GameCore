using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Root
{
    class LocationRule
    {
        public class CLocation : Base.Component
        {
            // Some Card defined as an entity, need CLocation to represent its location.
            // This is Not Position, don't have coordinates.
            //
            // BeNew(), set it a new location, with distributing a new Number.
            // After BeNew(), this will be an another location, previous one will be loss.
            //
            // While SetBelong(), the belong's belong can't be this,
            // and belong's level can't <= to this.
            //
            // GameCore.Core TODO: CLocation.RootLocation use Component.AutoSetTypeNumber()
            public enum TypeLevel
            {
                Box, Room, Building, Village, City, Tile, Region, Land, Planet, World, Root
            }
            private new const string type_name = "CLocation";
            public override string TypeName => type_name;
            private int number_distribute_reference = 1;
            public int Number { get; set; } = -1;
            public static CLocation RootLocation { get; } = new CLocation(0); // Number = 0
            private CLocation _belong = RootLocation;
            private TypeLevel _level = TypeLevel.Box;
            private CLocation(int this_is_only_for_RootLocation)
            {
                Number = 0;
                _level = TypeLevel.Root;
            }
            public static bool operator ==(CLocation a, CLocation b)
            {
                return a.Number == b.Number;
            }
            public static bool operator !=(CLocation a, CLocation b)
            {
                return a.Number != b.Number;
            }
            public void BeNew(TypeLevel level = TypeLevel.Box, CLocation belong_location_number = null)
            {
                // If belong_location is null, set to RootLocation
                SetLevel(level);
                if (SetBelong(belong_location_number))
                    SetBelong(RootLocation);
                if (number_distribute_reference == int.MaxValue)
                    number_distribute_reference = int.MinValue;
                if (number_distribute_reference == 0)
                    number_distribute_reference = 1000;
                Number = number_distribute_reference;
                number_distribute_reference++;
            }
            public TypeLevel Level { get => _level;}
            public bool SetLevel(TypeLevel level)
            {
                // level can't smaller than, or equal to belong's.
                if (level >= _belong.Level)
                    return true;
                _level = level;
                return false;
            }
            public CLocation Belong { get => _belong; }
            public bool SetBelong(CLocation this_belong_to_who)
            {
                // target's belong can't be this
                // target's level can't smaller than, or equal to this. (level <= this)
                // If that, do nothing, return true.
                if (this_belong_to_who.Belong == this 
                    || this_belong_to_who.Level <= _level)
                    return true;
                _belong = this_belong_to_who;
                return false;
            }
            public override bool Equals(object obj)
            {
                return obj is CLocation location &&
                       TypeName == location.TypeName &&
                       number_distribute_reference == location.number_distribute_reference &&
                       Number == location.Number &&
                       EqualityComparer<CLocation>.Default.Equals(_belong, location._belong) &&
                       _level == location._level &&
                       Level == location.Level &&
                       EqualityComparer<CLocation>.Default.Equals(Belong, location.Belong);
            }
            public override int GetHashCode()
            {
                return HashCode.Combine(TypeName, number_distribute_reference, Number, _belong, _level, Level, Belong);
            }
        }

    }
}
