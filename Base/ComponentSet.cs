using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    public class ComponentSet
    {
        /*
         * Not allow more than one entity of same type
         * 
         * While doing base controlling (Has, Get, Add, Remove), should care about parameters.
         * if don't put name, that's means to do it for all things of the type.
         * if put array, that's means do several times for things in array
         * 
         * Replace is same as Add(), but do remove while there's already one.
         */
        private SortedList<int, Component> components;
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
                    var c = Component.GetSpawner(add_new_components[i])?.SpawnBase();
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
        public ComponentSet(ComponentList component_list, SortedList<int, string> type_choose_name)
        {
            if(component_list != null)
            { // if list not null
                if (type_choose_name != null)
                { // if the type has specific-name thing to add.
                    foreach (int type_number in component_list.GetComponentTypeNumbers())
                    { // for all type's
                        if (type_choose_name.ContainsKey(type_number)
                            && type_choose_name[type_number] != null)
                        { // if there is a specific name to type_number
                            if (component_list.Get(type_number).ContainsKey(type_choose_name[type_number]))
                            { // and there is containing the thing with specific name in list
                                Add(component_list.Get(type_number)[type_choose_name[type_number]]);
                            }
                            else
                            { // there is no thing with specific name in list
                                if (component_list.Get(type_number).Values.Count > 0)
                                { //add it with index 0's thing
                                    Add(component_list.Get(type_number).Values[0]);
                                }
                            }
                        }
                        else
                        { // if type-specific-name list not have specific name
                            if (component_list.Get(type_number).Values.Count > 0)
                            {// if there has thing
                                Add(component_list.Get(type_number).Values[0]);
                            }
                        }
                    }
                }
                else
                {// if the type thing don't has specific-name
                    foreach (int type_number in component_list.GetComponentTypeNumbers())
                    { // for all types, add its index 0 's thing.
                        if (component_list.Get(type_number).Values.Count > 0)
                        { // if there has thing
                            Add(component_list.Get(type_number).Values[0]);
                        }
                    }
                }
            }
        }
        public int Count => components.Count;
        public IList<int> GetComponentTypeNumbers()
        {
            //return what types of component it have
            return components.Keys;
        }
        public bool Has(int type_number)
        {
            if (components.ContainsKey(type_number))
            {
                return true;
            }
            return false;
        }
        public bool Has(int[] type_numbers)
        {
            if (type_numbers == null)
                return false;
            foreach(int type_number in type_numbers)
            {
                if (!components.ContainsKey(type_number))
                    return false;
            }
            return true;
        }
        public bool Has(IList<int> type_numbers)
        {
            if (type_numbers == null)
                return false;
            foreach (int type_number in type_numbers)
            {
                if (!components.ContainsKey(type_number))
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
        public Component Get(int type_number)
        {
            foreach (var node in components)
            {
                if (node.Key == type_number)
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
        public List<Component> Get(int[] type_numbers)
        {
            if (type_numbers == null)
                return null;
            List<Component> cs = new List<Component>(type_numbers.Length);
            for(int i = 0; i < type_numbers.Length; i++)
            {
                foreach (var node in components)
                {
                    if (node.Key == type_numbers[i])
                        cs.Add(node.Value);
                }
            }
            return cs;
        }
        public List<Component> Get(IList<int> type_numbers)
        {
            if (type_numbers == null)
                return null;
            List<Component> cs = new List<Component>(type_numbers.Count);
            for (int i = 0; i < type_numbers.Count; i++)
            {
                foreach (var node in components)
                {
                    if (node.Key == type_numbers[i])
                        cs.Add(node.Value);
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
                    }
                }
            }
        }
        public void Remove(int type_number)
        {
            if (Has(type_number))
            {
                components.Remove(type_number);
            }
        }
        public void Remove(int[] type_numbers)
        {
            if(type_numbers != null)
            foreach (int type_number in type_numbers)
            {
            if (Has(type_number))
                {
                    components.Remove(type_number);
                }
            }
        }
        public void Remove(IList<int> type_numbers)
        {
            if (type_numbers != null)
                foreach (int type_number in type_numbers)
                {
                    if (Has(type_number))
                    {
                        components.Remove(type_number);
                    }
                }
        }
        public void Replace(Component thing)
        {
            // Same as Add(), but remove the target while there is
            if (thing != null)
            {
                if (Has(thing.TypeNumber))
                    components.Remove(thing.TypeNumber);
                components.Add(thing.TypeNumber, thing);
            }
        }
        public void Replace(Component[] things)
        {
            // Same as Add(), but remove the target while there is
            if (things != null)
            for (int i = 0; i< things.Length; i++)
            {
                Replace(things[i]);
            }
        }
    }
}
