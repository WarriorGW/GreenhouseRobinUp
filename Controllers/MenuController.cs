using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Menus;

namespace TheWarriorGW.GreenhouseRobinUp
{
    partial class ModEntry
    {
        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.OldMenu is CarpenterMenu)
            {
                var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();
                if (gh.buildingType.Value.StartsWith("GreenhouseRobinUp"))
                {
                    gh.buildingType.Set("Greenhouse");                  
                }
            }
        }
    }
}
