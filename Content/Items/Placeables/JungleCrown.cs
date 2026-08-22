using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Rarities;

namespace TimeCrusadeMod.Content.Items.Placeables
{
    internal class JungleCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = 200;
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.JungleSpores, 3)
                .AddIngredient(ItemID.RichMahogany, 16)
                .AddIngredient(ItemID.Moonglow, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}