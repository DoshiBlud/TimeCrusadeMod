using TimeCrusadeMod.Content.Tiles.Banners;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Items.Placeables.Banners
{
    public class BloodyPiercerBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<EnemyBanner3>(), (int)EnemyBanner3.StyleID.BloodyPiercer);
            Item.width = 12;
            Item.height = 28;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
    }
}