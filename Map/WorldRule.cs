using System.Drawing;
using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;

// TODO:declare a hook: LandMoved(land, new_posX, new_posY), hook_name:"LandMoved"

namespace GameCore.Map
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
            public List<int> LandCards { get; private set; } // reference to CLocation's Number
            public int FillingTypeNumber { get; private set; } // things filling between lands
            public CWorld(){
                LandCards = new List<int>();
            }
            public bool Init(int sizeX, int sizeY)
            {
                if(sizeX < 1 || sizeY < 1)
                    return true;
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
            _c_world_type_number = ComponentManager.GetSpawner<CWorld>().Type_Number;
            _c_land_type_number = ComponentManager.GetSpawner<LandRule.CLand>().Type_Number;
            _c_location_type_number = ComponentManager.GetSpawner<Root.LocationRule.CLocation>().Type_Number;
            return false;
        }
        public bool AddCWorld(Base.Card card, int sizeX, int sizeY)
        {
            if(AddComponent<CWorld>(card) == null)
                return true;
            var c_world = card.Get<CWorld>() as CWorld;
            if(c_world == null)
                return true;
            return c_world.Init(sizeX, sizeY);
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
            var c_location_land = land_card.Get(_c_location_type_number) as Root.LocationRule.CLocation;
            var c_land = land_card.Get(_c_land_type_number) as LandRule.CLand;
            var c_world = world_card.Get(_c_world_type_number) as CWorld;
            if(c_location_land == null || c_world == null || c_land == null)
                return true;
            if(positionX + c_land.SizeX > c_world.SizeX
                || positionY + c_land.SizeY > c_world.SizeY)
                return true;
            // adding
            c_land.PositionX = positionX;
            c_land.PositionY = positionY;
            c_world.LandCards.Add(land_card.Number);
            c_location_land.SetUpperCard(world_card.Number);
            return false;
        }
        public bool RemoveLand(Base.Card world_card, Base.Card land_card)
        {
            // the card need both CLand and CLocation
            if(!HasComponent(land_card, _c_land_type_number, _c_location_type_number))
                return true;
            if(!HasComponent(world_card, _c_world_type_number, _c_location_type_number))
                return true;
            var c_location_land = land_card.Get(_c_location_type_number) as Root.LocationRule.CLocation;
            var c_land = land_card.Get(_c_land_type_number) as LandRule.CLand;
            var c_world = world_card.Get(_c_world_type_number) as CWorld;
            if(c_location_land == null || c_world == null || c_land == null)
                return true;
            // remove
            c_land.PositionX = -1;
            c_land.PositionY = -1;
            c_world.LandCards.Remove(land_card.Number);
            c_location_land.SetUpperCard();
            return false;
        }
        public bool MoveLandTo(Base.Card world_card, int dstX, int dstY, Base.Card land_card)
        {
            // Not Check if the land overlap others
            // the card need both CLand and CLocation
            if(!HasComponent(land_card, _c_land_type_number, _c_location_type_number))
                return true;
            if(!HasComponent(world_card, _c_world_type_number, _c_location_type_number))
                return true;
            var c_land = land_card.Get(_c_land_type_number) as LandRule.CLand;
            var c_world = world_card.Get(_c_world_type_number) as CWorld;
            if(c_world == null || c_land == null)
                return true;
            if(dstX + c_land.SizeX > c_world.SizeX
                || dstY + c_land.SizeY > c_world.SizeY
                || dstX + c_land.SizeX < 0
                || dstY + c_land.SizeY < 0)
                return true;
            // change postion
            c_land.PositionX = dstX;
            c_land.PositionY = dstY;
            return false;
        }
    }
}
