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
        public int Number { get; set;} = -1;
        public string Name { get; set; } = "";
        public static bool IsUsable(Card card)
        {
            if (card == null)
                return false;
            return card.IsUsable();
        }
        public bool IsUsable()
        {
            if(Number < 0 || Number > Core.Cards.MaxNumber)
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
        private bool Init(int number, string name = null)
        {
            // load a card
            // set number and name, add this to global card list
            Clear();
            if(number < 0 || number > Core.Cards.MaxNumber)
                return true;
            Number = number;
            if (name != null)
                Name = name;
            else
                Name = "";
            concepts = new Dictionary<int, Concept>();
            if(Core.Cards.Add(this))
                return true;
            return false;
        }
        public bool InitBeNew(string name = null)
        {
            // be a new card, with new distributed number and specific name
            Clear();
            Core.Cards.MaxNumber++;
            return Init(Core.Cards.MaxNumber, name);
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
            JObject ojs = new JObject();
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
                var cj = c.ToJsonObject();
                if (!cj.ContainsKey("TypeName"))
                    cj.Add("TypeName", c.TypeName);
                concepts.Add(cj);
            }
            ojs.Add(new JProperty("Concepts", concepts));
            }catch(Exception e){
                return Core.State.WriteException<JObject>(e);
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
                if (!(ojs.ContainsKey("Number")))
                    return true;
                if (!(ojs.ContainsKey("Name")))
                    ojs.Add("Name", "");
                if(Init((int)ojs["Number"], (string)ojs["Name"]))
                    return true;
                JArray cs = (JArray)ojs["Concepts"];
                for(int i = 0; i < cs.Count ; i++)
                {
                    if (!((JObject)cs[i]).ContainsKey("TypeName"))
                        continue;
                    if (!ConceptManager.ContainsTypeName((string)cs[i]["TypeName"]))
                        continue;
                    var c = ConceptManager.GetSpawner((string)cs[i]["TypeName"]).SpawnBase();
                    if(!c.FromJsonObject((JObject)cs[i]))
                        Add(c);
                }
            }catch(Exception e){
                return Core.State.WriteException(e);
            }
            return false;
        }
    }
}
