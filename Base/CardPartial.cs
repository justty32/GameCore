using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    public partial class Card
    {
        // which is just edit from ConceptSet, but remove something.
        public Dictionary<int, Concept> concepts;
        public IList<Concept> Concepts { get => new List<Concept>(concepts.Values); }
        public int ConceptsCount => concepts.Count;
        public IList<int> ConceptsTypes => new List<int>(concepts.Keys); //return what types of concept it have
        public Card()
        {
            concepts = new Dictionary<int, Concept>();
        }
        public Card(string name)
        {
            concepts = new Dictionary<int, Concept>();
            if(name != null)
                Name = name;
        }
        public bool Has(int type_number) => concepts.ContainsKey(type_number);
        public bool Has(params int[] type_numbers)
        {
            if (type_numbers == null)
                return false;
            foreach (int type_number in type_numbers)
            {
                if (!concepts.ContainsKey(type_number))
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
                if (!concepts.ContainsKey(type_number))
                    return false;
            }
            return true;
        }
        public bool Has(string type_name) => concepts.ContainsKey(ConceptManager.GetTypeNumber(type_name));
        public bool Has(params string[] type_names)
        {
            if (type_names == null)
                return false;
            foreach (string type_name in type_names)
            {
                if (!concepts.ContainsKey(ConceptManager.GetTypeNumber(type_name)))
                    return false;
            }
            return true;
        }
        public bool Has<TConcept>() where TConcept : Concept, new()
        {
            return Has(ConceptManager.GetSpawner<TConcept>().TypeNumber);
        }
        public Concept Get(int type_number)
        {
            foreach (var node in concepts)
            {
                if (node.Key == type_number)
                    return node.Value;
            }
            return null;
        }
        public Concept Get(string type_name)
        {
            foreach (var node in concepts)
            {
                if (node.Value.TypeName.Equals(type_name))
                    return node.Value;
            }
            return null;
        }
        public TConcept Get<TConcept>() where TConcept : Concept, new()
        {
            // if any error, return null
            if (!Has<TConcept>())
                return null;
            return Get(ConceptManager.GetSpawner<TConcept>().TypeNumber) as TConcept;
        }
        public List<Concept> Get(params int[] type_numbers)
        {
            if (type_numbers == null)
                return null;
            List<Concept> cs = new List<Concept>(type_numbers.Length);
            for (int i = 0; i < type_numbers.Length; i++)
            {
                foreach (var node in concepts)
                {
                    if (node.Key == type_numbers[i])
                        cs.Add(node.Value);
                }
            }
            return cs;
        }
        public List<Concept> Get(IList<int> type_numbers)
        {
            if (type_numbers == null)
                return null;
            List<Concept> cs = new List<Concept>(type_numbers.Count);
            for (int i = 0; i < type_numbers.Count; i++)
            {
                foreach (var node in concepts)
                {
                    if (node.Key == type_numbers[i])
                        cs.Add(node.Value);
                }
            }
            return cs;
        }
        public CDynamic GetCDynamic(string type_name)
        {
            if (type_name == null)
                return null;
            if (Has(type_name))
            {
                var cd = Get(type_name);
                return cd as CDynamic;    
            }
            else
                return null;
        }               
        public bool Add(Concept thing)
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
                concepts.Add(thing.TypeNumber, thing);
                thing.Card = this;
                return false;
            }
        }
        public void Add(Concept[] things)
        {
            // Concepts of array should be multi type.
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
                concepts[type_number].Card = null;
                concepts.Remove(type_number);
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
        public void Remove<TConcept>() where TConcept : Concept, new()
        {
            Remove(ConceptManager.GetSpawner<TConcept>().TypeNumber);
        }
        public static Card Copy(Card target, bool init_new_one = true, string name = null, params int[] type_numbers)
        {
            if (target == null)
                return null;
            Card card = new Card(name);
            if (init_new_one)
            {
                if (card.InitBeNew(name))
                    return null;
            }
            List<int> types = new List<int>(target.ConceptsTypes);
            if (type_numbers.Length != 0)
                types = new List<int>(type_numbers);
            for(int i = 0; i < types.Count; i++)
            {
                Concept c = target.Get(types[i]);
                if (c == null)
                    continue;
                var cn = ConceptManager.GetSpawner(c.TypeNumber).SpawnBase();
                cn.FromJsonObject(c.ToJsonObject());
                cn.TypeNumber = c.TypeNumber;
                card.Add(cn);
            }
            return card;
        }
    }
}