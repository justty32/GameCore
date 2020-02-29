using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    public class Semblance
    {
        public Sort Type = 0;
        public int Number = 0;
        public int Sound = 0;
        public List<int> Attributes = new List<int>();
        public List<int> SoundAttributes = new List<int>();
        public enum Sort{
            Partical, Human, Beast, Plant
                , Item, Terrain, Building, Material
        }
    }
}
