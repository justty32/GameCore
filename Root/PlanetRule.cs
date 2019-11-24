using System;
using System.Collections.Generic;
using System.Text;

// TODO:declare a hook: WorldMoved(land, new_posZ), hook_name:"WorldMoved"

namespace GameCore.Root
{
    public class PlanetRule : Base.Rule
    {
        class CPlanet : Base.Component
        {
            private const string _type_name = "CPlanet";
            public override string TypeName => _type_name;
            public SortedList<int, int> CWorldList { get; private set; } // index is positionX, reference to CLocation's Number
            public CPlanet(){
                CWorldList = new SortedList<int, int>();
            }
        }
        private int _c_location_type_number = -1; // prestore
        private int _c_world_type_number = -1; // prestore
        private int _c_planet_type_number = -1; // prestore
        public int  CPlanetTypeNumber{ get => _c_planet_type_number ; }
        public bool Init()
        {
             // rule's initialize
            _c_world_type_number = Base.Component.GetSpawner<WorldRule.CWorld>().Type_Number;
            _c_planet_type_number = Base.Component.GetSpawner<CPlanet>().Type_Number;
            _c_location_type_number = Base.Component.GetSpawner<LocationRule.CLocation>().Type_Number;
            return false;
        }
        public bool AddCPlanet(Base.Card card)
        {
            return AddComponent<CPlanet>(card);
        }
        public bool BeNewCWorld(Base.Card card)
        {
            if(!HasComponent(card, _c_location_type_number, _c_planet_type_number))
                return true;
            return false;
        }
        public bool AddWorld(Base.Card planet_card, int positionZ, Base.Card world_card)
        {
            // the card need both CLand and CLocation
            if(!HasComponent(planet_card, _c_planet_type_number, _c_location_type_number))
                return true;
            if(!HasComponent(world_card, _c_world_type_number, _c_location_type_number))
                return true;
            var c_location_planet = planet_card.GetComponent(_c_location_type_number) as LocationRule.CLocation;
            var c_location_world = world_card.GetComponent(_c_world_type_number) as LocationRule.CLocation;
            var c_planet = planet_card.GetComponent(_c_planet_type_number) as CPlanet;
            var c_world = world_card.GetComponent(_c_world_type_number) as WorldRule.CWorld;
            if(c_location_planet == null || c_location_world == null
                || c_world == null || c_planet == null)
                return true;
            if(c_planet.CWorldList.ContainsKey(positionZ))
                return true;
            // adding
            c_world.PostionZ = positionZ;
            c_planet.CWorldList.Add(positionZ, c_location_world.Number);
            return c_location_world.SetUpperNumber(c_location_planet);
        }
        public bool RemoveWorld(Base.Card planet_card, Base.Card world_card)
        {
            // the card need both CLand and CLocation
            if(!HasComponent(planet_card, _c_planet_type_number, _c_location_type_number))
                return true;
            if(!HasComponent(world_card, _c_world_type_number, _c_location_type_number))
                return true;
            var c_location_planet = planet_card.GetComponent(_c_location_type_number) as LocationRule.CLocation;
            var c_location_world = world_card.GetComponent(_c_world_type_number) as LocationRule.CLocation;
            var c_planet = planet_card.GetComponent(_c_planet_type_number) as CPlanet;
            var c_world = world_card.GetComponent(_c_world_type_number) as WorldRule.CWorld;
            if(c_location_planet == null || c_location_world == null
                || c_world == null || c_planet == null)
                return true;
            // remove
            c_planet.CWorldList.Remove(c_world.PostionZ);
            c_world.PostionZ = -1;
            return c_location_world.SetUpperNumber(0);
        }
        public bool MoveLandTo(Base.Card planet_card, int dstZ, Base.Card world_card)
        {
            // the card need both CLand and CLocation
            if(!HasComponent(planet_card, _c_planet_type_number, _c_location_type_number))
                return true;
            if(!HasComponent(world_card, _c_world_type_number, _c_location_type_number))
                return true;
            var c_location_planet = planet_card.GetComponent(_c_location_type_number) as LocationRule.CLocation;
            var c_location_world = world_card.GetComponent(_c_world_type_number) as LocationRule.CLocation;
            var c_planet = planet_card.GetComponent(_c_planet_type_number) as CPlanet;
            var c_world = world_card.GetComponent(_c_world_type_number) as WorldRule.CWorld;
            if(c_location_planet == null || c_location_world == null
                || c_world == null || c_planet == null)
                return true;
            if(c_planet.CWorldList.ContainsKey(dstZ))
                return true;
            // adding
            c_planet.CWorldList.Remove(c_world.PostionZ);
            c_world.PostionZ = dstZ;
            c_planet.CWorldList.Add(dstZ, c_location_world.Number);
            return false;
        }
    }
}