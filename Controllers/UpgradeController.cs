using Force.DeepCloner;
using GreenhouseRobinUp;
using StardewModdingAPI;
using StardewValley.GameData.Buildings;
using TheWarriorGW.GreenhouseRobinUp.Data;

namespace TheWarriorGW.GreenhouseRobinUp
{
    partial class ModEntry
    {
        public void AddGreenhouseUpgrades(IAssetData data)
        {
            var dict = data.AsDictionary<string, BuildingData>();

            for (int level = 1; level <= 2; level++)
            {
                SingleUpgrade cost = Config.UpgradeMaterials[level];
                
                BuildingData upgradeData = dict.Data["Greenhouse"].DeepClone();
                upgradeData.Name = level == 1
                    ? I18n.CarpenterShop_FirstBluePrint_Name()
                    : I18n.CarpenterShop_SecondBluePrint_Name();
                upgradeData.Texture = Config.UseCustomGH ? $"Buildings\\GreenhouseUp{level}": "Buildings/Greenhouse";
                upgradeData.SourceRect = new Microsoft.Xna.Framework.Rectangle(0, 160, 112, 160);
                upgradeData.Description = level == 1
                    ? I18n.CarpenterShop_FirstBluePrint_Description()
                    : I18n.CarpenterShop_SecondBluePrint_Description();
                upgradeData.Builder = "Robin";
                upgradeData.BuildCondition = $"GreenhouseRobinUp.BuildCondition {level}";
                upgradeData.BuildingToUpgrade = "Greenhouse";
                upgradeData.BuildCost = cost.Gold;
                upgradeData.BuildDays = cost.BuildDays;
                upgradeData.BuildMaterials = new List<BuildingMaterial>()
                {
                    new() { ItemId = $"(O){(int)cost.Slot1.MaterialID}", Amount = cost.Slot1.Quantity },
                    new() { ItemId = $"(O){(int)cost.Slot2.MaterialID}", Amount = cost.Slot2.Quantity },
                    new() { ItemId = $"(O){(int)cost.Slot3.MaterialID}", Amount = cost.Slot3.Quantity }
                };

                // Establecer ModData
                upgradeData.ModData = new()
                {
                    { "TheWarriorGW.GreenhouseRobinUp.GreenLevel", level.ToString() }
                };

                // Agregar al diccionario
                dict.Data.Add($"GreenhouseRobinUp.Upgrade{level}", upgradeData);
            }
        }
    }
}
