using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    class Instance
    {
        private Instance pInstance = null;
        public Instance Get()
        {
            if (pInstance == null)
                pInstance = new Instance();
            return pInstance;
        }
    }
}
