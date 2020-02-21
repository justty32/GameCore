using GameCore.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace GameCore.Root
{
    public class LocationRule : Rule
    {
        public class CLocation : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CLocation";
            public List<int> Downs = new List<int>();
            public bool SetUpperCard(int card = 0)
            {
                return Core.Rules.LocationRule.SetUp(Rule.ToCard(card), card);
            }
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CLocation c = json.ToObject<CLocation>();
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
        }
        public class HINEntityLeaveLocation
        {
            public int Location = 0;
            public int Entity = 0;
        }
        public class HINEntityEnterLocation
        {
            public int Location = 0;
            public int Entity = 0;
        }
        public Hook<HINEntityLeaveLocation, object> HEntityLeaveLocation = new Hook<HINEntityLeaveLocation, object>("HEntityLeaveLocation");
        public Hook<HINEntityEnterLocation, object> HEntityEnterLocation = new Hook<HINEntityEnterLocation, object>("HEntityEnterLocation");
        public Hook<int, object> HLocationDestroy = new Hook<int, object>();
        private int _ctn_location = -1;
        private int _ctn_entity = -1;
        public LocationRule()
        {
            _ctn_location = Concept.Spawn<CLocation>().TypeNumber;
            _ctn_entity = Concept.Spawn<EntityRule.CEntity>().TypeNumber;
        }
        public override bool Init()
        {
            var en = Core.Rules.EntityRule;
            if (en == null)
                return true;
            en.HEntityDestroy.Bind(HFEntityDestroy);
            en.HLocationChange.Bind(HFLocationChange);
            return false;
        }
        public bool BeLocation(Card card, int up = 0, params int[] downs)
        {
            if (!UnHasConcept(card, _ctn_location))
                return true;
            Core.Rules.EntityRule.BeEntity(card, up);
            var cl = AddConcept<CLocation>(card);
            if(cl == null)
                return true;
            if (downs != null)
            {
                cl.Downs = new List<int>(downs.Length);
                for (int i = 0; i < downs.Length; i++)
                    if (!cl.Downs.Contains(downs[i]))
                        cl.Downs.Add(downs[i]);
            }
            return false;
        }
        public bool DestroyLocation(Card card)
        {
            // check concepts
            if(!HasConcept(card, _ctn_location))
                return true;
            // get concept
            var c = card.Get<CLocation>();
            if (c == null)
            {
                card.Remove(_ctn_location);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HLocationDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_location);
            return false;
        }
        public object HFEntityDestroy(int cdn)
        {
            if (cdn <= 0)
                return null;
            var c_entity = GetConcept<EntityRule.CEntity>(ToCard(cdn));
            if (c_entity == null)
                return null;
            int up = c_entity.Location;
            DestroyLocation(ToCard(up));
            return null;
        }
        public object HFLocationChange(EntityRule.HINLocationChange hin)
        {
            if (hin == null)
                return null;
            if (hin.Card <= 0)
                return null;
            RemoveDowns(ToCard(hin.Previous), hin.Card);
            AddDowns(ToCard(hin.Next), hin.Card);
            return null;
        }
        public IList<int> GetDowns(Card card)
        {
            if (!HasConcept(card, _ctn_location))
                return null;
            return card.Get<CLocation>().Downs;
        }
        public bool AddDowns(Card card, params int[] downs)
        {
            if (downs == null)
                return true;
            if (!HasConcept(card, _ctn_location))
                return true;
            var dls = card.Get<CLocation>().Downs;
            if (dls == null)
                dls = new List<int>(downs.Length);
            for (int i = 0; i < downs.Length; i++)
                if (downs[i] > 0 && !dls.Contains(downs[i]))
                {
                    dls.Add(downs[i]);
                    HINEntityEnterLocation h_in = new HINEntityEnterLocation();
                    h_in.Location = card.Number;
                    h_in.Entity = downs[i];
                    HEntityEnterLocation.CallAll(h_in);
                }
            return false;
        }
        public bool RemoveDowns(Card card, params int[] downs)
        {
            if (!HasConcept(card, _ctn_location))
                return true;
            if (downs == null)
                return true;
            var dls = card.Get<CLocation>().Downs;
            if (dls == null)
                dls = new List<int>();
            for (int i = 0; i < downs.Length; i++)
                if (dls.Contains(downs[i]))
                {
                    dls.Remove(downs[i]);
                    HINEntityLeaveLocation h_in = new HINEntityLeaveLocation();
                    h_in.Location = card.Number;
                    h_in.Entity = downs[i];
                    HEntityLeaveLocation.CallAll(h_in);
                }
            return false;
        }
        public int GetUp(Card card)
        {
            if (!HasConcept(card, _ctn_entity))
                return -1;
            return card.Get<EntityRule.CEntity>().Location;
        }
        public bool SetUp(Card card, int up = 0)
        {
            return Core.Rules.EntityRule.SetLocation(card, up);
        }
    }
}