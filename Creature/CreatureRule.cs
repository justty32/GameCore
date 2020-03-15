using GameCore.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Creature
{
    public class CreatureRule : Rule
    {
        public class CCreature : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CCreature";
            public Basix Basix = new Basix();
            public Attr Attr = new Attr();
            public Status Status = new Status();
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CCreature c = json.ToObject<CCreature>();
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
                var c = Spawn<CCreature>();
                if(c == null)
                    return null;
                c.Basix = Basix;
                c.Attr = Attr;
                c.Status = Status;
                return c;
            }
        }
        public struct Basix
        {
            public int STR;
            public int DEX;
            public int CON;
            public int PER;
            public int WIL;
            public int MAG;
        }
        public struct Attr
        {
            public int HPMax;
            public int MPMax;
            public int SPMax;
            public int Weight;
            public int Size;
            public int Vision;
            public int VisionNight;
            public int CarryMax;
            public int MoveSpeed;
        }
        public struct Status
        {
            public int HP;
            public int MP;
            public int SP;
            public int Health;
            public int Spirit;
            public int San;
            public int CarryWeight;
        }
        public Hook<int, object> HCreatureDestroy = new Hook<int, object>();
        private int _ctn_creaturee = -1;
        public CreatureRule()
        {
            _ctn_creaturee = Concept.Spawn<CCreature>().TypeNumber;
        }
        public bool BeCreature(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_creaturee))
                return true;
            // add concepts
            var c = AddConcept<CCreature>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyCreature(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CCreature>(_ctn_creaturee);
            if (c == null)
            {
                card.Remove(_ctn_creaturee);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HCreatureDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_creaturee);
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