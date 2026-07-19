using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Rarities;
using TimeCrusadeMod.Content.Tiles.Furniture;

namespace TimeCrusadeMod.Content.Items.Weapons.Ranged
{
    public class AxyArkShooter : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Gun");
            // Tooltip.SetDefault("A gun made from crystal, it shoots crystal bullets.");
        }

        public override void SetDefaults()
        {
            Item.damage = 284;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 46;
            Item.height = 80;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 14;
            Item.value = Item.sellPrice(platinum: 500);
            Item.rare = ModContent.RarityType<AxitrentonRarity>();
            Item.UseSound = SoundID.Item10;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<AxyArrow>();
            Item.shootSpeed = 18f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 2; // Number of projectiles per shot
            float rotation = MathHelper.ToRadians(10); // Total spread angle in degrees
            float rotations = MathHelper.ToRadians(5); // Total spread angle in degrees
            float extraProjectiles = 3;

            for (int i = 0; i < numberProjectiles; i++)
            {
                // Calculate the angle for each individual projectile
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));

                Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<AxyArrow>(), damage, knockback, player.whoAmI);
            }
            for (int i = 0; i < extraProjectiles; i++)
            {
                // Calculate the angle for each individual projectile
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotations, rotations, i) / (numberProjectiles + 1));

                Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<AxyKnife>(), damage, knockback, player.whoAmI);
            }
            return false; // Return false so vanilla doesn't shoot its own projectile
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8f, 2f); // Moves the position of the weapon in the player's hand.
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                // Creates dust at the player's weapon location
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<AxitrentonDust>());
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AxitrentonCrystal>(), 10);
            recipe.AddIngredient(ItemID.Phantasm);
            recipe.AddIngredient(ItemID.ClayBlock, 20);
            recipe.AddTile(ModContent.TileType<AxitrentonObserver>());
            recipe.Register();
        }
    }
}