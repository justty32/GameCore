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
            if(Components == null)
                return false;
            foreach(var c in Components)
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
            if(number < 0 || Core.Instance.Cards.Contains(number)
                || number > Core.Instance.Card_max_number)
                return true;
            Number = number;
            if(Core.Instance.Cards.Add(this))
                return true;
            Name = name;
            components = new Dictionary<int, Component>();
            return false;
        }
        public void InitBeNew(string name = null)
        {
            // be a new card, with new distributed number and specific name
            Clear();
            Core.Instance.Card_max_number++;
            Init(Core.Instance.Card_max_number, name);
        }
        public void Clear()
        {
            // set number to -1, and things to null 
            // remove this from global card list
            if(components != null)
                components.Clear();
            components = null;
            Name = "";
            if(Core.Instance.Cards.Contains(Number))
                Core.Instance.Cards.Remove(Number);
            Number = -1;
        }
        public JObject ToJsonObject()
        {
            // spawn a json object
            // number : number
            // name : name
            // components : [{c1's json object},{c2},{c3}]
            JObject ojs;
                try{
                ojs = new JObject();
                ojs.Add(new JProperty("Number", Number));
                ojs.Add(new JProperty("Name", Name));
                JArray component_numbers = new JArray();
                JArray component_names = new JArray();
                JArray components = new JArray();
                foreach(Component c in Components)
                {
                    component_numbers.Add(c.TypeNumber);
                    component_names.Add(c.TypeName);
                    components.Add(c.ToJsonObject());
                }
                ojs.Add(new JProperty("Component_numbers", component_numbers));
                ojs.Add(new JProperty("Components_names", component_names));
                ojs.Add(new JProperty("Components", components));
            }catch(Exception){
                return null;
            }
            return ojs;
        }
        public bool FromJsonObject(JObject ojs)
        { 
            // use Init() to initialize number and name
            // then took out component's json object and call component's FromJsonObject()
            if(ojs == null)
                return true;
            try{
                if(Init((int)ojs["Number"], (string)ojs["Name"]))
                    return true;
                JArray cs = (JArray)ojs["Components"];
                for(int i = 0; i < cs.Count ; i++)
                {
                    var c = Component.GetSpawner((string)cs[i]["TypeName"]).SpawnBase();
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
