using StardewModdingAPI.Events;

namespace TheWarriorGW.GreenhouseRobinUp
{
    partial class ModEntry
    {
        internal void OnLoad(object sender, SaveLoadedEventArgs e)
        {

            Monitor.Log("Invalidating Texture Cache at first Load");
            //Helper.GameContent.InvalidateCache("Maps/Greenhouse");
            //if (Config.UseCustomGH) Helper.GameContent.InvalidateCache("Buildings/Greenhouse");
        }

        internal void OnSaveCompleted(object sender, SavedEventArgs e)
        {
            Monitor.Log("Invalidating Texture Cache after save");
            //Helper.GameContent.InvalidateCache("Maps/Greenhouse");
            //if (Config.UseCustomGH) Helper.GameContent.InvalidateCache("Buildings/Greenhouse");
        }
    }
}
