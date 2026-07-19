using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Rarities;

namespace TimeCrusadeMod.Content.Items.Weapons.Melee
{
    public class Lunarang : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Saliquent Blade");
            // Tooltip.SetDefault("The best blade you will ever hear lol.");
        }

        public override void SetDefaults()
        {
            Item.damage = 120;
            Item.DamageType = DamageClass.Melee;
            Item.width = 20;
            Item.height = 52;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(gold: 3, silver: 20);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Lunarang>();
            Item.shootSpeed = 15;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarOre, 20);
            recipe.AddIngredient(ItemID.Trimarang);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}