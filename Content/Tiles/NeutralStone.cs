using TimeCrusadeMod.Content.Biomes;
using TimeCrusadeMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;
using System.Collections.Generic;

namespace TimeCrusadeMod.Content.Tiles
{
	public class NeutralStone : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileBlendAll[Type] = true;
			Main.tilePile[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<NeutralityCrystal>()] = true;
			Main.tileMerge[Type][ModContent.TileType<NeutralChisel>()] = true;

			DustType = ModContent.DustType<NeutralDust>();
			VanillaFallbackOnModDeletion = TileID.DiamondGemspark;

			AddMapEntry(new Color(150, 150, 150));
			HitSound = SoundID.Tink;
				MineResist = 1f;
				MinPick = 45;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override void ChangeWaterfallStyle(ref int style) {
			style = ModContent.GetInstance<NeutralWaterfallStyle>().Slot;
		}
	}
}
