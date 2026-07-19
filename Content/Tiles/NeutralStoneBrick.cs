using TimeCrusadeMod.Content.Biomes;
using TimeCrusadeMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Tiles
{
	public class NeutralStoneBrick : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tilePile[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<NeutralityCrystalBrick>()] = true;
			Main.tileMerge[Type][ModContent.TileType<NeutralChiselBrick>()] = true;

			DustType = ModContent.DustType<NeutralDust>();
			VanillaFallbackOnModDeletion = TileID.DiamondGemspark;

			AddMapEntry(new Color(150, 150, 150));
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
}
