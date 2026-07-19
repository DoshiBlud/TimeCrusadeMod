using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Dusts;

namespace TimeCrusadeMod.Content.Items.Weapons.Magic
{
    public class NeutralityShifter : ModItem
    {
       public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Saliquent Blade");
			// Tooltip.SetDefault("The best blade you will ever hear lol.");
		}

		public override void SetDefaults()
		{
			Item.damage = 104;
			Item.DamageType = DamageClass.Magic;
			Item.width = 80;
			Item.height = 80;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.knockBack = 56;
			Item.value = Item.sellPrice(gold: 3,silver: 20);
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item72;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<NeutralityMeteor>();
			Item.shootSpeed = 12f;
			Item.mana = 25;
		}
public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
    float numberProjectiles = 10; // Number of projectiles per shot
    float rotation = MathHelper.ToRadians(360); // Total spread angle in degrees

    for (int i = 0; i < numberProjectiles; i++) {
        // Calculate the angle for each individual projectile
        Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
        
		Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
		Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<NeutralityMeteor>(), damage, knockback, player. whoAmI);
	}
	            	for (int loops = 0; loops < 1; loops++) {
				for (int i = 0; i < 2; i++) {
					Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
					Dust d = Dust.NewDustPerfect(position, ModContent.DustType<NeutralDust>(), speed * 2 * (loops + 1), Scale: 0.7f);
					d.noGravity = false;
				}
			}
return false; // Return false so vanilla doesn't shoot its own projectile
}
public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<NeutralityBar>(), 5);
			recipe.AddIngredient(ModContent.ItemType<NeutralityFragments>(), 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult) {
			// We can use ModifyManaCost to dynamically adjust the mana cost of this item, similar to how Space Gun works with the Meteor armor set.
			// See ExampleHood to see how accessories give the reduce mana cost effect.
			if (player.statLife < player.statLifeMax2 / 2) {
				mult *= 0.5f; // Half the mana cost when at low health. Make sure to use multiplication with the mult parameter.
			}
		}
	}
}