using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Creature
{
    public class KnowledgeRule : Rule
    {
        public class CKnowledge : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CKnowledge";
            public int Difficulty = 0;
            public int Max = 10;
            public Dictionary<int, int> NeedKnowledges = new Dictionary<int, int>();
            public AttainmentRule.Skills NeedSkills = new AttainmentRule.Skills();
            public List<UpgradeEffect> UpgradeEffects = new List<UpgradeEffect>();
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CKnowledge c = json.ToObject<CKnowledge>();
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
                var c = Spawn<CKnowledge>();
                if(c == null)
                    return null;
                // do something
                return c;
            }
        }
        public struct UpgradeEffect
        {
            public int TriggerLevel;
            public int LearnKnowledge;
            public int LearnKnowledgeLevel;
            public int GetFeature;
            public int GetFeatureLevel;
        }
        public Hook<int, object> HKnowledgeDestroy = new Hook<int, object>();
        private int _ctn_knowledge = -1;
        public KnowledgeRule()
        {
            _ctn_knowledge = Concept.Spawn<CKnowledge>().TypeNumber;
        }
        public bool BeKnowledge(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_knowledge))
                return true;
            // add concepts
            var c = AddConcept<CKnowledge>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyKnowledge(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CKnowledge>(_ctn_knowledge);
            if (c == null)
            {
                card.Remove(_ctn_knowledge);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HKnowledgeDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_knowledge);
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
