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
        public bool Has(int type_number) => components.ContainsKey(type_number);
        public bool Has(params int[] type_numbers)
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
        public bool Has(string type_name) => components.ContainsKey(ComponentManager.GetTypeNumber(type_name));
        public bool Has(params string[] type_names)
        {
            if (type_names == null)
                return false;
            foreach (string type_name in type_names)
            {
                if (!components.ContainsKey(ComponentManager.GetTypeNumber(type_name)))
                    return false;
            }
            return true;
        }
        public bool Has<TComponent>() where TComponent : Component, new()
        {
            return Has(ComponentManager.GetSpawner<TComponent>().Type_Number);
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
        public Component Get(string type_name)
        {
            foreach (var node in components)
            {
                if (node.Value.TypeName.Equals(type_name))
                    return node.Value;
            }
            return null;
        }
        public TComponent Get<TComponent>() where TComponent : Component, new()
        {
            // if any error, return null
            if (!Has<TComponent>())
                return null;
            return Get(ComponentManager.GetSpawner<TComponent>().Type_Number) as TComponent;
        }
        public List<Component> Get(params int[] type_numbers)
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
        public IList<Component> Components { get => new List<Component>(components.Values); }
        public bool Add(Component thing)
        {
            // If there is already have same-type one, Do nothing, return true.
            // If TypeNumber is -1, Do nothing, return true.
            // If thing is already has a Card, Do nothing, return true.
            if (thing == null)
                return true;
            if (Has(thing.TypeNumber))
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
        public void Add(Component[] things)
        {
            // Components of array should be multi type.
            // If there is already have same-type one, Do nothing.
            // If TypeNumber is -1, Do nothing.
            // If a thing already has a Card, Do nothing.
            if (things != null)
            {
                for(int i = 0; i < things.Length; i++)
                {
                    Add(things[i]);
                }
            }
        }
        public void Remove(int type_number)
        {
            if (Has(type_number))
            {
                components[type_number].Card = null;
                components.Remove(type_number);
            }
        }
        public void Remove(params int[] type_numbers)
        {
            if (type_numbers != null)
                for(int i = 0; i < type_numbers.Length; i++)
                {
                    Remove(type_numbers[i]);
                }
        }
        public void Remove(IList<int> type_numbers)
        {
            if (type_numbers != null)
                for(int i = 0; i < type_numbers.Count; i++)
                {
                    Remove(type_numbers[i]);
                }
        }
        public void Remove<TComponent>() where TComponent : Component, new()
        {
            Remove(ComponentManager.GetSpawner<TComponent>().Type_Number);
        }
    }
}