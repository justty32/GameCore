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
                , "DicTalk", "DicName", "DicSound"
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
        public bool MulCard(params int[] numbers)
        {
            if (numbers == null)
                return true;
            var datas = Core.INeed.MulImportCard(Core.DirName, new List<int>(numbers));
            if (datas == null)
                return true;
            foreach(var data in datas)
            {
                try
                {
                    Card card = new Card();
                    if (card.FromJsonObject(data.Value))
                        continue;
                }catch(Exception e)
                {
                    Core.State.WriteException(e);
                    continue;
                }
            }
            return false;
        }
        public bool Card(params int[] numbers)
        {
            if (numbers == null)
                return true;
            Dictionary<int, List<int>> need_input_numbers = new Dictionary<int, List<int>>();
            foreach(int number in numbers)
            {
                if (!need_input_numbers.ContainsKey(number / CoreInfo.Card_amount_per_file))
                    need_input_numbers.Add(number / CoreInfo.Card_amount_per_file, new List<int>());
                need_input_numbers[number / CoreInfo.Card_amount_per_file].Add(number);
            }
            var datas = Core.INeed.ImportCard(Core.DirName, need_input_numbers);
            if (datas == null)
                return true;
            foreach(var data in datas)
            {
                try
                {
                    string jstr = data.Value;
                    if (jstr == null)
                        continue;
                    if (jstr.Length < 2)
                        continue;
                    JObject json = JObject.Parse(jstr);
                    Card card = new Card();
                    if (card.FromJsonObject(json))
                        continue;
                }catch(Exception e)
                {
                    Core.State.WriteException(e);
                    continue;
                }
            }
            return false;
        }
        public bool AllCards()
        {
            int[] numbers = new int[Core.Cards.MaxNumber + 1];
            for (int i = 0; i < Core.Cards.MaxNumber + 1; i++)
                numbers[i] = i;
            return Card(numbers);
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
        public bool NewThreadMulCard(bool[] results, params int[] numbers)
        {
            if (numbers == null || results == null)
                return true;
            Dictionary<int, JObject> indexs = new Dictionary<int, JObject>();
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0 || numbers[i] > Core.Cards.MaxNumber)
                    continue;
                Card card = Core.Cards[numbers[i]];
                if(card == null)
                    continue;
                if (!card.NeedSave)
                    continue;
                JObject json = card.ToJsonObject();
                if (json == null)
                    continue;
                try
                {
                    indexs.Add(i, json);
                }
                catch (Exception) { continue; }
            }
            return Core.INeed.NewThreadMulExportCard(Core.DirName, indexs, results);
        }
        public bool MulCard(params int[] numbers)
        {
            if (numbers == null)
                return true;
            // make multiple indexes
            Dictionary<int, List<KeyValuePair<int, JObject>>> indexs = new Dictionary<int, List<KeyValuePair<int, JObject>>>();
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0 || numbers[i] > Core.Cards.MaxNumber)
                    continue;
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
                    indexs.Add(numbers[i] / CoreInfo.Card_amount_per_file, new List<KeyValuePair<int, JObject>>());
                indexs[numbers[i] / CoreInfo.Card_amount_per_file].Add(new KeyValuePair<int, JObject>(numbers[i], json));
            }
            return Core.INeed.MulExportCard(Core.DirName, indexs);
        }
        public bool NewThreadCard(bool[] results, params int[] numbers)
        {
            if (numbers == null || results == null)
                return true;
            Dictionary<int, JObject> indexs = new Dictionary<int, JObject>();
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0 || numbers[i] > Core.Cards.MaxNumber)
                    continue;
                Card card = Core.Cards[numbers[i]];
                if(card == null)
                    continue;
                if (!card.NeedSave)
                    continue;
                JObject json = card.ToJsonObject();
                if (json == null)
                    continue;
                try
                {
                    indexs.Add(i, json);
                }
                catch (Exception) { continue; }
            }
            return Core.INeed.NewThreadExportCard(Core.DirName, indexs, results);
        }
        public bool Card(params int[] numbers)
        {
            if (numbers == null)
                return true;
            Dictionary<int, List<KeyValuePair<int, string>>> datas = new Dictionary<int, List<KeyValuePair<int, string>>>();
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0 || numbers[i] > Core.Cards.MaxNumber)
                    continue;
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
                    if(!datas.ContainsKey(numbers[i] / CoreInfo.Card_amount_per_file))
                        datas.Add(numbers[i]/CoreInfo.Card_amount_per_file, new List<KeyValuePair<int, string>>());
                    datas[numbers[i] / CoreInfo.Card_amount_per_file].Add(new KeyValuePair<int, string>(numbers[i], json.ToString()));
                }
                catch (Exception e)
                {
                    Core.State.WriteException(e);
                    continue;
                }
            }
            return Core.INeed.ExportCard(Core.DirName, datas);
        }
        public bool Cards(int[] numbers, bool multi_thread = false, bool[] start_new_thread = null)
        {
            try
            {
                if (numbers == null)
                    return true;
                if (multi_thread)
                {
                    if (start_new_thread != null)
                        return NewThreadMulCard(start_new_thread, numbers);
                    else
                        return MulCard(numbers);
                }
                else
                {
                    if (start_new_thread != null)
                        return NewThreadCard(start_new_thread, numbers);
                    else
                        return Card(numbers);
                }
            }catch(Exception e)
            {
                Core.State.AppendLogLine(e.Message);
                return true;
            }
        }
        public bool AllCards(bool multi_thread = false, bool[] start_new_thread = null)
        {
            return Cards(new List<int>(Core.Cards.cards.Keys).ToArray(), multi_thread, start_new_thread);
        }
    }
}