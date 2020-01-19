using System.Collections.Generic;
using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GameCore.Interface
{
    public class LanguageMuster
    {
        public class LanguageSet{
            public string English = "eng";
            public string SimplifyChinese = "chs";
            public string TraditionalChinese = "cht";
        }
        public class WordLinks{
            // usually use
            public string Dynamic;
        }
        public class RareWordLinks{

        }
        public class Expressions{
            // TODO
        }
        public LanguageSet LangSet;
        public WordLinks Word;
        public RareWordLinks RareWord;
        public Dictionary<string, string> WordDic{get;set;}
        public LanguageMuster()
        {
            // TODO: not test yet
            // which is in order to set variable's value to its name
            LangSet = new LanguageSet();
            WordDic = new Dictionary<string, string>();
            try{
                // for words
                JObject jw = JObject.FromObject(Word);
                foreach(var pair in jw)
                {
                    string str = (string)pair.Key;
                    if(str != null){
                        jw.Remove(str);
                        jw.Add(str, str);
                    }
                }
                Word = jw.ToObject<WordLinks>();
                if(Word == null)
                    Word = new WordLinks();
                // for rare words
                jw = JObject.FromObject(RareWord);
                foreach(var pair in jw)
                {
                    string str = (string)pair.Key;
                    if(str != null){
                        jw.Remove(str);
                        jw.Add(str, str);
                    }
                }
                RareWord = jw.ToObject<RareWordLinks>();
                if(RareWord == null)
                    RareWord = new RareWordLinks();
            }catch(Exception e){
                Core.State.WriteException(e);
            }
        }
        public static LanguageMuster FromJsonObject(JObject json)
        {
            if(json == null)
                return null;
            LanguageMuster lm = null; 
            try{
                lm = json.ToObject<LanguageMuster>();
            }catch(Exception e)
            {
                return Core.State.WriteException<LanguageMuster>(e);
            }
            return lm;
        }
        public JObject ToJsonObject()
        {
            JObject json = null;
            try{
                json = JObject.FromObject(this);
            }catch(Exception e){
                return Core.State.WriteException<JObject>(e);
            }
            return json;
        }
    }
}