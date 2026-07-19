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
	public class NeutralChisel : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tilePile[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<NeutralStone>()] = true;
            Main.tileMerge[Type][ModContent.TileType<NeutralityCrystal>()] = true;

			DustType = ModContent.DustType<NeutralDust>();
			VanillaFallbackOnModDeletion = TileID.DiamondGemspark;

			AddMapEntry(new Color(135, 135, 135));
			HitSound = SoundID.Tink;
				MineResist = 1f;
				MinPick = 43;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override void ChangeWaterfallStyle(ref int style) {
			style = ModContent.GetInstance<NeutralWaterfallStyle>().Slot;
		}
	}
}