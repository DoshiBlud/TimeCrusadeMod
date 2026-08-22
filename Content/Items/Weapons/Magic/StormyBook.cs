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

namespace TimeCrusadeMod.Content.Items.Weapons.Magic
{
    public class StormyBook : ModItem
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
            Item.damage = 21;
            Item.DamageType = DamageClass.Magic;
            Item.width = 32;
            Item.height = 34;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item10;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WaterBolt;
            Item.shootSpeed = 8f;
            Item.mana = 15;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            switch (Act)
            {
                case 1:
                    {
                        float numberProjectiles = 4; // Number of projectiles per shot
                        float rotation = MathHelper.ToRadians(360); // Total spread angle in degrees

                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            // Calculate the angle for each individual projectile
                            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));

                            Projectile.NewProjectile(source, position, perturbedSpeed, ProjectileID.WaterBolt, damage, knockback, player.whoAmI);
                        }
                        for (int r = 0; r < 3; r++)
                        {
                            // Calculate the angle for each individual projectile
                            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, r / (numberProjectiles - 1)));

                            Projectile.NewProjectile(source, position, perturbedSpeed, ProjectileID.WaterBolt, damage, knockback, player.whoAmI);
                        }
                        SoundEngine.PlaySound(SoundID.Item110 with { Pitch = 3f, PitchVariance = 0.1f });
                        if (++timer < 2)
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
            if (Main.rand.NextBool(3))
            {
                // Creates dust at the player's weapon location
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Water);
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.WaterBolt);
            recipe.AddIngredient(ItemID.Cloud, 8);
            recipe.AddIngredient(ItemID.TsunamiInABottle);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}