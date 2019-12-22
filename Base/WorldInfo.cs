using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace GameCore.Base
{
    public class WorldInfo
    {
        public string World_name = null;
        public int Init_seed = -1;
        public int Now_seed = -1; 
        internal int Card_max_number = -1; // don't touch it !!!
        public bool FromJsonString(string jstr)
        {
            WorldInfo o = null;
            try{
                o = JsonConvert.DeserializeObject<WorldInfo>(jstr);
                if(o == null)
                    return true;
            }catch(Exception){
                return true;
            }
            World_name = o.World_name;
            Init_seed = o.Init_seed;
            Now_seed = o.Now_seed;
            Card_max_number = o.Card_max_number;
            return false;
        }
        public string ToJsonString()
        {
            string js = null;
            try{
                js = JsonConvert.SerializeObject(this, Formatting.Indented).ToString();
            }catch(Exception){
                return null;
            }
            return js;
        }
    }
}