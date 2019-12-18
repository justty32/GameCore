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
        public bool IsUsable(INode node)
        {
            if (node != null)
                if (node.IsUsable())
                    return true;
            return false;
        }
    }
    public class CCom : Base.Component
    {
        public const string _type_name = "ChangeThis"; 
        public override string TypeName => _type_name ;
        public bool Init()
        {
            return false;
        }
        public override bool FromJsonObject(Newtonsoft.Json.Linq.JObject js)
        {
            if(base.FromJsonObject(js))
                return true;
            try{
                Init();
            }catch(Exception){
                return true;
            }
            return false;
        }
        public override JObject ToJsonObject()
        {
            JObject js = base.ToJsonObject();
            if(js == null)
                return null;
            try{
                // put data
            }catch(Exception){
                return null;
            }
            return js;
        }
    }
    public class RRule : Base.Rule
    {
        private int _ctn_ = -1;
        public bool Init()
        {
            return false;
        }
        public CCom AddCTile(Base.Card card)
        {
            return AddComponent<CCom>(card);
        }
        public bool AddCTile(Base.Card card, int a)
        {
            return AddComponent<CCom>(card).Init();
        }
        public bool RemoveCLocation(Base.Card card)
        {
            if(RemoveComponent<CCom>(card))
                return true;
            return false;
        }
        public override bool IsUsable()
        {
            return true;
        }
    }
}
