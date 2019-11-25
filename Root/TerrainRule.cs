using System.Collections.Generic;

namespace GameCore.Root
{
    public class TerrainRule : Base.Rule
    {
        public class CTerrainTemplate : Base.Component
        {
            private string _type_name = "CTerrainTemplate";
            public override string TypeName => _type_name;
        }
        public class CTerrain : Base.Component
        {
            private string _type_name = "CTerrain";
            public override string TypeName => _type_name; 
        }
        private int _c_location_type_number = -1;
        private int _c_landform_type_number = -1;
        private int _c_landform_template_number = -1;
        private int _c_terrain_type_number = -1;
        private int _c_terrain_template_number = -1;
        public List<int> TerrainTemplateCardList { get; private set; }= null; 
        public bool Init()
        {
            _c_location_type_number = Base.Component.GetSpawner<LocationRule.CLocation>().Type_Number;
            _c_landform_type_number = Base.Component.GetSpawner<LandformRule.CLandform>().Type_Number;
            _c_landform_template_number = Base.Component.GetSpawner<LandformRule.CLandformTemplate>().Type_Number;
            _c_terrain_type_number = Base.Component.GetSpawner<CTerrain>().Type_Number;
            _c_terrain_template_number = Base.Component.GetSpawner<CTerrainTemplate>().Type_Number;
            TerrainTemplateCardList = new List<int>(); 
            return false;
        }
        public bool AddCTerrainTemplate(Base.Card card)
        {
            if(AddComponent<CTerrainTemplate>(card))
                return true;
            if(TerrainTemplateCardList.Contains(card.Number))
                return true;
            TerrainTemplateCardList.Add(card.Number);
            return false; 
        }
    }
}