﻿using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using Newtonsoft.Json.Linq;

//dynamic concept and dynamic rule
//dynamic rule composed by scripts with hooks
// TODO: add dynamic rules' init process at rule manager's init()
// TODO: add a dictionary of processing CDynamic, relation is about dynamic typename

namespace GameCore.Interface
{
    public class Dynamic { 
        public List<string> CDynamicNames;
        public Dictionary<string, ConceptSpawner<CDynamic>> CDynamicSpawners;
        public Dynamic(List<string> dynamic_concept_names)
        {
            CDynamicNames = new List<string>(dynamic_concept_names);
            CDynamicSpawners = new Dictionary<string, ConceptSpawner<CDynamic>>(CDynamicNames.Count);
            for(int i = 0; i < CDynamicNames.Count; i++)
            {
                var sp = ConceptSpawner<CDynamic>.GetDynamicSpawner(CDynamicNames[i]);
                if (sp != null)
                    CDynamicSpawners.Add(sp.Type_Name, sp);
            }
        }
        public static bool IsCDynamic(JObject concept_json)
        {
            if (concept_json == null)
                return false;
            try
            {   if (Util.HasAnyFalse(
                    concept_json.ContainsKey("DataInt"),
                    concept_json.ContainsKey("DataFloat"),
                    concept_json.ContainsKey("DataString"),
                    concept_json.ContainsKey("DataBool"),
                    concept_json.ContainsKey("ArrayInt"),
                    concept_json.ContainsKey("ArrayFloat"),
                    concept_json.ContainsKey("ArrayString"),
                    concept_json.ContainsKey("ArrayBool")))
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
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
                    JObject data_int = (JObject)js["DataInt"];
                    foreach(var j in data_int)
                    {
                        DataInt.Add(j.Key, (int)j.Value);
                    }
                    JObject data_float = (JObject)js["DataFloat"];
                    foreach (var j in data_float)
                    {
                        DataFloat.Add(j.Key, (float)j.Value);
                    }
                    JObject data_string = (JObject)js["DataString"];
                    foreach (var j in data_string)
                    {
                        DataString.Add(j.Key, (string)j.Value);
                    }
                    JObject data_bool = (JObject)js["DataBool"];
                    foreach(var j in data_bool)
                    {
                        DataBool.Add(j.Key, (bool)j.Value);
                    }
                    JArray array_int = (JArray)js["ArrayInt"];
                    JArray array_float = (JArray)js["ArrayFloat"];
                    JArray array_string = (JArray)js["ArrayString"];
                    JArray array_bool = (JArray)js["ArrayBool"];
                    ArrayInt = new List<int>(array_int.Count);
                    ArrayString = new List<string>(array_string.Count);
                    ArrayBool = new List<bool>(array_bool.Count);
                    ArrayFloat = new List<float>(array_float.Count);
                    for (int i = 0; i < array_int.Count; i++)
                        ArrayInt.Add((int)array_int[i]);
                    for (int i = 0; i < array_float.Count; i++)
                        ArrayFloat.Add((float)array_float[i]); 
                    for (int i = 0; i < array_string.Count; i++)
                        ArrayString.Add((string)array_string[i]);
                    for (int i = 0; i < array_bool.Count; i++)
                        ArrayBool.Add((bool)array_bool[i]);
                }
                catch (Exception)
                {
                    return true;
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
                    foreach(var data in DataInt)
                    {
                        data_int.Add(data.Key, data.Value);
                    }
                    JObject data_float = new JObject();
                    foreach (var data in DataFloat)
                    {
                        data_int.Add(data.Key, data.Value);
                    }
                    JObject data_string = new JObject();
                    foreach (var data in DataString)
                    {
                        data_int.Add(data.Key, data.Value);
                    }
                    JObject data_bool = new JObject();
                    foreach (var data in DataBool)
                    {
                        data_int.Add(data.Key, data.Value);
                    }
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
                catch (Exception)
                {
                    return null;
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