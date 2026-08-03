using Microsoft.Xna.Framework;
using System.Threading;
using Terraria;
using Terraria.Audio;
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
    public class SeaficWaterlizer : ModItem
    {
        public int timer = 0;
        public int Act = 1;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Gun");
            // Tooltip.SetDefault("A gun made from crystal, it shoots crystal bullets.");
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 68;
            Item.height = 32;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = Item.sellPrice(platinum: 2);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item10;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<WaterSpike>();
            Item.shootSpeed = 14f;
            Item.useAmmo = ItemID.BottledWater;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            switch(Act)
            {
                case 1:
                    {
                        float numberProjectiles = 2; // Number of projectiles per shot
                        float rotation = MathHelper.ToRadians(10); // Total spread angle in degrees

                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            // Calculate the angle for each individual projectile
                            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));

                            Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<WaterSpike>(), damage, knockback, player.whoAmI);
                        }
                        SoundEngine.PlaySound(SoundID.Item176 with { Pitch = 2f, PitchVariance = 0.1f });
                        if (++timer < 2)
                        {
                            break;
                        }
                        Act = 2;
                        timer = 0;
                    }
                    break;
                case 2:
                    {
                        float numberProjectiless = 2; // Number of projectiles per shot
                        float rotations = MathHelper.ToRadians(5); // Total spread angle in degrees

                        for (int i = 0; i < numberProjectiless; i++)
                        {
                            // Calculate the angle for each individual projectile
                            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotations, rotations, i / (numberProjectiless - 1)));

                            Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<WaterSpike>(), damage, knockback, player.whoAmI);
                        }
                        SoundEngine.PlaySound(SoundID.Item176 with { Pitch = 2f, PitchVariance = 0.1f });
                        if (++timer < 2)
                        {
                            break;
                        }
                        Act = 3;
                        timer = 0;
                    }
                    break;
                case 3:
                    {
                        float numberProjectilesss = 2; // Number of projectiles per shot
                        float rotationss = MathHelper.ToRadians(2); // Total spread angle in degrees

                        for (int i = 0; i < numberProjectilesss; i++)
                        {
                            // Calculate the angle for each individual projectile
                            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotationss, rotationss, i / (numberProjectilesss - 1)));

                            Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<WaterSpike>(), damage, knockback, player.whoAmI);
                        }
                        SoundEngine.PlaySound(SoundID.Item176 with { Pitch = 2f, PitchVariance = 0.1f });
                        if (++timer < 2)
                        {
                            break;
                        }
                        Act = 4;
                        timer = 0;
                    }
                    break;
                case 4:
                    {
                        float numberProjectilessss = 2; // Number of projectiles per shot
                        float rotationsss = MathHelper.ToRadians(2); // Total spread angle in degrees
                        for (int i = 1; i < numberProjectilessss; i++)
                        {
                            // Calculate the angle for each individual projectile
                            Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.Lerp(-rotationsss, rotationsss, i / (numberProjectilessss - 1)));

                            Projectile.NewProjectile(source, position, perturbedSpeed, ProjectileID.WaterBolt, damage, knockback, player.whoAmI);
                        }
                        SoundEngine.PlaySound(SoundID.Item176 with { Pitch = 3f, PitchVariance = 0.1f });
                        if (++timer < 5)
                        {
                            break;
                        }
                        Act = 1;
                        timer = 0;
                    }
                    break;
            }
            return false; // Return false so vanilla doesn't shoot its own projectile
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8f, 4f); // Moves the position of the weapon in the player's hand.
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
            recipe.AddIngredient(ItemID.LunarOre, 14);
            recipe.AddIngredient(ItemID.WaterGun);
            recipe.AddIngredient(ItemID.Megashark);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}