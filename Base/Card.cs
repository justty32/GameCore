using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

// TODO : Now we have a card's json object, which has their own number and name
//        to make a standard, like how many card json object in a file
//        a file is a big json file, {card_amount : 55, cards : [{c1}. {c2}, ...{c55}]}

namespace GameCore.Base
{
    public partial class Card : Util.INode
    {
        // Need Init() after new(), Clear() while not using then.
        // Name be null, and Number be -1 initially
        // the Card is not usable, while Number is -1
        //
        // Init() to load a card, BeNewCard() to create a new card
        public int Number { get; private set;} = -1;
        public string Name { get; set; } = "";
        public bool IsUsable()
        {
            if(Number < 0)
                return false;
            if(Concepts == null)
                return false;
            foreach(var c in Concepts)
            {
                if(c == null)
                    return false;
                if(!c.IsUsable())
                    return false;
            }
            return true;
        }
        public bool Init(int number, string name = null)
        {
            // load a card
            // set number and name, add this to global card list
            Clear();
            if(number < 0 || Core.Cards.Contains(number)
                || number > Core.Cards.MaxNumber)
                return true;
            Number = number;
            if(Core.Cards.Add(this))
                return true;
            Name = name;
            concepts = new Dictionary<int, Concept>();
            return false;
        }
        public void InitBeNew(string name = null)
        {
            // be a new card, with new distributed number and specific name
            Clear();
            Core.Cards.MaxNumber++;
            Init(Core.Cards.MaxNumber, name);
        }
        public void Clear()
        {
            // set number to -1, and things to null 
            // remove this from global card list
            if(concepts != null)
                concepts.Clear();
            concepts = null;
            Name = "";
            if(Core.Cards.Contains(Number))
                Core.Cards.Remove(Number);
            Number = -1;
        }
        public JObject ToJsonObject()
        {
            // spawn a json object
            // number : number
            // name : name
            // concepts : [{c1's json object},{c2},{c3}]
            JObject ojs;
                try{
                ojs = new JObject();
                ojs.Add(new JProperty("Number", Number));
                ojs.Add(new JProperty("Name", Name));
                JArray concept_numbers = new JArray();
                JArray concept_names = new JArray();
                JArray concepts = new JArray();
                foreach(Concept c in Concepts)
                {
                    concept_numbers.Add(c.TypeNumber);
                    concept_names.Add(c.TypeName);
                    concepts.Add(c.ToJsonObject());
                }
                ojs.Add(new JProperty("Concept_numbers", concept_numbers));
                ojs.Add(new JProperty("Concept_names", concept_names));
                ojs.Add(new JProperty("Concepts", concepts));
            }catch(Exception){
                return null;
            }
            return ojs;
        }
        public bool FromJsonObject(JObject ojs)
        { 
            // use Init() to initialize number and name
            // then took out concept's json object and call concept's FromJsonObject()
            if(ojs == null)
                return true;
            try{
                if(Init((int)ojs["Number"], (string)ojs["Name"]))
                    return true;
                JArray cs = (JArray)ojs["Concepts"];
                for(int i = 0; i < cs.Count ; i++)
                {
                    var c = ConceptManager.GetSpawner((string)cs[i]["TypeName"]).SpawnBase();
                    if(!c.FromJsonObject((JObject)cs[i]))
                        Add(c);
                }
            }catch(Exception){
                return true;
            }
            return false;
        }
    }
}
