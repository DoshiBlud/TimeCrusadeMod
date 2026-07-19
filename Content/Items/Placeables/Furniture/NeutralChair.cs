using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Placeables;

namespace TimeCrusadeMod.Content.Items.Placeables.Furniture
{
	public class NeutralChair : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.NeutralChair>());
			Item.width = 12;
			Item.height = 30;
			Item.value = 150;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<Placeables.NeutralityFragments>(), 3)
                .AddIngredient(ItemID.WoodenChair, 1)
                .AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}