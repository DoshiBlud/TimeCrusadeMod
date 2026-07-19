using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Projectiles;

namespace TimeCrusadeMod.Content.Items.Weapons.Melee
{
    public class JungleSaber : ModItem
    {
       public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Saliquent Blade");
			// Tooltip.SetDefault("The best blade you will ever hear lol.");
		}

		public override void SetDefaults()
		{
			Item.damage = 17;
			Item.DamageType = DamageClass.Melee;
			Item.width = 65;
			Item.height = 65;
			Item.useTime = 120;
			Item.useAnimation = 19;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = false;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(gold: 2,silver: 20);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<JungleProjectile>();
			Item.shootSpeed = 15f;
		}

public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
    float numberProjectiles = 3; // Number of projectiles per shot
    float rotation = MathHelper.ToRadians(360); // Total spread angle in degrees

    for (int i = 0; i < numberProjectiles; i++) {
        // Calculate the angle for each individual projectile
        Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
        
		Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
		Projectile.NewProjectile(source, position, velocity, ProjectileID.Leaf, damage, knockback, player. whoAmI);
	}
return false; // Return false so vanilla doesn't shoot its own projectile
}
public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Wood, 25);
            recipe.AddIngredient(ItemID.JungleSpores, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}