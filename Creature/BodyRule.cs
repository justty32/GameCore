using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Creature
{
    public class BodyRule : Rule
    {
        public class CBody : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CBody";
            public Dictionary<int, Component> Components = new Dictionary<int, Component>();
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CBody c = json.ToObject<CBody>();
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
                var c = Spawn<CBody>();
                if(c == null)
                    return null;
                // do something
                return c;
            }
        }
        public struct Component
        {
            public int Name;
            public int Number; // if number is 0, go material
            public Thing.BodypartRule.Sort Type;
            public int MaterialNumber;
            public int MaterialAmount;
        }
        public Hook<int, object> HBodyDestroy = new Hook<int, object>();
        private int _ctn_body = -1;
        public BodyRule()
        {
            _ctn_body = Concept.Spawn<CBody>().TypeNumber;
        }
        public bool BeBody(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_body))
                return true;
            // add concepts
            var c = AddConcept<CBody>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyBody(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CBody>(_ctn_body);
            if (c == null)
            {
                card.Remove(_ctn_body);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HBodyDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_body);
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
