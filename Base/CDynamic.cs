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
        public void SetTypeName(string name) => _type_name = (name == null)?(""):(name);
        public Dictionary<string, int> DataInt = new Dictionary<string, int>();
        public Dictionary<string, float> DataFloat = new Dictionary<string, float>();
        public Dictionary<string, string> DataString = new Dictionary<string, string>();
        public Dictionary<string, bool> DataBool = new Dictionary<string, bool>();
        public List<int> ArrayInt = new List<int>();
        public List<float> ArrayFloat = new List<float>();
        public List<string> ArrayString = new List<string>();
        public List<bool> ArrayBool = new List<bool>();
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
            return false;
        }
        public override bool FromJsonObject(JObject js)
        {
            if (js == null)
                return true;
            try
            {
                if (!js.ContainsKey("TypeName"))
                    return true;
                SetTypeName((string)js["TypeName"]);
                DataInt = new Dictionary<string, int>();
                DataFloat = new Dictionary<string, float>();
                DataString = new Dictionary<string, string>();
                DataBool = new Dictionary<string, bool>();
                if (js.ContainsKey("DataInt"))
                {
                    JObject data_int = (JObject)js["DataInt"];
                    foreach(var j in data_int)
                    {
                        DataInt.Add(j.Key, (int)j.Value);
                    }
                }
                if (js.ContainsKey("DataFloat"))
                {
                    JObject data_float = (JObject)js["DataFloat"];
                    foreach (var j in data_float)
                    {
                        DataFloat.Add(j.Key, (float)j.Value);
                    }
                }
                if (js.ContainsKey("DataString"))
                {
                    JObject data_string = (JObject)js["DataString"];
                    foreach (var j in data_string)
                    {
                        DataString.Add(j.Key, (string)j.Value);
                    }
                }
                if (js.ContainsKey("DataBool"))
                {
                    JObject data_bool = (JObject)js["DataBool"];
                    foreach(var j in data_bool)
                    {
                        DataBool.Add(j.Key, (bool)j.Value);
                    }
                }
                ArrayInt = new List<int>();
                ArrayString = new List<string>();
                ArrayBool = new List<bool>();
                ArrayFloat = new List<float>();
                if (js.ContainsKey("ArrayInt"))
                {
                    JArray array_int = (JArray)js["ArrayInt"];
                    for (int i = 0; i < array_int.Count; i++)
                        ArrayInt.Add((int)array_int[i]);
                }
                if (js.ContainsKey("ArrayFloat"))
                {
                    JArray array_float = (JArray)js["ArrayFloat"];
                    for (int i = 0; i < array_float.Count; i++)
                        ArrayFloat.Add((float)array_float[i]); 
                }
                if (js.ContainsKey("ArrayString"))
                {
                    JArray array_string = (JArray)js["ArrayString"];
                    for (int i = 0; i < array_string.Count; i++)
                        ArrayString.Add((string)array_string[i]);
                }
                if (js.ContainsKey("ArrayBool"))
                {
                    JArray array_bool = (JArray)js["ArrayBool"];
                    for (int i = 0; i < array_bool.Count; i++)
                        ArrayBool.Add((bool)array_bool[i]);
                }
            }
            catch (Exception e)
            {
                return Core.State.WriteException(e);
            }
            if (!IsUsable())
                return true;
            return false;
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
                js.Add("TypeName", TypeName);
                JObject data_int = new JObject();
                JObject data_float = new JObject();
                JObject data_string = new JObject();
                JObject data_bool = new JObject();
                foreach(var data in DataInt)
                    data_int.Add(data.Key, data.Value);
                foreach (var data in DataFloat)
                    data_float.Add(data.Key, data.Value);
                foreach (var data in DataString)
                    data_string.Add(data.Key, data.Value);
                foreach (var data in DataBool)
                    data_bool.Add(data.Key, data.Value);
                JArray array_int = new JArray();
                JArray array_float = new JArray();
                JArray array_string = new JArray();
                JArray array_bool = new JArray();
                for (int i = 0; i < ArrayInt.Count; i++)
                    array_int.Add(ArrayInt[i]);
                for (int i = 0; i < ArrayFloat.Count; i++)
                    array_float.Add(ArrayFloat[i]);
                for (int i = 0; i < ArrayString.Count; i++)
                    array_string.Add(ArrayString[i]);
                for (int i = 0; i < ArrayBool.Count; i++)
                    array_bool.Add(ArrayBool[i]);
                js.Add("DataInt", data_int);
                js.Add("DataFloat", data_float);
                js.Add("DataString", data_string);
                js.Add("DataBool", data_bool);
                js.Add("ArrayInt", array_int);
                js.Add("ArrayFloat", array_float);
                js.Add("ArrayString", array_string);
                js.Add("ArrayBool", array_bool);
            }
            catch (Exception e)
            {
                return Core.State.WriteException<JObject>(e);
            }
            return js;
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
