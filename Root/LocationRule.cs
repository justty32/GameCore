using System.Linq;
using System.Security.AccessControl;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace GameCore.Root
{
    public class LocationRule : Base.Rule
    {
        public class CLocation : Base.Component
        {
            private readonly string _type_name = "CLocation";
            public override string TypeName => _type_name;
            public int UpperCard { get; private set; } = -1;
            public List<int> BelowCards { get; private set; }
            public bool SetUpperCard(int cdn = -1)
            {
                UpperCard = cdn;
                if (cdn < 0 || cdn > Core.Cards.MaxNumber)
                    UpperCard = 0;
                return false;
            }
            public bool Init(int upper_card, params int[] below_cards)
            {
                if(upper_card < 0)
                    UpperCard = -1;
                else
                    UpperCard = upper_card;
                if(below_cards != null)
                    BelowCards = new List<int>(below_cards);
                else
                    BelowCards = new List<int>();
                if(Core.RuleManager.LocationRule.LocationCards.Contains(Card.Number))
                    return true;
                Core.RuleManager.LocationRule.LocationCards.Add(Card.Number);
                return false;
            }
            public override bool IsUsable()
            {
                if (TypeNumber < 0 || BelowCards == null)
                    return false;
                return true;
            }
            public override bool FromJsonObject(JObject ojs)
            {
                if(ojs == null)
                    return true;
                try{
                    // check if the object's typename meets this
                    if(!((string)ojs[TypeName]).Equals(TypeName))
                        return true;
                    // take out data
                    UpperCard = (int)ojs["UpperCard"];
                    BelowCards = ((JArray)ojs["BelowCards"]).Select(c => (int)c).ToList();
                }catch(Exception){
                    return true;
                }
                return false;
            }
            public override JObject ToJsonObject()
            {
                // first type, example
                JObject e = JObject.FromObject(
                    new {
                        TypeName = TypeName,
                        UpperCard = UpperCard,
                        BelowCards = BelowCards
                    }
                );
                // second type, good but complex
                JObject js = new JObject(
                    //put typename, and data
                    new JProperty("TypeName", TypeName)
                    ,new JProperty("UpperCard", UpperCard)
                    ,new JProperty("BelowCards", new JArray(BelowCards))
                    );
                return js;
            }
        }
        private int _ctn_location = -1;
        public List<int> LocationCards {get; private set;} = null;
        public bool Init()
        {
            _ctn_location = Base.ComponentManager.GetSpawner<CLocation>().Type_Number;
            LocationCards = new List<int>(2000);
            return false; 
        }
        public bool AddCLocation(Base.Card card, int upper_card, params int[] below_cards)
        {
            return AddComponent<CLocation>(card).Init(upper_card, below_cards);
        }
        public bool RemoveCLocation(Base.Card card)
        {
            if(RemoveComponent<CLocation>(card))
                return true;
            LocationCards.Remove(card.Number);
            return false;
        }
        public override bool IsUsable()
        {
            if (_ctn_location >= 0 && LocationCards != null)
                return true;
            return false;
        }
    }
}