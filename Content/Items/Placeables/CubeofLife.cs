using System.Drawing;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Items.Placeables
{
    internal class CubeofLife : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 25;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 5);
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.White;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LifeCrystal, 3)
                .AddIngredient(ItemID.Bunny, 5)
                .AddIngredient(ItemID.Glass, 45)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
}
}

