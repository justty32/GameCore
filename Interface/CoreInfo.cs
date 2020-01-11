using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Interface
{
    // static data
    // Can't be changed by outside
    // Difference with Config.cs, is these things shouldn't be change
    // Be separated by version, same version offer same member and same value.
    public class CoreInfo
    {
        public static readonly string Version = "0.01";

        // not be used now
        public static readonly int Card_amount_per_file = 2000;
    }
}
