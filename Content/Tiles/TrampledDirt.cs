using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Tiles
{
	public class TrampledDirt : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tilePile[Type] = true;

			DustType = DustID.Dirt;
			VanillaFallbackOnModDeletion = TileID.DiamondGemspark;

			AddMapEntry(new Color(101, 67, 33));
			HitSound = SoundID.Dig;
				MineResist = 1f;
				MinPick = 10;
		}
	}
}
