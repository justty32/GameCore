using GameCore.Base;
using GameCore.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;

// TODO: module list, module, language, model list

namespace GameCore
{
    public partial class Core
    {

        public static bool _LoadConfig()
        {
            string json = Core.INeed.ImportConfig();
            if (json == null)
                return true;
            Interface.Config config = null;
            try
            {
                config = JsonConvert.DeserializeObject<Interface.Config>(json);
                if (config == null)
                    return true;
            }
            catch (Exception e)
            {
                return Core.State.WriteException(e);
            }
            p_instance._config = config;
            return false;
        }
        public static bool _LoadCommonResource()
        {
            Core.State.AppendLogLine("... loading common resource ...");
            JObject json_resouce_manager = new JObject();
            var resource_manager = new ResourceManager();
            // load resource dic
            Core.State.AppendLogLine("... loading resource dictionary...");
            string dir = "resourcedictionary";
            List<string> resource_dics = new List<string>
            {
                "DicIcon", "DicColor", "DicOrganism", "DicPartical", "DicTerrain"
                , "DicScene", "DicBuild", "DicItem", "DicMaterial", "DicWord", "DicDescript"
                , "DicTalk", "DicName"
            };
            // load files
            foreach(var resource_dic in resource_dics)
            {
                Core.State.AppendLogLine("loading " + resource_dic + ".json");
                JObject json_resouce_dic = null;
                string resource_dic_data = Core.INeed.ImportCommonFile(resource_dic + ".json", dir);
                if (resource_dic_data == null)
                {
                    Core.State.AppendLogLine(resource_dic + ".json load file failed");
                    continue;
                }
                try
                {
                    json_resouce_dic = JObject.Parse(resource_dic_data);
                    if (json_resouce_dic == null)
                    {
                        Core.State.AppendLogLine(resource_dic + ".json parse failed");
                        continue;
                    }
                    json_resouce_manager.Add(resource_dic, json_resouce_dic);
                }catch(Exception)
                {
                    Core.State.AppendLogLine(resource_dic + ".json load failed");
                    continue;
                }
            }
            Core.State.AppendLogLine("... loading resource dictionary finished...");
            // after adding all json objects, to object
            try
            {
                resource_manager = ResourceManager.FromJsonObject(json_resouce_manager);
                if (resource_manager == null)
                {
                    Core.State.AppendLogLine("resource manager load failed, be default");
                    resource_manager = new ResourceManager();
                }
            }
            catch (Exception)
            {
                Core.State.AppendLogLine("resource manager load failed, be default");
            }
            p_instance._resource_manager = resource_manager;
            Core.State.AppendLogLine("... loading common resource finish ...");
            return false;
        }
    }
}
namespace GameCore.Interface
{
    public class Load
    {
        public bool Config() => Core._LoadConfig();
        public bool CommonResource() => Core._LoadCommonResource();
        public bool SaveInfo()
        {
            string jstr = Core.INeed.ImportInfo(Core.DirName);
            if (jstr == null)
                return true;
            try
            {
                JObject json = JObject.Parse(jstr);
                if (Core.SaveInfo.FromJsonObject(json))
                    return true;
            }
            catch (Exception e)
            {
                return Core.State.WriteException(e);
            }
            return false;
        }
        public bool Rules()
        {
            string jstr = Core.INeed.ImportRules(Core.DirName);
            if (jstr == null)
                return true;
            try
            {
                JArray json = JArray.Parse(jstr);
                if (Core.RuleManager.FromJsonArray(json))
                    return true;
            }
            catch (Exception e)
            {
                return Core.State.WriteException(e);
            }
            return false;
        }
        public bool Card(params int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0 || numbers[i] > Core.Cards.MaxNumber)
                    return true;
                try
                {
                    string jstr = Core.INeed.ImportCard(Core.DirName, numbers[i]);
                    if (jstr == null)
                    {
                        Core.State.AppendLogLine("card number - " + numbers[i] + " load file string failed");
                        continue;
                    }
                    JObject json = JObject.Parse(jstr);
                    Base.Card card = new Card();
                    if (card.FromJsonObject(json))
                    {
                        Core.State.AppendLogLine("card number - " + numbers[i] + " load parse json object failed");
                        continue;
                    }
                }
                catch (Exception e)
                {
                    return Core.State.WriteException(e);
                }
            }
            return false;
        }
        public bool AllCards()
        {
            for (int i = 0; i < Core.Cards.MaxNumber; i++)
            {
                if (this.Card(i))
                    return true;
            }
            return false;
        }
        public Card CardFromString(string json_data)
        {
            if (json_data == null)
                return null;
            try
            {
                string jstr = json_data;
                if (jstr == null)
                    return null;
                JObject ojs = JObject.Parse(jstr);
                Card card = new Card();
                if (!Util.JObjectContainsKey(ojs, "Number"))
                    return null;
                if (!(Util.JObjectContainsKey(ojs, "Name")))
                    ojs.Add("Name", "");
                card.Number = (int)ojs["Number"];
                card.Name = (string)ojs["Name"];
                card.concepts = new Dictionary<int, Concept>();
                JArray cs = (JArray)ojs["Concepts"];
                for (int i = 0; i < cs.Count; i++)
                {
                    if (!(Util.JObjectContainsKey((JObject)cs[i], "TypeName")))
                        continue;
                    if (!ConceptManager.ContainsTypeName((string)cs[i]["TypeName"]))
                        continue;
                    var csp = ConceptManager.GetSpawner((string)cs[i]["TypeName"]);
                    if (csp == null)
                        continue;
                    var c = csp.SpawnBase().FromJsonObject((JObject)cs[i]);
                    if (c == null)
                        continue;
                    card.Add(c);
                }
                Core.State.AppendLogLine("load card from string failed");
                return card;
            }
            catch (Exception e)
            {
                return Core.State.WriteException<Card>(e);
            }
        }
        public Card CardFromOtherSave(string save_name, int number)
        {
            if (number < 0)
                return null;
            try
            {
                Core.State.AppendLogLine("load card from other save start");
                Core.State.AppendLogLine("card number - " + number);
                string jstr = Core.INeed.ImportCard(save_name, number);
                if (jstr == null)
                    return null;
                JObject ojs = JObject.Parse(jstr);
                Card card = new Card();
                if (!Util.JObjectContainsKey(ojs, "Number"))
                    return null;
                if (!(Util.JObjectContainsKey(ojs, "Name")))
                    ojs.Add("Name", "");
                card.Number = (int)ojs["Number"];
                card.Name = (string)ojs["Name"];
                card.concepts = new Dictionary<int, Concept>();
                JArray cs = (JArray)ojs["Concepts"];
                for (int i = 0; i < cs.Count; i++)
                {
                    if (!(Util.JObjectContainsKey((JObject)cs[i], "TypeName")))
                        continue;
                    if (!ConceptManager.ContainsTypeName((string)cs[i]["TypeName"]))
                        continue;
                    var csp = ConceptManager.GetSpawner((string)cs[i]["TypeName"]);
                    if (csp == null)
                        continue;
                    var c = csp.SpawnBase().FromJsonObject((JObject)cs[i]);
                    if (c == null)
                        continue;
                    card.Add(c);
                }
                Core.State.AppendLogLine("load card from other save finished");
                return card;
            }
            catch (Exception e)
            {
                return Core.State.WriteException<Card>(e);
            }
        }
    }
    public class Save
    {
        public bool Config()
        {
            try
            {
                string str = JsonConvert.SerializeObject(Core.Config);
                if (str == null)
                    return true;
                if (Core.INeed.ExportConfig(str))
                    return true;
            }
            catch (Exception e)
            {
                return Core.State.WriteException(e);
            }
            return false;
        }
        public bool SaveInfo()
        {
            try
            {
                JObject json = Core.SaveInfo.ToJsonObject();
                if (json == null)
                    return true;
                if (Core.INeed.ExportInfo(Core.DirName, json.ToString()))
                    return true;
            }
            catch (Exception e)
            {
                return Core.State.WriteException(e);
            }
            return false;
        }
        public bool Rules()
        {
            try
            {
                JArray json = Core.RuleManager.ToJsonArray();
                if (json == null)
                    return true;
                if (Core.INeed.ExportRules(Core.DirName, json.ToString()))
                    return true;
            }
            catch (Exception e)
            {
                return Core.State.WriteException(e);
            }
            return false;
        }
        public Dictionary<int, bool[]> MulCard(params int[] numbers)
        {
            if (numbers == null)
                return null;
            // make multiple indexes
            Dictionary<int, List<KeyValuePair<int, string>>> indexs = new Dictionary<int, List<KeyValuePair<int, string>>>();
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0 || numbers[i] > Core.Cards.MaxNumber)
                    return null;
                Card card = Core.Cards[numbers[i]];
                if(card == null)
                {
                    Core.State.AppendLogLine("card number - " + numbers[i] + " not in core card list, skip it");
                    continue;
                }
                if (!card.NeedSave)
                    continue;
                JObject json = card.ToJsonObject();
                if (json == null)
                {
                    Core.State.AppendLogLine("card number - " + numbers[i] + " saving to json object failed");
                    continue;
                }
                if (!indexs.ContainsKey(numbers[i] / CoreInfo.Card_amount_per_file))
                    indexs.Add(numbers[i] / CoreInfo.Card_amount_per_file, new List<KeyValuePair<int, string>>());
                indexs[numbers[i] / CoreInfo.Card_amount_per_file].Add(new KeyValuePair<int, string>(numbers[i], json.ToString()));
            }
            return Core.INeed.MulExportCard(Core.DirName, indexs);
        }
        public bool Card(params int[] numbers)
        {
            if (numbers == null)
                return true;
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0 || numbers[i] > Core.Cards.MaxNumber)
                    return true;
                try
                {
                    Card card = Core.Cards[numbers[i]];
                    if(card == null)
                    {
                        Core.State.AppendLogLine("card number - " + numbers[i] + " not in core card list, skip it");
                        continue;
                    }
                    if (!card.NeedSave)
                        continue;
                    JObject json = card.ToJsonObject();
                    if (json == null)
                    {
                        Core.State.AppendLogLine("card number - " + numbers[i] + " saving to json object failed");
                        continue;
                    }
                    if (Core.INeed.ExportCard(Core.DirName, numbers[i], json.ToString()))
                    {
                        Core.State.AppendLogLine("card number - " + numbers[i] + " saving to file stirng failed");
                        continue;
                    }
                }
                catch (Exception e)
                {
                    return Core.State.WriteException(e);
                }
            }
            return false;
        }
        public bool AllCards()
        {
            return this.Card(new List<int>(Core.Cards.cards.Keys).ToArray());
        }
    }
}