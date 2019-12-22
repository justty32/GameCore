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
    public class CCom : Base.Component
    {
        public readonly string _type_name = "ChangeThis"; 
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
    */
    /*
    public class RRule : Base.Rule
    {
        private int _ctn_ = -1;
        public bool Init()
        {
            _ctn_ = Base.Component.GetSpawner<CCom>().Type_Number;
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
        public override bool FromJsonObject(JObject js)
        {
            if(base.FromJsonObject(js))
                return true;
            try{
                //get data
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
    */
}
