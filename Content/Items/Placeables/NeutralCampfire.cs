using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Placeables;

namespace TimeCrusadeMod.Content.Items.Placeables
{
	public class NeutralCampfire : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.NeutralCampfire>(), 0);
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddRecipeGroup(RecipeGroupID.Wood, 10)
				.AddIngredient(ModContent.ItemType<NeutralTorch>(), 1)
				.Register();
		}
	}
}