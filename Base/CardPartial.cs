using System.Collections.Generic;

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
        public bool Has(int type_number)
        {
            if (!concepts.ContainsKey(type_number))
                return false;
            if (concepts[type_number] == null)
            {
                concepts.Remove(type_number);
                return false;
            }
            return true;
        }
        public bool Has(params int[] type_numbers)
        {
            if (type_numbers == null)
                return false;
            foreach (int type_number in type_numbers)
            {
                if (!Has(type_number))
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
                if (!Has(type_number))
                    return false;
            }
            return true;
        }
        public bool Has(string type_name)
        {
            int type_number = ConceptManager.GetTypeNumber(type_name);
            return Has(type_number);
        }
        public bool Has(params string[] type_names)
        {
            if (type_names == null)
                return false;
            foreach (string type_name in type_names)
            {
                if (!Has(type_name))
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
            if (!Has(type_number))
                return null;
            return concepts[type_number];
        }
        public Concept Get(string type_name)
        {
            return Get(ConceptManager.GetTypeNumber(type_name));
        }
        public TConcept Get<TConcept>() where TConcept : Concept, new()
        {
            return Get(ConceptManager.GetSpawner<TConcept>().TypeNumber) as TConcept;
        }
        public TConcept Get<TConcept>(int type_number) where TConcept : Concept, new()
        {
            return Get(type_number) as TConcept;
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
            concepts.Add(thing.TypeNumber, thing);
            thing.Card = this;
            return false;
        }
        public void Add(params Concept[] things)
        {
            // Concepts of array should be multi type.
            // If there is already have same-type one, Do nothing.
            // If TypeNumber is -1, Do nothing.
            // If a thing already has a Card, Do nothing.
            if (things != null)
            {
                for (int i = 0; i < things.Length; i++)
                {
                    Add(things[i]);
                }
            }
        }
        public void Remove(int type_number)
        {
            if (Has(type_number))
                concepts.Remove(type_number);
        }
        public void Remove(params int[] type_numbers)
        {
            if (type_numbers != null)
                for (int i = 0; i < type_numbers.Length; i++)
                    Remove(type_numbers[i]);
        }
        public void Remove<TConcept>() where TConcept : Concept, new()
        {
            Remove(ConceptManager.GetSpawner<TConcept>().TypeNumber);
        }
        public static Card Copy(Card target, bool init_new_one = true, params int[] specific_types)
        {
            if (target == null)
                return null;
            Card card = new Card();
            if (init_new_one)
            {
                if (card.InitBeNew())
                    return null;
            }
            List<int> types = new List<int>(target.ConceptsTypes);
            if (specific_types != null) 
                if (specific_types.Length != 0)
                    types = new List<int>(specific_types);
            for (int i = 0; i < types.Count; i++)
            {
                Concept c = target.Get(types[i]);
                if (c == null)
                    continue;
                var cc = c.Copy();
                if (cc == null)
                    continue;
                /*var c_spawner = ConceptManager.GetSpawner(c.TypeNumber);
                if (c_spawner == null)
                    continue;
                var cn = c_spawner.SpawnBase().FromJsonObject(c.ToJsonObject());
                if (cn == null)
                    continue;
                */
                card.Add(cc);
            }
            return card;
        }
    }
}