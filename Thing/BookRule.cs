using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Thing
{
    public class BookRule : Rule
    {
        public class CBook : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CBook";
            public GText Text = new GText();
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CBook c = json.ToObject<CBook>();
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
                var c = Spawn<CBook>();
                if (c == null)
                    return null;
                c.Text.CopyFrom(Text);
                return c;
            }
        }
        public Hook<int, object> HBookDestroy = new Hook<int, object>();
        private int _ctn_book = -1;
        public BookRule()
        {
            _ctn_book = Concept.Spawn<CBook>().TypeNumber;
        }
        public bool BeBook(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_book))
                return true;
            // add concepts
            var c = AddConcept<CBook>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyBook(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CBook>(_ctn_book);
            if (c == null)
            {
                card.Remove(_ctn_book);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HBookDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_book);
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
