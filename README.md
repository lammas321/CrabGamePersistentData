# CrabGamePersistentData
A small BepInEx mod for Crab Game that allows for certain data to be saved and loaded neatly.

## What does this mean?
Some lobbies that want to act more like servers may need to save information between game loads (persistently), such as an offline player's last known username, the player's [permission group](https://github.com/lammas321/CrabGamePermissionGroups), and who's been [banned](https://github.com/lammas321/CrabGameOverseer) previously.

When this mod is in use, it will create a txt file for every steam user that joins your lobby in your "BepInEx/config/lammas123.PersisitentData/ClientData/" directory, with the name being that user's steam id.
By itself, the mod will only save the player's last known username, but using other mods like PermissionGroups and Overseer will save extra information here.

Any information saved can be safely modified and deleted as you'd like, there *should* be minimal to no problems with doing so.
