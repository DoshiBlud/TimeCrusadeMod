using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Projectiles;

namespace TimeCrusadeMod.Content.Items.Weapons.Melee
{
    public class WoodenKnife : ModItem
    {
       public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Saliquent Blade");
			// Tooltip.SetDefault("The best blade you will ever hear lol.");
		}

		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = DamageClass.Melee;
			Item.width = 35;
			Item.height = 35;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = false;
			Item.knockBack = 1;
			Item.value = Item.sellPrice(silver: 25);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<WoodenKnifeProjectile>();
			Item.shootSpeed = 14f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{

        // Calculate the angle for each individual projectil
    // Shoot projectile 1 (e.g., a custom mod projectile)
    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<WoodenKnifeProjectile>(), damage, knockback, player.whoAmI);

    return false; // Prevents the default projectile from shooting
}
        public override void AddRecipes() {
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ItemID.Wood, 14);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
		}
    }
}