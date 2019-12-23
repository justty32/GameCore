using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace GameCore.Base
{
    public class WorldInfo
    {
        public string World_name = null;
        public int Init_seed = -1;
        public int Now_seed = -1; 
        internal int Card_max_number = -1; // don't touch it !!!
    }
}