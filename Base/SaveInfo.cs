using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Interface;
using Newtonsoft.Json.Linq;

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
                JArray array_cdyn = (JArray)json["CDynamicNames"];
                List<string> cdyn_names = new List<string>(array_cdyn.Count);
                for (int i = 0; i < array_cdyn.Count; i++)
                    cdyn_names.Add((string)array_cdyn[i]);
                if (Core.Dynamic.SetCDynamicNames(cdyn_names))
                    return true;
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
                JArray cdyna = new JArray();
                for(int i = 0; i < Core.Dynamic.CDynamicNames.Count; i++)
                {
                    cdyna.Add(Core.Dynamic.CDynamicNames[i]);
                }
                json.Add("CDynamicNames", cdyna);
            }
            catch (Exception)
            {
                return null;
            }
            return json;
        }
    }
}
