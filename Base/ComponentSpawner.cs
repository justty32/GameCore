
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    public interface IComponentSpawner {
        Component SpawnBase();
        int Type_Number { get; }
        string Type_Name { get; }
    }
    public class ComponentSpawner<TComponennt> : IComponentSpawner
        where TComponennt : Component, new()
    {
        /* To Spawn a Component by Component().
            * Every TypeNumber have a Spawner.
            *
            * Use Component.GetSpawner() to get the specific spawner.
            * It will return a existed one, or create a new one.
            * 
            * TypeNumber will be auto Distributed while creating a New Type Spawner.
            */
        public int Type_Number { get; } = -1;  // What type of Component to spawn
        public string Type_Name { get; set; } = null; // What type of Component to spawn
        private ComponentSpawner() {}
        private ComponentSpawner(int t_type_number) => Type_Number = t_type_number;
        public static ComponentSpawner<TComponennt> GetSpawner()
        {
            // Should only used by Component.
            var CI = Core.ComponentManager;
            TComponennt t = new TComponennt();
            if (t == null)
                return null;
            if (t.TypeName == null)
                return null;
            if (CI.SpawnerTypeNameSet == null)
                return null;
            if (CI.SpawnerList == null)
                return null;
            // if there already have one, return that
            // which is only judged by TypeName
            if (CI.SpawnerTypeNameSet.ContainsKey(t.TypeName))
                return (ComponentSpawner<TComponennt>)CI.SpawnerList[CI.SpawnerTypeNameSet[t.TypeName]];
            else
            {
                //create one, set type_number, set type_name
                var spawner = new ComponentSpawner<TComponennt>(CI.SpawnerList.Count);
                spawner.Type_Name = t.TypeName;
                //add list
                CI.SpawnerTypeNameSet.Add(t.TypeName, CI.SpawnerList.Count);
                CI.SpawnerList.Add(spawner.Type_Number, spawner);
                return spawner;
            }
        }
        public TComponennt Spawn()
        {
            /*Return by default constructer, Component().*/
            TComponennt component = new TComponennt();
            component.TypeNumber = Type_Number;
            return component;
        }
        public Component SpawnBase()
        {
            // Same as Spawn(), but return base reference
            return Spawn();
        }
    }
}
