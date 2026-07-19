using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Projectiles.Rockets;

namespace TimeCrusadeMod.Content.Items.Weapons.Melee
{
    public class MushroomMalice : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Saliquent Blade");
            // Tooltip.SetDefault("The best blade you will ever hear lol.");
        }

        public override void SetDefaults()
        {
            Item.damage = 55;
            Item.DamageType = DamageClass.Melee;
            Item.width = 56;
            Item.height = 56;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = false;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(gold: 3, silver: 20);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<TinyMushroom>();
            Item.shootSpeed = 10f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const int NumProjectiles = 5; // The number of projectiles that this gun will shoot.

            for (int i = 0; i < NumProjectiles; i++)
            {
                // Rotate the velocity randomly by 30 degrees at max.
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(0.5f);
                float speedX = -velocity.X * Main.rand.NextFloat(.4f, .9f) + Main.rand.NextFloat(-4f, 4f);
                float speedY = -velocity.Y * Main.rand.Next(40, 90) * 0.01f + Main.rand.Next(-10, 9) * 0.4f; // This is Vanilla code, a little harder to comprehend. This is just here to teach you that you can convert vanilla code to more readable code sometimes.

                // Create a projectile.
                Projectile.NewProjectileDirect(source, position + new Vector2(-4, -4), newVelocity, ModContent.ProjectileType<TinyMushroom>(), damage, knockback, player.whoAmI);

                Projectile.NewProjectileDirect(source, position + new Vector2(-4, -4), velocity, ModContent.ProjectileType<TinyGlowingMushroom>(), damage, knockback, player.whoAmI);

                Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                float ceilingLimit = target.Y;
                if (ceilingLimit > player.Center.Y - 200f)
                {
                    ceilingLimit = player.Center.Y - 200f;
                }
                // Loop these functions 3 times.
                for (int j = 0; j < 3; j++)
                {
                    position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
                    position.Y -= 200 * j;
                    Vector2 heading = target - position;

                    if (heading.Y < 0f)
                    {
                        heading.Y *= -10f;
                    }

                    if (heading.Y < 40f)
                    {
                        heading.Y = 40f;
                    }

                    heading.Normalize();
                    heading *= velocity.Length();
                    heading.Y += Main.rand.Next(-40, 41) * 0.02f;
                    Projectile.NewProjectileDirect(source, position, heading, ModContent.ProjectileType<TinyGlowingMushroom>(), damage * 2, knockback, player.whoAmI, 0f, ceilingLimit);
                }

            }

            return false; // Return false because we don't want tModLoader to shoot projectile
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8f, 2f); // Moves the position of the weapon in the player's hand.
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GlowingMushroom, 25);
            recipe.AddIngredient(ItemID.Mushroom, 25);
            recipe.AddIngredient(ItemID.IronBroadsword);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}