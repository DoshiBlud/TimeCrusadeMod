using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Projectiles;

namespace TimeCrusadeMod.Content.Items.Weapons.Melee
{
    public class StonedSlicer : ModItem
    {
       public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Saliquent Blade");
			// Tooltip.SetDefault("The best blade you will ever hear lol.");
		}

		public override void SetDefaults()
		{
			Item.damage = 24;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 44;
			Item.useAnimation = 23;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = false;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(gold: 2,silver: 20);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<StonePebble>();
			Item.shootSpeed = 14f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
	float rotation = MathHelper.ToRadians(14);
    // Shoot projectile 1 (e.g., a custom mod projectile)
    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<StonePebble>(), damage, knockback, player.whoAmI);

	Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<LargeStonePebble>(), damage, knockback, player.whoAmI);

    return false; // Prevents the default projectile from shooting
}
    }
}