using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Tiles.Banners;

namespace TimeCrusadeMod.Content.Items.Placeables.Banners
{
    public class ConsumerofBiomesBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<EnemyBanner6>(), (int)EnemyBanner6.StyleID.ConsumerofBiomes);
            Item.width = 12;
            Item.height = 28;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
    }
}