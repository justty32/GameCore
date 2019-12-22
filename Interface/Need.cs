using System;
using Newtonsoft.Json;

namespace GameCore.Interface
{
    public interface INeed
    {
        // for import, if any wrong, return null
        // for export, if any wrong, return true
        string ImportConfig();
        string ImportSaveInfo(string save_name);
        string ImportSaveRule(string save_name);
        string ImportSaveCards(string save_name, int index);
        //string ImportScript(string script_name);
        //string ImportModList();
        bool ExportConfig(string data);
        bool ExportSaveInfo(string save_name, string data);
        bool ExportSaveRule(string save_name, string data);
        bool ExportSaveCards(string save_name, int index, string data);
        //bool ExportScript(string script_name);
        //bool ExportModList();
        bool IsSaveExist(string save_name);
        bool NewSaveDir(string save_name);
        bool IsSaveDirLegal(string save_name);
    }
}