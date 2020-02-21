using GameCore.Base;
using Newtonsoft.Json.Linq;
using System;

namespace GameCore.Map
{
    public class TileRule : Base.Rule
    {
        public class CTile : Base.Concept
        {
            private readonly string _type_name = "CTile";
            public override string TypeName => _type_name;
            public int PositionX { get; internal set; } = -1;
            public int PositionY { get; internal set; } = -1;

            public bool Init(int position_x, int position_y)
            {
                PositionX = position_x;
                PositionY = position_y;
                return false;
            }

            public override Concept FromJsonObject(JObject ojs)
            {
                CTile c = null;
                var js = AlignJsonOjbect(ojs);
                if (js == null)
                    return null;
                try
                {
                    c = js.ToObject<CTile>();
                }
                catch (Exception e)
                {
                    return Core.State.WriteException<Concept>(e);
                }
                return c;
            }

            public override JObject ToJsonObject()
            {
                JObject js = base.ToJsonObject();
                if (js == null)
                    return null;
                try
                {
                    js.Add(new JProperty("PositionX", PositionX));
                    js.Add(new JProperty("PositionY", PositionY));
                }
                catch (Exception)
                {
                    return null;
                }
                return js;
            }
        }

        private int _ctn_location = -1;
        private int _ctn_tile = -1;

        public override bool Init()
        {
            _ctn_location = ConceptManager.GetSpawner<Root.LocationRule.CLocation>().TypeNumber;
            _ctn_tile = ConceptManager.GetSpawner<CTile>().TypeNumber;
            return false;
        }

        public CTile AddCTile(Base.Card card)
        {
            return AddConcept<CTile>(card);
        }

        public bool AddCTile(Base.Card card, int position_x, int position_y)
        {
            return AddConcept<CTile>(card).Init(position_x, position_y);
        }

        public override bool IsUsable()
        {
            return false;
        }
    }
}