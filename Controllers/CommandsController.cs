using StardewModdingAPI;
using StardewValley.Buildings;
using StardewValley;

namespace TheWarriorGW.GreenhouseRobinUp
{
    partial class ModEntry
    {
        private void SetGHLevel(string command, string[] args)
        {
            SetUpgradeLevel(int.Parse(args[0]));
        }

        private void GetGHLevel(string command, string[] args)
        {
            var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();

            Monitor.Log($"Greenhouse level {GetUpgradeLevel(gh)}.", LogLevel.Info);
        }

        private void ReloadGH(string command, string[] args)
        {
            if (!Context.IsWorldReady) return;
            Helper.GameContent.InvalidateCache("Maps/Greenhouse");
            Monitor.Log($"Reloaded", LogLevel.Info);
        }

        private void PrintGH(string command, string[] args)
        {
            VerifyGreenhouseUpgrades();
        }

        // Funcion para mover los arboles
        private void MoveTrees(string command, string[] args)
        {
            MoveTrees();
        }
    }
}
