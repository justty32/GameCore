using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public class Core
    {
        // Defines
        public class _Rules
        {
            public Root.TimeRule TimeRule { get; private set; }
            public Root.LocationRule LocationRule { get; private set; }
            public Root.LandRule LandRule { get; private set; }
            public Root.TileRule TileRule { get; private set; }
            public _Rules()
            {
                // make instances
                TimeRule = new Root.TimeRule();
                LocationRule = new Root.LocationRule();
                LandRule = new Root.LandRule();
                TileRule = new Root.TileRule();
            }
            public void Init( // many parameters
                Root.TimeRule.Time now_time,
                int LocationRule_location_number_distribute_reference
            )
            {
                // do Init
                TimeRule.Init();
                TimeRule.SetNowTime(now_time);
                LocationRule.Init(LocationRule_location_number_distribute_reference);
                LandRule.Init();
                TileRule.Init();
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
            // the order is reversion of Init
            _card_number_distribute_reference = -1;
            Rules = null;
            HookManager = null;
        }
        public void DataInit( // many parameters to Init everythings
            int card_number_distribute_reference,
            Root.TimeRule.Time now_time,
            int LocationRule_location_number_distribute_reference            
        ){
            // make instances fo things to visit
            _card_number_distribute_reference = card_number_distribute_reference;
            HookManager = new Base.HookManager();
            Rules = new _Rules();
            // do Init
            Rules.Init(now_time, LocationRule_location_number_distribute_reference);
        }
        internal int _card_number_distribute_reference = -1; // don't edit it !!!
        public Base.HookManager HookManager { get; private set; }
        public _Rules Rules { get; private set; }
    }
}
