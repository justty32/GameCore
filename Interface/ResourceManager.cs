using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

// TODO : model, action, face, body, thing, material...
// sound
// load this from INeed

namespace GameCore.Interface
{
    public  class ResourceManager
    {
        public Dictionary<string, int> DicIcon = new Dictionary<string, int>();
        public Dictionary<string, int> DicColor = new Dictionary<string, int>();
        public Dictionary<string, int> DicOrganism = new Dictionary<string, int>();
        public Dictionary<string, int> DicPartical = new Dictionary<string, int>();
        public Dictionary<string, int> DicTerrain = new Dictionary<string, int>();
        public Dictionary<string, int> DicScene = new Dictionary<string, int>();
        public Dictionary<string, int> DicBuild = new Dictionary<string, int>();
        public Dictionary<string, int> DicItem = new Dictionary<string, int>();
        public Dictionary<string, int> DicMaterial = new Dictionary<string, int>();
        public Dictionary<string, int> DicWord = new Dictionary<string, int>();
        public Dictionary<string, int> DicDescript = new Dictionary<string, int>();
        public Dictionary<string, int> DicTalk = new Dictionary<string, int>();
        public Dictionary<string, int> DicName = new Dictionary<string, int>();
        public Dictionary<int, string> IconDic = new Dictionary<int, string>();
        public Dictionary<int, string> ColorDic = new Dictionary<int, string>();
        public Dictionary<int, string> OrganismDic = new Dictionary<int, string>();
        public Dictionary<int, string> ParticalDic = new Dictionary<int, string>();
        public Dictionary<int, string> TerrainDic = new Dictionary<int, string>();
        public Dictionary<int, string> SceneDic = new Dictionary<int, string>();
        public Dictionary<int, string> BuildDic = new Dictionary<int, string>();
        public Dictionary<int, string> ItemDic = new Dictionary<int, string>();
        public Dictionary<int, string> MaterialDic = new Dictionary<int, string>();
        public Dictionary<int, string> WordDic = new Dictionary<int, string>();
        public Dictionary<int, string> DescriptDic = new Dictionary<int, string>();
        public Dictionary<int, string> TalkDic = new Dictionary<int, string>();
        public Dictionary<int, string> NameDic = new Dictionary<int, string>();
        private bool _Align()
        {
            List<Dictionary<string, int>> dics = new List<Dictionary<string, int>>
            {
                DicIcon, DicColor, DicOrganism, DicPartical, DicTerrain,
                DicScene, DicBuild, DicItem, DicMaterial, DicWord, 
                DicDescript, DicTalk, DicName
            };
            List<Dictionary<int, string>> tics = new List<Dictionary<int, string>>
            {
                IconDic, ColorDic, OrganismDic, ParticalDic, TerrainDic,
                SceneDic, BuildDic, ItemDic, MaterialDic, WordDic, 
                DescriptDic, TalkDic,  NameDic
            };
            for(int i = 0; i < dics.Count; i++)
                foreach (var kv in dics[i])
                    try
                    {
                        tics[i].Add(kv.Value, kv.Key);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
            return false;
        }
        public int GetIcon(string key)
        {
            if (key == null)
                return 0;
            if (DicIcon.ContainsKey(key))
                return DicIcon[key];
            else
                return 0;
        }
        public int GetColor(string key)
        {
            if (key == null)
                return 0;
            if (DicColor.ContainsKey(key))
                return DicColor[key];
            else
                return 0;
        }
        public int GetModel(string key, ModelType model_type)
        {
            if (key == null)
                return 0;
            Dictionary<string, int> dic = null;
            switch (model_type)
            {
                case ModelType.Organism:
                    dic = DicOrganism;
                    break;
                case ModelType.Partical:
                    dic = DicPartical;
                    break;
                case ModelType.Scene:
                    dic = DicScene;
                    break;
                case ModelType.Terrain:
                    dic = DicTerrain;
                    break;
                case ModelType.Item:
                    dic = DicItem;
                    break;
                default:
                    dic = DicPartical;
                    break;
            }
            if (dic.ContainsKey(key))
                return dic[key];
            else
                return 0;
        }
        public int GetMaterial(string key)
        {
            if (key == null)
                return 0;
            if (DicMaterial.ContainsKey(key))
                return DicMaterial[key];
            else
                return 0;
        }
        public int GetText(string key, TextType text_type)
        {
            if (key == null)
                return 0;
            Dictionary<string, int> dic = null;
            switch (text_type)
            {
                case TextType.Word:
                    dic = DicWord;
                    break;
                case TextType.Description:
                    dic = DicDescript;
                    break;
                case TextType.Talk:
                    dic = DicTalk;
                    break;
                case TextType.Name:
                    dic = DicName;
                    break;
                default:
                    dic = DicWord;
                    break;
            }
            if (dic.ContainsKey(key))
                return dic[key];
            else
                return 0;
        }
        public enum ModelType
        {
            Partical, Organism, Terrain, Scene, Item
        }
        public enum TextType
        {
            Word, Description, Talk, Name
        }
        public static ResourceManager FromJsonObject(JObject json)
        {
            if (json == null)
                return null;
            try
            {
                var rm = json.ToObject<ResourceManager>();
                rm._Align();
                return rm;
            }
            catch (Exception e)
            {
                return Core.State.WriteException<ResourceManager>(e);
            }
        }
        public JObject ToJsonObject()
        {
            JObject json = new JObject();
            try
            {
                json = JObject.FromObject(this);
            }
            catch (Exception e)
            {
                return Core.State.WriteException<JObject>(e);
            }
            return json;
        }
    }
}