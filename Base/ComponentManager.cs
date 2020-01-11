using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    public class ComponentManager{
        public Dictionary<string, int> SpawnerTypeNameSet{get; private set;}
        public Dictionary<int, IComponentSpawner> SpawnerList{get; private set;}
        public ComponentManager(){
            SpawnerTypeNameSet = new Dictionary<string, int>();
            SpawnerList = new Dictionary<int, IComponentSpawner>();
        }
        public static int GetTypesCount() => Core.ComponentManager.SpawnerList.Count; //return how many types of component.
        public static string GetTypeName(int type_number) {
            if(Core.ComponentManager.SpawnerList.ContainsKey(type_number))
                return Core.ComponentManager.SpawnerList[type_number]?.Type_Name;
            return null;
        }
        public static int GetTypeNumber(string type_name) {
            // return -1, if not find that.
            if(Core.ComponentManager.SpawnerTypeNameSet.ContainsKey(type_name))
                return Core.ComponentManager.SpawnerTypeNameSet[type_name];
            return -1;
        }
        public static int GetTypeNumber<TComponentType>()
            where TComponentType : Component, new()
        {
            return GetSpawner<TComponentType>().Type_Number;
        }
        public static ComponentSpawner<TComponennt> GetSpawner<TComponennt>()
            where TComponennt : Component, new()
        {
            /*
             * Return a spawner, which can spawn specific type of components.
             * Every Type has a only one spawner.
             */
            return ComponentSpawner<TComponennt>.GetSpawner();
        }
        public static IComponentSpawner GetSpawner(int type_number)
        {
            /*
             * Do Transformate by yourself.
             * It could return null.
             * 
             * Return a spawner, which can spawn specific type of components.
             * Every Type has a only one spawner.
             */
            if (type_number < 0 || type_number >= Core.ComponentManager.SpawnerList.Count)
                return null;
            return Core.ComponentManager.SpawnerList[type_number];
        }
        public static IComponentSpawner GetSpawner(string type_name)
        {
            /*
             * Do Transformate by yourself.
             * It could return null.
             * 
             * Return a spawner, which can spawn specific type of components.
             * Every Type has a only one spawner.
             */
            if (!Core.ComponentManager.SpawnerTypeNameSet.ContainsKey(type_name))
                return null;
            return Core.ComponentManager.SpawnerList[Core.ComponentManager.SpawnerTypeNameSet[type_name]];
        }
    }
}