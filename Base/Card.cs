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
        public string Name { get; set; } = null;
        public bool IsUsable()
        {
            if (Number >= 0)
                return true;
            return false;
        }
        public bool Init(int number, string name = null)
        {
            // load a card
            // set number and name, add this to global card list
            Clear();
            if(number < 0 || Core.Instance.Cards.Contains(number)
                || number > Core.Instance._card_number_distribute_reference)
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
            Core.Instance._card_number_distribute_reference++;
            Init(Core.Instance._card_number_distribute_reference, name);
        }
        public void Clear()
        {
            // set number to -1, and things to null 
            // remove this from global card list
            if(components != null)
                components.Clear();
            components = null;
            Name = null;
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
                JArray cs = new JArray();
                foreach(Component c in Components)
                {
                    cs.Add(c.ToJsonObject());
                }
                ojs.Add(new JProperty("Components", cs));
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
                Init((int)ojs["Number"], (string)ojs["Name"]);
                JArray cs = (JArray)ojs["Components"];
                for(int i = 0; i < cs.Count ; i++)
                {
                    var c = Component.GetSpawner((string)cs[i]["TypeName"]).SpawnBase();
                    if(!c.FromJsonObject((JObject)cs[i]))
                        AddComponent(c);
                }
            }catch(Exception){
                return true;
            }
            return false;
        }
    }
    public class CardList
    {
        // wrap the card list
        // TODO : a function: if the card are not in list, load that card from I/O 
        internal Dictionary<int, Card> cards{ get; private set;} = new Dictionary<int, Card>();
        public bool Add(Card card)
        {
            if(cards.ContainsKey(card.Number))
                return true;
            cards.Add(card.Number, card);
            return false;
        }
        public bool Remove(int number)
        {
            if(!cards.ContainsKey(number))
                return true;
            cards.Remove(number);
            return false;
        }
        public bool Contains(int number) => cards.ContainsKey(number);
        public Card this[int number]{
            get
            {
                if(!cards.ContainsKey(number)) 
                    return null;
                return cards[number];
            }
        }
    }
}
