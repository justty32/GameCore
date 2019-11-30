using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    public partial class Card
    {
        // which is just edit from ComponentSet, but remove something.
        private Dictionary<int, Component> components;
        public int ComponentsCount => components.Count;
        public IList<int> ComponentsTypesCount => new List<int>(components.Keys); //return what types of component it have
        public bool HasComponent(int type_number) => components.ContainsKey(type_number);
        public bool HasComponent(params int[] type_numbers)
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
        public bool HasComponent(IList<int> type_numbers)
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
        public bool HasComponent(string type_name) => components.ContainsKey(Component.GetTypeNumber(type_name));
        public bool HasComponent(params string[] type_names)
        {
            if (type_names == null)
                return false;
            foreach (string type_name in type_names)
            {
                if (!components.ContainsKey(Component.GetTypeNumber(type_name)))
                    return false;
            }
            return true;
        }
        public bool HasComponent<TComponent>() where TComponent : Component, new()
        {
            return HasComponent(Component.GetSpawner<TComponent>().Type_Number);
        }
        public Component GetComponent(int type_number)
        {
            foreach (var node in components)
            {
                if (node.Key == type_number)
                    return node.Value;
            }
            return null;
        }
        public Component GetComponent(string type_name)
        {
            foreach (var node in components)
            {
                if (node.Value.TypeName.Equals(type_name))
                    return node.Value;
            }
            return null;
        }
        public TComponent GetComponent<TComponent>() where TComponent : Component, new()
        {
            // if any error, return null
            if (!HasComponent<TComponent>())
                return null;
            return GetComponent(Component.GetSpawner<TComponent>().Type_Number) as TComponent;
        }
        public List<Component> GetComponent(params int[] type_numbers)
        {
            if (type_numbers == null)
                return null;
            List<Component> cs = new List<Component>(type_numbers.Length);
            for (int i = 0; i < type_numbers.Length; i++)
            {
                foreach (var node in components)
                {
                    if (node.Key == type_numbers[i])
                        cs.Add(node.Value);
                }
            }
            return cs;
        }
        public List<Component> GetComponent(IList<int> type_numbers)
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
        public IList<Component> Components { get => new List<Component>(components.Values); }
        public bool AddComponent(Component thing)
        {
            // If there is already have same-type one, Do nothing, return true.
            // If TypeNumber is -1, Do nothing, return true.
            // If thing is already has a Card, Do nothing, return true.
            if (thing == null)
                return true;
            if (HasComponent(thing.TypeNumber))
                return true;
            if (thing.Card != null)
                return true;
            else
            {
                components.Add(thing.TypeNumber, thing);
                thing.Card = this;
                return false;
            }
        }
        public void AddComponent(Component[] things)
        {
            // Components of array should be multi type.
            // If there is already have same-type one, Do nothing.
            // If TypeNumber is -1, Do nothing.
            // If a thing already has a Card, Do nothing.
            if (things != null)
            {
                for(int i = 0; i < things.Length; i++)
                {
                    AddComponent(things[i]);
                }
            }
        }
        public void RemoveComponent(int type_number)
        {
            if (HasComponent(type_number))
            {
                components[type_number].Card = null;
                components.Remove(type_number);
            }
        }
        public void RemoveComponent(params int[] type_numbers)
        {
            if (type_numbers != null)
                for(int i = 0; i < type_numbers.Length; i++)
                {
                    RemoveComponent(type_numbers[i]);
                }
        }
        public void RemoveComponent(IList<int> type_numbers)
        {
            if (type_numbers != null)
                for(int i = 0; i < type_numbers.Count; i++)
                {
                    RemoveComponent(type_numbers[i]);
                }
        }
    }
/*
    public class ComponentSet
    {
        //
        // Not allow more than one entity of same type
        // 
        // While doing base controlling (Has, Get, Add, Remove), should care about parameters.
        // if don't put name, that's means to do it for all things of the type.
        // if put array, that's means do several times for things in array
        // 
        // Replace is same as Add(), but do remove while there's already one.
        //
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
            AddComponent(add_existing_components);
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
                                AddComponent(component_list.Get(type_number)[type_choose_name[type_number]]);
                            }
                            else
                            { // there is no thing with specific name in list
                                if (component_list.Get(type_number).Values.Count > 0)
                                { //add it with index 0's thing
                                    AddComponent(component_list.Get(type_number).Values[0]);
                                }
                            }
                        }
                        else
                        { // if type-specific-name list not have specific name
                            if (component_list.Get(type_number).Values.Count > 0)
                            {// if there has thing
                                AddComponent(component_list.Get(type_number).Values[0]);
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
                            AddComponent(component_list.Get(type_number).Values[0]);
                        }
                    }
                }
            }
        }
        public int ComponentsCount => components.Count;
        public IList<int> GetComponentTypeNumbers()
        {
            //return what types of component it have
            return components.Keys;
        }
        public bool HasComponent(int type_number)
        {
            if (components.ContainsKey(type_number))
            {
                return true;
            }
            return false;
        }
        public bool HasComponent(int[] type_numbers)
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
        public bool HasComponent(IList<int> type_numbers)
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
        public bool HasComponent(string type_name){
            if (components.ContainsKey(Component.GetTypeNumber(type_name)))
            {
                return true;
            }
            return false;
        }
        public bool HasComponent(string[] type_names)
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
        public Component GetComponent(int type_number)
        {
            foreach (var node in components)
            {
                if (node.Key == type_number)
                    return node.Value;
            }
            return null;
        }
        public Component GetComponent(string type_name){
            foreach (var node in components)
            {
                if (node.Value.TypeName.Equals(type_name))
                    return node.Value;
            }
            return null;
        }
        public Component this[int i]
        {
            //
            // get -> just Get(i)
            // 
            // set -> Replace() and Add(), judge by Has(i)
            //
            get => GetComponent(i);
            set
            {
                if (HasComponent(i))
                    ReplaceComponent(value);
                else
                    AddComponent(value);
            }
        }
        public Component this[string str]
        {
            //
            // get -> just Get(i)
            // 
            // set -> Replace() and Add(), judge by Has(i)
            //
            get => GetComponent(str);
            set
            {
                if (HasComponent(str))
                    ReplaceComponent(value);
                else
                    AddComponent(value);
            }
        }
        public List<Component> GetComponent(int[] type_numbers)
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
        public List<Component> GetComponent(IList<int> type_numbers)
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
        public IList<Component> GetAllComponents() => components.Values;
        public bool AddComponent(Component thing)
        {
            // If there is already have same-type one, Do nothing, return true.
            // If TypeNumber is -1, Do nothing, return true.
            // If thing is already has a container, Do nothing, return true.
            if (thing == null)
                return true;
            if (HasComponent(thing.TypeNumber))
                return true;
            if (thing.Container != null)
                return true;
            else
            {
                components.Add(thing.TypeNumber, thing);
                thing.Container = this;
                return false;
            }
        }
        public void AddComponent(Component[] things)
        {
            // Components of array should be multi type.
            // If there is already have same-type one, Do nothing.
            // If TypeNumber is -1, Do nothing.
            // If a thing already has a container, Do nothing.
            if (things != null)
            {
                foreach (Component thing in things)
                {
                    if (!HasComponent(thing.TypeNumber) && thing.Container != null)
                    {
                        components.Add(thing.TypeNumber, thing);
                        thing.Container = this;
                    }
                }
            }
        }
        public void RemoveComponent(int type_number)
        {
            if (HasComponent(type_number))
            {
                components[type_number].Container = null;
                components.Remove(type_number);
            }
        }
        public void RemoveComponent(int[] type_numbers)
        {
            if(type_numbers != null)
            foreach (int type_number in type_numbers)
            {
            if (HasComponent(type_number))
                {
                    components.Remove(type_number);
                }
            }
        }
        public void RemoveComponent(IList<int> type_numbers)
        {
            if (type_numbers != null)
                foreach (int type_number in type_numbers)
                {
                    if (HasComponent(type_number))
                    {
                        components.Remove(type_number);
                    }
                }
        }
        public void ReplaceComponent(Component thing)
        {
            // Same as Add(), but remove the target while there is
            if (thing != null)
            {
                if (HasComponent(thing.TypeNumber))
                    components.Remove(thing.TypeNumber);
                components.Add(thing.TypeNumber, thing);
            }
        }
        public void ReplaceComponent(Component[] things)
        {
            // Same as Add(), but remove the target while there is
            if (things != null)
            for (int i = 0; i< things.Length; i++)
            {
                ReplaceComponent(things[i]);
            }
        }
    }
    */
}
