using System.Collections.Generic;

namespace GameCore.Interface
{
    // this is using json.serialize to make json string output
    // so variables in it should be careful
    public class Config
    {
        // used in Core, not Data
        // Difference with CoreInfo, these configurations are changeable every time
        // need to be offered a instance in Core, while Core.Init()
        public List<string> RulesInitOrder;
        public bool MultiThreadIO = true;
        public int MultiThreadIOMaxThreadCount = 4;
        public long MultiThreadIOExecuteLimitTimes = 400000000000;
        public bool MultiThreadIOIgnoreExecuteLimitTimes = true;
        public Config()
        {
            RulesInitOrder = new List<string>();
        }
    }
}