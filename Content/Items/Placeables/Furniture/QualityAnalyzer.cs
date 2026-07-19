using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Placeables;

namespace TimeCrusadeMod.Content.Items.Placeables.Furniture
{
	public class QualityAnalyzer : ModItem
	{
		public override void SetDefaults() {
			// ModContent.TileType<Tiles.Furniture.QualityAnalyzer>() retrieves the id of the tile that this item should place when used.
			// DefaultToPlaceableTile handles setting various Item values that placeable items use
			// Hover over DefaultToPlaceableTile in Visual Studio to read the documentation!
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.QualityAnalyzer>());
			Item.width = 48; // The item texture's width
			Item.height = 36; // The item texture's height
			Item.value = 150;
		}

		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) {
			itemGroup = ContentSamples.CreativeHelper.ItemGroup.CraftingObjects;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.LunarCraftingStation)
				.AddIngredient(ModContent.ItemType<ZilvicOre>(), 10)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}