using System.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

// TODO:declare a hook: LandMoved(land, new_posX, new_posY), hook_name:"LandMoved"

namespace GameCore.Root
{
    public class WorldRule : Base.Rule
    {
        public class CWorld : Base.Component
        {
            private const string _type_name = "CWorld";
            public override string TypeName => _type_name;
            public int SizeX { get; set; } = -1;
            public int SizeY { get; set; } = -1;
            public int PostionZ { get; set; } = -1;
            public List<int> CLandList { get; private set; } // reference to CLocation's Number
            public int FillingTypeNumber { get; private set; } // things filling between lands
            public CWorld(){
                CLandList = new List<int>();
            }
            public bool Init(int sizeX, int sizeY)
            {
                SizeX = sizeX;
                SizeY = sizeY;
                return false;
            }
            public bool SetFillingTypeNumber(int filled_thing)
            {
                if(filled_thing < 0)
                    return true;
                FillingTypeNumber = filled_thing;
                return false;
            }
        }
        private int _c_location_type_number = -1; // prestore
        private int _c_world_type_number = -1; // prestore
        private int _c_land_type_number = -1; // prestore
        public int  CWorldTypeNumber{ get => _c_world_type_number ; }
        public bool Init()
        {
             // rule's initialize
            _c_world_type_number = Base.Component.GetSpawner<CWorld>().Type_Number;
            _c_land_type_number = Base.Component.GetSpawner<LandRule.CLand>().Type_Number;
            _c_location_type_number = Base.Component.GetSpawner<LocationRule.CLocation>().Type_Number;
            return false;
        }
        public bool AddCWorld(Base.Card card)
        {
            return AddComponent<CWorld>(card);
        }
        public bool BeNewCWorld(Base.Card card)
        {
            if(!HasComponent(card, _c_location_type_number, _c_world_type_number))
                return true;
            return false;
        }
        public bool AddLand(Base.Card world_card, int positionX, int positionY, Base.Card land_card)
        {
            // Not Check if the land overlap others
            // the card need both CLand and CLocation
            if(!HasComponent(land_card, _c_land_type_number, _c_location_type_number))
                return true;
            if(!HasComponent(world_card, _c_world_type_number, _c_location_type_number))
                return true;
            if(positionX < 0 || positionY < 0)
                return true;
            var c_location_land = land_card.GetComponent(_c_location_type_number) as LocationRule.CLocation;
            var c_location_world = world_card.GetComponent(_c_world_type_number) as LocationRule.CLocation;
            var c_land = land_card.GetComponent(_c_land_type_number) as LandRule.CLand;
            var c_world = world_card.GetComponent(_c_world_type_number) as CWorld;
            if(c_location_land == null || c_location_world == null
                || c_world == null || c_land == null)
                return true;
            if(positionX + c_land.SizeX > c_world.SizeX
                || positionY + c_land.SizeY > c_world.SizeY)
                return true;
            // adding
            c_land.PositionX = positionX;
            c_land.PositionY = positionY;
            c_world.CLandList.Add(c_location_land.Number);
            return c_location_land.SetUpperNumber(c_location_world);
        }
        public bool RemoveLand(Base.Card world_card, Base.Card land_card)
        {
            // the card need both CLand and CLocation
            if(!HasComponent(land_card, _c_land_type_number, _c_location_type_number))
                return true;
            if(!HasComponent(world_card, _c_world_type_number, _c_location_type_number))
                return true;
            var c_location_land = land_card.GetComponent(_c_location_type_number) as LocationRule.CLocation;
            var c_land = land_card.GetComponent(_c_land_type_number) as LandRule.CLand;
            var c_world = world_card.GetComponent(_c_world_type_number) as CWorld;
            if(c_location_land == null || c_world == null || c_land == null)
                return true;
            // remove
            c_land.PositionX = -1;
            c_land.PositionY = -1;
            c_world.CLandList.Remove(c_location_land.Number);
            return c_location_land.SetUpperNumber(0);
        }
        public bool MoveLandTo(Base.Card world_card, int dstX, int dstY, Base.Card land_card)
        {
            // Not Check if the land overlap others
            // the card need both CLand and CLocation
            if(!HasComponent(land_card, _c_land_type_number, _c_location_type_number))
                return true;
            if(!HasComponent(world_card, _c_world_type_number, _c_location_type_number))
                return true;
            var c_location_land = land_card.GetComponent(_c_location_type_number) as LocationRule.CLocation;
            var c_location_world = world_card.GetComponent(_c_world_type_number) as LocationRule.CLocation;
            var c_land = land_card.GetComponent(_c_land_type_number) as LandRule.CLand;
            var c_world = world_card.GetComponent(_c_world_type_number) as CWorld;
            if(c_location_land == null || c_location_world == null
                || c_world == null || c_land == null)
                return true;
            if(dstX + c_land.SizeX > c_world.SizeX
                || dstY + c_land.SizeY > c_world.SizeY
                || dstX + c_land.SizeX < 0
                || dstY + c_land.SizeY < 0)
                return true;
            if(!c_world.CLandList.Contains(c_location_land.Number))
                return true;
            // change postion
            c_land.PositionX = dstX;
            c_land.PositionY = dstY;
            return false;
        }
    }
}
