using StardewModdingAPI;
using StardewValley.Buildings;
using StardewValley;

namespace TheWarriorGW.GreenhouseRobinUp
{
    partial class ModEntry
    {
        private void SetGHLevel(string command, string[] args)
        {
            var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();
            gh.modData[ModDataKey] = args[0];

            Helper.GameContent.InvalidateCache("Maps/Greenhouse");
            if (Config.UseCustomGH) Helper.GameContent.InvalidateCache("Buildings/Greenhouse");
            if (Config.UseCustomGH) Helper.GameContent.InvalidateCache("Buildings/CustomGH1");
            if (Config.UseCustomGH) Helper.GameContent.InvalidateCache("Buildings/CustomGH2");

            Monitor.Log($"Greenhouse is now level {args[0]}.", LogLevel.Info);

        }
        private void GetGHLevel(string command, string[] args)
        {
            var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();

            Monitor.Log($"Greenhouse level {GetUpgradeLevel(gh)}.", LogLevel.Info);
        }

        private void CleanGH(string command, string[] args)
        {
            Helper.GameContent.InvalidateCache("Maps/Greenhouse");
            if (Config.UseCustomGH) Helper.GameContent.InvalidateCache("Buildings/Greenhouse");
            if (Config.UseCustomGH) Helper.GameContent.InvalidateCache("Buildings/CustomGH1");
            if (Config.UseCustomGH) Helper.GameContent.InvalidateCache("Buildings/CustomGH2");
            Monitor.Log($"Cache cleaned", LogLevel.Info);
        }
    }
}
