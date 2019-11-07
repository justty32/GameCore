using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    public class ComponentList
    {
        /*
         * Different type components can be same name
         * Allow same type entities, but need to name it.
         * 
         * While doing base controlling (Has, Get, Add, Remove), should care about parameters.
         * if don't put name, that's means to do it for all things of the type.
         * if put array, that's means do several times for things in array
         * 
         * Replace is same as Add(), but do remove while there's already one.
         */
        private SortedList<int, SortedList<string, Component>> components;
        public ComponentList()
        {
            components = new SortedList<int, SortedList<string, Component>>();
        }
        public ComponentList(ComponentSet component_set, SortedList<int, string> type_specific_name = null)
        {
            // If don't specific the name, set it defaultly "_"
            if (component_set != null)
            if (type_specific_name == null)
            {
                foreach (var thing in component_set.Components)
                    if (thing != null)
                        Add("_", thing);
            }
            else
            {
                foreach (var thing in component_set.Components)
                {
                    if (thing != null)
                    if (type_specific_name.ContainsKey(thing.TypeNumber))
                        Add(type_specific_name[thing.TypeNumber], thing);
                    else
                        Add("_", thing);
                }
            }
        }
        public int CountOfTypes => components.Count;
        public int CountOfAll {
            get
            {
                int sum = 0;
                foreach (var things in components)
                    sum += things.Value.Count;
                return sum;
            }
        }
        public int GetCountOfType(int type_number) => components[type_number].Count;
        public IList<int> GetComponentTypeNumbers()
        {
            return components.Keys;
        }
        public bool Has(int type_number, string name)
        {
            if (Has(type_number))
                return components[type_number].ContainsKey(name);
            else
                return false;
        }
        public bool Has(int type_number)
        {
            return components.ContainsKey(type_number);    
        }
        public int GetComponentCount(int type_number)
        {
            return components[type_number].Count;
        }
        public Component Get(int type_number, string name)
        {
            if (Has(type_number, name))
                return components[type_number][name];
            else
                return null;
        }
        public SortedList<string, Component> Get(int type_number)
        {
            if (components.ContainsKey(type_number))
                return components[type_number];
            else
                return null;
        }
        public void Add(string name, Component thing)
        {
            if(thing != null)
            if (Has(thing.TypeNumber))
            {
                // if already have component(s) of that type
                components[thing.TypeNumber].Add(name, thing);
            }
            else
            {
                // if not yet have that type
                components.Add(thing.TypeNumber, new SortedList<string, Component>());
                components[thing.TypeNumber].Add(name, thing);
            }
        }
        public void Add(SortedList<string, Component> things)
        {
            foreach(var thing in things)
            {
                Add(thing.Key, thing.Value);
            }
        }
        public void Remove(int type_number, string name)
        {
            if (Has(type_number, name))
                components[type_number].Remove(name);
        }
        public void Remove(int type_number)
        {
            // Remove all components of that type
            if (Has(type_number))
                components.Remove(type_number);
        }
        public void Replace(string name, Component component)
        {
            // Same as Add(), but remove the target while there is
            if (component != null)
            {
                if (Has(component.TypeNumber, name))
                    Remove(component.TypeNumber, name);
                Add(name, component);
            }
        }
        public Component this[int type_number, string name]
        {
            get
            {
                return Get(type_number, name);
            }
            set
            {
                if(value != null)
                if(type_number == value.TypeNumber)
                    Replace(name, value);
            }
        }
        public string GetName(Component component)
        {
            if (component != null) 
            if (Has(component.TypeNumber))
            if (components[component.TypeNumber].ContainsValue(component))
            {
                foreach(var thing in components[component.TypeNumber])
                {
                    if (thing.Value == component)
                        return thing.Key;
                }
            }
            return null;
        }
    }
}
