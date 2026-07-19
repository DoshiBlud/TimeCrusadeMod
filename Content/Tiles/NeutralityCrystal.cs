using TimeCrusadeMod.Content.Biomes;
using TimeCrusadeMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using TimeCrusadeMod.Content.Tiles;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;
using System.Collections.Generic;

namespace TimeCrusadeMod.Content.Tiles
{
	public class NeutralityCrystal : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<NeutralChisel>()] = true;
			Main.tileMerge[Type][ModContent.TileType<NeutralStone>()] = true;
            Main.tilePile[Type] = true;
			

			DustType = ModContent.DustType<NeutralDust>();
			VanillaFallbackOnModDeletion = TileID.DiamondGemspark;

			AddMapEntry(new Color(175, 175, 175));
			HitSound = SoundID.Tink;
			MineResist = 2f;
			MinPick = 55;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override void ChangeWaterfallStyle(ref int style) {
			style = ModContent.GetInstance<NeutralWaterfallStyle>().Slot;
		}
	}
}
