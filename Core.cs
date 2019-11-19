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
        private static Core p_instance = null;
        public static Core Instance {
            get{
                if(p_instance == null)
                    p_instance = new Core();
                return p_instance;
            }
        }
        public static void InstanceRemove(bool are_you_sure = false){ if(are_you_sure){ p_instance = null; }}

        // World data
        // about world's rules, datas....
        // reset while load a new world
        public void DataRemove(){
            _rules = null;
        }
        public void DataInit( // many parameters
            Root.TimeRule.Time now_time,
            int LocationRule_location_number_distribute_reference            
        ){
            _rules = new Rules(now_time, LocationRule_location_number_distribute_reference);
        }
        private Rules _rules = null;
        public Rules rules { get => _rules; }
    }
}
