using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Root
{
    public class EntityRule : Rule
    {
        public class CEntity : Concept
        {
            public override string TypeName => _type_name;
            private static string _type_name = "CEntity";
            public int Location = 0;
            public int Influence = 0;
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CEntity c = json.ToObject<CEntity>();
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
                var c = Spawn<CEntity>();
                if (c == null)
                    return null;
                c.Location = Location;
                c.Influence = Influence;
                return c;
            }
        }
        public class HINLocationChange
        {
            public int Card = 0;
            public int Previous = 0;
            public int Next = 0;
        }
        public class HINInfluenceChange
        {
            public int Card = 0;
            public int Previous = 0;
            public int Next = 0;

        }
        public Hook<HINLocationChange, object> HLocationChange = new Hook<HINLocationChange, object>("HLocationChange");
        public Hook<HINInfluenceChange, object> HInfluenceChange = new Hook<HINInfluenceChange, object>("HInfluenceChange");
        public Hook<int, object> HEntityDestroy = new Hook<int, object>();
        private int _ctn_entity = -1;
        public EntityRule()
        {
            _ctn_entity = Concept.Spawn<CEntity>().TypeNumber;
        }
        public override bool Init()
        {
            var l = Core.Rules.LocationRule;
            if (l == null)
                return true;
            l.HLocationDestroy.Bind(HFLocationDestroy);
            return false;
        }
        public bool BeEntity(Card card, int location = 0)
        {
            // check condition
            if (location < 0)
                return true;
            // check has concepts
            if(!UnHasConcept(card, _ctn_entity))
                return true;
            // let the card could be save
            card.Definition = false;
            card.NeedSave = true;
            // add concepts
            var c = AddConcept<CEntity>(card);
            if(c == null)
                return true;
            // set attributes
            c.Location = location;
            // do other actions
            if (location > 0)
                if (Core.Rules.LocationRule.AddDowns(Core.Cards[location], card.Number))
                    return true;
            return false;
        }
        public bool DestroyEntity(Card card)
        {
            // check concepts
            if(!HasConcept(card, _ctn_entity))
                return true;
            // get concept
            var c = card.Get<CEntity>();
            if (c == null)
            {
                card.Remove(_ctn_entity);
                return true;
            }
            // let the card can't be save
            card.Definition = true;
            card.NeedSave = true;
            // make hook input
            // do actions
            // call hook
            HEntityDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_entity);
            return false;
        }
        public object HFLocationDestroy(int cdn)
        {
            if (cdn < 0)
                return null;
            var cl = GetConcept<LocationRule.CLocation>(ToCard(cdn));
            var ce = GetConcept<CEntity>(ToCard(cdn));
            if (cl == null || ce == null)
                return null;
            for(int i = 0; i < cl.Downs.Count; i++)
                Core.Rules.EntityRule.SetLocation(ToCard(i), ce.Location);
            return null;
        }
        public bool SetLocation(Card card, int location = 0)
        {
            // check condition
            if (location < 0)
                return true;
            // check has concepts
            // get concept
            var c = card.Get<CEntity>(_ctn_entity);
            if (c == null)
                return false;
            // make hook input
            var h_in = new HINLocationChange();
            h_in.Card = card.Number;
            h_in.Next = location;
            h_in.Previous = c.Location;
            // do actions
            c.Location = location;
            // make hook calling
            HLocationChange.CallAll(h_in);
            return false;
        }
        public bool SetInfluence(Card card, int influence = 0)
        {
            // check condition
            if (influence < 0)
                return true;
            // get concept
            var c = card.Get<CEntity>(_ctn_entity);
            if (c == null)
                return false;
            // make hook input
            var h_in = new HINInfluenceChange();
            h_in.Card = card.Number;
            h_in.Next = influence;
            h_in.Previous = c.Influence;
            // do actions
            c.Influence = influence;
            // make hook calling
            HInfluenceChange.CallAll(h_in);
            return false;
        }
    }
}
