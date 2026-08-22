using TimeCrusadeMod.Content.Tiles.Banners;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Items.Placeables.Banners
{
	public class WormofNeutralityBanner : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<EnemyBanner1>(), (int)EnemyBanner1.StyleID.WormofNeutralityHead);
			Item.width = 16;
			Item.height = 48;
			Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
		}
	}
}