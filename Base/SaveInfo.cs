using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Interface;
using Newtonsoft.Json.Linq;

// TODO: except this fromjson(), still need cdynamic's name_set and so on... 

    //also set Dynamic's CDynamicNames(at Interface/SL.cs), Cards.MaxNumber

namespace GameCore.Base
{
    public class SaveInfo
    {
        public string DependentVersion = CoreInfo.Version;
        public Dictionary<string, int> DependentModule = new Dictionary<string, int>();
        public int CardAmount = Core.Cards.MaxNumber;
        public List<string> CDynamicNames = Core.Dynamic.CDynamicNames;
        public bool FromJsonObject(JObject json)
        {
            if (json == null)
                return true;
            try
            {
                DependentVersion = (string)json["DependentVersion"];
                JObject modules = (JObject)json["DependentModule"];
                DependentModule = new Dictionary<string, int>();
                foreach(var module in modules.Properties())
                    DependentModule.Add(module.Name, (int)module.Value);
                CardAmount = (int)json["CardAmount"];
                Core.Cards.MaxNumber = CardAmount - 1;
            }catch(Exception)
            {
                return true;
            }
            return false;
        }
        public JObject ToJsonObject()
        {
            JObject json = new JObject();
            try
            {
                json.Add("DependentVersion", DependentVersion);
                JObject modules = new JObject();
                foreach (var module in DependentModule)
                {
                    modules.Add(module.Key, module.Value);
                }
                json.Add("DependentModule", modules);
                CardAmount = Core.Cards.MaxNumber + 1;
                json.Add("CardAmount", CardAmount);
            }
            catch (Exception)
            {
                return null;
            }
            return json;
        }
    }
}
