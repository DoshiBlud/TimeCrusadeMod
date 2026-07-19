using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Projectiles;

namespace TimeCrusadeMod.Content.Items.Weapons.Ranged
{
	public class SupremeStarShooter : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crystal Gun");
			// Tooltip.SetDefault("A gun made from crystal, it shoots crystal bullets.");
		}

		public override void SetDefaults()
		{
			Item.damage = 102;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 85;
			Item.height = 17;
			Item.useTime = 14;
			Item.useAnimation = 14;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 500;
			Item.value = Item.sellPrice(gold: 2,silver: 50);
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.NeutralityBullet>();
			Item.shootSpeed = 20f;
			Item.useAmmo = ModContent.ItemType<Ammo.NeutralityBullet>(); // Use the custom bullet as ammo
		}
public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    // Shoot projectile 1 (e.g., a custom mod projectile)
    Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<NeutralityProjectile1>(), damage, knockback, player.whoAmI);
    
    // Shoot projectile 2 (e.g., a vanilla projectile)
    Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<NeutralityBullet>(), damage, knockback, player.whoAmI);

    return false; // Prevents the default projectile from shooting
}

		public override void AddRecipes() {
		Recipe recipe = CreateRecipe();
		recipe.AddIngredient(ModContent.ItemType<NeutralityBar>(), 5);
		recipe.AddIngredient(ModContent.ItemType<NeutralityFragments>(), 3);
		recipe.AddTile(TileID.LunarCraftingStation);
		recipe.Register();
		}
	}
}