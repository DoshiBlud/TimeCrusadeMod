using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Dusts;

namespace TimeCrusadeMod.Content.Items.Weapons.Ranged
{
	public class PeddleShooter: ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crystal Gun");
			// Tooltip.SetDefault("A gun made from crystal, it shoots crystal bullets.");
		}

		public override void SetDefaults()
		{
			Item.damage = 4;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 80;
			Item.height = 28;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(gold: 1,silver: 50);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.FlowerSeedBullet>();
			Item.shootSpeed = 3f;
			Item.useAmmo = ModContent.ItemType<Ammo.FlowerSeedBullet>(); // Use the custom bullet as ammo
		}
public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    // Shoot projectile 1 (e.g., a custom mod projectile)
    // Shoot projectile 2 (e.g., a vanilla projectile)
    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<FlowerSeedBullet>(), damage, knockback, player.whoAmI);

    return false; // Prevents the default projectile from shooting
}
public override void MeleeEffects(Player player, Rectangle hitbox) {
    if (Main.rand.NextBool(5)) {
        // Creates dust at the player's weapon location
        Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<FlowerDust>());
    }
}

		public override void AddRecipes() {
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ItemID.Sunflower, 3);
        recipe.AddIngredient(ItemID.Revolver, 1);
		recipe.AddTile(TileID.WorkBenches);
		recipe.Register();
		}
	}
}