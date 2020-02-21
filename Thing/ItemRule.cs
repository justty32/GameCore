using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using GameCore.Base;
using GameCore.Root;

namespace GameCore.Thing
{
    public class ItemRule : Rule
    {
        public class CItem : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CItem";
            public int Value = 0; //價值
            public int Pretty = 0; //美觀
            public int Size = 0; //大小
            public int Weight = 0; //重量
            public float AgeingPerHour = 0; //老化速度
            public int Burn = 0;
            public Dictionary<int, int> Materials = new Dictionary<int, int>();
            public Dictionary<int, int> Components = new Dictionary<int, int>();
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CItem c = json.ToObject<CItem>();
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
        public Hook<int, object> HItemDestroy = new Hook<int, object>();
        private int _ctn_item = -1;
        public ItemRule()
        {
            _ctn_item = ConceptSpawner<CItem>.GetSpawner().TypeNumber;
        }
        public bool BeItem(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_item))
                return true;
            // add concepts
            var c = AddConcept<CItem>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyItem(Card card)
        {
            // check concepts
            if(!HasConcept(card, _ctn_item))
                return true;
            // get concept
            var c = card.Get<CItem>();
            if (c == null)
            {
                card.Remove(_ctn_item);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HItemDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_item);
            return false;
        }
    }
}