using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GameCore.Map
{
    public class TileRule : Base.Rule
    {
        public class CTile : Base.Component
        {
            private const string _type_name = "CTile";
            public override string TypeName => _type_name;
            public int PositionX { get; internal set; } = -1;
            public int PositionY { get; internal set; } = -1;
            public bool Init(int position_x, int position_y)
            {
                PositionX = position_x;
                PositionY = position_y;
                return false;
            }
            public override bool FromJsonObject(JObject js)
            {
                if(base.FromJsonObject(js))
                    return true;
                try{
                    Init((int)js["PositionX"]
                        ,(int)js["PositionY"]);
                }catch(Exception){
                    return true;
                }
                return false;
            }
            public override JObject ToJsonObject()
            {
                JObject js = base.ToJsonObject();
                if(js == null)
                    return null;
                try{
                    js.Add(new JProperty("PositionX", PositionX));
                    js.Add(new JProperty("PositionY", PositionY));
                }catch(Exception){
                    return null;
                }
                return js;
            }
        }
        private int _ctn_location = -1;
        private int _ctn_tile = -1;
        public bool Init()
        {
            _ctn_location = Base.Component.GetSpawner<Root.LocationRule.CLocation>().Type_Number;
            _ctn_tile = Base.Component.GetSpawner<CTile>().Type_Number;
            return false;
        }
        public CTile AddCTile(Base.Card card)
        {
            return AddComponent<CTile>(card);
        }
        public bool AddCTile(Base.Card card, int position_x, int position_y)
        {
            return AddComponent<CTile>(card).Init(position_x, position_y);
        }
        public override bool IsUsable()
        {
            return false;
        }
    }
}
