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
        public static bool is_debug = false;
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
            p_instance = new Core();
            p_instance._state = new State();
            p_instance._state.Log.AppendLine("Core initializing...");
            if (needed_interface == null)
                return p_instance._state.T(State.Ar.PN);
            p_instance._i_need = needed_interface;
            p_instance._load = new Load();
            p_instance._save = new Save();
            if (config != null)
            {
                p_instance._state.Log.AppendLine("load config from function's parameter");
                p_instance._config = config;
            }
            else
            {
                p_instance._state.Log.AppendLine("load config from file");
                if (p_instance._load.Config())
                    return true;
            }
            return false;
        }
        public static void Clear(bool are_you_sure = false){ 
            if(are_you_sure)
                p_instance = null;
            p_instance._state.Log.AppendLine("Core's instance clear");
        }

        public static string DirName {get => p_instance._dir_name;} //target save dir
        public static ConceptManager ConceptManager {get => p_instance._concept_mananager;}
        public static CardList Cards{ get => p_instance._cards;}
        public static HookManager HookManager { get => p_instance._hook_manager;}
        public static RuleManager Rules { get => p_instance._rule_manager;}
        public static Dynamic Dynamic { get => p_instance._dynamic; }
        private string _dir_name = null;
        private ConceptManager _concept_mananager = null;
        private CardList _cards = null;
        private HookManager _hook_manager = null;
        private RuleManager _rule_manager = null;
        private Dynamic _dynamic = null;
        public static bool LoadGame(string save_name)
        {
            p_instance._state.Log.AppendLine("loading game from - ");
            p_instance._state.Log.Append(save_name);
            if (save_name == null)
                return p_instance._state.T(State.Ar.PN);
            if (!p_instance._i_need.IsSaveDirExist(save_name) || !p_instance._i_need.IsSaveDirLegal(save_name))
                return p_instance._state.T(State.Ar.B, "target save dir not exist");
            p_instance._dir_name = save_name;
            p_instance._concept_mananager = new ConceptManager();
            p_instance._cards = new CardList();
            p_instance._hook_manager = new HookManager();
            p_instance._rule_manager = new RuleManager();
            p_instance._rule_manager.Init();
            p_instance._dynamic = new Dynamic(null);
            //TODO: load scripts, modules, dynamic...
            return false;
        }
        public static bool SaveGame(string save_name = null)
        {
            if(save_name == null)
                return true;
            p_instance._dir_name = save_name;
            p_instance._state.Log.AppendLine("save game into - ");
            p_instance._state.Log.AppendLine(save_name);
            if (!Core.INeed.IsSaveDirExist(save_name)){
                if(Core.INeed.NewSaveDir(save_name))
                    return true;
                if(Core.INeed.IsSaveDirLegal(save_name))
                    return true;
            }
            //TODO: save cards, scripts, modules, dynamic lists...
            return false;
        }
        public static bool CreateNewGame(string save_name = null)
        {
            if (save_name == null)
                return true;
            p_instance._state.Log.AppendLine("creating new game...");
            p_instance._state.Log.AppendLine("copy init data into new save dir...");
            //TODO: setting more templates of init save data types, not only new game
            p_instance._i_need.CopyInitSaveData("new game", save_name);
            p_instance._state.Log.AppendLine("loading init data...");
            if (LoadGame(save_name))
                return true;
            return false;
        }
        public static bool ExitGame()
        {
            p_instance._state.Log.AppendLine("release game data...");
            //TODO:release scripts, modules...
            p_instance._dynamic = null;
            p_instance._rule_manager = null;
            p_instance._hook_manager = null;
            p_instance._cards = null;
            p_instance._concept_mananager = null;
            p_instance._dir_name = null;
            p_instance._state.Log.AppendLine("game data release finished");
            return false;
        }
    }
}
