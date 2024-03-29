﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Creature
{
    public class RaceRule : Rule
    {
        public class CRace : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CRace";
            public int Sort = (int)RaceRule.Sort.Beast;
            public List<State> States = new List<State>();
            public int MaxYears = 100;
            public int SuperiorRace = 0;
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CRace c = json.ToObject<CRace>();
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
                var c = Spawn<CRace>();
                if(c == null)
                    return null;
                // do something
                return c;
            }
        }
        public enum Sort
        {
            Other, Human, Beast, Plant, Bird, Insect, Mechanic, Undead, Energy, God
        }
        public enum LiveState
        {
            Egg, Child, Adult, Elder, Dying
        }
        public struct State
        {
            public int LiveState;
            public int Years;
            public int Body;
        }
        public Hook<int, object> HRaceDestroy = new Hook<int, object>();
        private int _ctn_race = -1;
        public RaceRule()
        {
            _ctn_race = Concept.Spawn<CRace>().TypeNumber;
        }
        public bool BeRace(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_race))
                return true;
            // add concepts
            var c = AddConcept<CRace>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyRace(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CRace>(_ctn_race);
            if (c == null)
            {
                card.Remove(_ctn_race);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HRaceDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_race);
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
