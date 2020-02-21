using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    class Text
    {
        public int Type = (int)Sort.Name;
        public int Number = 0;
        public List<int> Parameters = new List<int>();
        public enum Sort
        {
            Name, Talk, Descipt
        }
    }
}
