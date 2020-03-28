using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using GameCore.Base;

namespace GameCore.Interface
{
    public class ResourceManager
    {
        public SortedDictionary<string, Resource> ResourceTemplate = new SortedDictionary<string, Resource>();
        public SortedDictionary<string, string> Scripts = new SortedDictionary<string, string>();
        public List<string> ScriptsEnvPreDo = new List<string>();
        public List<string> ScriptsEnvLastDo = new List<string>();
    }
}
