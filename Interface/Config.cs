using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GameCore.Interface
{
    public class Config
    {
        // used in Core, not Data
        // Difference with CoreInfo, is these configurations are changeable every time
        // need to offer a instance, while Core.Init()

        public bool is_Load_Card_While_Not_In_gList = true;
        public Config()
        {
            
        }
        public bool Set(Config config)
        {
            if(config == null)
                return true;
            // copy data to this
            is_Load_Card_While_Not_In_gList = config.is_Load_Card_While_Not_In_gList;
            return false;
        }
        public bool FromJsonString(string js)
        {
            if(js == null)
                return true;
            try{
                Config c = JsonConvert.DeserializeObject<Config>(js);
                is_Load_Card_While_Not_In_gList = c.is_Load_Card_While_Not_In_gList;
            }catch(Exception){
                return true;
            }
            return false;
        }
        public string ToJsonString()
        {
            string js = null;
            try{
                js = JsonConvert.SerializeObject(this);
            }catch(Exception){
                return null;
            }
            return js;
        }
    }
}
