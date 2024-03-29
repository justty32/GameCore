﻿using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    public class Util
    {
        public interface INode
        {
            bool IsUsable();
        }
        public static bool JObjectContainsKey(JObject json, string key)
        {
            if (key == null)
                return false;
            return json[key] != null;
        }
        public static bool HasAnyNull(params object[] obs)
        {
            foreach (var ob in obs)
            {
                if (ob == null)
                    return true;
            }
            return false;
        }
        public static bool HasAnyTrue(params bool[] bs)
        {
            foreach (bool b in bs)
            {
                if (b == true)
                    return true;
            }
            return false;
        }
        public static bool HasAnyFalse(params bool[] bs)
        {
            foreach (bool b in bs)
            {
                if (b == false)
                    return true;
            }
            return false;
        }
        public static bool HasAnyNegative(params int[] vs)
        {
            foreach (int i in vs)
            {
                if (i < 0)
                    return true;
            }
            return false;
        }
        public static bool HasAnyZero(params int[] vs)
        {
            foreach (int i in vs)
            {
                if (i == 0)
                    return true;
            }
            return false;
        }
        public static bool HasAnyPositive(params int[] vs)
        {
            foreach (int i in vs)
            {
                if (i > 0)
                    return true;
            }
            return false;
        }
        public static bool HasAllZero(params int[] vs)
        {
            foreach (int i in vs)
            {
                if (i != 0)
                    return false;
            }
            return true;
        }
        public static bool HasAllPositive(params int[] vs)
        {
            foreach (int i in vs)
            {
                if (i <= 0)
                    return false;
            }
            return true;
        }
        public static bool IsAllUsable(params INode[] nodes)
        {
            foreach(var n in nodes)
            {
                if (n == null)
                    return false;
                if (!n.IsUsable())
                    return false;
            }
            return true;
        }
    }
    public partial class GColor
    {
        public int Hue = 300;
        public int Sat = 50;
        public int Val = 50;
        public void AddHue(int h)
        {
            Hue += h;
            if (Hue > 360)
                Hue = Hue - (Hue / 360) * 360;
            else if (Hue < 0)
                Hue = 360 + (Hue - (Hue / 360) * 360);
        }
        public void AddSat(int s)
        {
            Sat += s;
            if (Sat > 75)
                Sat = 75;
            else if (Sat < 0)
                Sat = 0;
        }
        public void AddVal(int v)
        {
            Val += v;
            if (Val > 100)
                Val = 100;
            else if (Val < 30)
                Val = 30;
        }
        public void SetRandom(int seed)
        {
            System.Random r = new System.Random(seed);
            Hue = 0;
            Sat = 50;
            Val = 50;
            AddHue(r.Next());
            AddSat(r.Next());
            AddVal(r.Next());
        }
        public void SetWhite()
        {
            Sat = 0; Val = 100;
        }
        public void SetBlack()
        {
            Sat = 100; Val = 0;
        }
        public void SetGrey(int light)
        {
            Sat = 0;
            Val = light;
            if (Val > 100)
                Val = 100;
            else if (Val < 0)
                Val = 0;
        }
        public bool CopyFrom(GColor gcolor)
        {
            if (gcolor != null) 
            {
                Hue = gcolor.Hue;
                Sat = gcolor.Sat;
                Val = gcolor.Val;
                return false;
            }
            return true;
        }
        public bool CopyTo(GColor target)
        {
            if (target == null)
                return true;
            target.Hue = Hue;
            target.Sat = Sat;
            target.Val = Val;
            return false;
        }
        public void Align()
        {
            if(Hue > 360) { Hue = 360; }
            else if(Hue < 0) { Hue = 0; }
            if(Sat > 100) { Sat = 100; }
            else if(Sat < 0) { Sat = 0; }
            if(Val > 100) { Val = 100; }
            else if(Val < 0) { Val = 0; }
        }
        public GColor() { }
    }
    /*
    public class SampleRule : Rule
    {
        public class CSample : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CSample";
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CSample c = json.ToObject<CSample>();
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
            public override Concept Copy()
            {
                var c = Spawn<CSample>();
                if(c == null)
                    return null;
                // do something
                return c;
            }
        }
        public Hook<int, object> HSampleDestroy = new Hook<int, object>();
        private int _ctn_sample = -1;
        public SampleRule()
        {
            _ctn_sample = Concept.Spawn<CSample>().TypeNumber;
        }
        public bool BeSample(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_sample))
                return true;
            // add concepts
            var c = AddConcept<CSample>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroySample(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CSample>(_ctn_sample);
            if (c == null)
            {
                card.Remove(_ctn_sample);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HSampleDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_sample);
            return false;
        }
        public bool SomeAction(Card card)
        {
            // check and get concepts
            // check condition
            // make hook input
            // do actions
            // make hook calling
            return false;
        }
    }
    */
}