using SteamworksNative;

namespace PersistentData
{
    internal static class Utility
    {
        internal static CSteamID CSteamId => SteamManager.Instance.field_Private_CSteamID_0;
        internal static ulong ClientId => SteamManager.Instance.field_Private_CSteamID_0.m_SteamID;
    }
}