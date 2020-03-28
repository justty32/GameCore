using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    public class Script
    {
        public string Main;
        public Data Temp = new Data();
        public class Data
        {
            public void Clear()
            {
                DataInt = new Dictionary<string, int>();
                DataFloat = new Dictionary<string, float>();
                DataString = new Dictionary<string, string>();
                DataBool = new Dictionary<string, bool>();
                ArrayInt = new Dictionary<string, List<int>>();
                ArrayFloat = new Dictionary<string, List<float>>();
                ArrayString = new Dictionary<string, List<string>>();
                ArrayBool = new Dictionary<string, List<bool>>();
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
                if (key == null)
                    return false;
                if (DataInt.ContainsKey(key))
                    return true;
                if (DataFloat.ContainsKey(key))
                    return true;
                if (DataBool.ContainsKey(key))
                    return true;
                if (DataString.ContainsKey(key))
                    return true;
                if (ArrayInt.ContainsKey(key))
                    return true;
                if (ArrayFloat.ContainsKey(key))
                    return true;
                if (ArrayBool.ContainsKey(key))
                    return true;
                if (ArrayString.ContainsKey(key))
                    return true;
                return false;
            }
            public bool Remove(IEnumerable<string> keys)
            {
                if(keys == null)
                    return true;
                foreach(var key in keys)
                {
                    try
                    {
                        DataInt.Remove(key);
                        DataFloat.Remove(key);
                        DataBool.Remove(key);
                        DataString.Remove(key);
                        ArrayInt.Remove(key);
                        ArrayFloat.Remove(key);
                        ArrayBool.Remove(key);
                        ArrayString.Remove(key);
                    }
                    catch (Exception) { continue; }
                }
                return false;
            }
            public bool Add(Data d, bool is_cover_same_key_value = false)
            {
                if (d == null)
                    return true;
                try
                {
                    foreach (var par in d.DataInt)
                    {
                        if (!DataInt.ContainsKey(par.Key))
                            DataInt.Add(par.Key, par.Value);
                        else if (is_cover_same_key_value)
                        {
                            DataInt.Remove(par.Key);
                            DataInt.Add(par.Key, par.Value);
                        }
                    }
                    foreach (var par in d.DataFloat)
                    {
                        if (!DataFloat.ContainsKey(par.Key))
                            DataFloat.Add(par.Key, par.Value);
                        else if (is_cover_same_key_value)
                        {
                            DataFloat.Remove(par.Key);
                            DataFloat.Add(par.Key, par.Value);
                        }
                    }
                    foreach (var par in d.DataBool)
                    {
                        if (!DataBool.ContainsKey(par.Key))
                            DataBool.Add(par.Key, par.Value);
                        else if (is_cover_same_key_value)
                        {
                            DataBool.Remove(par.Key);
                            DataBool.Add(par.Key, par.Value);
                        }
                    }
                    foreach (var par in d.DataString)
                    {
                        if (!DataString.ContainsKey(par.Key))
                            DataString.Add(par.Key, par.Value);
                        else if (is_cover_same_key_value)
                        {
                            DataString.Remove(par.Key);
                            DataString.Add(par.Key, par.Value);
                        }
                    }
                    foreach(var par in d.ArrayInt)
                    {
                        if (ArrayInt.ContainsKey(par.Key))
                        {
                            if (is_cover_same_key_value)
                                ArrayInt.Remove(par.Key);
                            else
                                continue;
                        }
                        ArrayInt.Add(par.Key, new List<int>(par.Value.Count));
                        foreach (int val in par.Value)
                            ArrayInt[par.Key].Add(val);
                    }
                    foreach(var par in d.ArrayFloat)
                    {
                        if (ArrayFloat.ContainsKey(par.Key))
                        {
                            if (is_cover_same_key_value)
                                ArrayFloat.Remove(par.Key);
                            else
                                continue;
                        }
                        ArrayFloat.Add(par.Key, new List<float>(par.Value.Count));
                        foreach (float val in par.Value)
                            ArrayFloat[par.Key].Add(val);
                    }
                    foreach(var par in d.ArrayString)
                    {
                        if (ArrayString.ContainsKey(par.Key))
                        {
                            if (is_cover_same_key_value)
                                ArrayString.Remove(par.Key);
                            else
                                continue;
                        }
                        ArrayString.Add(par.Key, new List<string>(par.Value.Count));
                        foreach (string val in par.Value)
                            ArrayString[par.Key].Add(val);
                    }
                    foreach(var par in d.ArrayBool)
                    {
                        if (ArrayBool.ContainsKey(par.Key))
                        {
                            if (is_cover_same_key_value)
                                ArrayBool.Remove(par.Key);
                            else
                                continue;
                        }
                        ArrayBool.Add(par.Key, new List<bool>(par.Value.Count));
                        foreach (bool val in par.Value)
                            ArrayBool[par.Key].Add(val);
                    }
                }
                catch (Exception) { return true; }
                return false;
            }
            public bool CopyToCDynamic(CDynamic c)
            {
                if (c == null || !c.IsUsable())
                    return true;
                try
                {
                    c.DataInt.Clear();
                    c.DataFloat.Clear();
                    c.DataBool.Clear();
                    c.DataString.Clear();
                    c.ArrayInt.Clear();
                    c.ArrayFloat.Clear();
                    c.ArrayBool.Clear();
                    c.ArrayString.Clear();
                    foreach (var par in DataInt)
                        c.DataInt.Add(par.Key, par.Value);
                    foreach (var par in DataString)
                        c.DataString.Add(par.Key, par.Value);
                    foreach (var par in DataFloat)
                        c.DataFloat.Add(par.Key, par.Value);
                    foreach (var par in DataBool)
                        c.DataBool.Add(par.Key, par.Value);
                    foreach (var par in ArrayInt)
                    {
                        c.ArrayInt.Add(par.Key, new List<int>(par.Value.Count));
                        foreach (int val in ArrayInt[par.Key])
                            c.ArrayInt[par.Key].Add(val);
                    }
                    foreach (var par in ArrayFloat)
                    {
                        c.ArrayFloat.Add(par.Key, new List<float>(par.Value.Count));
                        foreach (float val in ArrayFloat[par.Key])
                            c.ArrayFloat[par.Key].Add(val);
                    }
                    foreach (var par in ArrayString)
                    {
                        c.ArrayString.Add(par.Key, new List<string>(par.Value.Count));
                        foreach (string val in ArrayString[par.Key])
                            c.ArrayString[par.Key].Add(val);
                    }
                    foreach (var par in ArrayBool)
                    {
                        c.ArrayBool.Add(par.Key, new List<bool>(par.Value.Count));
                        foreach (bool val in ArrayBool[par.Key])
                            c.ArrayBool[par.Key].Add(val);
                    }
                }
                catch (Exception) { return true; }
                return false;
            }
            public CDynamic CopyToNewCDynamic(string cdynamic_type_name)
            {
                if (cdynamic_type_name == null)
                    return null;
                var csp = ConceptSpawner<CDynamic>.GetCDynamicSpawner(cdynamic_type_name);
                if (csp == null)
                    return null;
                var c = csp.Spawn();
                if (c == null)
                    return null;
                if (CopyToCDynamic(c) == true)
                    return null;
                return c;
            }
            public bool CopyFromCDynamic(CDynamic c)
            {
                if (c == null || !c.IsUsable())
                    return true;
                try
                {
                    Clear();
                    foreach (var par in c.DataInt)
                        DataInt.Add(par.Key, par.Value);
                    foreach (var par in c.DataString)
                        DataString.Add(par.Key, par.Value);
                    foreach (var par in c.DataFloat)
                        DataFloat.Add(par.Key, par.Value);
                    foreach (var par in c.DataBool)
                        DataBool.Add(par.Key, par.Value);
                    foreach (var par in c.ArrayInt)
                    {
                        ArrayInt.Add(par.Key, new List<int>(par.Value.Count));
                        foreach (int val in c.ArrayInt[par.Key])
                            ArrayInt[par.Key].Add(val);
                    }
                    foreach (var par in c.ArrayFloat)
                    {
                        ArrayFloat.Add(par.Key, new List<float>(par.Value.Count));
                        foreach (float val in c.ArrayFloat[par.Key])
                            ArrayFloat[par.Key].Add(val);
                    }
                    foreach (var par in c.ArrayString)
                    {
                        ArrayString.Add(par.Key, new List<string>(par.Value.Count));
                        foreach (string val in c.ArrayString[par.Key])
                            ArrayString[par.Key].Add(val);
                    }
                    foreach (var par in c.ArrayBool)
                    {
                        ArrayBool.Add(par.Key, new List<bool>(par.Value.Count));
                        foreach (bool val in c.ArrayBool[par.Key])
                            ArrayBool[par.Key].Add(val);
                    }
                }
                catch (Exception)
                {
                    return true;
                }
                return false;
            }
            public JObject ToJsonObject()
            {
                JObject json = new JObject();
                try
                {
                    if (DataInt.Count > 0)
                        json.Add("DataInt", JObject.FromObject(DataInt));
                    if (DataFloat.Count > 0)
                        json.Add("DataFloat", JObject.FromObject(DataFloat));
                    if (DataBool.Count > 0)
                        json.Add("DataBool", JObject.FromObject(DataBool));
                    if (DataString.Count > 0)
                        json.Add("DataString", JObject.FromObject(DataString));
                    if (ArrayInt.Count > 0)
                        json.Add("ArrayInt", JObject.FromObject(ArrayInt));
                    if (ArrayFloat.Count > 0)
                        json.Add("ArrayFloat", JObject.FromObject(ArrayFloat));
                    if (ArrayBool.Count > 0)
                        json.Add("ArrayBool", JObject.FromObject(ArrayBool));
                    if (ArrayString.Count > 0)
                        json.Add("ArrayString", JObject.FromObject(ArrayString));
                }
                catch (Exception)
                {
                    return null;
                }
                return json;
            }
            public bool FromJsonObject(JObject data)
            {
                if (data == null)
                    return true;
                try
                {
                    var jdi = data["DataInt"];
                    DataInt = (jdi == null) ? (new Dictionary<string, int>()) : (jdi.ToObject<Dictionary<string, int>>());
                    if (DataInt == null)
                        DataInt = new Dictionary<string, int>();
                    var jdf = data["DataFloat"];
                    DataFloat = (jdf == null) ? (new Dictionary<string, float>()) : (jdf.ToObject<Dictionary<string, float>>());
                    if (DataFloat == null)
                        DataFloat = new Dictionary<string, float>();
                    var jdb = data["DataBool"];
                    DataBool = (jdb == null) ? (new Dictionary<string, bool>()) : (jdb.ToObject<Dictionary<string, bool>>());
                    if (DataBool == null)
                        DataBool = new Dictionary<string, bool>();
                    var jds = data["DataString"];
                    DataString = (jds == null) ? (new Dictionary<string, string>()) : (jds.ToObject<Dictionary<string, string>>());
                    if (DataString == null)
                        DataString = new Dictionary<string, string>();
                    var jai = data["ArrayInt"];
                    ArrayInt = (jai == null) ? (new Dictionary<string, List<int>>()) : (jai.ToObject<Dictionary<string, List<int>>>());
                    if (ArrayInt == null)
                        ArrayInt = new Dictionary<string, List<int>>();
                    var jaf = data["ArrayFloat"];
                    ArrayFloat = (jaf == null) ? (new Dictionary<string, List<float>>()) : (jaf.ToObject<Dictionary<string, List<float>>>());
                    if (ArrayFloat == null)
                        ArrayFloat = new Dictionary<string, List<float>>();
                    var jab = data["ArrayBool"];
                    ArrayBool = (jab == null) ? (new Dictionary<string, List<bool>>()) : (jab.ToObject<Dictionary<string, List<bool>>>());
                    if (ArrayBool == null)
                        ArrayBool = new Dictionary<string, List<bool>>();
                    var jas = data["ArrayString"];
                    ArrayString = (jas == null) ? (new Dictionary<string, List<string>>()) : (jas.ToObject<Dictionary<string, List<string>>>());
                    if (ArrayString == null)
                        ArrayString = new Dictionary<string, List<string>>();
                }
                catch (Exception)
                {
                    return true;
                }
                return false;
            }
        }
        public JObject ToJsonObject()
        {
            JObject json = new JObject();
            try
            {
                if (Main != null)
                    json.Add("Main", Main);
                json.Add("Temp", Temp.ToJsonObject());
            }
            catch (Exception)
            {
                return null;
            }
            return json;
        }
        public bool FromJsonObject(JObject json)
        {
            try
            {
                if (json == null)
                    return true;
                Main = (string)json["Main"];
                return Temp.FromJsonObject((JObject)json["Temp"]);
            }
            catch (Exception) { return true; }
        }
        public static Data DoCommonScript(string name, Data input = null)
        {
            // do the script in core resource manager
            if (name == null)
                return null;
            string str;
            Script script = new Script();
            if(Core.ResourceManager.Scripts.TryGetValue(name, out str))
            {
                script.Main = (string)str.Clone();
                return Core.INeed.ExecuteScript(script, input);
            }
            return null;
        }
    }
}
