using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    class Semblance
    {
        public int Type = 0;
        public int Number = 0;
        public List<int> Attributes = new List<int>();
        public enum Sort{
            Partical, Human, Beast, Plant
                , Item, Terrain, Building
        }
    }
}
