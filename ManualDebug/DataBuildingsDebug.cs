using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Force.DeepCloner;
using GreenhouseRobinUp;
using StardewModdingAPI;
using StardewValley.Buildings;
using StardewValley;
using StardewValley.GameData.Buildings;
using TheWarriorGW.GreenhouseRobinUp.Data;
using System.Text.Json;

namespace GreenhouseRobinUp.ManualDebug
{
    public class DataBuildingsDebug
    {
        private class GreenhouseUpgradeData
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Texture { get; set; }
            public string BuildingType { get; set; }
            public string IndoorMap { get; set; }
            public string BuildCondition { get; set; }
            public string BuildingToUpgrade { get; set; }
            public string NonInstanced { get; set; }
            public Dictionary<string, string> CustomFields { get; set; }
            public Dictionary<string, string> ModData { get; set; }
        }

        public void DataBuildings(IAssetData data, IModHelper Helper)
        {
            var buildingsData = data.AsDictionary<string, BuildingData>().Data;

            var relevantData = new Dictionary<string, GreenhouseUpgradeData>();

            foreach (var buildingEntry in buildingsData)
            {
                var building = buildingEntry.Value;

                var serializableBuilding = new GreenhouseUpgradeData
                {
                    Name = building.Name,
                    Description = building.Description,
                    Texture = building.Texture,
                    BuildingType = building.BuildingType,
                    IndoorMap = building.IndoorMap,
                    BuildCondition = building.BuildCondition,
                    BuildingToUpgrade = building.BuildingToUpgrade,
                    NonInstanced = building.NonInstancedIndoorLocation,
                    CustomFields = building.CustomFields,
                    ModData = building.ModData
                };

                relevantData.Add(buildingEntry.Key, serializableBuilding);
            }

            // Convertir el diccionario a JSON
            string jsonData = JsonSerializer.Serialize(relevantData, new JsonSerializerOptions { WriteIndented = true });

            // Guardar el JSON en un archivo
            string filePath = Path.Combine(Helper.DirectoryPath, "BuildingsData.json");
            File.WriteAllText(filePath, jsonData);

            // Mensaje de depuración
            Console.WriteLine($"Los datos de 'Data/Buildings' se han guardado en {filePath}.");
        }
    }
}
