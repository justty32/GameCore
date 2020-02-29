using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Thing
{
    public class WeaponRule : Rule
    {
        public class CWeapon : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CWeapon";
            public int NeedHand = 1;
            public bool[] Type = new bool[4];
            public Melee Melee;
            public Throw Throw;
            public Bow Bow;
            public Gun Gun;
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    var c = Spawn<CWeapon>();
                    for (int i = 0; i < 4; i++)
                        Type[i] = (bool)(((JArray)json["Type"])[i]);
                    if(Type[0] == true)
                        Melee = json["Melee"].ToObject<Melee>();
                    if(Type[1] == true)
                        Throw = json["Throw"].ToObject<Throw>();
                    if(Type[2] == true)
                        Bow = json["Bow"].ToObject<Bow>();
                    if(Type[3] == true)
                        Gun = json["Gun"].ToObject<Gun>();
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
                    JObject json = base.ToJsonObject();
                    if (json == null)
                        return null;
                    JArray jarray_type = new JArray();
                    for(int i = 0; i < Type.Length; i++)
                        jarray_type.Add(Type[i]);
                    json.Add("Type", jarray_type);
                    if(Type[0] == true)
                    {
                        JObject jo_melee = JObject.FromObject(Melee);
                        json.Add("Melee", jo_melee);
                    }
                    if(Type[1] == true)
                    {
                        JObject jo_throw = JObject.FromObject(Throw);
                        json.Add("Throw", jo_throw);
                    }
                    if(Type[2] == true)
                    {
                        JObject jo_bow = JObject.FromObject(Bow);
                        json.Add("Bow",jo_bow);
                    }
                    if(Type[3] == true)
                    {
                        JObject jo_gun = JObject.FromObject(Gun);
                        json.Add("Gun", jo_gun);
                    }
                    return json;
                }catch(Exception e)
                {
                    return Core.State.WriteException<JObject>(e);
                }
            }
            public override Concept Copy()
            {
                var c = Spawn<CWeapon>();
                if(c == null)
                    return null;
                c.NeedHand = NeedHand;
                for (int i = 0; i < 4; i++)
                    c.Type[i] = Type[i];
                if (Type[0] == true)
                    c.Melee = Melee;
                if (Type[1] == true)
                    c.Throw = Throw;
                if (Type[2] == true)
                    c.Bow = Bow;
                if (Type[3] == true)
                    c.Gun = Gun;
                return c;
            }
            public override bool IsUsable()
            {
                if (Type == null)
                    return false;
                for (int i = 0; i < 4; i++)
                    if (Type[i] == true)
                        return true;
                return false;
            }
        }
        public class CAmmo : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CAmmo";
            public int AmountMax = 0;
            public int Amount = 0;
            public int DamageCut;
            public int DamageStab;
            public int DamageBlunt;
            public int Penetrate; //穿透
            public int Heavy;
            public int Disturb;
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CAmmo c = json.ToObject<CAmmo>();
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
                var c = Spawn<CAmmo>();
                if(c == null)
                    return null;
                c.AmountMax = AmountMax;
                c.Amount = Amount;
                c.DamageBlunt = DamageBlunt;
                c.DamageCut = DamageCut;
                c.DamageStab = DamageStab;
                c.Penetrate = Penetrate;
                c.Heavy = Heavy;
                c.Disturb = Disturb;
                return c;
            }
        }
        public struct Melee
        {
            public int DamageCut;
            public int DamageStab;
            public int DamageBlunt;
            public int Penetrate; //穿透
            public int Precise; //精準
            public int Cooldown;
            public int Speed;
            public int DistanceMax;
            public int DistanceMin;
            public int Heavy; //沉重
            public int Disturb; //擾亂
        }
        public struct Throw
        {
            public int Damage; //力道
            public int Precise; //精準
            public int PreciseTime;
            public int Penetrate; //穿透
            public int Heavy; //沉重
            public int Disturb; //擾亂
            public int Cooldown;
            public int Speed;
            public int FlySpeed;
            public ShootTrackType DistanceType;
            public int DiffusionScope;
            public int DistanceMax;
            public int DistanceMin;
        }
        public struct Bow
        {
            public int Damage; //力道
            public int Precise; //精準
            public int PreciseTime;
            public int Penetrate; //穿透
            public int Cooldown;
            public int Speed;
            public int ArrowSpeed;
            public int DistanceMax;
            public int DistanceMin;
            public ShootTrackType TrackType;
            public int DiffusionScope;
            public int Heavy; //沉重
            public int Disturb; //擾亂
            public int SpecificAmmo;
            public AmmoType AmmoType;
        }
        public struct Gun
        {
            public int DamagePerTime; //力道
            public int Precise; //精準
            public int PreciseTime;
            public int Penetrate; //穿透
            public int ConsumeAmmoPerTime;
            public int ReloadTime;
            public int MagazineCap;
            public int HeatMax;
            public int HeatPerTime;
            public int CoolPerTime;
            public int Speed;
            public int BulletSpeed;
            public int DistanceMax;
            public int DistanceMin;
            public ShootTrackType TrackType;
            public int DiffusionScope;
            public int Heavy; //沉重
            public int Disturb; //擾亂
            public int SpecificAmmo;
            public AmmoType AmmoType;
            public int AmmoPerShoot;
        }
        public enum WeaponType
        {
            Melee, Throw, Bow, Gun
        }
        public enum AmmoType
        {
            Arrow, Bullet, Ball
        }
        public enum ShootTrackType
        {
            Parabola, Laser, Diffusion
        }
        public Hook<int, object> HWeaponDestroy = new Hook<int, object>();
        public Hook<int, object> HAmmoDestroy = new Hook<int, object>();
        private int _ctn_weapon = -1;
        private int _ctn_ammo = -1;
        public WeaponRule()
        {
            _ctn_weapon = Concept.Spawn<CWeapon>().TypeNumber;
            _ctn_ammo = Concept.Spawn<CAmmo>().TypeNumber;
        }
        public bool BeAmmo(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_ammo))
                return true;
            // add concepts
            var c = AddConcept<CAmmo>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyAmmo(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CAmmo>(_ctn_ammo);
            if (c == null)
            {
                card.Remove(_ctn_ammo);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HAmmoDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_ammo);
            return false;
        }
        public bool BeWeapon(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_weapon))
                return true;
            // add concepts
            var c = AddConcept<CWeapon>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyWeapon(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CWeapon>(_ctn_weapon);
            if (c == null)
            {
                card.Remove(_ctn_weapon);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HWeaponDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_weapon);
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
}
