using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Thing
{
    public class FoodRule : Rule
    {
        public class CFood : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CFood";
            public int Nutrition = 0;
            public int Eatable = 0;
            public int Delicious = 0;
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CFood c = json.ToObject<CFood>();
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
            public override Concept Copy()
            {
                var c = Spawn<CFood>();
                if(c == null)
                    return null;
                c.Nutrition = Nutrition;
                c.Eatable = Eatable;
                c.Delicious = Delicious;
                return c;
            }
        }
        public enum Eatable
        {
            Veget, Meat, Mech, Mana, Undead
        }
        public Hook<int, object> HFoodDestroy = new Hook<int, object>();
        private int _ctn_food = -1;
        public FoodRule()
        {
            _ctn_food = Concept.Spawn<CFood>().TypeNumber;
        }
        public bool BeFood(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_food))
                return true;
            // add concepts
            var c = AddConcept<CFood>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyFood(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CFood>(_ctn_food);
            if (c == null)
            {
                card.Remove(_ctn_food);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HFoodDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_food);
            return false;
        }
        public bool SomeAction(Card card)
        {
            // check and get concepts
            // check condition
            // make hook input
            // do actions
            // make hook calling
            return false;
        }
    }
}
