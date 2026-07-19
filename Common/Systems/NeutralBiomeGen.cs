using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TimeCrusadeMod.Content.Tiles;

namespace TimeCrusadeMod.Common.Systems
{
    public class NeutralBiomeGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (genIndex != -1)
            {
                tasks.Insert(genIndex + 1, new PassLegacy("Neutral Biome", (generationProgress, config) =>
                {
                    generationProgress.Message = "Generating Neutral Biome";

                    // Scale the biome size with the current world dimensions so small and large worlds
                    // generate proportionally sized neutral biome blobs instead of the same fixed area.
                    double worldScale = Math.Sqrt((double)(Main.maxTilesX * Main.maxTilesY) / (4200.0 * 1800.0));
                    int stoneRadius = Math.Max(180, (int)Math.Round(525.0 * worldScale));
                    int chiselRadius = Math.Max(120, (int)Math.Round(345.0 * worldScale));
                    int crystalRadius = Math.Max(60, (int)Math.Round(125.0 * worldScale));

                    int spreadX = Math.Max(40, (int)Math.Round(Main.maxTilesX * 0.02));
                    int spreadY = Math.Max(40, (int)Math.Round(Main.maxTilesY * 0.06));

                    int x = Main.maxTilesX / 2 + WorldGen.genRand.Next(-spreadX, spreadX + 1);
                    int y = Main.maxTilesY / 2 + WorldGen.genRand.Next(-spreadY, spreadY + 1);
                    y = Math.Max((int)Main.rockLayer + 60, Math.Min(y, Main.maxTilesY - 120));

                    // TileRunner creates a blob-like shape of tiles in that area.
                    // Parameters: X, Y, Strength (Size), Steps, TileType
                    WorldGen.TileRunner(x, y, stoneRadius, stoneRadius, (ushort)ModContent.TileType<NeutralStone>());
                    WorldGen.TileRunner(x, y, chiselRadius, chiselRadius, (ushort)ModContent.TileType<NeutralChisel>());
                    WorldGen.TileRunner(x, y, crystalRadius, crystalRadius, (ushort)ModContent.TileType<NeutralityCrystal>());
                }));                               
    }
    }
    }
}
