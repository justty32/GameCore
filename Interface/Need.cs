using System.Collections.Generic;
using Newtonsoft.Json.Linq;
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
        string ImportCommonFile(string file_name, string second_dir_name = null, string third_dir_name = null);
        bool ExportCommonFile(string data, string file_name, string second_dir_name = null, string third_dir_name = null);
        bool SetScriptEnv(ScriptEnv script_env);
        bool ClearScriptEnv();
        Base.Script.Data ExecuteScript(Base.Script script, Base.Script.Data input);
        string ImportConfig();
        bool ExportConfig(string data);
        string ImportInfo(string save_dir_name);
        bool ExportInfo(string save_dir_name, string data);
        string ImportRules(string save_dir_name);
        bool ExportRules(string save_dir_name, string data);
        Dictionary<int, string> ImportCard(string save_dir_name, Dictionary<int, List<int>> number_sets);
        bool ExportCard(string save_dir_name, Dictionary<int, List<KeyValuePair<int, string>>> muldatas);
        bool NewThreadExportCard(string save_dir_name, Dictionary<int, JObject> datas, bool[] results);
        bool NewThreadMulExportCard(string save_dir_name, Dictionary<int, JObject> muldatas, bool[] results);
        bool MulExportCard(string save_dir_name, Dictionary<int, List<KeyValuePair<int, JObject>>> muldatas);
        Dictionary<int, JObject> MulImportCard(string save_dir_name, List<int> numbers);
        bool IsSaveDirExist(string save_dir_name);
        bool NewSaveDir(string save_dir_name);
        bool IsSaveDirLegal(string save_dir_name);
        bool CopyInitSaveData(string source_name, string target_name);
        bool CopySaveData(string source_name, string target_name);
    }

    /*
public class Need : GameCore.Interface.INeed
{
    public bool CopySaveData(string source_name, string target_name)
    {
        string s = ComposePaths(ResourceDir, SavesDir, source_name);
        string d = ComposePaths(ResourceDir, SavesDir, target_name);
        if (!_IsSaveDirLegal(s))
            return true;
        if (_DirectoryCopy(s, d, true))
            return true;
        return false;
    }
    public bool CopyInitSaveData(string source_name, string target_name)
    {
        string s = ComposePaths(ResourceDir, CommonDir, GameInitDataDir, source_name);
        string d = ComposePaths(ResourceDir, SavesDir, target_name);
        if (!_IsSaveDirLegal(s))
            return true;
        if (_DirectoryCopy(s, d, true))
            return true;
        return false;
    }
    public bool Export(string save_dir_name, string file_name, string data, string second_dir_name = null)
    {
        // delete existing file
        if (data == null)
            return true;
        string path = ComposePaths(ResourceDir, SavesDir, save_dir_name, second_dir_name);
        if (!Directory.Exists(path))
            return true;
        path += file_name;
        if (File.Exists(path))
            File.Delete(path);
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.Write(data);
        }
        return false;
    }
    public bool ExportCommonFile(string data, string file_name, string second_dir_name = null, string third_dir_name = null)
    {
        // delete existing file
        if (data == null || file_name == null)
            return true;
        string path = ComposePaths(ResourceDir, CommonDir, second_dir_name, third_dir_name);
        if (!Directory.Exists(path))
            return true;
        path += file_name;
        if (File.Exists(path))
            File.Delete(path);
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.Write(data);
        }
        return false;
    }
    public bool ExportCard(string save_dir_name, Dictionary<int, List<KeyValuePair<int, string>>> muldatas)
    {
        if (save_dir_name == null || muldatas == null)
            return true;
        if (save_dir_name.Length == 0)
            return true;
        foreach (var prdatas in muldatas)
        {
            try
            {
                var datas = prdatas.Value;
                int file_index = prdatas.Key;
                if (datas == null || file_index < 0)
                    return true;
                // preprocess file path
                string file_name = "card" + file_index.ToString() + ".json";
                string meta_name = "card" + file_index.ToString() + ".cwed";
                string file_path = ComposePaths(ResourceDir, SavesDir, save_dir_name, "cards");
                string meta_path = string.Copy(file_path);
                if (file_path == null)
                    return true;
                file_path += file_name;
                meta_path += meta_name;
                // check if meta file ok
                FileInfo meta_file_info = new FileInfo(meta_path);
                if (!meta_file_info.Exists)
                {
                    File.Create(meta_path).Dispose();
                    byte[] meta_new_bytes = new byte[CoreInfo.Card_amount_per_file * 2 * sizeof(int)];
                    File.WriteAllBytes(meta_path, meta_new_bytes);
                }
                else if (meta_file_info.Length < CoreInfo.Card_amount_per_file * 2 * sizeof(int))
                {
                    File.Delete(meta_path);
                    File.Create(meta_path).Dispose();
                    byte[] meta_new_bytes = new byte[CoreInfo.Card_amount_per_file * 2 * sizeof(int)];
                    File.WriteAllBytes(meta_path, meta_new_bytes);
                }
                // be sure that card file is ok
                if (!File.Exists(file_path))
                    File.Create(file_path).Dispose();
                // read and write meta data
                FileStream cfm = File.Open(meta_path, FileMode.Open, FileAccess.ReadWrite);
                FileStream cfd = File.Open(file_path, FileMode.Open, FileAccess.ReadWrite);
                foreach (var pad in datas)
                {
                    byte[] card_bytes = Encoding.UTF8.GetBytes(pad.Value);
                    byte[] meta_bytes = new byte[2 * sizeof(int)];
                    int[] meta_ints = new int[2];
                    int index = pad.Key - CoreInfo.Card_amount_per_file * file_index;
                    cfm.Seek(index * sizeof(int) * 2, SeekOrigin.Begin);
                    cfm.Read(meta_bytes, 0, meta_bytes.Length);
                    Buffer.BlockCopy(meta_bytes, 0, meta_ints, 0, meta_bytes.Length);
                    if (meta_ints[1] < card_bytes.Length)
                        meta_ints[0] = (int)cfd.Length;
                    meta_ints[1] = card_bytes.Length;
                    Buffer.BlockCopy(meta_ints, 0, meta_bytes, 0, meta_bytes.Length);
                    cfm.Seek(index * sizeof(int) * 2, SeekOrigin.Begin);
                    cfm.Write(meta_bytes, 0, meta_bytes.Length);
                    cfd.Seek(meta_ints[0], SeekOrigin.Begin);
                    cfd.Write(card_bytes, 0, card_bytes.Length);
                }
                // get card data's byte
                cfd.Dispose();
                cfm.Dispose();
            }
            catch(Exception e)
            {
                return true;
            }
        }
        return false;
    }
    /*
    public bool ExportCard(string save_dir_name, int number, string data)
    {
        // meta file : cards' data byte start point
        // card file : utf-8 no bom, nothing at file head
        if (data == null || number < 0 || save_dir_name == null)
            return true;
        if (data.Length == 0 || save_dir_name.Length == 0)
            return true;
        try
        {
            // preprocess file path
            int file_index = number / CardAmountPerFile;
            int index = number - file_index * CardAmountPerFile;
            string file_name = "card" + file_index.ToString() + ".json";
            string meta_name = "card" + ".cwed";
            string file_path = ComposePaths(ResourceDir, SavesDir, save_dir_name, "cards");
            string meta_path = string.Copy(file_path);
            if (file_path == null)
                return true;
            file_path += file_name;
            meta_path += meta_name;
            // get card data's byte
            byte[] card_bytes = Encoding.UTF8.GetBytes(data);
            // be sure that meta file is ok
            if (!File.Exists(meta_path))
            {
                File.Create(meta_path).Dispose();
                if (Core.Cards.MaxNumber <= 0)
                    return true;
                byte[] meta_new_bytes = new byte[(Core.Cards.MaxNumber + 1) * 3 * sizeof(int)];
                File.WriteAllBytes(meta_path, meta_new_bytes);
            }
            // be sure that card file is ok
            if (!File.Exists(file_path))
                File.Create(file_path).Dispose();
            // read and write meta data
            FileStream cfm = File.Open(meta_path, FileMode.Open, FileAccess.ReadWrite);
            // expand meta file
            if (cfm.Length < (Core.Cards.MaxNumber + 1) * 3 * sizeof(int))
            {
                cfm.Seek(0, SeekOrigin.End);
                byte[] meta_new_bytes = new byte[(Core.Cards.MaxNumber + 1) * 3 * sizeof(int) - cfm.Length];
                cfm.Write(meta_new_bytes, 0, meta_new_bytes.Length);
            }
            cfm.Seek(number * sizeof(int) * 3, SeekOrigin.Begin);
            byte[] meta_bytes = new byte[3 * sizeof(int)];
            int[] meta_ints = new int[3];
            cfm.Read(meta_bytes, 0, meta_bytes.Length);
            Buffer.BlockCopy(meta_bytes, 0, meta_ints, 0, meta_bytes.Length);
            meta_ints[0] = file_index;
            if (meta_ints[2] < card_bytes.Length)
            {
                FileInfo file_info = new FileInfo(file_path);
                meta_ints[1] = (int)file_info.Length;
            }
            meta_ints[2] = card_bytes.Length;
            int start_byte = meta_ints[1];
            int size_byte = meta_ints[2];
            Buffer.BlockCopy(meta_ints, 0, meta_bytes, 0, meta_bytes.Length);
            cfm.Seek(number * sizeof(int) * 3, SeekOrigin.Begin);
            cfm.Write(meta_bytes, 0, meta_bytes.Length);
            cfm.Dispose();
            // read and process card
            FileStream cfd = File.Open(file_path, FileMode.Open, FileAccess.ReadWrite);
            cfd.Seek(start_byte, SeekOrigin.Begin);
            cfd.Write(card_bytes, 0, card_bytes.Length);
            cfd.Dispose();
        } catch (Exception e)
        {
            return true;
        }
        return false;
    }
    */
    /*
    public bool ExportConfig(string data)
    {
        // delete existing file
        if (data == null)
            return true;
        string path = (ResourceDir + "/");
        if (!Directory.Exists(path))
            return true;
        path += "config.json";
        if (File.Exists(path))
            File.Delete(path);
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.Write(data);
        }
        return false;
    }
    public bool ExportInfo(string save_dir_name, string data)
    {
        return Export(save_dir_name, "info.json", data);
    }
    public bool ExportRules(string save_dir_name, string data)
    {
        return Export(save_dir_name, "rules.json", data);
    }
    public Dictionary<int, bool[]> MulExportCard(string save_dir_name, Dictionary<int, List<KeyValuePair<int, string>>> muldatas)
    {
        if (muldatas == null)
            return null;
        Dictionary<int, bool[]> results = new Dictionary<int, bool[]>();
        foreach (var prdatas in muldatas)
        {
            var datas = prdatas.Value;
            int file_index = prdatas.Key;
            if (datas == null || file_index < 0 || save_dir_name == null)
                return null;
            if (save_dir_name.Length == 0)
                return null;
            // preprocess file path
            string file_name = "card" + file_index.ToString() + ".json";
            string meta_name = "card" + file_index.ToString() + ".cwed";
            string file_path = ComposePaths(ResourceDir, SavesDir, save_dir_name, "cards");
            string meta_path = string.Copy(file_path);
            if (file_path == null)
                return null;
            file_path += file_name;
            meta_path += meta_name;
            results.Add(file_index, new bool[2]);
            Thread thread = new Thread(((Need)Core.INeed)._MulExportCard);
            var par = new Tuple<string, string, int, List<KeyValuePair<int, string>>, bool[]>(
                file_path, meta_path, file_index, datas, results[file_index]
                );
            thread.Start(par);
        }
        return results;
    }
    private void _MulExportCard(object data)
    {
        var par = (Tuple<string, string, int, List<KeyValuePair<int, string>>, bool[]>)data;
        string file_path = par.Item1;
        string meta_path = par.Item2;
        int file_index = par.Item3;
        List<KeyValuePair<int, string>> datas = par.Item4;
        bool[] result = par.Item5;
        try
        {
            FileInfo meta_file_info = new FileInfo(meta_path);
            if (!meta_file_info.Exists)
            {
                File.Create(meta_path).Dispose();
                byte[] meta_new_bytes = new byte[CoreInfo.Card_amount_per_file * 2 * sizeof(int)];
                File.WriteAllBytes(meta_path, meta_new_bytes);
            }
            else if (meta_file_info.Length < CoreInfo.Card_amount_per_file * 2 * sizeof(int))
            {
                File.Delete(meta_path);
                File.Create(meta_path).Dispose();
                byte[] meta_new_bytes = new byte[CoreInfo.Card_amount_per_file * 2 * sizeof(int)];
                File.WriteAllBytes(meta_path, meta_new_bytes);
            }
            // be sure that card file is ok
            if (!File.Exists(file_path))
                File.Create(file_path).Dispose();
            // read and write meta data
            FileStream cfm = File.Open(meta_path, FileMode.Open, FileAccess.ReadWrite);
            FileStream cfd = File.Open(file_path, FileMode.Open, FileAccess.ReadWrite);
            foreach (var pad in datas)
            {
                byte[] card_bytes = Encoding.UTF8.GetBytes(pad.Value);
                byte[] meta_bytes = new byte[2 * sizeof(int)];
                int[] meta_ints = new int[2];
                int index = pad.Key - CoreInfo.Card_amount_per_file * file_index;
                cfm.Seek(index * sizeof(int) * 2, SeekOrigin.Begin);
                cfm.Read(meta_bytes, 0, meta_bytes.Length);
                Buffer.BlockCopy(meta_bytes, 0, meta_ints, 0, meta_bytes.Length);
                if (meta_ints[1] < card_bytes.Length)
                    meta_ints[0] = (int)cfd.Length;
                meta_ints[1] = card_bytes.Length;
                Buffer.BlockCopy(meta_ints, 0, meta_bytes, 0, meta_bytes.Length);
                cfm.Seek(index * sizeof(int) * 2, SeekOrigin.Begin);
                cfm.Write(meta_bytes, 0, meta_bytes.Length);
                cfd.Seek(meta_ints[0], SeekOrigin.Begin);
                cfd.Write(card_bytes, 0, card_bytes.Length);
            }
            // get card data's byte
            cfd.Dispose();
            cfm.Dispose();
        }
        catch (Exception e)
        {
            if (result.Length > 1)
                result[1] = true;
            result[0] = true;
        }
        result[0] = true;
    }
    public string ImportCommonFile(string file_name, string second_dir_name = null, string third_dir_name = null)
    {
        string path = ComposePaths(ResourceDir, CommonDir, second_dir_name, third_dir_name);
        if (!Directory.Exists(path))
            return null;
        path += (file_name);
        if (!File.Exists(path))
            return null;
        using (StreamReader sr = File.OpenText(path))
        {
            return sr.ReadToEnd();
        }
    }
    public string Import(string save_dir_name, string file_name, string second_dir_name = null)
    {
        string path = ComposePaths(ResourceDir, SavesDir, save_dir_name, second_dir_name);
        if (!Directory.Exists(path))
            return null;
        path += (file_name);
        if (!File.Exists(path))
            return null;
        using (StreamReader sr = File.OpenText(path))
        {
            return sr.ReadToEnd();
        }
    }
    public Dictionary<int, string> ImportCard(string save_dir_name, Dictionary<int, List<int>> number_sets)
    {
        if(save_dir_name == null || number_sets == null)
            return null;
        Dictionary<int, string> datas = new Dictionary<int, string>();
        foreach(var number_set in number_sets)
        {
            if (number_set.Value == null)
                continue;
            int file_index = number_set.Key;
            string file_name = "card" + file_index.ToString() + ".json";
            string meta_name = "card" + file_index.ToString() + ".cwed";
            string file_path = ComposePaths(ResourceDir, SavesDir, save_dir_name, "cards");
            string meta_path = file_path;
            if (file_path == null)
                continue;
            file_path += file_name;
            meta_path += meta_name;
            // be sure that both file are exist
            if (!File.Exists(meta_path) || !File.Exists(file_path))
                continue;
            FileStream cfm = File.OpenRead(meta_path);
            FileStream cfd = File.OpenRead(file_path);
            foreach(int number in number_set.Value)
            {
                try
                {
                    if (number < 0 || number > Core.Cards.MaxNumber)
                        continue;
                    byte[] meta_bytes = new byte[2 * sizeof(int)];
                    int[] meta_ints = new int[2];
                    cfm.Seek(number * 2 * sizeof(int), SeekOrigin.Begin);
                    cfm.Read(meta_bytes, 0, meta_bytes.Length);
                    Buffer.BlockCopy(meta_bytes, 0, meta_ints, 0, meta_bytes.Length);
                    if (meta_ints[0] < 0 || meta_ints[0] < 0)
                        continue;
                    if (cfd.Length < meta_ints[0] + meta_ints[1])
                        continue;
                    cfd.Seek(meta_ints[0], SeekOrigin.Begin);
                    var o_buffer = new byte[meta_ints[1]];
                    cfd.Read(o_buffer, 0, o_buffer.Length);
                    string data = Encoding.UTF8.GetString(o_buffer);
                    datas.Add(number, data);
                }catch(Exception e)
                {
                    Core.State.WriteException(e);
                    continue;
                }
            }
            cfm.Dispose();
            cfd.Dispose();
        }
        return datas;
    }
    /*
    public string ImportCard(string save_dir_name, int number)
    {
        if(save_dir_name == null || number < 0)
            return null;
        try
        {
            int file_index = number / CardAmountPerFile;
            int index = number - file_index * CardAmountPerFile;
            string file_name = "card" + file_index.ToString() + ".json";
            string meta_name = "card.cwed";
            string file_path = ComposePaths(ResourceDir, SavesDir, save_dir_name, "cards");
            string meta_path = string.Copy(file_path);
            if (file_path == null)
                return null;
            file_path += file_name;
            meta_path += meta_name;
            // be sure that both file are exist
            if (!File.Exists(meta_path) || !File.Exists(file_path))
                return null;
            FileStream cfm = File.OpenRead(meta_path);
            byte[] meta_bytes = new byte[3 * sizeof(int)];
            int[] meta_ints = new int[3];
            cfm.Seek(number * 3 * sizeof(int), SeekOrigin.Begin);
            cfm.Read(meta_bytes, 0, meta_bytes.Length);
            cfm.Dispose();
            Buffer.BlockCopy(meta_bytes, 0, meta_ints, 0, meta_bytes.Length);
            if (meta_ints[0] != file_index)
                return null;
            int start_byte = meta_ints[1];
            int size_byte = meta_ints[2];
            FileStream cfd = File.OpenRead(file_path);
            if (cfd.Length < start_byte + size_byte)
                return null;
            cfd.Seek(start_byte, SeekOrigin.Begin);
            var o_buffer = new byte[size_byte];
            cfd.Read(o_buffer, 0, o_buffer.Length);
            string data = Encoding.UTF8.GetString(o_buffer);
            cfd.Dispose();
            return data;
        }catch(Exception e)
        {
            return null;
        }
    }
    */
    /*
    public string ImportConfig()
    {
        string path = ComposePaths(ResourceDir);
        if (!Directory.Exists(path))
            return null;
        path += "config.json";
        if (!File.Exists(path))
            return null;
        using (StreamReader sr = File.OpenText(path))
        {
            return sr.ReadToEnd();
        }
    }
    public string ImportInfo(string save_dir_name)
    {
        return Import(save_dir_name, "info.json");
    }
    public string ImportRules(string save_dir_name)
    {
        return Import(save_dir_name, "rules.json");
    }
    public bool IsFileExist(string save_dir_name, string file_name, string second_dir_name = null)
    {
        if (save_dir_name == null || file_name == null)
            return false;
        string fd = ComposePaths(ResourceDir, SavesDir, save_dir_name, second_dir_name);
        string fn = fd + file_name;
        FileInfo fi = new FileInfo(fn);
        return fi.Exists;
    }
    public bool IsSaveDirExist(string save_dir_name)
    {
        string s = ComposePaths(ResourceDir, SavesDir, save_dir_name);
        DirectoryInfo di = new DirectoryInfo(s);
        return di.Exists;
    }
    public bool IsSaveDirLegal(string save_dir_name)
    {
        if (save_dir_name == null)
            return false;
        string s = ComposePaths(ResourceDir, SavesDir, save_dir_name);
        return _IsSaveDirLegal(s);
    }
    public bool NewSaveDir(string save_dir_name)
    {
        // if save dir already exist, return true;
        if (IsSaveDirExist(save_dir_name))
            return true;
        string save = ComposePaths(ResourceDir, SavesDir, save_dir_name);
        string cards = ComposePaths(ResourceDir, SavesDir, save_dir_name, "cards");
        if (!Directory.CreateDirectory(save).Exists)
            return true;
        if (!Directory.CreateDirectory(cards).Exists)
            return true;
        return false;
    }
    public string ComposePaths(params string[] dirs)
    {
        // automatically add slash "/"
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < dirs.Length; i++)
        {
            if (dirs[i] != null)
            {
                sb.Append(dirs[i]);
                sb.Append("/");
            }
        }
        return sb.ToString();
    }
    private bool _IsSaveDirLegal(string path)
    {
        // only check dir construction, not check file
        DirectoryInfo di = new DirectoryInfo(path);
        if (!di.Exists)
            return false;
        var subdis = di.GetDirectories();
        //check dir "cards"
        foreach (var sd in subdis)
            if (sd.Name.Equals("cards"))
                return true;
        return false;
    }
    private static bool _DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);
        if (!dir.Exists)
        {
            return true;
        }
        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }
        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }
        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                _DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
        return false;
    }
    public readonly int CardAmountPerFile = CoreInfo.Card_amount_per_file;
    public string ResourceDir = "resources";
    public string SavesDir = "saves";
    public string CardsDir = "cards";
    public string CommonDir = "common";
    public string GameInitDataDir = "gameinitdata";
}
*/
}