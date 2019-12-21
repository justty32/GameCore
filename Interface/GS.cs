using System;
namespace GameCore.Interface
{
    public class Get
    {
        
    }
    public class Set
    {
        public bool Config(Config config)
        {
            return Core.Instance.Config.Set(config);
        }
    }
}