
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Tiles.Banners;

namespace TimeCrusadeMod.Content.Items.Placeables.Banners
{
    public class CorruptRollerBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<EnemyBanner4>(), (int)EnemyBanner4.StyleID.CorruptRoller);
            Item.width = 12;
            Item.height = 28;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
    }
}