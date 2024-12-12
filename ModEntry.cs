using System.Reflection.Emit;
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
            return greenhouse.modData.TryGetValue(ModDataKey, out string level)
               ? int.Parse(level)
               : 0;
        }

        public override void Entry(IModHelper helper)
        {
            //set up for translations
            I18n.Init(helper.Translation);
            //read settings
            Config = Helper.ReadConfig<ModConfig>();

            helper.ConsoleCommands.Add("ghruset", "Sets the size of the Greenhouse.\n\nUsage: ghruset <value>\n- value: integer between 0 and 2 only", SetGHLevel);
            helper.ConsoleCommands.Add("ghruget", "Returns current size of the Greenhouse.\n\nUsage: ghruget", GetGHLevel);

            //Register Event Listeners
            helper.Events.GameLoop.SaveLoaded += OnLoad;
            helper.Events.GameLoop.Saved += OnSaveCompleted;
            helper.Events.Display.MenuChanged += OnMenuChanged;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Content.AssetRequested += OnAssetRequested;

            //Set de condition for the upgrade
            GameStateQuery.Register("GreenhouseRobinUp.BuildCondition", CheckForUpgrade);
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

        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (Context.IsWorldReady && e.Name.IsEquivalentTo("Maps/Greenhouse"))
            {
                var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();
                int level = GetUpgradeLevel(gh);

                if (level > 0 && level <= 2)
                {
                    e.LoadFromModFile<Map>($"assets/Greenhouse_{level}.tmx", AssetLoadPriority.Medium);
                }
            }

            //if (Context.IsWorldReady && e.Name.IsEquivalentTo("Buildings/Greenhouse") && Config.UseCustomGH)
            //{
            //    var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();
            //    int level = GetUpgradeLevel(gh);

            //    Console.WriteLine($"Greenhouse requested, now level: {level}");

            //    if (level > 0 && level <= 2)
            //    {
            //        e.LoadFromModFile<Texture2D>($"assets/CustomGH_{GetUpgradeLevel(gh)}.png", AssetLoadPriority.Medium);
            //    }
            //};

            //if (e.Name.IsEquivalentTo("Buildings/GreenhouseUp1") || e.Name.IsEquivalentTo("Buildings/GreenhouseUp2") || e.Name.IsEquivalentTo("Buildings/Greenhouse") && Config.UseCustomGH && Context.IsWorldReady)
            //{
            //    var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();
            //    int level = GetUpgradeLevel(gh);
            //    if (level > 0 && level <= 2)
            //    {
            //        Console.WriteLine($"Se pidio CustomGH{level}, siguiente {level + 1}");
            //        e.LoadFromModFile<Texture2D>($"assets/CustomGH_{level}.png", AssetLoadPriority.Medium);
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Se pidio Greenhouse normal");
            //        //e.LoadFrom(() => this.Helper.GameContent.Load<Texture2D>("Buildings/Greenhouse"), AssetLoadPriority.Medium);
            //    }
            //}

            //if (Context.IsWorldReady)
            //{
            if (e.Name.IsEquivalentTo("Buildings/GreenhouseUp1"))
            {
                if (Config.UseCustomGH)
                {
                    e.LoadFromModFile<Texture2D>("assets/CustomGH_1.png", AssetLoadPriority.Medium);
                }
                else
                {
                    e.LoadFrom(() => this.Helper.GameContent.Load<Texture2D>("Buildings/Greenhouse"), AssetLoadPriority.Medium);
                }
                Console.WriteLine("Buildings/GreenhouseUp1 requested");
            }
            if (e.Name.IsEquivalentTo("Buildings/GreenhouseUp2"))
            {
                if (Config.UseCustomGH)
                {
                    e.LoadFromModFile<Texture2D>("assets/CustomGH_2.png", AssetLoadPriority.Medium);
                }
                else
                {
                    e.LoadFrom(() => this.Helper.GameContent.Load<Texture2D>("Buildings/Greenhouse"), AssetLoadPriority.Medium);
                }
                Console.WriteLine("Buildings/GreenhouseUp2 requested");
            }
            //if (e.Name.IsEquivalentTo("Buildings/Greenhouse"))
            //{
            //    Console.WriteLine("Buildings/Greenhouse requested");
            //    e.LoadFrom(() => Game1.content.Load<Texture2D>("Buildings/Greenhouse"), AssetLoadPriority.Medium);
            //}
            //}

            //if (Config.UseCustomGH)
            //{
            //    if(e.Name.IsEquivalentTo("Buildings/GreenhouseUp1")) e.LoadFromModFile<Texture2D>("assets/CustomGH_1.png", AssetLoadPriority.Medium);
            //    if(e.Name.IsEquivalentTo("Buildings/GreenhouseUp2")) e.LoadFromModFile<Texture2D>("assets/CustomGH_2.png", AssetLoadPriority.Medium);
            //}

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
