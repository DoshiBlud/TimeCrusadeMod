using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Rarities;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Tiles.Furniture;

namespace TimeCrusadeMod.Content.Items.Weapons.Magic
{
    public class AxyNemesis : ModItem
    {
        public override void SetDefaults()
        {
            // DefaultToStaff handles setting various Item values that magic staff weapons use.
            // Hover over DefaultToStaff in Visual Studio to read the documentation!
            // Shoot a black bolt, also known as the projectile shot from the onyx blaster.
            Item.DefaultToStaff(ModContent.ProjectileType<AxyMagic>(), 26, 40, 33);
            Item.width = 38;
            Item.height = 38;
            Item.damage = 278;
            Item.mana = 100;
            Item.UseSound = SoundID.Item70;
            Item.rare = ModContent.RarityType<AxitrentonRarity>();
            Item.knockBack = 10;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 2; // Number of projectiles per shot
            float numberProjectile = 1; // Number of projectiles per shot
            float rotation = MathHelper.ToRadians(5); // Total spread angle in degrees
            float rotationed = MathHelper.ToRadians(0); // Total spread angle in degrees

            for (int i = 0; i < numberProjectiles; i++)
            {
                // Calculate the angle for each individual projectile
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));

                Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<AxyMagic>(), damage, knockback, player.whoAmI);
                Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                float ceilingLimit = target.Y;
                if (ceilingLimit > player.Center.Y - 200f)
                {
                    ceilingLimit = player.Center.Y - 200f;
                }
                // Loop these functions 3 times.
                for (int j = 0; i < 1; i++)
                {
                    position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
                    position.Y -= 50 * i;
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
                    Projectile.NewProjectileDirect(source, position, heading, ModContent.ProjectileType<AxyMagic>(), damage * 2, knockback, player.whoAmI, 0f, ceilingLimit);
                }
            }
            for (int i = 0; i < numberProjectile; i++)
            {
                // Calculate the angle for each individual projectile
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotationed, rotationed, i / (numberProjectiles - 1)));
                Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                float ceilingLimit = target.Y;
                if (ceilingLimit > player.Center.Y - 200f)
                {
                    ceilingLimit = player.Center.Y - 200f;
                }
                // Loop these functions 3 times.
                for (int j = 0; i < 4; i++)
                {
                    position = player.Center - new Vector2(Main.rand.NextFloat(401) * player.direction, 600f);
                    position.Y -= 100 * i;
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
                    Projectile.NewProjectileDirect(source, position, heading, ModContent.ProjectileType<AxyCloud>(), damage * 1, knockback, player.whoAmI, 0f, ceilingLimit);
                }
            }
            return false; // Return false so vanilla doesn't shoot its own projectile
        }

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            // We can use ModifyManaCost to dynamically adjust the mana cost of this item, similar to how Space Gun works with the Meteor armor set.
            // See ExampleHood to see how accessories give the reduce mana cost effect.
            if (player.statLife < player.statLifeMax2 / 2)
            {
                mult *= 0.5f; // Half the mana cost when at low health. Make sure to use multiplication with the mult parameter.
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(15))
            {
                // Creates dust at the player's weapon location
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<AxitrentonDust>());
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AxitrentonCrystal>(), 10);
            recipe.AddIngredient(ItemID.LunarFlareBook);
            recipe.AddIngredient(ItemID.ClayBlock, 20);
            recipe.AddTile(ModContent.TileType<AxitrentonObserver>());
            recipe.Register();
        }
    }
}