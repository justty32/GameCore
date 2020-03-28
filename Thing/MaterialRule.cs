using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Thing
{
    public class MaterialRule : Rule
    {
        public class CMaterial : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CMaterial";
            public int Value = 0;
            public int Pretty = 0;
            public int Ageing = 0;
            public Sort Type = Sort.None;
            public Physic Physic = new Physic();
            public Chemist Chemist = new Chemist();
            // magic
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CMaterial c = json.ToObject<CMaterial>();
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
        public enum Sort
        {
            None, Compose, Gas, Water, Mud, Dirt, Rock, Metal,
            Blood, Meat, Bone, Tendon, Leather, Fur,
            Feather, Horn, Animal,
            Wood, Leaf, Fiber, Vine, Fungus, Plant,
            Crystal, Power
        }
        public class Physic
        {
            public int Color = 0;
            public int Weight = 0;
            public int Size = 0;
            public int Hard = 0; //硬度
            public int Tough = 0; //韌性
            public int Duct = 0; //延展性
            public int Brit = 0; //脆度
            public int Fric = 0; //摩擦力
            public int Elast = 0; //彈性(是否能作用視材質而定)
            public int Visco = 0; //黏性
            public int Optic = 0; //透光度(0為不透明)
            public int Refract = 0; //折射(鑽石極高)
            public int Reflect = 0; //反射
            public int Thermal = 0; //導熱
        }
        public class Chemist
        {
            public int Activity = 0; //活性
            public int Burn = 0; //可燃性(燃點)(金屬等材質無燃點)
            public int Heat = 0; //燃燒放出熱量(用以計算燃燒溫度)
            public int BurnTime = 0;
            public int Melt = 0; //熔點
            public int Boil = 0; //沸點(金屬等材質無沸點)
            public int Explode = 0; //爆炸強度(擴散速度)(與強度共同決定範圍)
            public int ExplodeHeat = 0; //爆炸能量(總強度)(燃點即爆點)
            public int Corro = 0; //腐蝕
            public int AnCorro = 0; //抗腐蝕
            public int Tox = 0; //毒性(生物毒性)
            public int Eletric = 0; //導電
            public int Capacit = 0; //電容
            public int Mag = 0; //磁性
            public int AnMag = 0; //抗磁性
            public int Solub = 0; //對水的溶解速度(0為不溶於水)
            public int Absort = 0; //吸水性
        }
        public bool BeMaterial(Card card, Sort sort)
        {
            if (HasConcept(card, _ctn_material))
                return true;
            var c = AddConcept<CMaterial>(card);
            if (c == null)
                return true;
            if (sort != Sort.None)
            {
                var bp = BasicSortPhysics[(int)sort];
                var bc = BasicSortChemists[(int)sort];
                c.Physic = JObject.FromObject(bp).ToObject<Physic>();
                c.Chemist = JObject.FromObject(bc).ToObject<Chemist>();
            }
            if (c.Physic == null)
                c.Physic = new Physic();
            if (c.Chemist == null)
                c.Chemist = new Chemist();
            return false;
        }
        private int _ctn_material = 0;
        public Dictionary<int, Physic> BasicSortPhysics = new Dictionary<int, Physic>();
        public Dictionary<int, Chemist> BasicSortChemists = new Dictionary<int, Chemist>();
        public MaterialRule()
        {
            _ctn_material = Concept.Spawn<CMaterial>().TypeNumber;
            for(int i = 0; i < Enum.GetValues(typeof(Sort)).Length; i++)
            {
                BasicSortPhysics.Add(i, new Physic());
                BasicSortChemists.Add(i, new Chemist());
            }
        }
        public override bool Init()
        {
            BasicSortPhysics = new Dictionary<int, Physic>();
            BasicSortChemists = new Dictionary<int, Chemist>();
            for(int i = 0; i < Enum.GetValues(typeof(Sort)).Length; i++)
            {
                BasicSortPhysics.Add(i, new Physic());
                BasicSortChemists.Add(i, new Chemist());
            }
            return false;
        }
        public override bool IsUsable()
        {
            if (BasicSortPhysics == null || BasicSortChemists == null)
                return false;
            if (BasicSortPhysics.Count != Enum.GetValues(typeof(Sort)).Length)
                return false;
            if (BasicSortChemists.Count != BasicSortPhysics.Count)
                return false;
            return true;
        }
        public override bool FromJsonObject(JObject js)
        {
            if (base.FromJsonObject(js))
                return true;
            try
            {
                BasicSortPhysics = new Dictionary<int, Physic>();
                BasicSortChemists = new Dictionary<int, Chemist>();
                JObject jp = (JObject)js["Physic"];
                JObject jc = (JObject)js["Chemist"];
                foreach(int val in Enum.GetValues(typeof(Sort)))
                {
                    var pp = jp[Enum.GetName(typeof(Sort), val)];
                    if (pp != null)
                        BasicSortPhysics.Add(val, pp.ToObject<Physic>());
                    else
                        BasicSortPhysics.Add(val, new Physic());
                    if (BasicSortPhysics[val] == null) 
                        BasicSortPhysics.Add(val, new Physic());
                    var cc = jc[Enum.GetName(typeof(Sort), val)];
                    if (cc != null)
                        BasicSortChemists.Add(val, cc.ToObject<Chemist>());
                    else
                        BasicSortChemists.Add(val, new Chemist());
                    if (BasicSortChemists[val] == null) 
                        BasicSortChemists.Add(val, new Chemist());
                }
                if (!IsUsable())
                    return true;
            }
            catch (Exception e)
            {
                return Core.State.WriteException(e);
            }
            return false;
        }
        public override JObject ToJsonObject()
        {
            var js = base.ToJsonObject();
            try
            {
                JObject jp = new JObject();
                JObject jc = new JObject();
                foreach(int val in Enum.GetValues(typeof(Sort)))
                {
                    jp.Add(Enum.GetName(typeof(Sort), val),
                        JObject.FromObject(BasicSortPhysics[val]));
                    jc.Add(Enum.GetName(typeof(Sort), val),
                        JObject.FromObject(BasicSortChemists[val]));
                }
                js.Add("Physic", jp);
                js.Add("Chemist", jc);
            }
            catch (Exception e)
            {
                return Core.State.WriteException<JObject>(e);
            }
            return js;
        }
    }
}
