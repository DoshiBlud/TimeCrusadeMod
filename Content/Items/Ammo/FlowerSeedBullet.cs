using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Items.Placeables;

namespace TimeCrusadeMod.Content.Items.Ammo
{
    public class FlowerSeedBullet : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Projectile");
        }
        public override void SetDefaults()
        {
            Item.damage = 4; // The damage for projectiles isn't actually 12, it actually is the damage combined with the projectile and the item together.
			Item.DamageType = DamageClass.Ranged;
			Item.width = 13;
			Item.height = 13;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true; // This marks the item as consumable, making it automatically be consumed when it's used as ammunition, or something else, if possible.
			Item.knockBack = 0.2f;
			Item.value = 3;
            Item.shoot = ModContent.ProjectileType<Projectiles.FlowerSeedBullet>();
            Item.shootSpeed = 20f; // The speed of the projectile. This value equivalent to Silver Bullet since ExampleBullet's Projectile.extraUpdates is 1.
			Item.rare = ItemRarityID.White; // The projectile that weapons fire when using this item as ammunition. // The speed of the projectile. This value equivalent to Silver Bullet since ExampleBullet's Projectile.extraUpdates is 1.
			Item.ammo = Item.type; // The ammo class this ammo belongs to.
        }

        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddIngredient(ItemID.Seed, 1)
                .AddIngredient(ItemID.EmptyBullet, 15)
                .AddTile(TileID.Tables)
                .Register();
        }
    }
}