using System;
using Newtonsoft.Json;

namespace GameCore.Interface
{
    public interface INeed
    {
        // for import, if any wrong, return null
        // for export, if any wrong, return true
        // if second_dir_name is null, means that the file is in first level of folder
        string Import(string save_dir_name, string file_name, string second_dir_name = null);
        bool Export(string save_dir_name, string file_name, string data, string second_dir_name = null);
        bool IsFileExist(string save_dir_name, string file_name, string second_dir_name = null);
        string ImportConfig();
        bool ExportConfig(string data);
        string ImportInfo(string save_dir_name);
        bool ExportInfo(string save_dir_name, string data);
        string ImportRules(string save_dir_name);
        bool ExportRules(string save_dir_name, string data);
        string ImportCard(string save_dir_name, int number);
        bool ExportCard(string save_dir_name, string data);
        bool IsSaveDirExist(string save_dir_name);
        bool NewSaveDir(string save_dir_name);
        bool IsSaveDirLegal(string save_dir_name);
        bool CopyInitSaveData(string source_name, string target_name);
    }
}