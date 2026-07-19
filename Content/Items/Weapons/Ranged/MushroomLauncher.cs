using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Tiles.Furniture;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Projectiles.Rockets;

namespace TimeCrusadeMod.Content.Items.Weapons.Ranged
{
	public class MushroomLauncher : ModItem
	{
		public override void SetDefaults() {
			Item.damage = 56;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 62;
			Item.height = 22;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 13;
			Item.value = Item.sellPrice(gold: 7,silver: 20);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MushroomRocket>();
			Item.shootSpeed =4f;
		}
		public void AI()
		{
			for (int i = 0; i < 2; i++) {
	Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
	Dust d = Dust.NewDustPerfect(Main.LocalPlayer.Top, ModContent.DustType<MushroomDust>(), speed * 5, Scale: 2f);
	d.noGravity = true;
	Dust dg = Dust.NewDustPerfect(Main.LocalPlayer.Top + speed * 32, DustID.GlowingMushroom, speed * 2, Scale: 2f);
}
		}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			const int NumProjectiles = 1; // The number of projectiles that this gun will shoot.

			for (int i = 0; i < NumProjectiles; i++) {
				// Rotate the velocity randomly by 30 degrees at max.
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(2));

				// Decrease velocity randomly for nicer visuals.
				newVelocity *= 1f - Main.rand.NextFloat(0.5f);

				// Create a projectile.
				Projectile.NewProjectileDirect(source, position + new Vector2(-4, -4), newVelocity, ModContent.ProjectileType<MushroomRocket>(), damage, knockback, player.whoAmI);

				Projectile.NewProjectileDirect(source, position + new Vector2(-4, -4), velocity, ModContent.ProjectileType<GlowingMushroomRocket>(), damage, knockback, player.whoAmI);

			}

			return false; // Return false because we don't want tModLoader to shoot projectile
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8f, 2f); // Moves the position of the weapon in the player's hand.
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Every projectile shot from this gun has a 1/3 chance of being an ExampleInstancedProjectile
            if (Main.rand.NextBool(3))
            {
                type = ModContent.ProjectileType<MushroomRocket>();
                type = ModContent.ProjectileType<GlowingMushroomRocket>();
            }
        }


        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GlowingMushroom, 25);
            recipe.AddIngredient(ItemID.Mushroom, 25);
            recipe.AddIngredient(ItemID.RocketLauncher);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}