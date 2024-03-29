﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Thing
{
    public class ToolRule : Rule
    {
        public class CTool : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CTool";
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CTool c = json.ToObject<CTool>();
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
                var c = Spawn<CTool>();
                if(c == null)
                    return null;
                // do something
                return c;
            }
        }
        public Hook<int, object> HToolDestroy = new Hook<int, object>();
        private int _ctn_tool = -1;
        public ToolRule()
        {
            _ctn_tool = Concept.Spawn<CTool>().TypeNumber;
        }
        public bool BeTool(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_tool))
                return true;
            // add concepts
            var c = AddConcept<CTool>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyTool(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CTool>(_ctn_tool);
            if (c == null)
            {
                card.Remove(_ctn_tool);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HToolDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_tool);
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
