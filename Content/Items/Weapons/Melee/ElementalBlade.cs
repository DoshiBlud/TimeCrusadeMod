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
    public class ElementalBlade : ModItem
    {
       public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Saliquent Blade");
			// Tooltip.SetDefault("The best blade you will ever hear lol.");
		}

		public override void SetDefaults()
		{
			Item.damage = 35;
			Item.DamageType = DamageClass.Melee;
			Item.width = 83;
			Item.height = 83;
			Item.useTime = 21;
			Item.useAnimation = 21;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = false;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(gold: 3,silver: 20);
			Item.rare = ModContent.RarityType<ElementalRarity>();
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CypheringSlices>();
			Item.shootSpeed = 20f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
{
    // Shoot projectile 1 (e.g., a custom mod projectile)
    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<CypheringSlices>(), damage, knockback, player.whoAmI);

    return false; // Prevents the default projectile from shooting
}
public override void MeleeEffects(Player player, Rectangle hitbox) {
    if (Main.rand.NextBool(7)) {
        // Creates dust at the player's weapon location
        Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<CypherDust>());
    }
}
public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ElementalBar>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}