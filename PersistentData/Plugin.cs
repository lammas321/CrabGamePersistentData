using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using System.Globalization;
using System.IO;

namespace PersistentData
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public sealed class PersistentData : BasePlugin
    {
        public override void Load()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            Directory.CreateDirectory(Api.PersistentDataPath);
            Directory.CreateDirectory(Api.PersistentClientDataPath);

            string[] directories = Directory.GetFiles(Api.PersistentClientDataPath);
            Api.PersistentClientDataIds.EnsureCapacity(directories.Length);

            foreach (string filePath in directories)
                if (ulong.TryParse(Path.GetFileNameWithoutExtension(Path.GetFileName(filePath)), NumberStyles.None, CultureInfo.InvariantCulture.NumberFormat, out ulong clientId))
                    Api.PersistentClientDataIds.Add(clientId);

            Harmony harmony = new(MyPluginInfo.PLUGIN_NAME);
            harmony.PatchAll(typeof(Patches));

            Log.LogInfo($"Initialized [{MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION}]");
        }
    }
}