using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using Newtonsoft.Json.Linq;

namespace GameCore.Creature
{
    public class AttainmentRule : Rule
    {
        public class CAttainment : Concept
        {
            public override string TypeName => _type_name;
            private string _type_name = "CAttainment";
            public Dictionary<int, int> Knowledges = new Dictionary<int, int>();
            public Skills Skills = new Skills();
            public override Concept FromJsonObject(JObject ojson)
            {
                var json = AlignJsonOjbect(ojson);
                if (json == null)
                    return null;
                try
                {
                    CAttainment c = json.ToObject<CAttainment>();
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
                var c = Spawn<CAttainment>();
                if(c == null)
                    return null;
                c.Knowledges = new Dictionary<int, int>(Knowledges);
                c.Skills = Skills;
                return c;
            }
        }
        public struct Skills{
            public SkillsNormal Normal;
            public SkillsCraft Craft;
            public SkillsFight Fight;
            public SkillsBig Big;
            public SkillsWar War;
        }
        public struct SkillsNormal
        {
            public int Acrobatics; //特技:攀爬
            public int Fly;
            public int Swim;
            public int Ride;
            public int Diplomacy; //交涉:唬騙、威嚇、察言觀色
            public int Perform; //表演
            public int Lead; //領導
            public int SleightHand; //巧手:解除裝置
            public int Stealth; //隱匿:逃脫
            public int Survival; //生存
            public int Scout; //偵查
            public int Read; //讀書
            public int Teaching; //教育
            public int Art; //藝術
            public int Preach; //傳教
        }
        public struct SkillsCraft
        {
            public int herbalism; //採集
            public int Mining; //採礦
            public int Skinning; //剝皮
            public int Alchemy; //藥劑
            public int Smithing;  //鍛造
            public int Enchantingv; //附魔
            public int Engineer; //工程
            public int Jewelcraft; //珠寶加工
            public int Leatherwork; //制皮
            public int Tailoring; //裁縫
            public int Cooking; //烹飪
            public int Healing; //醫療
            public int Fishing; //釣魚
            public int Plantin; //種植
            public int Taming; //馴服
            public int Draw; //繪畫
            public int Write; //寫作
        };
        public struct SkillsFight
        {
            public int Sword; //劍
            public int Dagger; //匕
            public int Axe; //斧
            public int Spear; //槍
            public int Hammer; //錘
            public int Stick; //棍
            public int Shield; //盾
            public int Martial; //格鬥
            public int Throwing; //投擲
            public int Bow; //弓
            public int Crossbow; //弩
            public int Gun; //火槍
            public int Armor_heavy; //重甲
            public int Armor_medium; //中甲
            public int Armor_light; //輕甲
            public int Block; //格檔
            public int Evasion; //閃避
        };
        public struct SkillsBig
        {
            public int Diplomacy;  //外交
            public int Politics;   //政治(針對內部人事的調停、交涉，又稱手段)
            public int Intrigue; //陰謀(包括陽謀)
            public int Command;  //武力(戰術、戰場上的領導力、單個部隊的領導力)
            public int Stewardship; //管理(對產業的管理熟悉)
            public int Business;    //商業(對商業的熟悉)
        };
        public struct SkillsWar
        {
            public int Infantry; //步兵
            public int Archery; //弓兵
            public int Cavalry; //騎兵
            public int MountArchery; //弓騎兵
            public int Siege; //攻城器械
            public int Wizard; //巫師
            public int Ship; //水軍
            public int Air; //空軍
        }
        public Hook<int, object> HAttainmentDestroy = new Hook<int, object>();
        private int _ctn_attainment = -1;
        public AttainmentRule()
        {
            _ctn_attainment = Concept.Spawn<CAttainment>().TypeNumber;
        }
        public bool BeAttainment(Card card)
        {
            // check condition
            // check has concepts
            if(!UnHasConcept(card, _ctn_attainment))
                return true;
            // add concepts
            var c = AddConcept<CAttainment>(card);
            if(c == null)
                return true;
            // set attributes
            // do other actions
            return false;
        }
        public bool DestroyAttainment(Card card)
        {
            // check concepts
            // get concept
            var c = card.Get<CAttainment>(_ctn_attainment);
            if (c == null)
            {
                card.Remove(_ctn_attainment);
                return true;
            }
            // make hook input
            // do actions
            // call hook
            HAttainmentDestroy.CallAll(card.Number);
            // remove concept
            card.Remove(_ctn_attainment);
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
