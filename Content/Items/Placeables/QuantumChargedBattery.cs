using Microsoft.Xna.Framework;
using System.Drawing;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Items.Placeables
{
    internal class QuantumChargedBattery : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 30;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.White;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BatteryCase>());
            recipe.AddIngredient(ModContent.ItemType<QuantumEnergy>(), 100);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

