using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GameCore.Base;

// TODO: module list, module, language, model list

namespace GameCore.Interface
{
    public class Load
    {
        public bool Config()
        {
            string json = Core.INeed.ImportConfig();
            if(json == null)
                return true;
            Config config = null;
            try{
                config = JsonConvert.DeserializeObject<Config>(json);
            }catch (Exception e){
                return Core.State.WriteException(e);
            }
            return config == null;
        }
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
            for(int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0 || numbers[i] > Core.Cards.MaxNumber)
                    return true;
                try
                {
                    string jstr = Core.INeed.ImportCard(Core.DirName, numbers[i]);
                    if (jstr == null)
                        return true;
                    JObject json = JObject.Parse(jstr);
                    Base.Card card = new Card();
                    if (card.FromJsonObject(json))
                        return true;
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
        public Card CardFromOtherSave(string save_name, int number)
        {
            if (number < 0)
                return null;
            try
            {
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
                for(int i = 0; i < cs.Count ; i++)
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
            try{
                string str = JsonConvert.SerializeObject(Core.Config);
                if(str == null)
                    return true;
                if(Core.INeed.ExportConfig(str))
                    return true;
            }catch (Exception e){
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
            }catch (Exception e)
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
        public bool Card(params int[] numbers)
        {
            if (numbers == null)
                return true;
            for(int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0 || numbers[i] > Core.Cards.MaxNumber)
                    return true;
                try
                {
                    JObject json = Core.Cards[numbers[i]].ToJsonObject();
                    if (json == null)
                        return true;
                    if (Core.INeed.ExportCard(Core.DirName, numbers[i], json.ToString()))
                        return true;
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
