using BepInEx;
using System.Collections.Generic;
using System.IO;

namespace PersistentData
{
    public static class Api
    {
        public static string PersistentDataPath { get; internal set; } = Path.Combine(Paths.ConfigPath, MyPluginInfo.PLUGIN_GUID, "Data");
        public static string PersistentClientDataPath { get; internal set; } = Path.Combine(Paths.ConfigPath, MyPluginInfo.PLUGIN_GUID, "ClientData");

        public static HashSet<ulong> PersistentClientDataIds { get; internal set; } = [];


        public static DataFile GetDataFile(string fileId)
        {
            DataFile file = new();
            file.data = [];
            file.Id = fileId;
            file.FilePath = Path.Combine(PersistentDataPath, $"{fileId}.txt");
            file.ReadFile();
            return file;
        }
        public static ClientDataFile GetClientDataFile(ulong clientId)
        {
            ClientDataFile file = new();
            file.data = [];
            file.ClientId = clientId;
            file.FilePath = Path.Combine(PersistentClientDataPath, $"{clientId}.txt");
            file.ReadFile();
            return file;
        }
    }


    public struct DataFile
    {
        internal Dictionary<string, string> data;
        public string Id { get; internal set; }
        public string FilePath { get; internal set; }

        public readonly string[] GetKeys()
            => [.. data.Keys];
        public readonly bool ContainsKey(string key)
            => data.ContainsKey(key);
        public readonly string Get(string key)
            => ContainsKey(key) ? data[key] : null;
        public readonly bool Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || key.StartsWith('#') || key.Contains(':') || key.Contains('\n') || string.IsNullOrEmpty(value) || value.Contains('\n'))
                return false;

            data[key] = value;
            return true;
        }
        public readonly bool Remove(string key)
            => data.Remove(key);

        public readonly bool ReadFile()
        {
            if (!File.Exists(FilePath))
                return false;

            data.Clear();
            foreach (string line in File.ReadAllLines(FilePath))
            {
                if (line.Length == 0 || line.StartsWith('#'))
                    continue;

                int index = line.IndexOf(':');
                if (index != -1)
                    data[line[..index].Trim(' ')] = line[(index + 1)..].Trim(' ');
            }

            return true;
        }
        public readonly void SaveFile()
        {
            string[] contents = new string[data.Count];
            int index = 0;
            foreach (string key in data.Keys)
                contents[index++] = $"{key}:{data[key]}";

            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            File.WriteAllLines(FilePath, contents);
        }
        public readonly bool DeleteFile()
        {
            bool exists = File.Exists(FilePath);
            if (exists)
                File.Delete(FilePath);
            return exists;
        }
    }

    public struct ClientDataFile
    {
        internal Dictionary<string, string> data;
        public ulong ClientId { get; internal set; }
        public string FilePath { get; internal set; }

        public readonly string[] GetKeys()
            => [.. data.Keys];
        public readonly bool ContainsKey(string key)
            => data.ContainsKey(key);
        public readonly string Get(string key)
            => ContainsKey(key) ? data[key] : null;
        public readonly bool Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key) || key.StartsWith('#') || key.Contains(':') || key.Contains('\n') || string.IsNullOrEmpty(value) || value.Contains('\n'))
                return false;

            data[key] = value;
            return true;
        }
        public readonly bool Remove(string key)
            => data.Remove(key);

        public readonly bool ReadFile()
        {
            if (!File.Exists(FilePath))
            {
                Api.PersistentClientDataIds.Remove(ClientId);
                return false;
            }

            data.Clear();
            Api.PersistentClientDataIds.Add(ClientId);
            foreach (string line in File.ReadAllLines(FilePath))
            {
                if (line.Length == 0 || line.StartsWith('#'))
                    continue;

                int index = line.IndexOf(':');
                if (index != -1)
                    data[line[..index].Trim(' ')] = line[(index + 1)..].Trim(' ');
            }

            return true;
        }
        public readonly void SaveFile()
        {
            string[] contents = new string[data.Count];
            int index = 0;
            foreach (string key in data.Keys)
                contents[index++] = $"{key}:{data[key]}";

            Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
            File.WriteAllLines(FilePath, contents);
            Api.PersistentClientDataIds.Add(ClientId);
        }
        public readonly bool DeleteFile()
        {
            bool exists = File.Exists(FilePath);
            if (exists)
                File.Delete(FilePath);
            Api.PersistentClientDataIds.Remove(ClientId);
            return exists;
        }
    }
}