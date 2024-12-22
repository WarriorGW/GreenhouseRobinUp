using Force.DeepCloner;
using GreenhouseRobinUp;
using StardewModdingAPI;
using StardewValley.Buildings;
using StardewValley;
using StardewValley.GameData.Buildings;
using TheWarriorGW.GreenhouseRobinUp.Data;
using GreenhouseRobinUp.ManualDebug;
using Microsoft.Xna.Framework;

namespace TheWarriorGW.GreenhouseRobinUp
{
    partial class ModEntry
    {
        public void VerifyGreenhouseUpgrades()
        {
            Monitor.Log("Verifying Greenhouse upgrades...", LogLevel.Info);  // Log al inicio del método
            if (Game1.currentLocation is Farm farm)
            {
                foreach (var building in farm.buildings)
                {
                    if (building is GreenhouseBuilding greenhouse)
                    {
                        Console.WriteLine("#############################################");
                        Console.WriteLine($"Building type: {greenhouse.buildingType.Value}");
                        Console.WriteLine($"Building ID: {greenhouse.id}");
                        Console.WriteLine($"Greenhouse found at position: {greenhouse.tileX}, {greenhouse.tileY}");
                        Console.WriteLine($"Upgrade level: {GetUpgradeLevel(greenhouse)}");
                        Console.WriteLine($"Using map indoors: {greenhouse.indoors.Name}");
                        Console.WriteLine($"Get map indoors: {greenhouse.GetIndoors().Name}");
                        Console.WriteLine($"Name map indoors: {greenhouse.GetIndoorsName()}");
                        Console.WriteLine($"Type map indoors: {greenhouse.GetIndoorsType()}");
                        //Console.WriteLine($"Actual map indoors: {greenhouse.GetData().IndoorMap}");
                        Console.WriteLine($"{greenhouse.GetIndoors().warps}");
                        Console.WriteLine("#############################################");
                    }
                }
            }
        }

        private readonly Dictionary<int, string> GreenhouseSizes = new()
        {
            { 1, "15x15" },
            { 2, "21x21" }
        };

        public void AddGreenhouseUpgrades(IAssetData data)
        {
            var dict = data.AsDictionary<string, BuildingData>();

            Monitor.Log("Adding Greenhouse upgrades...", LogLevel.Info);

            for (int level = 1; level <= 2; level++)
            {
                Monitor.Log($"Adding upgrade for level {level}...", LogLevel.Info);

                SingleUpgrade cost = Config.UpgradeMaterials[level];
                BuildingData upgradeData = dict.Data["Greenhouse"].DeepClone();

                string sizeDescription = GreenhouseSizes.ContainsKey(level) ? GreenhouseSizes[level] : "unknown size";

                upgradeData.Name = I18n.CarpenterShop_BluePrint_Name(level);
                upgradeData.Texture = "Buildings/Greenhouse";
                upgradeData.Description = I18n.CarpenterShop_BluePrint_Description(sizeDescription);
                upgradeData.Builder = "Robin";
                upgradeData.BuildCondition = $"GreenhouseRobinUp.BuildCondition {level},PLAYER_HAS_MAIL Any ccPantry";
                upgradeData.BuildingToUpgrade = "Greenhouse";
                upgradeData.BuildCost = cost.Gold;
                upgradeData.BuildDays = cost.BuildDays;
                upgradeData.BuildMaterials = CreateBuildMaterials(cost);
                upgradeData.ModData[ModDataKey] = level.ToString();

                // Agregar al diccionario de construcciones de Robin
                dict.Data.Add($"GreenhouseRobinUp{level}", upgradeData);

                // Just to debug
                //var dataBuildingsDebug = new DataBuildingsDebug();
                //dataBuildingsDebug.DataBuildings(data, Helper);
            }
        }

        private static List<BuildingMaterial> CreateBuildMaterials(SingleUpgrade cost)
        {
            return new List<BuildingMaterial>
            {
                new() { ItemId = $"(O){(int)cost.Slot1.MaterialID}", Amount = cost.Slot1.Quantity },
                new() { ItemId = $"(O){(int)cost.Slot2.MaterialID}", Amount = cost.Slot2.Quantity },
                new() { ItemId = $"(O){(int)cost.Slot3.MaterialID}", Amount = cost.Slot3.Quantity }
            };
        }
    }
}
