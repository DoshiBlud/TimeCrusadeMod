using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Rarities;

namespace TimeCrusadeMod.Content.Items.Tools
{
    public class IllusionaryEateroftheUniverse : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 80;
            Item.value = Item.sellPrice(gold: 2);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 3;
            Item.useAnimation = 20;
            Item.autoReuse = true;
            Item.rare = ModContent.RarityType<IllusionaryRarity>();
            Item.pick = 500;
            Item.damage = 80;
            Item.knockBack = 5;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StarDevourer>());
            recipe.AddIngredient(ModContent.ItemType<IllusionaryDescendentBar>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}