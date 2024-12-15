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


            Monitor.Log($"Greenhouse is now level {args[0]}.", LogLevel.Info);

        }
        private void GetGHLevel(string command, string[] args)
        {
            var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();

            Monitor.Log($"Greenhouse level {GetUpgradeLevel(gh)}.", LogLevel.Info);
        }

        private void ReloadGH(string command, string[] args)
        {
            if (!Context.IsWorldReady) return;
            //Helper.GameContent.InvalidateCache("Maps/Greenhouse");
            //if (Config.UseCustomGH) Helper.GameContent.InvalidateCache("Buildings/Greenhouse");
            //if (Config.UseCustomGH) Helper.GameContent.InvalidateCache("Buildings/CustomGH1");
            //if (Config.UseCustomGH) Helper.GameContent.InvalidateCache("Buildings/CustomGH2");
            //var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();
            //if (gh == null) Console.WriteLine("No gh found");
            //gh.ReloadBuildingData();
            //Helper.GameContent.InvalidateCache("Maps/Greenhouse");
            //Helper.GameContent.InvalidateCache("Maps/GreenhouseUp1");
            //Helper.GameContent.InvalidateCache("Maps/GreenhouseUp2");
            
            Monitor.Log($"Reloaded", LogLevel.Info);
        }

        private void PrintGH(string command, string[] args)
        {
            VerifyGreenhouseUpgrades();
        }
    }
}
