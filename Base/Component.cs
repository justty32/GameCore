using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    public abstract class Component
    {
        /*
         * Every Derives should implement TypeName{get;}, Which is the basis to distinguish type of it
         * TypeNumber is also auto-distributed by TypeName, one associated to one.
         * 
         * To create an entity, use GetSpawner<>().Spawn() first, instead of GetSpawner().Spawn() or default constructor.
         * Only if after it, the TypeNumber of the component-type is effective.
         */
        public int TypeNumber { get; set; } = -1; // Which is auto distributed by GameCore, Don't set it Directly ! 
        public abstract string TypeName { get; }
        public List<ComponentSet> Owners { get; } = new List<ComponentSet>();
        private static SortedList<string, int> _spawnerTypeNameSet = new SortedList<string, int>();
        private static SortedList<int, ISpawner> _spawnerList = new SortedList<int, ISpawner>();
        public static int TypeCount => _spawnerList.Count; //return how many types of component.
        public int TypeNumberAutoSet()
        {
            /*
             * Set TypeNumber by TypeName, then return TypeNumber.
             * If there isn't have specific spawner yet, TypeNumber not change.
             * 
             * type_number setting while spawner be create.
             * use Component.GetSpawner<Type>() to create spawner
             */
            if (_spawnerTypeNameSet.ContainsKey(TypeName))
                TypeNumber = _spawnerTypeNameSet[TypeName];
            return TypeNumber;
        }
        public static string GetTypeName(int type_number) {
            if(_spawnerList.ContainsKey(type_number))
                return _spawnerList[type_number]?.Type_Name;
            return null;
        }
        public static int GetTypeNumber(string type_name) {
            // return -1, if not find that.
            if(_spawnerTypeNameSet.ContainsKey(type_name))
                return _spawnerTypeNameSet[type_name];
            return -1;
        }
        public Component() {
            /*
             * default create, not recommend. please use Component.GetSpawner().Spawn().
             * Should use TypeNumberAutoSet() After, to set the TypeNumber, or still be -1.
             * 
             * If there isn't have specific spawner yet, TypeNumber not change.
             * TypeNumber setting while spawner be create.
             * use Component.GetSpawner<Type>() to create spawner
             */
        }
        public interface ISpawner {
            Component SpawnBaseComponent();
            int Type_Number { get; }
            string Type_Name { get; }
        }
        public class Spawner<TComponennt> : ISpawner
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
            private Spawner() { }
            private Spawner(int t_type_number) => Type_Number = t_type_number;
            internal static Spawner<TComponennt> _GetSpawner()
            {
                // Should only used by Component.
                TComponennt t = new TComponennt();
                if (t == null)
                    return null;
                if (t.TypeName == null)
                    return null;
                // if there already have one, return that
                // which is only judged by TypeName
                if (_spawnerTypeNameSet.ContainsKey(t.TypeName))
                    return (Spawner<TComponennt>)_spawnerList[_spawnerTypeNameSet[t.TypeName]];
                else
                {
                    //create one, set type_number, set type_name
                    var spawner = new Spawner<TComponennt>(_spawnerList.Count);
                    spawner.Type_Name = t.TypeName;
                    //add list
                    _spawnerTypeNameSet.Add(t.TypeName, _spawnerList.Count);
                    _spawnerList.Add(spawner.Type_Number, spawner);
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
            public Component SpawnBaseComponent()
            {
                return Spawn();
            }
        }
        public static Spawner<TComponennt> GetSpawner<TComponennt>()
            where TComponennt : Component, new()
        {
            /*
             * Return a spawner, which can spawn specific type of components.
             * Every Type has a only one spawner.
             */
            return Spawner<TComponennt>._GetSpawner();
        }
        public static ISpawner GetSpawner(int type_number)
        {
            /*
             * Do Transformate by yourself.
             * It could return null.
             * 
             * Return a spawner, which can spawn specific type of components.
             * Every Type has a only one spawner.
             */
            if (type_number < 0 || type_number >= _spawnerList.Count)
                return null;
            return _spawnerList[type_number];
        }
        public static ISpawner GetSpawner(string type_name)
        {
            /*
             * Do Transformate by yourself.
             * It could return null.
             * 
             * Return a spawner, which can spawn specific type of components.
             * Every Type has a only one spawner.
             */
            if (!_spawnerTypeNameSet.ContainsKey(type_name))
                return null;
            return _spawnerList[_spawnerTypeNameSet[type_name]];
        }
    }
}

