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
                        Console.WriteLine($"Building second try to ID: {greenhouse.upgradeName}");
                        Console.WriteLine($"Greenhouse found at position: {greenhouse.tileX}, {greenhouse.tileY}");
                        Console.WriteLine($"Upgrade level: {GetUpgradeLevel(greenhouse)}");
                        Console.WriteLine($"Using map indoors: {greenhouse.indoors.Name}");
                        Console.WriteLine($"Get map indoors: {greenhouse.GetIndoors().Name}");
                        Console.WriteLine($"Name map indoors: {greenhouse.GetIndoorsName()}");
                        Console.WriteLine($"Type map indoors: {greenhouse.GetIndoorsType()}");
                        //Console.WriteLine($"Actual map indoors: {greenhouse.GetData().IndoorMap}");
                        Console.WriteLine($"{greenhouse.GetIndoors().warps}");
                        //Console.WriteLine($"Upgrade name: {greenhouse.upgradeName}");
                        //Console.WriteLine(Game1.getLocationFromName("Greenhouse").GetData().);
                        Console.WriteLine("#############################################");
                    }
                }
                //if (greenhouses.Count > 1)
                //{
                //    GreenhouseBuilding highestLevelGreenhouse = greenhouses
                //        .OrderByDescending(g => GetUpgradeLevel(g))
                //        .First();

                //    foreach (var greenhouse in greenhouses)
                //    {
                //        if (greenhouse != highestLevelGreenhouse)
                //        {
                //            Vector2 ghlocation = new(greenhouse.tileX, greenhouse.tileY);
                //            farm.destroyStructure(ghlocation); // Elimina la Greenhouse
                //            Monitor.Log($"Removed duplicate Greenhouse at ({greenhouse.tileX}, {greenhouse.tileY}).", LogLevel.Warn);
                //        }
                //    }

                //    Monitor.Log($"Kept Greenhouse at ({highestLevelGreenhouse.tileX}, {highestLevelGreenhouse.tileY}) with level {GetUpgradeLevel(highestLevelGreenhouse)}.", LogLevel.Info);
                //}
            }
        }

        public void AddGreenhouseUpgrades(IAssetData data)
        {
            var dict = data.AsDictionary<string, BuildingData>();

            Monitor.Log("Adding Greenhouse upgrades...", LogLevel.Info);  // Log al inicio del método

            //BuildingData ghData = dict.Data["Greenhouse"];

            //if (ghData.ModData[ModDataKey] == null)
            //{
            //    ghData.ModData = new()
            //    {
            //        { "TheWarriorGW.GreenhouseRobinUp.GreenLevel", "0" }
            //    };
            //}

            //if (!ghData.ModData.ContainsKey(ModDataKey))
            //{
            //    ghData.ModData[ModDataKey] = "0";
            //}

            for (int level = 1; level <= 2; level++)
            {
                Monitor.Log($"Adding upgrade for level {level}...", LogLevel.Info);  // Log por cada nivel de actualización

                SingleUpgrade cost = Config.UpgradeMaterials[level];

                BuildingData upgradeData = dict.Data["Greenhouse"].DeepClone();
                //BuildingData upgradeData = dict.Data["Greenhouse"];

                upgradeData.Name = level == 1
                    ? I18n.CarpenterShop_FirstBluePrint_Name()
                    : I18n.CarpenterShop_SecondBluePrint_Name();
                //upgradeData.Texture = Config.UseCustomGH ? $"Buildings\\GreenhouseUp{level}" : "Buildings/Greenhouse";
                upgradeData.Texture = "Buildings/Greenhouse";
                //upgradeData.SourceRect = new Rectangle(0, 160, 112, 160);
                //upgradeData.IndoorMap = $"GreenhouseUp{level}";
                //upgradeData.NonInstancedIndoorLocation = $"GreenhouseRobinUp.GreenhouseUp{level}";
                upgradeData.Description = level == 1
                    ? I18n.CarpenterShop_FirstBluePrint_Description()
                    : I18n.CarpenterShop_SecondBluePrint_Description();
                upgradeData.Builder = "Robin";
                upgradeData.BuildCondition = $"GreenhouseRobinUp.BuildCondition {level}";
                //upgradeData.BuildingToUpgrade = level == 1 ? "Greenhouse" : "Big Greenhouse";
                upgradeData.BuildingToUpgrade = "Greenhouse";
                upgradeData.BuildCost = cost.Gold;
                upgradeData.BuildDays = cost.BuildDays;
                upgradeData.BuildMaterials = new List<BuildingMaterial>()
                {
                    new() { ItemId = $"(O){(int)cost.Slot1.MaterialID}", Amount = cost.Slot1.Quantity },
                    new() { ItemId = $"(O){(int)cost.Slot2.MaterialID}", Amount = cost.Slot2.Quantity },
                    new() { ItemId = $"(O){(int)cost.Slot3.MaterialID}", Amount = cost.Slot3.Quantity }
                };

                Monitor.Log($"Setting ModData for level {level}...", LogLevel.Info); // Log la asignación de ModData

                // Establecer ModData
                upgradeData.ModData = new()
                {
                    { "TheWarriorGW.GreenhouseRobinUp.GreenLevel", level.ToString() }
                };

                // Agregar al diccionario
                dict.Data.Add($"GreenhouseRobinUp{level}", upgradeData);

                //var dataBuildingsDebug = new DataBuildingsDebug();
                //dataBuildingsDebug.DataBuildings(data, Helper);
            }
        }
    }
}
