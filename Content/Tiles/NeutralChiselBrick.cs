using TimeCrusadeMod.Content.Biomes;
using TimeCrusadeMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using System.Collections.Generic;
using TimeCrusadeMod.Content.Tiles;

namespace TimeCrusadeMod.Content.Tiles
{
	public class NeutralChiselBrick : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
            Main.tilePile[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<NeutralStoneBrick>()] = true;
            Main.tileMerge[Type][ModContent.TileType<NeutralityCrystalBrick>()] = true;

			DustType = ModContent.DustType<NeutralDust>();
			VanillaFallbackOnModDeletion = TileID.DiamondGemspark;

			AddMapEntry(new Color(135, 135, 135));
			HitSound = SoundID.Tink;
				MineResist = 1f;
				MinPick = 35;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override void ChangeWaterfallStyle(ref int style) {
			style = ModContent.GetInstance<NeutralWaterfallStyle>().Slot;
		}
	}
        public class NeutralChiselBrickSystem : ModSystem
    {
        public static bool NeutralChiselSpawned = true;
        public override void PostWorldGen()
        {
            NeutralChiselSpawned = true;
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -2)
            {
                tasks.Insert(ShiniesIndex + 1, new PassLegacy("Neutral Chisel", (Progress, config) =>
                {
                    OreGeneration(Progress);
                }));
            }
        }

        private void OreGeneration(GenerationProgress progress)
        {
            progress.Message = "Generating Neutral Chisel";

            for (int i = 2; i < (int)((Main.maxTilesX * Main.maxTilesY) * 1); i++)
            {
                int x = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 95);
                int y = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 120);
                if (Main.tile[x, y].TileType == ModContent.TileType<NeutralStone>() && Main.tile[x, y].HasTile)
                {
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), ModContent.TileType<NeutralChisel>());
                }
            }
        }
    }
}