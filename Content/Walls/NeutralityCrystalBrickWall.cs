using TimeCrusadeMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Walls
{
	public class NeutralityCrystalBrickWall : ModWall
	{
		public override void SetStaticDefaults() {
			Main.wallHouse[Type] = true;

			DustType = ModContent.DustType<NeutralDust>();
			VanillaFallbackOnModDeletion = WallID.DiamondGemspark;

			AddMapEntry(new Color(110, 110, 110));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
}