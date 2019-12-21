using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Interface
{
    // Don't be changed by outside
    // Difference with Config.cs, is these things shouldn't be change
    // are separate by version, same version offer same member and same value.
    public class CoreInfo
    {
        public int Version { get; private set; } = 10;
        public int Card_amount_per_file { get; private set; } = 2000;
    }
}
