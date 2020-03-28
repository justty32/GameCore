using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace GameCore.Base
{
    public partial class Card : Util.INode
    {
        // Need Init() after new(), Clear() while not using then.
        // Number be -1 initially
        // the Card is not usable while Number is -1
        // Init() to load a card, BeNewCard() to create a new card
        public int Number { get; set; } = -1;
        public bool Definition = true;
        public bool NeedSave = true;
        public static bool IsUsable(Card card)
        {
            if (card == null)
                return false;
            return card.IsUsable();
        }
        public bool IsUsable()
        {
            if (Number < 0 || Number > Core.Cards.MaxNumber)
                return false;
            if (Concepts == null)
                return false;
            foreach (var c in Concepts)
            {
                if (c == null)
                    return false;
                if (!c.IsUsable())
                    return false;
            }
            return true;
        }
        private bool Init(int number)
        {
            // load a card
            // set number, add this to global card list
            Clear();
            if (number < 0 || number > Core.Cards.MaxNumber)
                return true;
            Number = number;
            concepts = new Dictionary<int, Concept>();
            if (Core.Cards.Add(this))
                return true;
            return false;
        }
        public bool InitBeNew()
        {
            // be a new card, with new distributed number
            Core.Cards.MaxNumber++;
            return Init(Core.Cards.MaxNumber);
        }
        public void Clear()
        {
            // set number to -1, and things to null
            // remove this from global card list
            concepts = null;
            if(Number >= 0)
                Core.Cards.Remove(Number);
            Number = -1;
        }
        public JObject ToJsonObject()
        {
            // spawn a json object
            // number : number
            // concepts : [{c1's json object},{c2},{c3}]
            JObject ojs = new JObject();
            try
            {
                ojs = new JObject();
                ojs.Add(new JProperty("Number", Number));
                ojs.Add(new JProperty("Definition", Definition));
                JArray concept_numbers = new JArray();
                JArray concept_names = new JArray();
                JArray concepts = new JArray();
                foreach (Concept c in Concepts)
                {
                    concept_numbers.Add(c.TypeNumber);
                    concept_names.Add(c.TypeName);
                    var cj = c.ToJsonObject();
                    if (!Util.JObjectContainsKey(cj, "TypeName"))
                        cj.Add("TypeName", c.TypeName);
                    concepts.Add(cj);
                }
                ojs.Add(new JProperty("Concepts", concepts));
            }
            catch (Exception e)
            {
                return Core.State.WriteException<JObject>(e);
            }
            return ojs;
        }
        public bool FromJsonObject(JObject ojs)
        {
            // use Init() to initialize number
            // then took out concept's json object and call concept's FromJsonObject()
            if (ojs == null)
                return true;
            try
            {
                if (!(Util.JObjectContainsKey(ojs, "Number")))
                    return true;
                if (Init((int)ojs["Number"]))
                    return true;
                Definition = (bool)ojs["Definition"];
                NeedSave = !Definition;
                JArray cs = (JArray)ojs["Concepts"];
                for (int i = 0; i < cs.Count; i++)
                {
                    if (!(Util.JObjectContainsKey((JObject)cs[i], "TypeName")))
                        continue;
                    if (!ConceptManager.ContainsTypeName((string)cs[i]["TypeName"]))
                        continue;
                    var c_spawner = ConceptManager.GetSpawner((string)cs[i]["TypeName"]);
                    if (c_spawner == null)
                        continue;
                    var c = c_spawner.SpawnBase().FromJsonObject((JObject)cs[i]);
                    if (c != null)
                        Add(c);
                }
            }
            catch (Exception e)
            {
                return Core.State.WriteException(e);
            }
            return false;
        }
    }
}