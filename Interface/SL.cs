using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GameCore.Base;

// these are using Core.Instance.INeed and Cards so much, be careful about they
// gamecore.instance-> init -> data_init -

namespace GameCore.Interface
{
    public class Load
    {
        public Config Config()
        {
            string json = Core.Instance.INeed.ImportConfig();
            if(json == null)
                return null;
            Config config = null;
            try{
                config = JsonConvert.DeserializeObject<Config>(json);
            }catch(Exception){
                return null;
            }
            return config;
        }
        public WorldInfo WorldInfo()
        {
            // load Core.Instance.Save_Name/info.json
            if(Core.Instance.Save_Name == null)
                return null;
            string json = Core.Instance.INeed.ImportSaveInfo(Core.Instance.Save_Name);
            if(json == null)
                return null;
            WorldInfo wi = null; 
            try{
                wi =Newtonsoft.Json.JsonConvert.DeserializeObject<WorldInfo>(json);
            }catch(Exception){
                return null;
            }
            return wi;
        }
        public bool CardFragment(int index, bool is_cover = false)
        {
            // load all cards in a json file, but no cover
            // if there is already the number of card, the card won't be loaded
            // if specific isn't null, will only load specific cards.
            string json = Core.Instance.INeed.ImportSaveCards(Core.Instance.Save_Name, index);
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
                    if(is_cover && Core.Instance.Cards[i] != null)
                        Core.Instance.Cards[i].Clear();
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
            for(int index = 0; index < Core.Instance.Card_max_number / Core.Instance.CoreInfo.Card_amount_per_file; index++)
            {
                string json = Core.Instance.INeed.ImportSaveCards(Core.Instance.Save_Name, index);
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
                        if(is_cover && Core.Instance.Cards[i] != null)
                            Core.Instance.Cards[i].Clear();
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
                int index = i / Core.Instance.CoreInfo.Card_amount_per_file;
                JArray oj = null;
                // is already load the JArray, set it
                if(indexes.Contains(index))
                    oj = jarrays[index];
                int start = index * Core.Instance.CoreInfo.Card_amount_per_file;
                int position = i - start;
                try{
                    // oj == null means not yet load JArray
                    if(oj == null){
                        string json = Core.Instance.INeed.ImportSaveCards(Core.Instance.Save_Name, index);
                        if(json == null)
                            return true;
                        if(json.Equals(""))
                            return true;
                        oj = JArray.Parse(json);
                        jarrays.Add(oj);
                        indexes.Add(index);
                    }
                    // if want cover it, and there is alreay one
                    if(is_cover && Core.Instance.Cards[i] != null)
                        Core.Instance.Cards[i].Clear();
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
            JObject js = JObject.Parse(Core.Instance.INeed.ImportSaveRule(Core.Instance.Save_Name));
            try{
                foreach(var rule in js)
                {
                    if(Core.Instance.Rules.RuleDic[rule.Key].FromJsonObject((JObject)rule.Value))
                        return true;
                }
            }catch(Exception){
                return true;
            }
            return false;
        }
    }
    public class Save
    {
        public bool Config()
        {
            try{
                string str = JsonConvert.SerializeObject(Core.Instance.Config);
                if(str == null)
                    return true;
                if(Core.Instance.INeed.ExportConfig(str))
                    return true;
            }catch(Exception){
                return true;
            }
            return false;
        }
        public bool WorldInfo()
        {
            if(Core.Instance.Save_Name == null)
                return true;
            try{
                string json = JsonConvert.SerializeObject(Core.Instance.WorldInfo);
                if(json == null)
                    return true;
                if(Core.Instance.INeed.ExportSaveInfo(Core.Instance.Save_Name, json))
                    return true;
            }catch(Exception){
                return true;
            }
            return false;
        }
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
                int index = cn / Core.Instance.CoreInfo.Card_amount_per_file;
                // if index not yet in list, add it
                if(!indexes.Contains(index))
                    indexes.Add(index);
            }
            // iterate for all index
            // one index, do one save
            foreach(int cns in indexes){
                int index = cns;
                int start = index * Core.Instance.CoreInfo.Card_amount_per_file;
                // count all cards in fragment, if the card hasn't in Core.Cards yet
                // add them into list, then, load it.
                List<int> need_load_cards = new List<int>(numbers.Length);
                for(int i = start; i < start + Core.Instance.CoreInfo.Card_amount_per_file; i++)
                {
                    if(Core.Instance.Cards[i] == null)
                    {
                        need_load_cards.Add(i);
                    }
                }
                if(Core.Instance.Load.Card(false, need_load_cards.ToArray()))
                    return true;
                // after load, all card of fragment is ready to save
                try
                {
                    // add them all into a JArray
                    JArray ja = new JArray();
                    for(int i = start; i < start + Core.Instance.CoreInfo.Card_amount_per_file; i++)
                    {
                        if(Core.Instance.Cards[i] == null)
                            return true;
                        ja.Add(Core.Instance.Cards[i].ToJsonObject());
                    }
                    // save it
                    if(Core.Instance.INeed.ExportSaveCards(Core.Instance.Save_Name, index, ja.ToString()))
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
            return Card(Core.Instance.Cards.cards.Keys.ToArray());
        }
        public bool Rules()
        {
            try{
                JObject js = new JObject();
                foreach(var rule in Core.Instance.Rules.RuleDic)
                {
                    js.Add(rule.Key, rule.Value.ToJsonObject());
                }
                Core.Instance.INeed.ExportSaveRule(Core.Instance.Save_Name, js.ToString());
            }catch(Exception){
                return true;
            }
            return false;
        }
    }
}
