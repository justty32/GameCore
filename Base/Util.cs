using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    public class Util
    {
        public interface INode
        {
            bool IsUsable();
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
                    JObject json = JObject.FromObject(this);
                    return json;
                }catch(Exception e)
                {
                    return Core.State.WriteException<JObject>(e);
                }
            }
        }
        private int _ctn_sample = -1;
        public SampleRule()
        {
            _ctn_sample = ConceptSpawner<CSample>.GetSpawner().TypeNumber;
        }
    }
    */
}
