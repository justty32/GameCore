using GameCore.Base;
using Newtonsoft.Json.Linq;
using System;

namespace GameCore.Creature
{
    public class CreatureRule : Rule
    {
        public class Basix
        {
            public int Str;
            public int Dex;
            public int Con;
            public int Int;
            public int Spi;
            public int Mag;
        }
        public class Attrs
        {
            public string Name;
            public int Race;
            public int Sex;
            public int Years;
            public bool IsAlive;
            public int HP;
            public int SP;
            public int MP;
            public int HPMax;
            public int SPMax;
            public int MPMax;
            public int Health;
            public int Awareness;
            public int Size;
            public int Height;
            public int Weight;
            public int Vision;
            public int VisionNight;
            public int MoveSpeed;
            public int CarryWeight;
        }
        public class Fight
        {
            public int MeleeDamage;
            public int MeleePrecision;
            public int DistanceDamage;
            public int DistancePrecision;
            public int Distance;
            public int Defend;
            public int Evasion;
        }
        public class CCreature : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CCreature";
            public Basix Basix = new Basix();
            public Attrs Attrs = new Attrs();
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CCreature c = json.ToObject<CCreature>();
                    return c;
                }
                catch (Exception e)
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
                }
                catch (Exception e)
                {
                    return Core.State.WriteException<JObject>(e);
                }
            }
        }
        public class Status
        {
            public int Place;
            public int HP;
            public int SP;
            public int MP;
            public int Health;
        }
        private int _ctn_Creature = -1;
        public CreatureRule()
        {
            _ctn_Creature = ConceptSpawner<CCreature>.GetSpawner().TypeNumber;
        }
    }
}