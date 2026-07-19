using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Projectiles;

namespace TimeCrusadeMod.Content.Items.Weapons.Magic
{
    public class RockyStaff : ModItem
    {
       public override void SetStaticDefaults()
		{
			Item.staff[Type] = true;
			// DisplayName.SetDefault("Saliquent Blade");
			// Tooltip.SetDefault("The best blade you will ever hear lol.");
		}

		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Magic;
			Item.width = 60;
			Item.height = 60;
			Item.useTime = 23;
			Item.useAnimation = 23;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(silver: 44);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<StonePebble>();
			Item.shootSpeed = 18f;
			Item.mana = 5;
		}
public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
    float numberProjectiles = 3; // Number of projectiles per shot
    float rotation = MathHelper.ToRadians(15); // Total spread angle in degrees

    for (int i = 0; i < numberProjectiles; i++) {
        // Calculate the angle for each individual projectile
        Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
        
        // Spawn the projectile
        Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<StonePebble>(), damage, knockback, player.whoAmI);
    }
    
    return false; // Return false so vanilla doesn't shoot its own projectile
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