using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Placeables;

namespace TimeCrusadeMod.Content.Items.Placeables.Furniture
{
	public class NeutralTable : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.NeutralTable>());
			Item.width = 38;
			Item.height = 24;
			Item.value = 150;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.WoodenTable)
                .AddIngredient(ModContent.ItemType<Placeables.NeutralityFragments>(), 3)
                .AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}