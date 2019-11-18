using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public class Core
    {
        // Defines
        public class Rules
        {
            public Root.TimeRule TimeRule { get; }
            public Root.LocationRule LocationRule { get; }
            public Rules(
                Root.TimeRule.Time now_time,
                int LocationRule_location_number_distribute_reference
            )
            {
                TimeRule = new Root.TimeRule();
                TimeRule.SetNowTime(now_time);
                LocationRule = new Root.LocationRule(LocationRule_location_number_distribute_reference);
            }
        }

        // Gamecore
        // about GetSet, Script, Module, SaveLoad... 
        // only reset while start the program
        public static Core Instance { get; } = new Core();

        // World data
        // about world's rules, datas....
        // reset while load a new world

        //TODO: make a function, to init the rules
        //just a place holder
        public Rules rules { get; } = new Rules(new Root.TimeRule.Time(), 0);
    }
}
