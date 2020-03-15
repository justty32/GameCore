using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Creature
{
    public class HeroRule : Rule
    {
        public class CHero : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CHero";
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CHero c = json.ToObject<CHero>();
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
                var c = Spawn<CHero>();
                if(c == null)
                    return null;
                // do something
                return c;
            }
        }
        public Hook<int, object> HHeroDestroy = new Hook<int, object>();
        private int _ctn_hero = -1;
        public HeroRule()
        {
            _ctn_hero = Concept.Spawn<CHero>().TypeNumber;
        }
        public bool BeHero(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_hero))
                return true;
            // add concepts
            var c = AddConcept<CHero>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyHero(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CHero>(_ctn_hero);
            if (c == null)
            {
                card.Remove(_ctn_hero);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HHeroDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_hero);
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
