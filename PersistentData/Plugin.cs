using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;
using System.Globalization;
using System.IO;

namespace PersistentData
{
    [BepInPlugin($"lammas123.{MyPluginInfo.PLUGIN_NAME}", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class PersistentData : BasePlugin
    {
        public override void Load()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            Directory.CreateDirectory(Api.PersistentDataPath);
            Directory.CreateDirectory(Api.PersistentClientDataPath);
            foreach (string filePath in Directory.GetFiles(Api.PersistentClientDataPath))
                if (ulong.TryParse(Path.GetFileNameWithoutExtension(Path.GetFileName(filePath)), NumberStyles.None, CultureInfo.InvariantCulture.NumberFormat, out ulong clientId))
                    Api.PersistentClientDataIds.Add(clientId);

            Harmony.CreateAndPatchAll(typeof(Patches));
            Log.LogInfo($"Loaded [{MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION}]");
        }
    }
}