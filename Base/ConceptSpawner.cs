using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    public interface IConceptSpawner
    {
        Concept SpawnBase();

        int TypeNumber { get; }
        string TypeName { get; }
    }

    public class ConceptSpawner<TComponennt> : IConceptSpawner
        where TComponennt : Concept, new()
    {
        /* To Spawn a Concept by Concept().
            * Every TypeNumber have a Spawner.
            *
            * Use Concept.GetSpawner() to get the specific spawner.
            * It will return a existed one, or create a new one.
            *
            * TypeNumber will be auto Distributed while creating a New Type Spawner.
            */
        public int TypeNumber { get; } = -1;  // What type of Concept to spawn
        public string TypeName { get; set; } = null; // What type of Concept to spawn

        private ConceptSpawner()
        {
        }

        private ConceptSpawner(int t_type_number) => TypeNumber = t_type_number;

        public static ConceptSpawner<TComponennt> GetSpawner()
        {
            // Should only used by Concept.
            var CM = Core.ConceptManager;
            TComponennt t = new TComponennt();
            if (t == null)
                return null;
            if (t.TypeName == null)
                return null;
            if (CM.SpawnerTypeNameSet == null)
                return null;
            if (CM.SpawnerList == null)
                return null;
            // if there already have one, return that
            // which is only judged by TypeName
            if (CM.SpawnerTypeNameSet.ContainsKey(t.TypeName))
                return (ConceptSpawner<TComponennt>)CM.SpawnerList[CM.SpawnerTypeNameSet[t.TypeName]];
            else
            {
                //create one, set type_number, set type_name
                var spawner = new ConceptSpawner<TComponennt>(CM.SpawnerList.Count);
                spawner.TypeName = t.TypeName;
                //add list
                CM.SpawnerTypeNameSet.Add(t.TypeName, CM.SpawnerList.Count);
                CM.SpawnerList.Add(spawner.TypeNumber, spawner);
                return spawner;
            }
        }

        public static ConceptSpawner<Base.CDynamic> GetCDynamicSpawner(string type_name)
        {
            var CM = Core.ConceptManager;
            if (type_name == null)
                return null;
            if (CM.SpawnerTypeNameSet == null)
                return null;
            if (CM.SpawnerList == null)
                return null;
            // if there already have one, return that
            // which is only judged by TypeName
            if (CM.SpawnerTypeNameSet.ContainsKey(type_name))
                return (ConceptSpawner<Base.CDynamic>)CM.SpawnerList[CM.SpawnerTypeNameSet[type_name]];
            else
            {
                //create one, set type_number, set type_name
                var spawner = new ConceptSpawner<Base.CDynamic>(CM.SpawnerList.Count);
                spawner.TypeName = type_name;
                //add list
                CM.SpawnerTypeNameSet.Add(type_name, CM.SpawnerList.Count);
                CM.SpawnerList.Add(spawner.TypeNumber, spawner);
                // if dynamic not have it yet, add it
                if (!Core.Dynamic.CDynamicSpawners.ContainsKey(type_name))
                    Core.Dynamic.CDynamicSpawners.Add(type_name, spawner);
                if (!Core.Dynamic.CDynamicNames.Contains(type_name))
                    Core.Dynamic.CDynamicNames.Add(type_name);
                return spawner;
            }
        }

        public TComponennt Spawn()
        {
            /*Return by default constructer, Concept().*/
            TComponennt concept = new TComponennt();
            concept.TypeNumber = TypeNumber;
            return concept;
        }

        public Concept SpawnBase()
        {
            // Same as Spawn(), but return base reference
            return Spawn();
        }

        public static TComponennt SpawnFromJsonObject(JObject json)
        {
            if (json == null)
                return null;
            var sp = GetSpawner();
            if (sp == null)
                return null;
            var c = sp.Spawn();
            if (c == null)
                return null;
            return (c.FromJsonObject(json)) as TComponennt;
        }
    }
}