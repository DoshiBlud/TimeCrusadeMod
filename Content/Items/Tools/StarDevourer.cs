using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Items.Placeables;

namespace TimeCrusadeMod.Content.Items.Tools
{
    public class StarDevourer : ModItem
    {
        public  override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.sellPrice(gold: 2);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 5;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Red;
            Item.pick = 310;
            Item.damage = 65;
            Item.knockBack = 4;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = false;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SolarFlarePickaxe, 1);
            recipe.AddIngredient(ItemID.VortexPickaxe, 1);
            recipe.AddIngredient(ItemID.StardustPickaxe, 1);
            recipe.AddIngredient(ItemID.NebulaPickaxe, 1);
            recipe.AddIngredient(ItemID.LunarBar, 35);
            recipe.AddIngredient(ModContent.ItemType<ElementalBar>(), 5);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
    }
}
}