﻿using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Base;
using GameCore.Interface;

//  rule's init order list, annotations, logs, try catch replacing
//  Dynamic rules, scripts, modules, simplify concept acess
//  resources relation in Interface
//  language interface
// hair, face, nose, eyes...
//body adjust, movement,

namespace GameCore
{
    public partial class Core
    {
        public static bool is_debug = false;
        private static Core p_instance = null;
        public static INeed INeed { get => p_instance._i_need; }
        public static State State { get => p_instance._state; }
        public static Config Config { get => p_instance._config; }
        public static Load Load { get => p_instance._load; }
        public static Save Save { get => p_instance._save; }
        public static Common Common { get => p_instance._common; }
        private INeed _i_need = null;
        private State _state = null;
        private Config _config = null;
        private Load _load = null;
        private Save _save = null;
        private Common _common= null;
        private Core(){}
        public static bool Init(INeed needed_interface, Config config = null)
        {
            p_instance = new Core();
            p_instance._state = new State();
            p_instance._state.Log.AppendLine("Core initializing...");
            if (needed_interface == null)
                return p_instance._state.T(State.Ar.PN, "needed interface null");
            p_instance._i_need = needed_interface;
            p_instance._load = new Load();
            p_instance._save = new Save();
            p_instance._common = new Common();
            if (config != null)
            {
                p_instance._state.Log.AppendLine("load config from function's parameter");
                p_instance._config = config;
            }
            else
            {
                p_instance._state.Log.AppendLine("load config from file");
                if (p_instance._load.Config())
                {
                    p_instance._config = new Config();
                    return p_instance._state.T(State.Ar.B, "loading config failed");
                }
            }
            p_instance._state.Log.AppendLine("load config success");
            p_instance._state.AppendLogLine("loading commmon resource");
            if(p_instance._load.CommonResource())
                p_instance._state.AppendLogLine("load commmon resource failed...");
            p_instance._state.AppendLogLine("Core initialized success...");
            return false;
        }
        public static void Clear(bool are_you_sure = false){ 
            if(are_you_sure)
                p_instance = null;
            p_instance._state.Log.AppendLine("Core's instance clear");
        }

        public static string DirName {get => p_instance._dir_name;} //target save dir
        public static ConceptManager ConceptManager {get => p_instance._concept_mananager;}
        public static SaveInfo SaveInfo { get => p_instance._save_info; }
        public static CardList Cards{ get => p_instance._cards;}
        public static HookManager HookManager { get => p_instance._hook_manager;}
        public static RuleManager _RuleManager { get => p_instance._rule_manager;}
        public static RulesCollection Rules { get => p_instance._rules; }
        public static Dynamic Dynamic { get => p_instance._dynamic; }
        public static ScriptEnv ScriptEnv { get => p_instance._script_env; }
        private string _dir_name {
            get { return _dir_name_in; }
            set {
                p_instance._cards.ResetChangedCards();
                _dir_name_in = value;
            } }
        private string _dir_name_in = null;
        private ConceptManager _concept_mananager = null;
        private SaveInfo _save_info = null;
        private CardList _cards = null;
        private HookManager _hook_manager = null;
        private RuleManager _rule_manager = null;
        private RulesCollection _rules = null;
        private Dynamic _dynamic = null;
        private ScriptEnv _script_env = null;
        public static bool LoadGame(string save_name, bool load_all_cards = false)
        {
            if (p_instance == null)
                return p_instance._state.T(State.Ar.B, "core hasn't initialized yet, load game failed");
            if (save_name == null)
                return p_instance._state.T(State.Ar.PN, "target save dir isn't exist");
            p_instance._state.Log.Append("...Loading game from - ");
            p_instance._state.Log.AppendLine(save_name);
            if (!(p_instance._i_need.IsSaveDirExist(save_name) && p_instance._i_need.IsSaveDirLegal(save_name)))
                return p_instance._state.T(State.Ar.B, "target save dir isn't exist");
            p_instance._concept_mananager = new ConceptManager();
            p_instance._dynamic = new Dynamic();
            p_instance._cards = new CardList();
            p_instance._dir_name = save_name;
            p_instance._save_info = new SaveInfo();
            p_instance._hook_manager = new HookManager();
            p_instance._rule_manager = new RuleManager();
            p_instance._script_env = new ScriptEnv();
            p_instance._rules = new RulesCollection();
            p_instance._rule_manager.Rules = p_instance._rules;
            p_instance._state.AppendLogLine("load save info");
            if (Load.SaveInfo())
                return p_instance._state.T(State.Ar.B, "failed");
            p_instance._state.AppendLogLine("clear the script enviroment first");
            p_instance._i_need.ClearScriptEnv();
            p_instance._state.AppendLogLine("initailize the script enviroment");
            if (p_instance._i_need.SetScriptEnv(p_instance._script_env))
                p_instance._state.AppendLogLine("script enviroment initialize failed");
            p_instance._state.AppendLogLine("the script enviroment initialize finished");
            p_instance._state.AppendLogLine("start rules initializing");
            p_instance._rule_manager.Init(p_instance._config.RulesInitOrder);
            p_instance._state.AppendLogLine("load rules");
            if (Load.Rules())
                return p_instance._state.T(State.Ar.B, "failed");
            if (load_all_cards)
            {
                p_instance._state.AppendLogLine("loading cards");
                if (p_instance._config.MultiThreadIO)
                {
                    p_instance._state.T(State.Ar.B, "load cards multi threading");
                    int[] numbers = null;
                    if (Core.Cards.MaxNumber >= 0)
                    {
                        numbers = new int[Core.Cards.MaxNumber + 1];
                        for (int i = 0; i < Core.Cards.MaxNumber + 1; i++)
                            numbers[i] = i;
                        if (Load.MulCard(numbers))
                            return p_instance._state.T(State.Ar.B, "load cards failed");
                    }
                    else
                        p_instance._state.AppendLogLine("cards amount is zero, no need to load");
                    p_instance._state.T(State.Ar.B, "load cards multi threading finished");
                }
                else
                {
                    if (Load.AllCards())
                        return p_instance._state.T(State.Ar.B, "load cards failed");
                }
            }
            p_instance._state.AppendLogLine("load game successed");
            //TODO: load scripts, modules, dynamic...
            return false;
        }
        public static bool SaveGame(string save_name = null
            , bool save_all_cards = false, bool[] start_new_thread_to_save_card = null)
        {
            if (p_instance == null)
                return p_instance._state.T(State.Ar.B, "core hasn't initialized yet, load game failed");
            if(save_name == null && p_instance._dir_name == null)
                return p_instance._state.T(State.Ar.B, "save dir's name not specificed while saving game");
            bool is_save_all_cards = save_all_cards;
            // because the accessing of _dir_name will wash it, so make a copy
            int[] changed_cards = new int[Cards.ChangedCards.Count];
            Cards.ChangedCards.CopyTo(changed_cards);
            string pre_save_name = p_instance._dir_name;
            p_instance._dir_name = save_name == null ? pre_save_name : save_name;
            p_instance._state.Log.Append("save game into - ");
            p_instance._state.Log.AppendLine(save_name);
            if(pre_save_name != null && p_instance._i_need.IsSaveDirLegal(pre_save_name))
                if(!p_instance._dir_name.Equals(pre_save_name) && !p_instance._i_need.IsSaveDirExist(save_name))
                    p_instance._i_need.CopySaveData(pre_save_name, p_instance._dir_name);
            if (!Core.INeed.IsSaveDirExist(p_instance._dir_name)){
                p_instance._state.Log.Append(p_instance._dir_name);
                p_instance._state.Log.AppendLine("not exist, start create new dir");
                if(Core.INeed.NewSaveDir(p_instance._dir_name))
                    return p_instance._state.T(State.Ar.B, "create new save dir failed, save game failed");
                if(!Core.INeed.IsSaveDirLegal(p_instance._dir_name))
                    return p_instance._state.T(State.Ar.B, "create new save dir failed, save game failed");
                is_save_all_cards = true;
            }
            p_instance._state.Log.AppendLine("saving cards");
            if (is_save_all_cards)
            {
                p_instance._state.AppendLog("save all cards");
                if (Core.Config.MultiThreadIO) { p_instance._state.AppendLog("-- multi thread"); }
                if (start_new_thread_to_save_card!= null) { p_instance._state.AppendLogLine("-- start new thread"); }
                if (Save.AllCards(Core.Config.MultiThreadIO, start_new_thread_to_save_card))
                    return p_instance._state.T(State.Ar.B, "save failed");
            }
            else
            {
                if (Core.Config.MultiThreadIO) { p_instance._state.AppendLog("-- multi thread"); }
                if (start_new_thread_to_save_card!= null) { p_instance._state.AppendLogLine("-- start new thread"); }
                if (Save.Cards(changed_cards, Core.Config.MultiThreadIO, start_new_thread_to_save_card))
                    return p_instance._state.T(State.Ar.B, "save failed");
            }
            p_instance._state.Log.AppendLine("saving rules");
            if (Save.Rules())
                return p_instance._state.T(State.Ar.B, "save rules failed");
            p_instance._state.Log.AppendLine("saving save info");
            if (Save.SaveInfo())
                return p_instance._state.T(State.Ar.B, "save save info failed");
            p_instance._state.Log.AppendLine("save finished success.");
            //TODO: save cards, scripts, modules, dynamic lists...
            return false;
        }
        public static bool CreateNewGame(string save_name = null)
        {
            if (save_name == null)
                return true;
            if (p_instance == null)
                return true;
            if(p_instance._i_need.IsSaveDirExist(save_name))
                return State.AppendLogLine("the save dir has already exist");
            p_instance._state.Log.AppendLine("creating new game...");
            p_instance._state.Log.AppendLine("copy init data into new save dir...");
            //TODO: setting more templates of init save data types, not only new game
            if (p_instance._i_need.CopyInitSaveData("NewWorld", save_name))
                return State.AppendLogLine("copy failed");
            p_instance._state.Log.AppendLine("loading init data...");
            if (LoadGame(save_name))
                return State.AppendLogLine("load game failed");
            p_instance._state.Log.AppendLine("init game success...");
            return false;
        }
        public static bool ExitGame()
        {
            if (p_instance == null)
                return true;
            p_instance._state.Log.AppendLine("release game data...");
            //TODO:release scripts, modules...
            p_instance._state.AppendLogLine("clear the script enviroment");
            p_instance._i_need.ClearScriptEnv();
            p_instance._script_env = null;
            p_instance._dynamic = null;
            p_instance._rules = null;
            p_instance._rule_manager = null;
            p_instance._hook_manager = null;
            p_instance._cards = null;
            p_instance._concept_mananager = null;
            //p_instance._dir_name = null;
            p_instance._state.Log.AppendLine("game data release finished");
            return false;
        }
    }
}
