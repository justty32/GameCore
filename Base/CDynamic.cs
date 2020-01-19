using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using GameCore.Interface;

namespace GameCore.Base
{
    public class CDynamic : Concept
    {
        public override string TypeName => _type_name;
        private string _type_name = "dynamic concept";
        public bool SetType(string type_name)
        {
            // auto set type name and number
            // and add spawner into global spawner list
            // !! not change its data
            var spawner = ConceptManager.GetCDynamicSpawner(type_name);
            if (spawner == null)
                return true;
            _type_name = spawner.TypeName;
            TypeNumber = spawner.TypeNumber;
            return false;
        }
        public Dictionary<string, int> DataInt = new Dictionary<string, int>();
        public Dictionary<string, float> DataFloat = new Dictionary<string, float>();
        public Dictionary<string, string> DataString = new Dictionary<string, string>();
        public Dictionary<string, bool> DataBool = new Dictionary<string, bool>();
        public Dictionary<string, List<int>> ArrayInt = new Dictionary<string, List<int>>();
        public Dictionary<string, List<float>> ArrayFloat = new Dictionary<string, List<float>>();
        public Dictionary<string, List<string>> ArrayString = new Dictionary<string, List<string>>();
        public Dictionary<string, List<bool>> ArrayBool = new Dictionary<string, List<bool>>();
        public bool HasKey(string key)
        {
            if (DataInt.ContainsKey(key))
                return true;
            if (DataFloat.ContainsKey(key))
                return true;
            if (DataString.ContainsKey(key))
                return true;
            if (DataBool.ContainsKey(key))
                return true;
            return Util.HasAnyTrue(ArrayInt.ContainsKey(key)
                , ArrayFloat.ContainsKey(key)
                , ArrayString.ContainsKey(key)
                , ArrayBool.ContainsKey(key));
        }
        public override Concept FromJsonObject(JObject js)
        {
            // which will set type name, number 
            // return null while the type name not in Dynamic.CDynamicNameSet
            if (js == null)
                return null;
            try
            {
                if (!js.ContainsKey("TypeName"))
                    return null;
                if (!Core.Dynamic.CDynamicNames.Contains((string)js["TypeName"]))
                    return null;
                if(js.ContainsKey("Card"))
                    js.Remove("Card");
                CDynamic c = js.ToObject<CDynamic>();
                if (c == null)
                    return null;
                if (c.SetType((string)js["TypeName"]))
                    return null;
                if (!c.IsUsable())
                    return null;
                return c;
            }
            catch (Exception e)
            {
                return Core.State.WriteException<Concept>(e);
            }
        }
        public override JObject ToJsonObject()
        {
            JObject js = new JObject();
            if (js == null)
                return null;
            if (!IsUsable())
                return null;
            try
            {
                js = JObject.FromObject(this);
                if (js != null)
                    js.Remove("Card");
                return js;
            }
            catch (Exception e)
            {
                return Core.State.WriteException<JObject>(e);
            }
        }
        public override bool IsUsable()
        {
            if (base.IsUsable() == false)
                return false;
            if (Util.HasAnyNull(
                DataInt, DataFloat, DataString, DataBool,
                ArrayInt, ArrayFloat, ArrayString, ArrayBool
                ))
                return false;
            return true;
        }
    }
}
