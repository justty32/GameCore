using System;
using System.Text;
using System.Collections.Generic;
using GameCore.Base;
using XLua;

namespace GameCore.Interface
{
    public class ScriptEnv
    {
        public ScriptEnv()
        {
            AddStaticFunction(_ScriptEnvReserved._FunctionPaths, "CS.GameCore.Interface._ScriptEnvReserved.", false);
            Core.State.AppendLogLine("get pre/last do strings from common resource");
            for(int i = 0; i < Core.ResourceManager.ScriptsEnvPreDo.Count; i++)
                AddPreDoString(Core.ResourceManager.ScriptsEnvPreDo[i]);
            for(int i = 0; i < Core.ResourceManager.ScriptsEnvLastDo.Count; i++)
                AddLastDoString(Core.ResourceManager.ScriptsEnvLastDo[i]);
        }
        public static readonly string[] ReservedWords = new string[]
        {
            "TempData", "InputData", "OutputData"
        };
        public Dictionary<string, int> VarInt { get; private set; } = new Dictionary<string, int>();
        public Dictionary<string, string> VarString { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, bool> VarBool { get; private set; } = new Dictionary<string, bool>();
        public Dictionary<string, float> VarFloat { get; private set; } = new Dictionary<string, float>();
        public Dictionary<string, string> VarStaticFunction { get; private set; } = new Dictionary<string, string>();
        public List<string> LastDoString { get; private set; } = new List<string>();
        public List<string> PreDoString { get; private set; } = new List<string>();
        public bool IsVarNameBeUsed(string name)
        {
            if (name == null)
                return false;
            foreach (var reserved_word in ReservedWords)
                if (name.Equals(reserved_word))
                    return true;
            if (VarInt.ContainsKey(name))
                return true;
            if (VarString.ContainsKey(name))
                return true;
            if (VarBool.ContainsKey(name))
                return true;
            if (VarFloat.ContainsKey(name))
                return true;
            if (VarStaticFunction.ContainsKey(name))
                return true;
            return false;
        }
        public bool AddInt(string name, int value)
        {
            try
            {
                if (name == null)
                    return true;
                if (IsVarNameBeUsed(name))
                    return true;
                VarInt.Add(name, value);
                return false;
            }
            catch (Exception e)
            {
                Core.State.AppendLogLine(e.Message);
                return true;
            }
        }
        public bool AddString(string name, string value)
        {
            try
            {
                if (name == null || value == null)
                    return true;
                if (IsVarNameBeUsed(name))
                    return true;
                VarString.Add(name, value);
                return false;
            }
            catch (Exception e)
            {
                Core.State.AppendLogLine(e.Message);
                return true;
            }
        }
        public bool AddFloat(string name, float value)
        {
            try
            {
                if (name == null)
                    return true;
                if (IsVarNameBeUsed(name))
                    return true;
                VarFloat.Add(name, value);
                return false;
            }
            catch (Exception e)
            {
                Core.State.AppendLogLine(e.Message);
                return true;
            }
        }
        public bool AddBool(string name, bool value)
        {
            try
            {
                if (name == null)
                    return true;
                if (IsVarNameBeUsed(name))
                    return true;
                VarBool.Add(name, value);
                return false;
            }
            catch (Exception e)
            {
                Core.State.AppendLogLine(e.Message);
                return true;
            }
        }
        public bool AddStaticFunction(string name, string full_path
            , bool is_path_add_CS_and_point_front = true)
        {
            try
            {
                if (name == null || full_path == null)
                    return true;
                if (IsVarNameBeUsed(name))
                    return true;
                string n_path;
                if (is_path_add_CS_and_point_front)
                    n_path = "CS." + full_path;
                else
                    n_path = full_path;
                VarStaticFunction.Add(name, n_path);
                return false;
            }
            catch (Exception e)
            {
                Core.State.AppendLogLine(e.Message);
                return true;
            }
        }
        public bool AddStaticFunction(IEnumerable<string> names
            , string pre_path, bool is_path_add_CS_and_point_front = true)
        {
            if (pre_path == null)
                return true;
            try
            {
                string n_path;
                if (is_path_add_CS_and_point_front)
                    n_path = "CS." + pre_path;
                else
                    n_path = pre_path;
                foreach (var name in names)
                {
                    if (name == null)
                        continue;
                    if (IsVarNameBeUsed(name))
                        continue;
                    VarStaticFunction.Add(name, n_path + name);
                }
            }
            catch (Exception e)
            {
                Core.State.AppendLogLine(e.Message);
                return true;
            }
            return false;
        }
        public bool AddLastDoString(string text)
        {
            if (text == null)
                return true;
            LastDoString.Add(text);
            return false;
        }
        public bool AddPreDoString(string text)
        {
            if (text == null)
                return true;
            PreDoString.Add(text);
            return false;
        }
    }
    [LuaCallCSharp]
    public static class _ScriptEnvReserved
    {
        public static List<string> _FunctionPaths = new List<string>()
        {
            "AddCDynamicToCard"
        };
        public static bool AddCDynamicToCard(int card_number, string cdynamic_type_name)
        {
            if (card_number <= 0)
                return true;
            if (cdynamic_type_name == null)
                return true;
            var cd = Core.Cards[card_number];
            if (cd == null)
                return true;
            var csp = ConceptManager.GetCDynamicSpawner(cdynamic_type_name);
            if (csp == null)
                return true;
            var c = csp.Spawn();
            if (c == null)
                return true;
            return cd.Add(c);
        }
    }
    public class CFeature
    {
        public Dictionary<string, Script> Scripts;
        public void DoSome() { }
        public CFeature() {
            Core.ScriptEnv.AddBool("do_some_1", true);
            Core.ScriptEnv.AddStaticFunction("DoSome", "GameCore.Interface.CFeature"); 
        }
        public HookClient<int, int> HCOtherDoSome = new HookClient<int, int>(HCFuncOtherDoSome);
        public static int HCFuncOtherDoSome(int var) {
            Script.Data input = new Script.Data();
            input.DataInt.Add("var", var);
            var result = Core.INeed.ExecuteScript(null, input);
            if (result == null)
                return -1;
            return result.DataInt["var"];
        }
    }
}