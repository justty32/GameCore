using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GameCore.Base;

// TODO: Now, only implement SLs about config and cards
//       and that cards can only be SL by full stuck in one file now.
//       So, next step is implement the fragment SL about cards,
//       also about rules, scripts, mods....

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
            }catch(Exception){
                return true;
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
                JArray array_cdyn = (JArray)json["CDynamicNames"];
                List<string> cdyn_names = new List<string>(array_cdyn.Count);
                for (int i = 0; i < array_cdyn.Count; i++)
                    cdyn_names.Add((string)array_cdyn[i]);
                if (Core.Dynamic.SetCDynamicNames(cdyn_names))
                    return true;
            }
            catch (Exception)
            {
                return true;
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
            catch (Exception)
            {
                return true;
            }
            return false;
        }
        public bool Card(params int[] numbers) 
        {
            for(int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0 || numbers[i] >= Core.Cards.MaxNumber)
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
                catch (Exception)
                {
                    return true;
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
        /*
        public bool CardFragment(int index, bool is_cover = false)
        {
            // load all cards in a json file, but no cover
            // if there is already the number of card, the card won't be loaded
            // if specific isn't null, will only load specific cards.
            string json = Core.INeed.ImportSaveCards(Core.Save_Name, index);
            if(json == null)
                return true;
            if(json.Equals(""))
                return true;
            try{
                JArray oj = JArray.Parse(json);
                for(int i = 0; i < oj.Count; i++)
                {
                    Base.Card card = new Base.Card();
                    // if want cover it, and there is alreay one
                    if(is_cover && Core.Cards[i] != null)
                        Core.Cards[i].Clear();
                    card.FromJsonObject((JObject)oj[i]);
                }
            }catch(Exception){
                return true;
            }
            return false;
        }
        public bool CardAll(bool is_cover = false)
        {
            // won't cover the card which is alreay in Core.Cards
            for(int index = 0; index < Core.Card_max_number / Core.CoreInfo.Card_amount_per_file; index++)
            {
                string json = Core.INeed.ImportSaveCards(Core.Save_Name, index);
                if(json == null)
                    return true;
                if(json.Equals(""))
                    return true;
                try{
                    JArray oj = JArray.Parse(json);
                    for(int i = 0; i < oj.Count; i++)
                    {
                        Base.Card card = new Base.Card();
                        // if want cover it, and there is alreay one
                        if(is_cover && Core.Cards[i] != null)
                            Core.Cards[i].Clear();
                        card.FromJsonObject((JObject)oj[i]);
                    }
                }catch(Exception){
                    return true;
                }
            }
            return false;
        }
        public bool Card(bool is_cover, params int[] numbers)
        {
            if(numbers == null)
                return true;
            List<int> indexes = new List<int>(numbers.Length);
            List<JArray> jarrays = new List<JArray>(numbers.Length);
            foreach(int i in numbers)
            {
                int index = i / Core.CoreInfo.Card_amount_per_file;
                JArray oj = null;
                // is already load the JArray, set it
                if(indexes.Contains(index))
                    oj = jarrays[index];
                int start = index * Core.CoreInfo.Card_amount_per_file;
                int position = i - start;
                try{
                    // oj == null means not yet load JArray
                    if(oj == null){
                        string json = Core.INeed.ImportSaveCards(Core.Save_Name, index);
                        if(json == null)
                            return true;
                        if(json.Equals(""))
                            return true;
                        oj = JArray.Parse(json);
                        jarrays.Add(oj);
                        indexes.Add(index);
                    }
                    // if want cover it, and there is alreay one
                    if(is_cover && Core.Cards[i] != null)
                        Core.Cards[i].Clear();
                    Base.Card card = new Base.Card();
                    card.FromJsonObject((JObject)oj[position]);
                }catch(Exception){
                    return true;
                }
            }
            return false;
        }
        public bool Rules()
        {
            JObject js = JObject.Parse(Core.INeed.ImportSaveRule(Core.Save_Name));
            try{
                foreach(var rule in js)
                {
                    if(Core.Rules.RuleDic[rule.Key].FromJsonObject((JObject)rule.Value))
                        return true;
                }
            }catch(Exception){
                return true;
            }
            return false;
        }
        */
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
            }catch(Exception){
                return true;
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
                JArray cdyna = new JArray(Core.Dynamic.CDynamicNames.Count);
                for(int i = 0; i < Core.Dynamic.CDynamicNames.Count; i++)
                {
                    cdyna.Add(Core.Dynamic.CDynamicNames[i]);
                }
                json.Add("CDynamicNames", cdyna);
                if (Core.INeed.ExportInfo(Core.DirName, json.ToString()))
                    return true;
            }catch(Exception)
            {
                return true;
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
            catch (Exception)
            {
                return true;
            }
            return false;
        }
        public bool Card(params int[] numbers)
        {
            if (numbers == null)
                return true;
            for(int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0 || numbers[i] >= Core.Cards.MaxNumber)
                    return true;
                try
                {
                    JObject json = Core.Cards[numbers[i]].ToJsonObject();
                    if (json == null)
                        return true;
                    if (Core.INeed.ExportCard(Core.DirName, json.ToString()))
                        return true;
                }
                catch (Exception)
                {
                    return true;
                }
            }
            return false;
        }
        public bool AllCards()
        {
            return this.Card(new List<int>(Core.Cards.cards.Keys).ToArray());
        }
        /*
        public bool Card(params int[] numbers)
        {
            if(numbers == null)
                return true;
            // get all index need to save
            List<int> indexes = new List<int>();
            foreach(int cn in numbers)
            {
                if(cn < 0)
                    return true;
                int index = cn / Core.CoreInfo.Card_amount_per_file;
                // if index not yet in list, add it
                if(!indexes.Contains(index))
                    indexes.Add(index);
            }
            // iterate for all index
            // one index, do one save
            foreach(int cns in indexes){
                int index = cns;
                int start = index * Core.CoreInfo.Card_amount_per_file;
                // count all cards in fragment, if the card hasn't in Core.Cards yet
                // add them into list, then, load it.
                List<int> need_load_cards = new List<int>(numbers.Length);
                for(int i = start; i < start + Core.CoreInfo.Card_amount_per_file; i++)
                {
                    if(Core.Cards[i] == null)
                    {
                        need_load_cards.Add(i);
                    }
                }
                if(Core.Load.Card(false, need_load_cards.ToArray()))
                    return true;
                // after load, all card of fragment is ready to save
                try
                {
                    // add them all into a JArray
                    JArray ja = new JArray();
                    for(int i = start; i < start + Core.CoreInfo.Card_amount_per_file; i++)
                    {
                        if(Core.Cards[i] == null)
                            return true;
                        ja.Add(Core.Cards[i].ToJsonObject());
                    }
                    // save it
                    if(Core.INeed.ExportSaveCards(Core.Save_Name, index, ja.ToString()))
                        return true;
                }catch(Exception){
                    return true;
                }
                return false;
            }
            return false;
        }
        public bool CardAll()
        {
            return Card(Core.Cards.cards.Keys.ToArray());
        }
        public bool Rules()
        {
            try{
                JObject js = new JObject();
                foreach(var rule in Core.Rules.RuleDic)
                {
                    js.Add(rule.Key, rule.Value.ToJsonObject());
                }
                Core.INeed.ExportSaveRule(Core.Save_Name, js.ToString());
            }catch(Exception){
                return true;
            }
            return false;
        }
        */
    }
}
