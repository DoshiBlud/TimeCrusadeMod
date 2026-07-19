using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Items.Placeables;

namespace TimeCrusadeMod.Content.Items.Tools
{
    public class SciFiPickaxe : ModItem
    {
        public  override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.value = Item.sellPrice(silver: 68);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Blue;
            Item.pick = 200;
            Item.damage = 40;
            Item.knockBack = 3;
            Item.UseSound = SoundID.Item1;
            Item.noMelee = false;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<SciFiBar>(25);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
    }
}
}