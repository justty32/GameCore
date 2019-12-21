using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using GameCore.Interface;

// TODO: load data, basic datapreload mechanism, object pool for card
namespace GameCore
{
    public class Core
    {
        // Gamecore
        // about GetSet, Script, Module, SaveLoad... 
        // only reset while start the program
        private static Core p_instance = null;
        public INeed INeed {get; private set;} = null;
        public State State { get; private set; } = null;
        public CoreInfo CoreInfo { get; private set; } = null; 
        public Config Config { get; private set; } = null;
        public Load Load { get; private set; } = null;
        public Save Save { get; private set; } = null;
        public Get Get {get; private set;} = null;
        public Set Set {get; private set;} = null;
        private Core(){}
        public bool Init(INeed needed_interface, Config config = null)
        {
            if(needed_interface == null)
                return true;
            State = new State();
            CoreInfo = new CoreInfo();
            Config = new Config();
            if(config != null)
                if(Config.Set(config))
                    return true;
            else
                if(Config.FromJsonString(INeed.ImportConfig()))
                    return true;
            Get = new Get();
            Set = new Set();
            Load = new Load();
            Save = new Save();
            INeed = needed_interface;
            return false;
        }
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
            component_spawner_list = null;
            component_spawner_type_name_set = null;
            card_number_distribute_reference = -1;
            Random = null;
            _now_seed = -1;
            _init_seed = -1;
            World_name = null;
        }
        public void DataInit( // many parameters to Init everythings
            string world_name,
            int init_seed,
            int now_seed,
            int card_number_distribute_reference,
            Root.TimeRule.Time now_time
        ){
            // make instances fo things to visit
            World_name = world_name;
            _init_seed = init_seed;
            _now_seed = now_seed;
            Random = new Random(_now_seed);
            component_spawner_type_name_set = new Dictionary<string, int>();
            component_spawner_list = new Dictionary<int, Base.Component.ISpawner>();
            this.card_number_distribute_reference = card_number_distribute_reference;
            Cards = new CardList();
            HookManager = new Base.HookManager();
            Rules = new Rules();
            // do Init
            Rules.Init(now_time);
        }
        public string World_name = null;
        private int _init_seed = -1;
        private int _now_seed = -1; 
        internal Random Random { get; private set; } = null;
        internal Dictionary<string, int> component_spawner_type_name_set { get; private set; }// don't edit it !!!
        internal Dictionary<int, Component.ISpawner> component_spawner_list { get; private set; }// don't edit it !!!
        internal int card_number_distribute_reference = -1; // don't touch it !!!
        public CardList Cards{ get; private set;}
        public HookManager HookManager { get; private set; }
        public Rules Rules { get; private set; }
    }
}
