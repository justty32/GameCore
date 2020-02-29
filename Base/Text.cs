using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GameCore.Base
{
    public class GText
    {
        public int Type = (int)Sort.Name;
        public int Number = 0;
        public List<int> Parameters = new List<int>();
        public enum Sort
        {
            Name, Talk, Descipt
        }
        public bool CopyFrom(GText text)
        {
            if (text == null)
                return true;
            Type = text.Type;
            Number = text.Number;
            Parameters = new List<int>(text.Parameters);
            return false;
        }
    }
}
