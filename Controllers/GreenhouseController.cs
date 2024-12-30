using StardewModdingAPI.Events;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.TerrainFeatures;
using Microsoft.Xna.Framework;

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
        private void MoveTrees()
        {
            if (!Context.IsWorldReady) return;
            var gh = Game1.getFarm().buildings.OfType<GreenhouseBuilding>().FirstOrDefault();
            if (gh == null) return;
            int level = GetUpgradeLevel(gh);
            var ghl = gh.GetIndoors();
            if (ghl == null) return;

            var treesToMove = ghl.terrainFeatures.Values.OfType<FruitTree>();
            Dictionary<int, Vector2> moveToLevel = new()
            {
                { 0, new(0, 0) },
                { 1, new(4, 4) },
                { 2, new(6, 6) }
            };
            List<FruitTree> ListBottomLeft = new();
            List<FruitTree> ListBottomRight = new();
            List<FruitTree> ListRight = new();
            List<FruitTree> ListTopRight = new();
            foreach (var tree in treesToMove)
            {
                if (BottomLeftSideTree(level, tree))
                {
                    Console.WriteLine($"Adding tree at {tree.Tile.X} {tree.Tile.Y} to List 1 (BottomLeftSide)");
                    ListBottomLeft.Add(tree);
                }
                else if (BottomRightSideTree(level, tree))
                {
                    Console.WriteLine($"Adding tree at {tree.Tile.X} {tree.Tile.Y} to List 2 (BottomRightSide)");
                    ListBottomRight.Add(tree);
                }
                else if (RightSideTree(level, tree))
                {
                    Console.WriteLine($"Adding tree at {tree.Tile.X} {tree.Tile.Y} to List 2 (RightSide)");
                    ListRight.Add(tree);
                }
                else if (TopSideTree(level, tree))
                {
                    Console.WriteLine($"Adding tree at {tree.Tile.X} {tree.Tile.Y} to List 2 (TopSide)");
                    ListTopRight.Add(tree);
                }
                else
                {
                    Console.WriteLine($"Tree at {tree.Tile.X} {tree.Tile.Y} did not meet criteria for any list");
                }
            }
            foreach (var tree in ListBottomRight)
            {
                Vector2 newLocation = new(tree.Tile.X + moveToLevel[level].X, tree.Tile.Y + moveToLevel[level].Y);
                ghl.terrainFeatures.Remove(tree.Tile);
                Console.WriteLine($"ListBottomRight: Adding {newLocation.X}, {newLocation.Y}");
                ghl.terrainFeatures.Add(newLocation, tree);
            }
            foreach (var tree in ListBottomLeft)
            {
                Vector2 newLocation = new(tree.Tile.X, tree.Tile.Y + moveToLevel[level].Y);
                ghl.terrainFeatures.Remove(tree.Tile);
                Console.WriteLine($"ListBottomLeft: Adding {newLocation.X}, {newLocation.Y}");
                ghl.terrainFeatures.Add(newLocation, tree);
            }
            foreach (var tree in ListRight)
            {
                Vector2 newLocation = new(tree.Tile.X + moveToLevel[level].X, tree.Tile.Y);
                ghl.terrainFeatures.Remove(tree.Tile);
                Console.WriteLine($"ListRight: Adding {newLocation.X}, {newLocation.Y}");
                ghl.terrainFeatures.Add(newLocation, tree);
            }
            foreach (var tree in ListTopRight)
            {
                Vector2 newLocation = new(tree.Tile.X + moveToLevel[level].X, tree.Tile.Y);
                ghl.terrainFeatures.Remove(tree.Tile);
                Console.WriteLine($"ListTopRight: Adding {newLocation.X}, {newLocation.Y}");
                ghl.terrainFeatures.Add(newLocation, tree);
            }
            
        }
        private static bool BottomRightSideTree(int newLevel, FruitTree fruitTree)
        {
            float x = fruitTree.Tile.X;
            float y = fruitTree.Tile.Y;
            switch (newLevel)
            {
                case 1:
                    return (double)x >= 10.0 && (double)x <= 16.0 && (double)y >= 22.0 && (double)y <= 23.0;
                case 2:
                    return (double)x >= 12.0 && (double)x <= 20.0 && (double)y >= 26.0 && (double)y <= 27.0;
                default:
                    return false;
            }
        }
        private static bool BottomLeftSideTree(int newLevel, FruitTree fruitTree)
        {
            float x = fruitTree.Tile.X;
            float y = fruitTree.Tile.Y;
            switch (newLevel)
            {
                case 1:
                    return (double)x >= 2.0 && (double)x <= 8.0 && (double)y >= 22.0 && (double)y <= 23.0;
                case 2:
                    return (double)x >= 2.0 && (double)x <= 10.0 && (double)y >= 26.0 && (double)y <= 27.0;
                default:
                    return false;
            }
        }
        private static bool RightSideTree(int newLevel, FruitTree fruitTree)
        {
            float x = fruitTree.Tile.X;
            float y = fruitTree.Tile.Y;
            switch (newLevel)
            {
                case 1:
                    return (double)x >= 16.0 && (double)x <= 17.0 && (double)y >= 9.0 && (double)y <= 21.0;
                case 2:
                    return (double)x >= 20.0 && (double)x <= 21.0 && (double)y >= 9.0 && (double)y <= 27.0;
                default:
                    return false;
            }
        }
        private static bool TopSideTree(int newLevel, FruitTree fruitTree)
        {
            float x = fruitTree.Tile.X;
            float y = fruitTree.Tile.Y;
            switch (newLevel)
            {
                case 1:
                    return (double)x >= 10.0 && (double)x <= 15.0 && (double)y >= 7.0 && (double)y <= 8.0;
                case 2:
                    return (double)x >= 12.0 && (double)x <= 19.0 && (double)y >= 7.0 && (double)y <= 8.0;
                default:
                    return false;
            }
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
            //MoveTrees();
        }
        private void OnDayEnding(object sender, DayEndingEventArgs e)
        {
            FixGreenhouseType("OnDayEnding");
        }

        internal void OnLoad(object sender, SaveLoadedEventArgs e)
        {
            FixGreenhouseType("OnLoad");
            //MoveTrees();
        }

        internal void OnSaveCompleted(object sender, SavedEventArgs e)
        {
            FixGreenhouseType("OnSaveCompleted");
        }

        internal void OnSaving(object sender, SavingEventArgs e)
        {
            FixGreenhouseType("OnSaving");
            //MoveTrees();
        }
    }
}
