using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Tiles.Banners;

namespace TimeCrusadeMod.Content.Items.Placeables.Banners
{
    public class DungeonSpikedSlimeBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<EnemyBanner5>(), (int)EnemyBanner5.StyleID.DungeonSpikedSlime);
            Item.width = 12;
            Item.height = 28;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
    }
}