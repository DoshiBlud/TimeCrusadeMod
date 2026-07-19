using TimeCrusadeMod.Content.Items.Placeables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Items.Placeables
{
	public class NeutralityCrystalBrickWall : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 400;
		}

		public override void SetDefaults() {
			// ModContent.WallType<Walls.NeutralityCrystalBrickWall>() retrieves the id of the wall that this item should place when used.
			// DefaultToPlaceableWall handles setting various Item values that placeable wall items use.
			// Hover over DefaultToPlaceableWall in Visual Studio to read the documentation!
			Item.DefaultToPlaceableWall(ModContent.WallType<Walls.NeutralityCrystalBrickWall>());
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe(4)
            .AddIngredient(ModContent.ItemType<NeutralityCrystalBrick>(), 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}