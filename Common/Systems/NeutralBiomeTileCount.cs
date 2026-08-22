using System;
using TimeCrusadeMod.Content.Tiles;
using Terraria;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Common.Systems
{
    public class NeutralBiomeTileCount : ModSystem
    {
        public int NeutralStoneCount { get; private set; }
        public int NeutralChiselCount { get; private set; }

        public int NeutralityCrystalCount { get; private set; }

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            double worldScale = Math.Sqrt((double)(Main.maxTilesX * Main.maxTilesY) / (4200.0 * 1800.0));

            NeutralChiselCount = (int)Math.Round(250.0 * worldScale);
            NeutralChiselCount += tileCounts[ModContent.TileType<NeutralChisel>()];
            NeutralStoneCount = (int)Math.Round(100.0 * worldScale);
            NeutralStoneCount += tileCounts[ModContent.TileType<NeutralStone>()];
            NeutralityCrystalCount = (int)Math.Round(25.0 * worldScale);
            NeutralityCrystalCount += tileCounts[ModContent.TileType<NeutralityCrystal>()];
        }
    }
}
