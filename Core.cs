using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public class Core
    {
        // Defines
        public class State
        {
            // ActionResult means the result after doing something
            // Function can change AR, to descript the state of its processing
            public enum ActionResult
            {
                Good, ProcessBad, ParameterNull, ParameterIllegal, NotHasComponent
            }
            public ActionResult AR { get; internal set; } = ActionResult.Good;
        }
        public class _Rules
        {
            public Root.TimeRule TimeRule { get; private set; }
            public Root.LocationRule LocationRule { get; private set; }
            public Root.LandRule LandRule { get; private set; }
            public Root.TileRule TileRule { get; private set; }
            public Root.WorldRule WorldRule { get; private set;}
            public Root.PlanetRule PlanetRule { get; private set;}
            public Root.TerrainRule TerrainRule { get; private set;}
            public Root.LandformRule LandformRule { get; private set;}
            public _Rules()
            {
                // make instances
                TimeRule = new Root.TimeRule();
                LocationRule = new Root.LocationRule();
                LandRule = new Root.LandRule();
                TileRule = new Root.TileRule();
                WorldRule = new Root.WorldRule();
                PlanetRule = new Root.PlanetRule();
                TerrainRule = new Root.TerrainRule();
                LandformRule = new Root.LandformRule();
            }
            public void Init( // many parameters
                Root.TimeRule.Time now_time
            )
            {
                // do Init
                TimeRule.Init();
                TimeRule.SetNowTime(now_time);
                LocationRule.Init();
                LandRule.Init();
                TileRule.Init();
                WorldRule.Init();
                PlanetRule.Init();
                LandformRule.Init();
                TerrainRule.Init();
            }
        }

        // Gamecore
        // about GetSet, Script, Module, SaveLoad... 
        // only reset while start the program
        private static Core p_instance = null;
        public State Status { get; private set; } = new State();
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
            Rules = null;
            HookManager = null;
            foreach(var card in Cards.cards)
                card.Value.Clear();
            Cards = null;
            _card_number_distribute_reference = -1;
        }
        public void DataInit( // many parameters to Init everythings
            int card_number_distribute_reference,
            Root.TimeRule.Time now_time
        ){
            // make instances fo things to visit
            _card_number_distribute_reference = card_number_distribute_reference;
            Cards = new Base.CardList();
            HookManager = new Base.HookManager();
            Rules = new _Rules();
            // do Init
            Rules.Init(now_time);
        }
        internal int _card_number_distribute_reference = -1; // don't edit it !!!
        public Base.CardList Cards{ get; private set;}
        public Base.HookManager HookManager { get; private set; }
        public _Rules Rules { get; private set; }
    }
}
