using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Items.Placeables.Furniture;
using TimeCrusadeMod.Content.Rarities;

namespace TimeCrusadeMod.Content.Items.Placeables.Furniture
{
    public class AxitrentonObserver : ModItem
    {
        public override void SetDefaults()
        {
            // ModContent.TileType<Tiles.Furniture.QualityAnalyzer>() retrieves the id of the tile that this item should place when used.
            // DefaultToPlaceableTile handles setting various Item values that placeable items use
            // Hover over DefaultToPlaceableTile in Visual Studio to read the documentation!
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.AxitrentonObserver>());
            Item.width = 48; // The item texture's width
            Item.height = 34; // The item texture's height
            Item.value = 170;
            Item.rare = ModContent.RarityType<AxitrentonRarity>();
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.CraftingObjects;
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AxitrentonCrystal>(), 10)
                .AddIngredient(ModContent.ItemType<QualityAnalyzer>())
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}