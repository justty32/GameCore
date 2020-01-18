using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    public class ConceptManager{
        public Dictionary<string, int> SpawnerTypeNameSet{get; private set;}
        public Dictionary<int, IConceptSpawner> SpawnerList{get; private set;}
        public ConceptManager(){
            SpawnerTypeNameSet = new Dictionary<string, int>();
            SpawnerList = new Dictionary<int, IConceptSpawner>();
        }
        public static bool ContainsTypeName(string type_name) { if (type_name == null) { return false; } return Core.ConceptManager.SpawnerTypeNameSet.ContainsKey(type_name); }
        public static int GetTypesCount() => Core.ConceptManager.SpawnerList.Count; //return how many types of concept.
        public static string GetTypeName(int type_number) {
            if(Core.ConceptManager.SpawnerList.ContainsKey(type_number))
                return Core.ConceptManager.SpawnerList[type_number]?.TypeName;
            return null;
        }
        public static int GetTypeNumber(string type_name) {
            // return -1, if not find that.
            if(Core.ConceptManager.SpawnerTypeNameSet.ContainsKey(type_name))
                return Core.ConceptManager.SpawnerTypeNameSet[type_name];
            return -1;
        }
        public static int GetTypeNumber<TConceptType>()
            where TConceptType : Concept, new()
        {
            return GetSpawner<TConceptType>().TypeNumber;
        }
        public static ConceptSpawner<TComponennt> GetSpawner<TComponennt>()
            where TComponennt : Concept, new()
        {
            /*
             * Return a spawner, which can spawn specific type of concepts.
             * Every Type has a only one spawner.
             */
            return ConceptSpawner<TComponennt>.GetSpawner();
        }
        public static IConceptSpawner GetSpawner(int type_number)
        {
            /*
             * Do Transformate by yourself.
             * It could return null.
             * 
             * Return a spawner, which can spawn specific type of concepts.
             * Every Type has a only one spawner.
             */
            if (type_number < 0 || type_number >= Core.ConceptManager.SpawnerList.Count)
                return null;
            return Core.ConceptManager.SpawnerList[type_number];
        }
        public static IConceptSpawner GetSpawner(string type_name)
        {
            /*
             * Do Transformate by yourself.
             * It could return null.
             * 
             * Return a spawner, which can spawn specific type of concepts.
             * Every Type has a only one spawner.
             */
            if (!Core.ConceptManager.SpawnerTypeNameSet.ContainsKey(type_name))
                return null;
            return Core.ConceptManager.SpawnerList[Core.ConceptManager.SpawnerTypeNameSet[type_name]];
        }
    }
}