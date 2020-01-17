using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Map
{
    public class LandRule : Base.Rule
    {
        public class CLand : Base.Concept
        {
            private const string _type_name = "CLand";
            public override string TypeName => _type_name;
            public List<List<int>> TileCards { get; private set; } 
            public int SizeX { get; private set; } = -1;
            public int SizeY { get; private set; } = -1;
            public int PositionX { get; internal set; } = -1;
            public int PositionY { get; internal set; } = -1;
            public bool Init(int sizeX, int sizeY)
            {
                // not BeNew(), just allocate the memory of array
                TileCards = new List<List<int>>(sizeX);
                if (TileCards == null || sizeX < 1 || sizeY < 1)
                    return true;
                for (int i = 0; i < TileCards.Count; i++)
                {
                    TileCards[i] = new List<int>(sizeY);
                    if (TileCards[i] == null)
                        return true;
                    for (int j = 0; j < TileCards[i].Count; j++)
                        TileCards[i].Add(-1);
                }
                SizeX = sizeX;
                SizeY = sizeY;
                return false;
            }
        }
        private int _c_location_type_number = -1; // prestore
        private int _c_tile_type_number = -1; // prestore
        private int _c_land_type_number = -1; // prestore
        public int CLandTypeNumber { get => _c_location_type_number; }
        public override bool Init()
        {
            // rule's initialize
            _c_land_type_number = Base.ConceptManager.GetSpawner<CLand>().Type_Number;
            _c_tile_type_number = Base.ConceptManager.GetSpawner<TileRule.CTile>().Type_Number;
            _c_location_type_number = Base.ConceptManager.GetSpawner<Root.LocationRule.CLocation>().Type_Number;
            return false;
        }
        public bool AddCLand(Base.Card card, int sizeX, int sizeY)
        {
            if(AddConcept<CLand>(card) == null)
                return true;
            var c_land = card.Get<CLand>() as CLand;
            if(c_land == null)
                return true;
            return c_land.Init(sizeX, sizeY);
        }
        public bool SetTile(Base.Card land_card, int positionX, int positionY, Base.Card tile_card)
        {
            // land card need both concept CLand and CLocation
            // tile card need both  concept CTile and CLocation
            if(!HasConcept(land_card, _c_land_type_number, _c_location_type_number))
                return true;
            if(!HasConcept(tile_card, _c_tile_type_number, _c_location_type_number))
                return true;
            // set land's tile_list[x][y] to tile's CLocation number
            var c_land = land_card.Get(_c_land_type_number) as CLand;
            var c_tile = tile_card.Get(_c_tile_type_number) as TileRule.CTile;
            var c_location_tile = tile_card.Get(_c_location_type_number) as Root.LocationRule.CLocation;
            if (c_land == null || c_tile == null || c_location_tile == null)
                return true;
            if (positionX < 0 || positionY < 0)
                return true;
            c_land.TileCards[positionX][positionY] = tile_card.Number;
            c_tile.PositionX = positionX;
            c_tile.PositionY = positionY;
            // set tile's CLocation's UpperNumber
            c_location_tile.SetUpperCard(land_card.Number);
            return false;
        }
    }
}
