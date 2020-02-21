using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using GameCore.Base;
using GameCore.Root;

namespace GameCore.Thing
{
    public class StoreRule : Rule
    {
        public class CStore : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CStore";
            public int Capacity = 1;
            public int Amount = 0;
            public int Weight = 0; //當前總重量
            public int Size = 0; //當前總大小
            public List<Thing> Things = new List<Thing>();
            public override bool IsUsable()
            {
                if (Capacity < 1)
                    Capacity = 1;
                if (Things == null)
                {
                    Things = new List<Thing>(Capacity);
                    for (int i = 0; i < Capacity; i++)
                        Things.Add(new Thing());
                    for (int i = 0; i < Things.Count; i++)
                        Things[i].StoreIndex = i;
                }
                if (Things.Count < Capacity)
                {
                    for (int i = 0; i < Capacity - Things.Count; i++)
                        Things.Add(new Thing());
                    for (int i = 0; i < Things.Count; i++)
                        Things[i].StoreIndex = i;
                }
                return true;
            }
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CStore c = json.ToObject<CStore>();
                    return c;
                }catch(Exception e)
                {
                    return Core.State.WriteException<Concept>(e);
                }
            }
            public override JObject ToJsonObject()
            {
                try
                {
                    Card temp_c = this.Card;
                    Card = null;
                    JObject json = JObject.FromObject(this);
                    if(json["Card"] != null)
                        json.Remove("Card");
                    Card = temp_c;
                    return json;
                }catch(Exception e)
                {
                    return Core.State.WriteException<JObject>(e);
                }
            }
        }
        public class Thing
        {
            public int Number = 0;
            public int Amount = 0;
            public float Durability = 0;
            public int StoreIndex = -1;
        }
        public class HINStoreChange
        {
            public int Card = 0;
            public Thing Pre = new Thing();
            public bool IsMove = false;
            public HINStoreChange() { }
            public HINStoreChange(int card, Thing thing, bool is_move_thing = false)
            {
                Pre = new Thing();
                IsMove = is_move_thing;
                if(thing != null || card > -1)
                {
                    Card = card;
                    Pre.Number = thing.Number;
                    Pre.Amount = thing.Amount;
                    Pre.Durability = thing.Durability;
                    Pre.StoreIndex = thing.StoreIndex;
                }
            }
        }
        public Hook<HINStoreChange, object> HStoreChange = new Hook<HINStoreChange, object>("HStoreChange");
        public Hook<int, object> HStoreDestroy = new Hook<int, object>();
        private int _ctn_store = -1;
        private int _ctn_item = -1;
        private int _ctn_location = -1;
        private int _ctn_entity = -1;
        public StoreRule()
        {
            _ctn_store = ConceptSpawner<CStore>.GetSpawner().TypeNumber;
            _ctn_item = Concept.Spawn<ItemRule.CItem>().TypeNumber;
            _ctn_location = Concept.Spawn<LocationRule.CLocation>().TypeNumber;
            _ctn_entity = Concept.Spawn<EntityRule.CEntity>().TypeNumber;
        }
        public bool BeStore(Card card, int capacity = 1)
        {
            // check condition
            if (capacity < 1)
                return true;
            // check concept
            if (!UnHasConcept(card, _ctn_store))
                return true;
            // add concept
            var c = AddConcept<CStore>(card);
            if (c == null)
                return true;
            // set
            c.Capacity = capacity;
            c.Things = null;
            c.IsUsable();
            return false;
        }
        public bool DestroyStore(Card card)
        {
            // check concepts
            if(!HasConcept(card, _ctn_store))
                return true;
            // get concept
            var c = card.Get<CStore>();
            if (c == null)
            {
                card.Remove(_ctn_store);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HStoreDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_store);
            return false;
        }
        public bool ExpandCapacity(Card card, int add_capacity = 0)
        {
            if (add_capacity < 0)
                return true;
            var c = GetConcept<CStore>(card);
            if (c == null)
                return true;
            c.Capacity += add_capacity;
            c.Things.Add(new Thing());
            c.Things[c.Things.Count - 1].StoreIndex = c.Things.Count - 1;
            return false;
        }
        public bool NewThing(Card card, int number, int amount = 1
            , float durability = 1.0f, bool auto_expand_capacity = true)
        {
            // check condition
            if(number <= 0 || amount < 1 || durability < -1)
                return false;
            // check has concepts
            // get concept
            var c = GetConcept<CStore>(card);
            if (c == null)
                return true;
            if (c.Amount + 1 > c.Capacity)
            {
                if (auto_expand_capacity)
                {
                    if (ExpandCapacity(card, 1))
                        return true;
                }else
                    return true;
            }
            // make hook input
            HINStoreChange hin = new HINStoreChange();
            hin.Card = card.Number;
            // do actions
            for(int i = 0; i < c.Things.Count; i++)
            {
                if(c.Things[i].Number != 0)
                {
                    var t = c.Things[i];
                    hin = new HINStoreChange(card.Number, t);
                    t.Number = number;
                    t.Amount = amount;
                    t.Durability = durability;
                    c.Amount++;
                }
            }
            // make hook calling
            HStoreChange.CallAll(hin);
            return false;
        }
        public bool DeleteThing(Card card, int thing_index)
        {
            // check condition
            if (thing_index < 0)
                return true;
            // check has concepts
            // get concept
            var c = GetConcept<CStore>(card);
            if (c == null)
                return true;
            if (thing_index >= c.Things.Count)
                return true;
            // make hook input
            HINStoreChange hin = new HINStoreChange();
            hin.Card = card.Number;
            // do actions
            var t = c.Things[thing_index];
            if (t.Number <= 0)
            {
                hin = new HINStoreChange(card.Number, t);
                t.Number = 0;
                t.Amount = 0;
                t.Durability = 0;
                c.Amount--;
            }
            // make hook calling
            HStoreChange.CallAll(hin);
            return false;
        }
        public List<Thing> GetThing(Card card, int number)
        {
            // check condition
            if (number < 0)
                return null;
            // check and get concepts
            var c = GetConcept<CStore>(card);
            if (c == null)
                return null;
            // make hook input
            // do actions
            List<Thing> ts = new List<Thing>();
            foreach(var t in c.Things)
            {
                if (t.Number == number)
                    ts.Add(t);
            }
            // make hook calling
            return ts;
        }
        public bool AddThing(Card card, int number, int amount = 1
            , float condition_durability = 1.0f
            , bool auto_expand_capacity = true)
        {
            if (number == 0)
                return true;
            var things = GetThing(card, number);
            if (things == null)
                return true;
            else if (things.Count == 0)
                return NewThing(card, number, amount, condition_durability, auto_expand_capacity);
            for(int i = 0; i < things.Count; i++)
            {
                var thing = things[i];
                if (thing.Number == number)
                    if (thing.Durability == condition_durability)
                        return ControlThing(card, thing.StoreIndex
                            , amount, 0);
            }
            return NewThing(card, number, amount, condition_durability, auto_expand_capacity);
        }
        public bool ControlThing(Card card, int thing_index
            , int add_amount = 0, float add_durability = 0.0f, int set_number = 0)
        {
            // check condition
            if (thing_index < 0 || set_number < 0)
                return true;
            // check and get concepts
            var c = GetConcept<CStore>(card);
            if (c == null)
                return true;
            if (thing_index >= c.Things.Count)
                return true;
            // make hook input
            HINStoreChange hin = new HINStoreChange();
            hin.Card = card.Number;
            // do actions
            var t = c.Things[thing_index];
            hin.Pre.Number = t.Number;
            hin.Pre.Amount = t.Amount;
            hin.Pre.Durability = t.Durability;
            hin.Pre.StoreIndex = t.StoreIndex;
            t.Number = set_number;
            t.Amount += add_amount;
            t.Durability += add_durability;
            // make hook calling
            if (t.Number == 0 || t.Amount <= 0 || t.Durability <= 0)
                DeleteThing(card, thing_index);
            else
                HStoreChange.CallAll(hin);
            return false;
        }
        public bool CalWeightAndSize(Card card)
        {
            var c = GetConcept<CStore>(card);
            if (c == null)
                return true;
            var ts = c.Things;
            int amount = 0;
            int weight = 0;
            int size = 0;
            for(int i = 0; i < ts.Count; i++)
            {
                var t = ts[i];
                if (t == null)
                    t = new Thing();
                if (t.Number < 0)
                    t.Number = 0;
                if (t.Amount < 0)
                    t.Amount = 0;
                if (t.Durability < 0)
                    t.Durability = 0;
                if(t.Number != 0)
                {
                    amount++;
                    var citem = GetConcept<ItemRule.CItem>(ToCard(t.Number));
                    if (citem == null)
                        DeleteThing(card, t.Number);
                    weight += citem.Weight * t.Amount;
                    size += citem.Size * t.Amount;
                }
            }
            c.Amount = amount;
            c.Weight = weight;
            c.Size = size;
            return false;
        }
        public bool SortThings(Card card)
        {
            // check condition
            // check and get concepts
            var c = GetConcept<CStore>(card);
            if (c == null)
                return true;
            // make hook input
            // do actions
            int f = 0;
            var ts = c.Things;
            for(int i = 0; i < ts.Count; i++)
            {
                var t = ts[i];
                if (t.Number == 0)
                    continue;
                bool n = false;
                for(int j = 0; j < f; j++)
                {
                    var tj = ts[j];
                    if(t.Number == tj.Number
                        && t.Durability == tj.Durability)
                    {
                        HINStoreChange hin1 = new HINStoreChange(card.Number, t, true);
                        HINStoreChange hin2 = new HINStoreChange(card.Number, tj);
                        tj.Amount += t.Amount;
                        t.Number = 0;
                        t.Amount = 0;
                        t.Durability = 0;
                        c.Amount--;
                        HStoreChange.CallAll(hin1);
                        HStoreChange.CallAll(hin2);
                        n = true;
                        break;
                    }
                }
                if (!n && f >= 0 && f < ts.Count)
                {
                    var tf = ts[f];
                    if (tf.Number != 0)
                        continue;
                    HINStoreChange hin1 = new HINStoreChange(card.Number, t, true);
                    HINStoreChange hin2 = new HINStoreChange(card.Number, tf);
                    tf.Number = t.Number;
                    tf.Amount = t.Amount;
                    tf.Durability = t.Durability;
                    t.Number = 0;
                    t.Amount = 0;
                    t.Durability = 0;
                    HStoreChange.CallAll(hin1);
                    HStoreChange.CallAll(hin2);
                }
                f++;
            }
            // make hook calling
            return false;
        }
        public bool MoveTo(Card card, int thing_index, Card target_card)
        {
            if (card == null || target_card == null
                || thing_index < 0)
                return true;
            var c = GetConcept<CStore>(card);
            if (c == null)
                return true;
            if (thing_index >= c.Things.Count)
                return true;
            var t = c.Things[thing_index];
            HINStoreChange hin = new HINStoreChange(card.Number, t, true);
            t.Number = t.Amount = 0;
            t.Durability = 0;
            c.Amount--;
            HStoreChange.CallAll(hin);
            NewThing(target_card, hin.Pre.Number, hin.Pre.Amount
                , hin.Pre.Durability);
            return false;
        }
    }
}
