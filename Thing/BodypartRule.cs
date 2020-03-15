using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Thing
{
    public class BodypartRule : Rule
    {
        public class CBodypart : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CBodypart";
            // TODO
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CBodypart c = json.ToObject<CBodypart>();
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
                var c = Spawn<CBodypart>();
                if(c == null)
                    return null;
                // do something
                return c;
            }
        }
        public Hook<int, object> HBodypartDestroy = new Hook<int, object>();
        private int _ctn_bodypart = -1;
        public BodypartRule()
        {
            _ctn_bodypart = Concept.Spawn<CBodypart>().TypeNumber;
        }
        public bool BeBodypart(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_bodypart))
                return true;
            // add concepts
            var c = AddConcept<CBodypart>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyBodypart(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CBodypart>(_ctn_bodypart);
            if (c == null)
            {
                card.Remove(_ctn_bodypart);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HBodypartDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_bodypart);
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
