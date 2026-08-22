using TimeCrusadeMod.Content.Tiles.Banners;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Items.Placeables.Banners
{
    public class NeutralSlimeBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<EnemyBanner2>(), (int)EnemyBanner2.StyleID.NeutralSlime);
            Item.width = 12;
            Item.height = 28;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
    }
}