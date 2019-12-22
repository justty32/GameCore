﻿using System;
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
        public CoreInfo CoreInfo {get; private set;} = CoreInfo.Instance; 
        public Config Config { get; private set; } = null;
        public Load Load { get; private set; } = null;
        public Save Save { get; private set; } = null;
        private Core(){}
        public bool Init(INeed needed_interface, Config config = null)
        {
            if(needed_interface == null)
                return true;
            State = new State();
            Config = new Config();
            if(config != null)
                if(Config.Set(config))
                    return true;
            else
                if(Config.FromJsonString(INeed.ImportConfig()))
                    return true;
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
            Random = null;
            WorldInfo = new WorldInfo();
        }
        public bool DataInit( // many parameters to Init everythings
            WorldInfo world_info
        ){
            if(world_info == null)
                return true;
            // make instances fo things to visit
            WorldInfo = world_info;
            Random = new Random(WorldInfo.Now_seed);
            component_spawner_type_name_set = new Dictionary<string, int>();
            component_spawner_list = new Dictionary<int, Base.Component.ISpawner>();
            Cards = new CardList();
            HookManager = new Base.HookManager();
            Rules = new Rules();
            // do Init
            Rules.Init();
            return false;
        }
        public WorldInfo WorldInfo {get; private set;} = new WorldInfo();
        public string World_name {get => WorldInfo.World_name; set => WorldInfo.World_name = value;}
        private int _init_seed {get => WorldInfo.Init_seed; set => WorldInfo.Init_seed = value;}
        private int _now_seed {get => WorldInfo.Now_seed; set => WorldInfo.Now_seed = value;}
        internal Random Random { get; private set; } = null;
        internal Dictionary<string, int> component_spawner_type_name_set { get; private set; }// don't edit it !!!
        internal Dictionary<int, Component.ISpawner> component_spawner_list { get; private set; }// don't edit it !!!
        internal int Card_max_number {get => WorldInfo.Card_max_number; set => WorldInfo.Card_max_number = value;}
        public CardList Cards{ get; private set;}
        public HookManager HookManager { get; private set; }
        public Rules Rules { get; private set; }
    }
}
