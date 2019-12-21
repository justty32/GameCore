using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Interface
{
    public class Config
    {
        // used in Core, not Data
        // Difference with CoreInfo, is these configurations are changeable every time
        // need to offer a instance, while Core.Init()
        public Config()
        {
            
        }
        public bool Set(Config config)
        {
            if(config == null)
                return true;
            if(!config.IsUsable())
                return true;
            // copy data to this
            return false;
        }
        public bool IsUsable()
        {
            if (Util.HasAnyNegative(
                // data check
                ))
                return false;
            return true;
        }
        public bool FromJsonString(string js)
        {
            if(js == null)
                return true;
            JObject oj;
            try{
                oj = JObject.Parse(js);
                // get data
            }catch(Exception){
                return true;
            }
            return false;
        }
        public string ToJsonString()
        {
            JObject js = new JObject();
            if(js == null)
                return null;
            try{
                // put data
            }catch(Exception){
                return null;
            }
            return js.ToString();
        }
    }
}
