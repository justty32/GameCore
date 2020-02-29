using System.Collections.Generic;
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
        string ImportConfig();
        bool ExportConfig(string data);
        string ImportInfo(string save_dir_name);
        bool ExportInfo(string save_dir_name, string data);
        string ImportRules(string save_dir_name);
        bool ExportRules(string save_dir_name, string data);
        Dictionary<int, string> ImportCard(string save_dir_name, Dictionary<int, List<int>> number_sets);
        bool ExportCard(string save_dir_name, Dictionary<int, List<KeyValuePair<int, string>>> muldatas);
        Dictionary<int, bool[]> MulExportCard(string save_dir_name, Dictionary<int, List<KeyValuePair<int, string>>> muldatas);
        bool IsSaveDirExist(string save_dir_name);
        bool NewSaveDir(string save_dir_name);
        bool IsSaveDirLegal(string save_dir_name);
        bool CopyInitSaveData(string source_name, string target_name);
        bool CopySaveData(string source_name, string target_name);
    }

    /*
    public class Need : GameCore.Interface.INeed
    {
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
                int file_index = number / CardAmountPerFile;
                int index = number - file_index * CardAmountPerFile;
                string file_name = "card" + file_index.ToString() + ".json";
                string meta_name = "card" + file_index.ToString() + ".cwed";
                string file_path = ComposePaths(ResourceDir, SavesDir, save_dir_name, "cards");
                string meta_path = string.Copy(file_path);
                if (file_path == null)
                    return true;
                file_path += file_name;
                meta_path += meta_name;
                byte[] card_bytes = Encoding.UTF8.GetBytes(data);
                // if card file not exist, create both two, and write it
                if (!File.Exists(file_path))
                {
                    if (File.Exists(meta_path))
                        File.Delete(meta_path);
                    File.Create(meta_path);
                    File.Create(file_path);
                    File.WriteAllBytes(file_path, card_bytes);
                    int[] meta_ints = new int[CardAmountPerFile];
                    meta_ints[0] = card_bytes.Length;
                    byte[] meta_bytes = new byte[meta_ints.Length * sizeof(int)];
                    Buffer.BlockCopy(meta_ints, 0, meta_bytes, 0, meta_bytes.Length);
                    File.WriteAllBytes(meta_path, meta_bytes);
                    return false;
                }
                else if (File.Exists(meta_path) && File.Exists(file_path))
                {
                    // if both are exist, deal with meta file first
                    byte[] meta_bytes = File.ReadAllBytes(meta_path);
                    int[] meta_ints = new int[meta_bytes.Length / sizeof(int)];
                    Buffer.BlockCopy(meta_bytes, 0, meta_ints, 0, meta_ints.Length);
                    // get cut point, and cut range
                    int start_point = 0, end_point = 0;
                    int cut_range = 0;
                    for (int i = 0; i < index; i++)
                        start_point += meta_ints[i];
                    end_point = start_point + meta_ints[index];
                    for (int i = index + 1; i < CardAmountPerFile; i++)
                        cut_range += meta_ints[i];
                    // modify and restore meta file
                    meta_ints[index] = card_bytes.Length;
                    Buffer.BlockCopy(meta_ints, 0, meta_bytes, 0, meta_bytes.Length);
                    File.WriteAllBytes(meta_path, meta_bytes);
                    // deal with card file, cut it, then glue it after adding data
                    FileStream cfo = File.OpenRead(file_path);
                    FileStream cfi = File.OpenWrite(file_path);
                    if (cut_range > 0)
                    {
                        var cut_buffer = new byte[cut_range];
                        cfo.Read(cut_buffer, end_point, cut_range);
                        cfi.Write(card_bytes, start_point, card_bytes.Length);
                        cfi.Write(cut_buffer, start_point + card_bytes.Length, cut_buffer.Length);
                    }
                    else if (cut_range == 0)
                        cfi.Write(card_bytes, start_point, card_bytes.Length);
                    else
                        return true;
                    return false;
                }
                else
                    return true;
            }
            catch (Exception e)
            {
                //Debug.LogWarning(e.Message);
                return true;
            }
        }
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
        public string Import(string save_dir_name, string file_name, string second_dir_name = null)
        {
            string path = ComposePaths(ResourceDir, SavesDir, save_dir_name, second_dir_name);
            if (!Directory.Exists(path))
                return null;
            path += (file_name + "/");
            if (!File.Exists(path))
                return null;
            using (StreamReader sr = File.OpenText(path))
            {
                return sr.ReadToEnd();
            }
        }
        public string ImportCard(string save_dir_name, int number)
        {
            if (save_dir_name == null || number < 0)
                return null;
            try
            {
                int file_index = number / CardAmountPerFile;
                int index = number - file_index * CardAmountPerFile;
                string file_name = "card" + file_index.ToString() + ".json";
                string meta_name = "card" + file_index.ToString() + ".cwed";
                string file_path = ComposePaths(ResourceDir, SavesDir, save_dir_name, "cards");
                string meta_path = string.Copy(file_path);
                if (file_path == null)
                    return null;
                file_path += file_name;
                meta_path += meta_name;
                if (File.Exists(meta_path) && File.Exists(file_path))
                {
                    // if both are exist, deal with meta file first
                    byte[] meta_bytes = File.ReadAllBytes(meta_path);
                    int[] meta_ints = new int[meta_bytes.Length / sizeof(int)];
                    Buffer.BlockCopy(meta_bytes, 0, meta_ints, 0, meta_ints.Length);
                    // get cut start point, and range
                    int start_point = 0;
                    int cut_range = 0;
                    for (int i = 0; i < index; i++)
                        start_point += meta_ints[i];
                    cut_range = meta_ints[index];
                    // get data
                    byte[] data_byte = new byte[cut_range];
                    FileStream ofs = File.OpenRead(file_path);
                    ofs.Read(data_byte, start_point, cut_range);
                    string data = Encoding.UTF8.GetString(data_byte);
                    return data;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                //Debug.LogWarning(e.Message);
                return null;
            }
        }
        public string ImportConfig()
        {
            string path = ResourceDir;
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
            for (int i = 0; i < dirs.Length; i++)
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
                //Debug.Log("While INeed.DirectoryCopy(), Source directory does not exist or could not be found: "
                //+ sourceDirName);
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
        public readonly int CardAmountPerFile = 10000;
        public string ResourceDir = "resources";
        public string SavesDir = "saves";
        public string CardsDir = "cards";
        public string CommonDir = "common";
        public string GameInitDataDir = "gameinitdata";
    }
    */
}