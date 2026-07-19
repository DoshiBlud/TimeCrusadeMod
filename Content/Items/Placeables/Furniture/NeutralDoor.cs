using TimeCrusadeMod.Content.Tiles.Furniture;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TimeCrusadeMod.Content.Items.Placeables;

namespace TimeCrusadeMod.Content.Items.Placeables.Furniture
{
	public class NeutralDoor : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<NeutralDoorClosed>());
			Item.width = 14;
			Item.height = 28;
			Item.value = 150;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ModContent.ItemType<NeutralityFragments>(), 4)
                .AddIngredient(ItemID.WoodenDoor, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}