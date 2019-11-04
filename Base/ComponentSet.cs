using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    public class ComponentSet
    {
        /*
         * Restore Components, Do Not allow multiple entity of same Component Type
         *
         * dst_num means TypeNumber
         *
         */
        private SortedList<int, Component> components;
        public List<Card> Owners { get; } = new List<Card>();
        public ComponentSet() => components = new SortedList<int, Component>();
        public ComponentSet(int components_count)
        {
            components_count = components_count < 0 ? 100 : components_count;
            components = new SortedList<int, Component>(components_count);
        }
        public ComponentSet(int[] add_new_components)
        {
            components = new SortedList<int, Component>(add_new_components.Length);
            if(add_new_components != null)
            for(int i = 0; i < add_new_components.Length; i++)
            {
                if (add_new_components[i] >= 0)
                {
                    var c = Component.GetSpawner(add_new_components[i])?.SpawnBaseComponent();
                    if (c != null)
                        components.Add(add_new_components[i], c);
                }
            }
        }
        public ComponentSet(Component[] add_existing_components)
        {
            components = new SortedList<int, Component>(add_existing_components.Length);
            Add(add_existing_components);
        }
        public IList<int> GetComponentTypeNumbers()
        {
            //return what types of component it have
            return components.Keys;
        }
        public bool Has(int dst_num)
        {
            if (components.ContainsKey(dst_num))
            {
                return true;
            }
            return false;
        }
        public bool Has(int[] dst_nums)
        {
            if (dst_nums == null)
                return false;
            foreach(int dst_num in dst_nums)
            {
                if (!components.ContainsKey(dst_num))
                    return false;
            }
            return true;
        }
        public bool Has(string type_name){
            if (components.ContainsKey(Component.GetTypeNumber(type_name)))
            {
                return true;
            }
            return false;
        }
        public bool Has(string[] type_names)
        {
            if (type_names == null)
                return false;
            foreach(string type_name in type_names)
            {
                if (!components.ContainsKey(Component.GetTypeNumber(type_name)))
                    return false;
            }
            return true;
        }
        public Component Get(int dst_num)
        {
            foreach (var node in components)
            {
                if (node.Key == dst_num)
                    return node.Value;
            }
            return null;
        }
        public Component Get(string type_name){
            foreach (var node in components)
            {
                if (node.Value.TypeName.Equals(type_name))
                    return node.Value;
            }
            return null;
        }
        public Component this[int i]
        {
            /*
             * get -> just Get(i)
             * 
             * set -> Replace() and Add(), judge by Has(i)
             */
            get => Get(i);
            set
            {
                if (Has(i))
                    Replace(value);
                else
                    Add(value);
            }
        }
        public Component this[string str]
        {
            /*
             * get -> just Get(i)
             * 
             * set -> Replace() and Add(), judge by Has(i)
             */
            get => Get(str);
            set
            {
                if (Has(str))
                    Replace(value);
                else
                    Add(value);
            }
        }
        public Component[] Get(int[] dst_nums)
        {
            if (dst_nums == null)
                return null;
            Component[] cs = new Component[dst_nums.Length];
            for(int i = 0; i < dst_nums.Length; i++)
            {
                foreach (var node in components)
                {
                    if (node.Key == dst_nums[i])
                        cs[i] = node.Value;
                }
            }
            return cs;
        }
        public IList<Component> Components => components.Values;
        public bool Add(Component thing)
        {
            // If there is already have same-type one, Do nothing, return true.
            // If TypeNumber is -1, Do nothing, return true.
            if (thing == null)
                return true;
            if (Has(thing.TypeNumber))
                return true;
            else
            {
                components.Add(thing.TypeNumber, thing);
                thing.Owners.Add(this);
                return false;
            }
        }
        public void Add(Component[] things)
        {
            // Components of array should be multi type.
            // If there is already have same-type one, Do nothing.
            // If TypeNumber is -1, Do nothing.
            if (things != null)
            {
                foreach (Component thing in things)
                {
                    if (!Has(thing.TypeNumber))
                    {
                        components.Add(thing.TypeNumber, thing);
                        thing.Owners.Add(this);
                    }
                }
            }
        }
        public void Remove(int dst_num)
        {
            if (Has(dst_num))
            {
                components[dst_num].Owners.Remove(this);
                components.Remove(dst_num);
            }
        }
        public void Remove(int[] dst_nums)
        {
            if(dst_nums != null)
            foreach (int dst_num in dst_nums)
            {
            if (Has(dst_num))
                {
                    components[dst_num].Owners.Remove(this);
                    components.Remove(dst_num);
                }
            }
        }
        public void Replace(Component thing)
        {
            /* 
             * It only do Replace, not Add.
             * If the parameter is null, Do Nothing.
             * If there isn't have one yet, Do Nothing.
             */
            if(thing != null)
            if (Has(thing.TypeNumber))
            {
                components.Remove(thing.TypeNumber);
                components.Add(thing.TypeNumber, thing);
            }
        }
        public void Replace(Component[] things)
        {
            /* 
             * It only do Replace, not Add.
             * If the array is null, Do Nothing.
             * If there isn't have one yet, Do Nothing.
             * If one of the array is null, Not Replace that one.
             */
            if (things != null)
            for (int i = 0; i< things.Length; i++)
            {
                if (Has(things[i].TypeNumber))
                {
                    components.Remove(things[i].TypeNumber);
                    components.Add(things[i].TypeNumber, things[i]);
                }
            }
        }
    }
}
