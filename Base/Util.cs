using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    public class Util
    {
        public interface INode
        {
            bool IsUsable();
        }
        public bool IsUsable(INode node)
        {
            if (node != null)
                if (node.IsUsable())
                    return true;
            return false;
        }
    }
}
