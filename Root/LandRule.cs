using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Root
{
    public class LandRule : Base.Rule
    {
        public class CLand : Base.Component
        {

            private const string _type_name = "CLand";
            public override string TypeName => _type_name;
            public List<List<int>> CTileNumberList { get; private set; } //stored CTile component's numbers
            public bool Init(int sizeX, int sizeY)
            {
                // not BeNew(), just allocate the memory of array
                CTileNumberList = new List<List<int>>(sizeX);
                if (CTileNumberList == null)
                    return true;
                for (int i = 0; i < CTileNumberList.Count; i++)
                {
                    CTileNumberList[i] = new List<int>(sizeY);
                    if (CTileNumberList[i] == null)
                        return true;
                    for (int j = 0; j < CTileNumberList[i].Count; j++)
                        CTileNumberList[i].Add(-1);
                }
                return false;
            }
        }
        private int _c_location_type_number = -1; // prestore
        private int _c_tile_type_number = -1; // prestore
        private int _c_land_type_number = -1; // prestore
        public int CLandTypeNumber { get => _c_location_type_number; }
        public bool Init()
        {
            // rule's initialize
            _c_location_type_number = Base.Component.GetSpawner<CLand>().Type_Number;
            _c_tile_type_number = Base.Component.GetSpawner<TileRule.CTile>().Type_Number;
            _c_land_type_number = Base.Component.GetSpawner<LocationRule.CLocation>().Type_Number;
            return false;
        }
        public bool AddCLand(Base.Card card)
        {
            return AddComponent<CLand>(card);
        }
        public bool BeNewCLand(Base.Card land_card, int sizeX, int sizeY)
        {
            // sizeXY must bigger than 0
            if (land_card == null || sizeX < 1 || sizeY < 1)
                return true;
            // be new land
            if (!land_card.HasComponent(_c_land_type_number))
                return true;
            var c_land = land_card.GetComponent(_c_land_type_number) as CLand;
            if (c_land == null)
                return true;
            return c_land.Init(sizeX, sizeY);
        }
        public bool SetTile(Base.Card land_card, int positionX, int positionY, Base.Card tile_card)
        {
            // land card need component CLand and CLocation
            // tile card need component CTile and CLocation
            // Not check if the CLocation is legally, only check if has these.
            if (land_card == null || tile_card == null || positionX < 0 || positionY < 0)
                return true;
            if ((!land_card.HasComponent<CLand>()) && (!land_card.HasComponent<LocationRule.CLocation>()))
                return true;
            if ((!tile_card.HasComponent<TileRule.CTile>()) && (!tile_card.HasComponent<LocationRule.CLocation>()))
                return true;
            // set land's tile_list[x][y] to tile's CLocation number
            var c_land = land_card.GetComponent(_c_land_type_number) as CLand;
            var c_tile = tile_card.GetComponent(_c_tile_type_number) as TileRule.CTile;
            var c_location_land = land_card.GetComponent(_c_location_type_number) as LocationRule.CLocation;
            var c_location_tile = tile_card.GetComponent(_c_location_type_number) as LocationRule.CLocation;
            if (c_land == null || c_tile == null || c_location_land == null || c_location_tile == null)
                return true;
            c_land.CTileNumberList[positionX][positionY] = c_location_tile.Number;
            c_tile.PositionX = positionX;
            c_tile.PositionY = positionY;
            // set tile's CLocation's UpperNumber
            return c_location_tile.SetUpperNumber(c_location_land.Number);
        }
    }
}
