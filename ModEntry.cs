using System.Collections;
using System.Reflection.Emit;
using System.Text.Json;
using Force.DeepCloner;
using GreenhouseRobinUp;
using GreenhouseRobinUp.APIs;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Delegates;
using StardewValley.GameData.Buildings;
using StardewValley.GameData.Locations;
using TheWarriorGW.GreenhouseRobinUp.Data;
using xTile;

namespace TheWarriorGW.GreenhouseRobinUp
{
    partial class ModEntry : Mod
    {
        private ModConfig Config;
        const string ModDataKey = "TheWarriorGW.GreenhouseRobinUp.GreenLevel";

        public static int GetUpgradeLevel(GreenhouseBuilding greenhouse)
        {
            //return greenhouse.modData.TryGetValue(ModDataKey, out string level)
            //   ? int.Parse(level)
            //   : 0;
            // Si modData contiene el valor de "GreenhouseRobinUp.GreenLevel"
            if (greenhouse.modData.TryGetValue(ModDataKey, out string level))
            {
                if (int.TryParse(level, out int parsedLevel))
                {
                    Console.WriteLine($"Greenhouse level found in ModData from GetUpgradeLevel(): {parsedLevel}");
                    return parsedLevel; // Retorna el valor correctamente parseado
                }
                else
                {
                    Console.WriteLine($"Invalid Greenhouse level in ModData: {level}. Returning default level 0.");
                    return 0; // Si el valor no es un número válido, regresa el nivel por defecto (0)
                }
            }
            else
            {
                // Si no existe el ModData, posiblemente la *Greenhouse* no ha sido actualizada, así que asignamos nivel 0.
                Console.WriteLine("No Greenhouse level found in ModData. Returning default level 0.");
                return 0;
            }
        }

        public override void Entry(IModHelper helper)
        {
            //set up for translations
            I18n.Init(helper.Translation);
            //read settings
            Config = Helper.ReadConfig<ModConfig>();

            helper.ConsoleCommands.Add("ghruset", "Sets the size of the Greenhouse.\n\nUsage: ghruset <value>\n- value: integer between 0 and 2 only", SetGHLevel);
            helper.ConsoleCommands.Add("ghruget", "Returns current size of the Greenhouse.\n\nUsage: ghruget", GetGHLevel);
            helper.ConsoleCommands.Add("reloadgh", "Reloads Greenhouse data.\n\nUsage: reloadgh", ReloadGH);
            helper.ConsoleCommands.Add("printgh", "Prints the info of all Greenhouses.\n\nUsage: printgh", PrintGH);

            //Register Event Listeners
            helper.Events.GameLoop.SaveLoaded += OnLoad;
            helper.Events.GameLoop.Saved += OnSaveCompleted;
            helper.Events.Display.MenuChanged += OnMenuChanged;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Content.AssetRequested += OnAssetRequested;
            helper.Events.Display.RenderedWorld += OnRenderedWorld;

            helper.Events.Content.AssetReady += OnAssetReady;
            helper.Events.World.BuildingListChanged += OnBuildingListChanged;
            helper.Events.World.LocationListChanged += OnLocationListChanged;

            //Set de condition for the upgrade
            GameStateQuery.Register("GreenhouseRobinUp.BuildCondition", CheckForUpgrade);
        }

        private void OnAssetReady(object sender, AssetReadyEventArgs e)
        {
            //// Verificar si el asset solicitado es 'Data/Buildings'
            //if (e.NameWithoutLocale.IsEquivalentTo("Data/Buildings"))
            //{
            //    // Obtener los datos del asset como un diccionario
            //    var buildingsData = Helper.GameContent.Load<>("Data/Buildings");

            //    // Convertir el diccionario a JSON usando System.Text.Json
            //    string json = JsonSerializer.Serialize(buildingsData, new JsonSerializerOptions { WriteIndented = true });

            //    // Especificamos la ruta del archivo donde queremos guardar los datos
            //    string filePath = Path.Combine(Helper.DirectoryPath, "BuildingsData.json");

            //    // Escribimos el JSON en el archivo
            //    File.WriteAllText(filePath, json);

            //    // Mensaje de depuración
            //    Monitor.Log($"Los datos de 'Data/Buildings' se han guardado en {filePath}.", LogLevel.Info);
            //}
            //if (e.Name.IsEquivalentTo("Maps/GreenhouseUp1"))
            //{
            //    this.Data
            //}
        }

        private void OnBuildingListChanged(object sender, BuildingListChangedEventArgs e)
        {
            Monitor.Log($"Building list changed:", LogLevel.Info);
            foreach (var builds in e.Added)
            {
                Monitor.Log($"{builds.buildingType} buildings added", LogLevel.Info);
            }
            Monitor.Log($"{e.Removed} buildings removed.", LogLevel.Info);
            foreach (var builds in e.Removed)
            {
                Monitor.Log($"{builds.buildingType} buildings removed", LogLevel.Info);
            }
        }

        private void OnLocationListChanged(object sender, LocationListChangedEventArgs e)
        {
            Monitor.Log($"Location list changed:", LogLevel.Info);
            foreach (var locs in e.Added)
            {
                Monitor.Log($"{locs.Name} location added", LogLevel.Info);
            }
            Monitor.Log($"{e.Removed} location removed.", LogLevel.Info);
            foreach (var locs in e.Removed)
            {
                Monitor.Log($"{locs.Name} location removed", LogLevel.Info);
            }
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            //Monitor.Log($"Loaded config: Build1={Config.UpgradeMaterials[1].Gold} {Config.UpgradeMaterials[1].Slot1.Quantity} {Config.UpgradeMaterials[1].Slot1.Quantity}", LogLevel.Info);

            // API Content patcher
            var api = Helper.ModRegistry.GetApi<IContentPatcherAPI>("Pathoschild.ContentPatcher");
            api?.RegisterToken(ModManifest, "GreenHouseLevel", () =>
            {
                if (Context.IsWorldReady)
                {
                    var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();
                    return new[] { GetUpgradeLevel(gh).ToString() };
                }
                else return new[] { "0" };
            });

            //Game1.content.Load<Map>("Maps/GreenhouseUp1");
            //Game1.content.Load<Map>("Maps/GreenhouseUp2");
            LoadModConfigMenu();
        }

        internal bool CheckForUpgrade(string[] query, GameStateQueryContext context)
        {
            var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();
            int bluePrintLevel = GetUpgradeLevel(gh) + 1;
            bool parseTest = int.TryParse(query[^1], out int requestedLevel);

            if (!parseTest)
            {
                Monitor.Log($"{string.Join(" ", query)} is not a valid query, {query[^1]} was unable to be parsed to an int.", LogLevel.Error);
                return false;
            }

            //Don't add blueprint if haven't built prev level
            if (bluePrintLevel != requestedLevel) return false;

            return true;
        }

        //private int frameCounter = 0;
        //private const int logFrequency = 600; // Número de cuadros entre logs

        private void OnRenderedWorld(object sender, RenderedWorldEventArgs e)
        {
            //frameCounter++;
            //if (frameCounter >= logFrequency)
            //{
            //    VerifyGreenhouseUpgrades();
            //    frameCounter = 0; // Reinicia el contador
            //}
        }

        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/Locations"))
            {
                Console.WriteLine($"Asset requested: {e.Name}");
                e.Edit(asset =>
                {
                    var dict = asset.AsDictionary<string, LocationData>();
                    foreach (var locationEntry in dict.Data)
                    {
                        var locationName = locationEntry.Key; // Nombre de la ubicación (ID)
                        var location = locationEntry.Value; // Objeto de la ubicación
                        if (locationName.Contains("Greenhouse"))
                        {
                            Console.WriteLine($"Location ID: {locationName}");
                            Console.WriteLine($"DisplayName: {location.DisplayName}");
                            Console.WriteLine($"MapPath: {location.CreateOnLoad?.MapPath}");
                        }
                    }
                });
            }

            if (e.Name.StartsWith("Buildings/G") || e.Name.StartsWith("Maps/G") || e.Name.StartsWith("Data/B"))
            {
                Console.WriteLine($"Asset requested: {e.Name}");
            }

            if (e.Name.IsEquivalentTo("Buildings/GreenhouseUp1"))
            {
                Console.WriteLine("Loading CustomGH_1");
                e.LoadFromModFile<Texture2D>("assets/CustomGH_1.png", AssetLoadPriority.Medium);
            }

            if (e.Name.IsEquivalentTo("Buildings/GreenhouseUp2"))
            {
                Console.WriteLine("Loading CustomGH_2");
                e.LoadFromModFile<Texture2D>("assets/CustomGH_2.png", AssetLoadPriority.Medium);
            }

            if (e.NameWithoutLocale.IsEquivalentTo("Data/Buildings"))
            {
                e.Edit(delegate (IAssetData data)
                {
                    AddGreenhouseUpgrades(data);
                });
            }

        }
    }
}
