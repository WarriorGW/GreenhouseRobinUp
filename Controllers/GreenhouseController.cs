using StardewModdingAPI.Events;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Menus;

namespace TheWarriorGW.GreenhouseRobinUp
{
    // for anyone reading this file, the Greenhouse doubles because of the type, so we need to change it to the original type
    partial class ModEntry
    {
        public static int GetUpgradeLevel(GreenhouseBuilding greenhouse)
        {
            return greenhouse.modData.TryGetValue(ModDataKey, out string level)
               ? int.Parse(level)
               : 0;
        }
        public void SetUpgradeLevel(int level)
        {
            var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();
            gh.modData[ModDataKey] = level.ToString();
            Monitor.Log($"Greenhouse is now level {level}.", LogLevel.Info);
        }
        private void FixGreenhouseType(string context)
        {
            var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();
            if (gh != null && gh.buildingType.Value.StartsWith("GreenhouseRobinUp"))
            {
                int ghLevel = GetUpgradeLevel(gh);
                Monitor.Log($"Level pre-change: {GetUpgradeLevel(gh)}", LogLevel.Debug);
                Monitor.Log($"{gh.buildingType.Value} building found in '{context}', changing building type to Greenhouse.", LogLevel.Info);
                gh.buildingType.Set("Greenhouse");
                Monitor.Log($"Updated building type: {gh.buildingType.Value}");
                Monitor.Log($"Level post-change: {GetUpgradeLevel(gh)}", LogLevel.Debug);
                SetUpgradeLevel(ghLevel);
                Monitor.Log($"Level post manual modify: {GetUpgradeLevel(gh)}", LogLevel.Debug);
            }
        }
        private void OnDayStarting(object sender, DayStartedEventArgs e)
        {
            FixGreenhouseType("OnDayStarting");
        }
        private void OnDayEnding(object sender, DayEndingEventArgs e)
        {
            FixGreenhouseType("OnDayEnding");
        }

        internal void OnLoad(object sender, SaveLoadedEventArgs e)
        {
            FixGreenhouseType("OnLoad");
        }

        internal void OnSaveCompleted(object sender, SavedEventArgs e)
        {
            FixGreenhouseType("OnSaveCompleted");
        }

        internal void OnSaving(object sender, SavingEventArgs e)
        {
            FixGreenhouseType("OnSaving");
        }
    }
}
