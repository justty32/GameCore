using System;
using System.Collections.Generic;
using System.Text;

// TODO: load data, basic datapreload mechanism, object pool for card
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
            public Map.LandRule LandRule { get; private set; }
            public Map.TileRule TileRule { get; private set; }
            public Map.WorldRule WorldRule { get; private set;}
            public Map.PlanetRule PlanetRule { get; private set;}
            public Map.TerrainRule TerrainRule { get; private set;}
            public Map.LandformRule LandformRule { get; private set;}
            public _Rules()
            {
                // make instances
                TimeRule = new Root.TimeRule();
                LocationRule = new Root.LocationRule();
                LandRule = new Map.LandRule();
                TileRule = new Map.TileRule();
                WorldRule = new Map.WorldRule();
                PlanetRule = new Map.PlanetRule();
                TerrainRule = new Map.TerrainRule();
                LandformRule = new Map.LandformRule();
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
            _component_spawner_list = null;
            _component_spawner_type_name_set = null;
            _card_number_distribute_reference = -1;
            Random = null;
            _now_seed = -1;
            _init_seed = -1;
        }
        public void DataInit( // many parameters to Init everythings
            int init_seed,
            int now_seed,
            int card_number_distribute_reference,
            Root.TimeRule.Time now_time
        ){
            // make instances fo things to visit
            _init_seed = init_seed;
            _now_seed = now_seed;
            Random = new Random(_now_seed);
            _component_spawner_type_name_set = new Dictionary<string, int>();
            _component_spawner_list = new Dictionary<int, Base.Component.ISpawner>();
            _card_number_distribute_reference = card_number_distribute_reference;
            Cards = new Base.CardList();
            HookManager = new Base.HookManager();
            Rules = new _Rules();
            // do Init
            Rules.Init(now_time);
        }
        private int _init_seed = -1;
        private int _now_seed = -1; 
        internal Random Random { get; private set; } = null;
        internal Dictionary<string, int> _component_spawner_type_name_set { get; private set; }// don't edit it !!!
        internal Dictionary<int, Base.Component.ISpawner> _component_spawner_list { get; private set; }// don't edit it !!!
        internal int _card_number_distribute_reference = -1; // don't edit it !!!
        public Base.CardList Cards{ get; private set;}
        public Base.HookManager HookManager { get; private set; }
        public _Rules Rules { get; private set; }
    }
}
