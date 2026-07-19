using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Dusts;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace TimeCrusadeMod.Content.Items.Weapons.Melee
{
    public class StarlightIntersection : ModItem
    {
		private static Asset<Texture2D> glowTexture;

		public override void Load() {
			glowTexture = ModContent.Request<Texture2D>(Texture + "_Glowmask");
		}
       public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Saliquent Blade");
			// Tooltip.SetDefault("The best blade you will ever hear lol.");
		}

		public override void SetDefaults()
		{
			Item.damage = 106;
			Item.DamageType = DamageClass.Melee;
			Item.width = 80;
			Item.height = 80;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = false;
			Item.knockBack = 59;
			Item.value = Item.sellPrice(gold: 3,silver: 20);
			Item.rare = ItemRarityID.Red;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<NeutralityProjectile1>();
			Item.shootSpeed = 25f;
		}
public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
    float numberProjectiles = 1; // Number of projectiles per shot
    float rotation = MathHelper.ToRadians(3); // Total spread angle in degrees

    for (int i = 0; i < numberProjectiles; i++) {
        // Calculate the angle for each individual projectile
        Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
        
		Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
		Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<NeutralityProjectile>(), damage, knockback, player. whoAmI);
		Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<NeutralityProjectile1>(), damage, knockback, player. whoAmI);
	}
return false; // Return false so vanilla doesn't shoot its own projectile
}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
			// Draw the glow texture when in the game world.
			Texture2D texture = glowTexture.Value;
			spriteBatch.Draw
			(
				texture,
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}

public override void MeleeEffects(Player player, Rectangle hitbox) {
    if (Main.rand.NextBool(3)) {
        // Creates dust at the player's weapon location
        Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<NeutralDust>());
    }
}
public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<NeutralityBar>(), 5);
			recipe.AddIngredient(ModContent.ItemType<NeutralityFragments>(), 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
    }
}