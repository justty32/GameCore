using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Thing
{
    public class ArmorRule : Rule
    {
        public class CArmor : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CArmor";
            public int ProtSharp = 0;
            public int ProtBlung = 0;
            public int ProtHeat = 0;
            public int Burden = 0;
            public int ComfortHot = 0;
            public int ComfortCold = 0;
            public int DurableJustify = 0;
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CArmor c = json.ToObject<CArmor>();
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
                var c = Spawn<CArmor>();
                if (c == null)
                    return null;
                c.ProtSharp = ProtSharp;
                c.ProtBlung = ProtBlung;
                c.ProtHeat = ProtHeat;
                c.Burden = Burden;
                c.ComfortHot = ComfortHot;
                c.ComfortCold = ComfortCold;
                c.DurableJustify = DurableJustify;
                return c;
            }
        }
        public Hook<int, object> HArmorDestroy = new Hook<int, object>();
        private int _ctn_armor = -1;
        public ArmorRule()
        {
            _ctn_armor = Concept.Spawn<CArmor>().TypeNumber;
        }
        public bool BeArmor(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_armor))
                return true;
            // add concepts
            var c = AddConcept<CArmor>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyArmor(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CArmor>(_ctn_armor);
            if (c == null)
            {
                card.Remove(_ctn_armor);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HArmorDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_armor);
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
