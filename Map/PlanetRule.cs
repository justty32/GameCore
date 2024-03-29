using GameCore.Base;
using System.Collections.Generic;

namespace GameCore.Map
{
    public class PlanetRule : Base.Rule
    {
        private class CPlanet : Base.Concept
        {
            private const string _type_name = "CPlanet";
            public override string TypeName => _type_name;
            public Dictionary<int, int> CWorldList { get; private set; } // index is positionX, reference to CLocation's Number

            public bool Init()
            {
                CWorldList = new Dictionary<int, int>();
                return false;
            }
        }

        private int _c_location_type_number = -1; // prestore
        private int _c_world_type_number = -1; // prestore
        private int _c_planet_type_number = -1; // prestore
        public int CPlanetTypeNumber { get => _c_planet_type_number; }

        public override bool Init()
        {
            // rule's initialize
            _c_world_type_number = ConceptSpawner<WorldRule.CWorld>.GetSpawner().TypeNumber;
            _c_planet_type_number = ConceptSpawner<CPlanet>.GetSpawner().TypeNumber;
            _c_location_type_number = ConceptSpawner<Root.LocationRule.CLocation>.GetSpawner().TypeNumber;
            return false;
        }

        public bool AddCPlanet(Base.Card card)
        {
            if (AddConcept<CPlanet>(card) == null)
                return true;
            var c_planet = card.Get<CPlanet>();
            if (c_planet == null)
                return true;
            return c_planet.Init();
        }

        public bool AddWorld(Base.Card planet_card, int positionZ, Base.Card world_card)
        {
            // if at dstZ already has a world, do nothing, return true
            // the card need both CLand and CLocation
            if (!HasConcept(planet_card, _c_planet_type_number, _c_location_type_number))
                return true;
            if (!HasConcept(world_card, _c_world_type_number, _c_location_type_number))
                return true;
            var c_location_world = world_card.Get(_c_world_type_number) as Root.LocationRule.CLocation;
            var c_planet = planet_card.Get(_c_planet_type_number) as CPlanet;
            var c_world = world_card.Get(_c_world_type_number) as WorldRule.CWorld;
            if (c_location_world == null || c_world == null || c_planet == null)
                return true;
            if (c_planet.CWorldList.ContainsKey(positionZ))
                return true;
            // adding
            c_world.PostionZ = positionZ;
            c_planet.CWorldList.Add(positionZ, world_card.Number);
            c_location_world.SetUpperCard(world_card.Number);
            return false;
        }

        public bool RemoveWorld(Base.Card planet_card, Base.Card world_card)
        {
            // the card need both CLand and CLocation
            if (!HasConcept(planet_card, _c_planet_type_number, _c_location_type_number))
                return true;
            if (!HasConcept(world_card, _c_world_type_number, _c_location_type_number))
                return true;
            var c_location_world = world_card.Get(_c_world_type_number) as Root.LocationRule.CLocation;
            var c_planet = planet_card.Get(_c_planet_type_number) as CPlanet;
            var c_world = world_card.Get(_c_world_type_number) as WorldRule.CWorld;
            if (c_location_world == null || c_world == null || c_planet == null)
                return true;
            // remove
            c_planet.CWorldList.Remove(c_world.PostionZ);
            c_world.PostionZ = -1;
            c_location_world.SetUpperCard();
            return false;
        }

        public bool MoveLandTo(Base.Card planet_card, int dstZ, Base.Card world_card)
        {
            // if at dstZ already has a world, do nothing, return true
            // the card need both CLand and CLocation
            if (!HasConcept(planet_card, _c_planet_type_number, _c_location_type_number))
                return true;
            if (!HasConcept(world_card, _c_world_type_number, _c_location_type_number))
                return true;
            var c_location_world = world_card.Get(_c_world_type_number) as Root.LocationRule.CLocation;
            var c_planet = planet_card.Get(_c_planet_type_number) as CPlanet;
            var c_world = world_card.Get(_c_world_type_number) as WorldRule.CWorld;
            if (c_location_world == null || c_world == null || c_planet == null)
                return true;
            if (c_planet.CWorldList.ContainsKey(dstZ))
                return true;
            // move
            c_planet.CWorldList.Remove(c_world.PostionZ);
            c_world.PostionZ = dstZ;
            c_planet.CWorldList.Add(dstZ, world_card.Number);
            return false;
        }
    }
}