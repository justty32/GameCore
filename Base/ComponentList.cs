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
         * if only put type_number, that means to do it for all things of the type.
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
            // If don't specific the name, set it as its TypeName
            if (component_set != null)
            if (type_specific_name == null)
            {
                foreach (var thing in component_set.GetAllComponents())
                    if (thing != null)
                        Add(thing.TypeName, thing);
            }
            else
            {
                foreach (var thing in component_set.GetAllComponents())
                {
                    if (thing != null)
                    if (type_specific_name.ContainsKey(thing.TypeNumber))
                        Add(type_specific_name[thing.TypeNumber], thing);
                    else
                        Add(thing.TypeName, thing);
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
            // get how many types in list.
            return components.Keys;
        }
        public bool Has(string name)
        {
            if(name != null)
            foreach(var sort_list in components)
            {
                if (sort_list.Value.ContainsKey(name))
                    return true;
            }
            return false;
        }
        public bool Has(int type_number)
        {
            return components.ContainsKey(type_number);    
        }
        public Component Get(string name)
        {
            if(name != null)
            if (Has(name))
            {
                foreach(var sort_list in components)
                {
                    if (sort_list.Value.ContainsKey(name))
                        return sort_list.Value[name];
                }
            }
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
            if(name != null && thing != null)
            if (!Has(thing.TypeNumber))
            {
                if(!components.ContainsKey(thing.TypeNumber))
                    components.Add(thing.TypeNumber, new SortedList<string, Component>());
                components[thing.TypeNumber].Add(name, thing);
                thing.Belong = this;
            }
        }
        public void Add(SortedList<string, Component> things)
        {
            if(things != null)
            foreach(var thing in things)
            {
                Add(thing.Key, thing.Value);
                thing.Value.Belong = this;
            }
        }
        public void Remove(string name)
        {
            if(name != null)
                foreach(var sort_list in components)
                {
                    if (sort_list.Value.ContainsKey(name))
                    {
                        sort_list.Value[name].Belong = null;
                        sort_list.Value.Remove(name);
                        break;
                    }
                }
        }
        public void Remove(int type_number)
        {
            // Remove all components of that type
            if (Has(type_number))
            {
                foreach(var thing in components[type_number])
                {
                    thing.Value.Belong = null;
                    components[type_number].Remove(thing.Key);
                }
                components.Remove(type_number);
            }
        }
        public void Replace(string name, Component component)
        {
            // Same as Add(), but remove the target while there is
            if (name != null && component != null)
            {
                if (Has(name))
                    Remove(name);
                Add(name, component);
            }
        }
        public Component this[string name]
        {
            get
            {
                return Get(name);
            }
            set
            {
                if (value != null)
                    Replace(name, value);
            }
        }
        public SortedList<string, Component> this[int type_number]
        {
            get { return Get(type_number); }
            set
            {
                if (Has(type_number))
                    Remove(type_number);
                Add(value);
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
