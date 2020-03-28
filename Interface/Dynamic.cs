using GameCore.Base;
using System.Collections.Generic;

//dynamic concept and dynamic rule
//dynamic rule composed by scripts with hooks

namespace GameCore.Interface
{
    public class Dynamic
    {
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
            //replace cdynamic names list into new one
            if (dynamic_concept_names == null)
                return true;
            CDynamicNames = new List<string>(dynamic_concept_names);
            CDynamicSpawners = new Dictionary<string, ConceptSpawner<CDynamic>>(CDynamicNames.Count);
            for (int i = 0; i < CDynamicNames.Count; i++)
            {
                var sp = ConceptSpawner<CDynamic>.GetCDynamicSpawner(CDynamicNames[i]);
            }
            return false;
        }
    }
}