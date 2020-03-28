using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;

namespace GameCore.Base
{
    public abstract class Concept : Util.INode
    {
        /*
         * Every Derives should implement TypeName{get;}, Which is the basis to distinguish type of it
         * TypeNumber is also auto-distributed by TypeName, one associated to one.
         *
         * To create an entity, use GetSpawner<>().Spawn() first, instead of default constructor.
         * Only if after it, the TypeNumber of the concept-type is effective.
         */
        public int TypeNumber { get; set; } = -1; // Which is auto distributed by GameCore, Don't set it Directly !
        public abstract string TypeName { get; }
        public virtual JObject ToJsonObject()
        {
            JObject js = null;
            try
            {
                js = new JObject(new JProperty("TypeName", TypeName));
                if (js == null)
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
            return js;
        }
        public virtual Concept FromJsonObject(JObject js)
        {
            if (js == null)
                return null;
            try
            {
                if (!((string)js["TypeName"]).Equals(TypeName))
                    return null;
            }
            catch (Exception e)
            {
                return Core.State.WriteException<Concept>(e);
            }
            return ConceptManager.GetSpawner(TypeName).SpawnBase();
        }
        public static JObject AlignJsonOjbect(JObject js)
        {
            if (js == null)
                return null;
            try
            {
                // check is legal
                if (!Util.JObjectContainsKey(js, "TypeName"))
                    return null;
                // remove redundantion
                if (Util.JObjectContainsKey(js, "Card"))
                    js.Remove("Card");
                // reset type number
                int tn = ConceptManager.GetTypeNumber((string)js["TypeName"]);
                if (tn < 0)
                    return null;
                if (Util.JObjectContainsKey(js, "TypeNumber"))
                    js.Remove("TypeNumber");
                js.Add("TypeNumber", tn);
            }
            catch (Exception e)
            {
                return Core.State.WriteException<JObject>(e);
            }
            return js;
        }
        public virtual Concept Copy()
        {
            Concept c = null;
            try
            {
                var j = ToJsonObject();
                var jj = AlignJsonOjbect(j);
                c = FromJsonObject(jj);
            }catch(Exception)
            {
                return null;
            }
            return c;
        }
        public Card Card { get; set; } = null;
        public static TConcept Spawn<TConcept>()
            where TConcept : Concept, new()
        {
            return ConceptSpawner<TConcept>.GetSpawner().Spawn();
        }
        public virtual bool IsUsable()
        {
            if (TypeNumber >= 0)
                return true;
            return false;
        }
        public int AutoSetTypeNumber()
        {
            /*
             * Set TypeNumber by TypeName, then return TypeNumber.
             * If there isn't have specific spawner yet, TypeNumber not change.
             *
             * type_number setting while spawner be create.
             * use Concept.GetSpawner<Type>() to create spawner
             */
            if (Core.ConceptManager.SpawnerTypeNameSet.ContainsKey(TypeName))
                TypeNumber = Core.ConceptManager.SpawnerTypeNameSet[TypeName];
            return TypeNumber;
        }
        protected Concept()
        {
            /*
             * default create, not recommend. please use Concept.GetSpawner().Spawn().
             * Should use TypeNumberAutoSet() After, to set the TypeNumber, or still be -1.
             *
             * If there isn't have specific spawner yet, TypeNumber not change.
             * TypeNumber setting while spawner be create.
             * use Concept.GetSpawner<Type>() to create spawner
             */
        }
    }
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
        public override Concept Copy()
        {
            if (!IsUsable())
                return null;
            var csp = ConceptSpawner<CDynamic>.GetCDynamicSpawner(TypeName);
            if (csp == null)
                return null;
            var c = csp.Spawn();
            if (c == null)
                return null;
            try
            {
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
            catch (Exception) { return null; }
            return c;
        }
        public override Concept FromJsonObject(JObject js)
        {
            // which will set type name, number
            // return null while the type name not in Dynamic.CDynamicNameSet
            if (js == null)
                return null;
            try
            {
                if (!Util.JObjectContainsKey(js, "TypeName"))
                    return null;
                if (!Core.Dynamic.CDynamicNames.Contains((string)js["TypeName"]))
                    return null;
                if (Util.JObjectContainsKey(js, "Card"))
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
    public class ConceptManager
    {
        public Dictionary<string, int> SpawnerTypeNameSet { get; private set; }
        public Dictionary<int, IConceptSpawner> SpawnerList { get; private set; }
        public ConceptManager()
        {
            SpawnerTypeNameSet = new Dictionary<string, int>();
            SpawnerList = new Dictionary<int, IConceptSpawner>();
        }
        public static bool ContainsTypeName(string type_name)
        {
            if (type_name == null) { return false; }
            return Core.ConceptManager.SpawnerTypeNameSet.ContainsKey(type_name);
        }
        public static int GetTypesCount() => Core.ConceptManager.SpawnerList.Count; //return how many types of concept.
        public static string GetTypeName(int type_number)
        {
            if (Core.ConceptManager.SpawnerList.ContainsKey(type_number))
                return Core.ConceptManager.SpawnerList[type_number]?.TypeName;
            return null;
        }
        public static int GetTypeNumber(string type_name)
        {
            // return -1, if not find that.
            if (Core.ConceptManager.SpawnerTypeNameSet.ContainsKey(type_name))
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
        public static ConceptSpawner<CDynamic> GetCDynamicSpawner(string name)
        {
            return ConceptSpawner<CDynamic>.GetCDynamicSpawner(name);
        }
        public TComponennt SpawnFromJsonObject<TComponennt>(JObject json)
            where TComponennt : Concept, new()
        {
            return ConceptSpawner<TComponennt>.SpawnFromJsonObject(json);
        }
    }
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