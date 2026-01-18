using HarmonyLib;
using SteamworksNative;

namespace PersistentData
{
    internal static class Patches
    {
        //   Anti Bepinex detection (Thanks o7Moon: https://github.com/o7Moon/CrabGame.AntiAntiBepinex)
        [HarmonyPatch(typeof(EffectManager), nameof(EffectManager.Method_Private_Void_GameObject_Boolean_Vector3_Quaternion_0))] // Ensures effectSeed is never set to 4200069 (if it is, modding has been detected)
        [HarmonyPatch(typeof(LobbyManager), nameof(LobbyManager.Method_Private_Void_0))] // Ensures connectedToSteam stays false (true means modding has been detected)
        //[HarmonyPatch(typeof(Deobf_MenuSnowSpeedModdingDetector), nameof(Deobf_MenuSnowSpeedModdingDetector.Method_Private_Void_0))] // Would ensure snowSpeed is never set to Vector3.zero (though it is immediately set back to Vector3.one due to an accident on Dani's part lol)
        [HarmonyPrefix]
        internal static bool PreBepinexDetection()
            => false;



        //   Create client data for the host
        [HarmonyPatch(typeof(LobbyManager), nameof(LobbyManager.StartLobby))]
        [HarmonyPatch(typeof(LobbyManager), nameof(LobbyManager.StartPracticeLobby))]
        [HarmonyPostfix]
        internal static void PostLobbyManagerStartLobby()
        {
            ClientDataFile hostFile = Api.GetClientDataFile(SteamUser.GetSteamID().m_SteamID);
            hostFile.Set("Username", SteamFriends.GetFriendPersonaName(SteamUser.GetSteamID()));
            hostFile.SaveFile();
        }

        //   Create client data for those that join
        [HarmonyPatch(typeof(LobbyManager), nameof(LobbyManager.OnPlayerJoinLeaveUpdate))]
        [HarmonyPostfix]
        internal static void PostLobbyManagerOnPlayerJoinLeaveUpdate(CSteamID param_1, bool param_2)
        {
            if (!SteamManager.Instance.IsLobbyOwner() || !param_2)
                return;

            ClientDataFile clientFile = Api.GetClientDataFile(param_1.m_SteamID);
            clientFile.Set("Username", SteamFriends.GetFriendPersonaName(param_1));
            clientFile.SaveFile();
        }
    }
}