using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Creature
{
    public class FaceRule : Rule
    {
        public class CFace : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CFace";
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CFace c = json.ToObject<CFace>();
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
                var c = Spawn<CFace>();
                if(c == null)
                    return null;
                // do something
                return c;
            }
        }
        public Hook<int, object> HFaceDestroy = new Hook<int, object>();
        private int _ctn_face = -1;
        public FaceRule()
        {
            _ctn_face = Concept.Spawn<CFace>().TypeNumber;
        }
        public bool BeFace(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_face))
                return true;
            // add concepts
            var c = AddConcept<CFace>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyFace(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CFace>(_ctn_face);
            if (c == null)
            {
                card.Remove(_ctn_face);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HFaceDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_face);
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
