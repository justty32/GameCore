using System;
using System.Collections.Generic;
namespace GameCore.Root
{
    public class LandformRule : Base.Rule
    {
        public class CLandformTemplate : Base.Component
        {
            private string _type_name = "CLandformTemplate";
            public override string TypeName => _type_name;
            public bool Init()
            {
                if(Card == null)
                    return true;
                if(Card.Number < 0)
                    return true;
                var card_list = Core.Instance.Rules.LandformRule.LandformTemplateCardList;
                for(int i = 0; i < card_list.Count; i++)
                    if(card_list[i] == Card.Number)
                        return true;
                card_list.Add(Card.Number);
                return false;
            }
        }
        public class CLandform : Base.Component
        {
            private string _type_name = "CLandform";
            public override string TypeName => _type_name;
        }
        private int _c_location_type_number = -1;
        private int _c_landform_type_number = -1;
        private int _c_terrain_type_number = -1;
        private int _landform_template_number_distribute_reference = -1;
        public List<int> LandformTemplateCardList = null; 
        public bool Init(int landform_template_number_distribute_reference)
        {
            _c_location_type_number = Base.Component.GetSpawner<LocationRule.CLocation>().Type_Number;
            _c_landform_type_number = Base.Component.GetSpawner<CLandform>().Type_Number;
            _c_terrain_type_number = Base.Component.GetSpawner<TerrainRule.CTerrain>().Type_Number;
            _landform_template_number_distribute_reference = landform_template_number_distribute_reference;
            LandformTemplateCardList = new List<int>(); 
            return false;
        }
        public bool AddCLandformTemplate(Base.Card card)
        {
            if(AddComponent<CLandformTemplate>(card))
                return true;
            var c_landform_template = card.GetComponent<CLandformTemplate>() as CLandformTemplate;
            if(c_landform_template == null)
                return true;
            return c_landform_template.Init();
        }
    }
}