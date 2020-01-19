using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using Newtonsoft.Json.Linq;

//dynamic concept and dynamic rule
//dynamic rule composed by scripts with hooks
// TODO: add dynamic rules' init process at rule manager's init()

namespace GameCore.Interface
{
    public class Dynamic { 
        public List<string> CDynamicNames;
        public Dictionary<string, ConceptSpawner<CDynamic>> CDynamicSpawners;
        public bool RegisterCDynamic(string name)
        {
            var sp = ConceptManager.GetCDynamicSpawner(name);
            if (sp == null)
                return true;
            return false;
        }
        public bool SetCDynamicNames(List<string> dynamic_concept_names)
        {
            if (dynamic_concept_names == null)
                return true;
            CDynamicNames = new List<string>(dynamic_concept_names);
            CDynamicSpawners = new Dictionary<string, ConceptSpawner<CDynamic>>(CDynamicNames.Count);
            for(int i = 0; i < CDynamicNames.Count; i++)
            {
                var sp = ConceptSpawner<CDynamic>.GetCDynamicSpawner(CDynamicNames[i]);
            }
            return false;
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
}
