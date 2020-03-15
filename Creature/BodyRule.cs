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
            public Dictionary<int, int> DefaultConstruct = new Dictionary<int, int>();
            public List<Component> OtherPart = new List<Component>();
            public Dictionary<int, int> DefaultParts = new Dictionary<int, int>();
            public CreatureRule.Basix Basix = new CreatureRule.Basix();
            public CreatureRule.Attr Attr = new CreatureRule.Attr();
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
                c.DefaultConstruct = new Dictionary<int, int>(DefaultConstruct);
                c.OtherPart = new List<Component>(OtherPart);
                c.DefaultParts = new Dictionary<int, int>(DefaultParts);
                c.Basix = Basix;
                c.Attr = Attr;
                return c;
            }
        }
        public struct Component
        {
            public int Number;
            public int PartType;
        }
        public enum Part
        {
            Other, Head, Neck, Trunk, Arm, Hand, Finger, Leg, Foot,
            Tail, Wing, Fin, Antenna
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
