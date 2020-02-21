using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using GameCore.Base;

namespace GameCore.Thing
{
    public class CraftRule
    {
        public class CFormula : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CFormula";
            public List<Component> Components = new List<Component>();
            public List<Result> Results = new List<Result>();
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CFormula c = json.ToObject<CFormula>();
                    return c;
                }catch(Exception e)
                {
                    return Core.State.WriteException<Concept>(e);
                }
            }
            public override JObject ToJsonObject()
            {
                try
                {
                    Card temp_c = this.Card;
                    Card = null;
                    JObject json = JObject.FromObject(this);
                    if(json["Card"] != null)
                        json.Remove("Card");
                    Card = temp_c;
                    return json;
                }catch(Exception e)
                {
                    return Core.State.WriteException<JObject>(e);
                }
            }
        }
        public class Result
        {
            public int IsMaterial;
            public int Number; 
            public int Amount;
            public List<int> States = new List<int>();
        }
        public class Component
        {
            public int Number; // 0 meas material
            public List<int> Sort = new List<int>();
            public int Amount;
            public List<int> States = new List<int>();
        }
        public class DemandSkill
        {
            // TODO:ds
        }
        private int _ctn_formula = -1;
        public CraftRule()
        {
            _ctn_formula = ConceptSpawner<CFormula>.GetSpawner().TypeNumber;
        }
    }
}