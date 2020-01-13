using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using GameCore.Interface;

// TODO: object pool for card
//       create new game, exit game
//       load/save game

namespace GameCore
{
    public class Core
    {
        private static Core p_instance = null;
        public static INeed INeed {get => p_instance._i_need;}
        public static State State { get => p_instance._state;}
        public static Config Config { get => p_instance._config;}
        public static Load Load { get => p_instance._load;}
        public static Save Save { get => p_instance._save;}
        private INeed _i_need = null;
        private State _state = null;
        private Config _config = null;
        private Load _load = null;
        private Save _save = null;
        private Core(){}
        public static bool Init(INeed needed_interface, Config config = null)
        {
            if(needed_interface == null)
                return true;
            p_instance = new Core();
            p_instance._state = new State();
            p_instance._i_need = needed_interface;
            p_instance._load = new Load();
            p_instance._save = new Save();
            if(config != null)
                p_instance._config = config;
            else
                return true;
            return false;
        }
        public static void Clear(bool are_you_sure = false){ 
            if(are_you_sure)
                p_instance = null;
        }
        
        public static string DirName {get => p_instance._dir_name;} //target save dir
        public static Random Random { get => p_instance._random;}
        public static ConceptManager ConceptManager {get => p_instance._concept_mananager;}
        public static CardList Cards{ get => p_instance._cards;}
        public static HookManager HookManager { get => p_instance._hook_manager;}
        public static RuleManager Rules { get => p_instance._rule_manager;}
        private string _dir_name = null;
        private Random _random = null;
        private ConceptManager _concept_mananager = null;
        private CardList _cards = null;
        private HookManager _hook_manager = null;
        private RuleManager _rule_manager = null;
        public static bool LoadGame(string save_name)
        {
            if(save_name == null)
                return true;
            // check save dir exist
            if(!Core.INeed.IsSaveDirExist(save_name) || !Core.INeed.IsSaveDirLegal(save_name))
                return true;
            // set save name
            p_instance._dir_name = save_name;
            return false;
        }
        public static bool SaveGame(string save_name = null)
        {
            // must specific the save dir name
            if(save_name == null)
                return true;
            // change target
            p_instance._dir_name = save_name;
            // if not cover, create new dir
            if(!Core.INeed.IsSaveDirExist(save_name)){
                if(Core.INeed.NewSaveDir(save_name))
                    return true;
                if(Core.INeed.IsSaveDirLegal(save_name))
                    return true;
            }
            return false;
        }
        public static bool CreateNewGame()
        {
            return false;
        }
        public static bool ExitGame()
        {
            return false;
        }
    }
}
