using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using GameCore.Interface;

namespace GameCore.Interface
{
    public class ScriptsManager
    {
        public Dictionary<string, IScript> Scripts = new Dictionary<string, IScript>();
    }
    public interface IScript 
    {
    
    }
    public class Script<Tinput, Toutput> : IScript
    {
        public Toutput CallFunc(Tinput input)
        {
            return default;
        }
    }
}
