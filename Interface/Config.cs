using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GameCore.Interface
{
    public class Config
    {
        // used in Core, not Data
        // Difference with CoreInfo, is these configurations are changeable every time
        // need to offer a instance, while Core.Init()

        public bool is_Load_Card_While_Not_In_gList = true;
    }
}
