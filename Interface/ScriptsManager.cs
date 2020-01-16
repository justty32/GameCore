using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using GameCore.Interface;

// TODO: make a connection of sciprt and outside script, include Add(), Remove(), ....

namespace GameCore.Interface
{
    public class ScriptsManager
    {
        public Dictionary<string, object> Scripts = new Dictionary<string, object>();
        public Script<Tin, Tout> Get<Tin, Tout>(string script_name)
            where Tin : class
            where Tout : class
        {
            if (script_name == null)
                return null;
            if (!Scripts.ContainsKey(script_name))
                return null;
            return Scripts[script_name] as Script<Tin, Tout>;
        }
    }
    public class Script<Tin, Tout>
        where Tin : class
        where Tout : class
    {
        public Tout Call(Tout tout)
        {
            return null;
        }
        /*public KeyValuePair<bool, Tout> CallB(Tout tout)
        {
            var result = Call(tout);
            if (result == null)
                return new KeyValuePair<bool, Tout>(true, result);
            else
                return new KeyValuePair<bool, Tout>(false, result);
        }*/
    }
}
