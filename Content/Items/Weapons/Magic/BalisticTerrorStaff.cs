using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Rarities;

namespace TimeCrusadeMod.Content.Items.Weapons.Magic
{
    public class BalisticTerrorStaff : ModItem
    {
        public int timer = 0;
        public int Act = 1;
        public override void SetDefaults()
        {
            Item.damage = 160;
            Item.DamageType = DamageClass.Magic;
            Item.width = 70;
            Item.height = 70;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 27;
            Item.value = Item.sellPrice(gold: 7, silver: 20);
            Item.rare = ModContent.RarityType<TerrifierRarity>();
            Item.UseSound = SoundID.Item73;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<FearfulPebble>();
            Item.shootSpeed = 17f;
            Item.mana = 36;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            switch(Act)
            {
                case 1:
                    {
                        const int NumProjectiles = 5; // The number of projectiles that this gun will shoot.

                        for (int i = 0; i < NumProjectiles; i++)
                        {
                            // Rotate the velocity randomly by 30 degrees at max.
                            Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));

                            // Decrease velocity randomly for nicer visuals.
                            newVelocity *= 1f - Main.rand.NextFloat(0.5f);

                            // Create a projectile.
                            Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<FearfulPebble>(), damage, knockback, player.whoAmI);
                        }
                        if (++timer < 5)
                        {
                            break;
                        }
                        Act = 2;
                        timer = 0;
                    }
                    break;
                case 2:
                    {
                        const int NumProjectile = 1; // The number of projectiles that this gun will shoot.

                        for (int i = 0; i < NumProjectile; i++)
                        {
                            // Rotate the velocity randomly by 30 degrees at max.
                            Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(4));

                            // Decrease velocity randomly for nicer visuals.
                            newVelocity *= 1f - Main.rand.NextFloat(0.5f);

                            // Create a projectile.
                            Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<FearfulStar>(), damage, knockback, player.whoAmI);
                        }
                        if (++timer < 8)
                        {
                            break;
                        }
                        Act = 1;
                        timer = 0;
                    }
                    break;
            }


            return false; // Return false because we don't want tModLoader to shoot projectile
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
    }
}