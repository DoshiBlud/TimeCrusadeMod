using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Rarities;

namespace TimeCrusadeMod.Content.Items.Weapons.Ranged
{
    public class MaliciousNightmares : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Gun");
            // Tooltip.SetDefault("A gun made from crystal, it shoots crystal bullets.");
        }

        public override void SetDefaults()
        {
            Item.damage = 230;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 50;
            Item.height = 70;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 30;
            Item.value = Item.sellPrice(gold: 6, silver: 50);
            Item.rare = ModContent.RarityType<TerrifierRarity>();
            Item.UseSound = SoundID.Item10;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<MaliciousArrow>();
            Item.shootSpeed = 35f;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(9));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            const int NumProjectiles = 3; // The number of projectiles that this gun will shoot.

            for (int i = 0; i < NumProjectiles; i++)
            {
                // Rotate the velocity randomly by 30 degrees at max.
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));

                // Decrease velocity randomly for nicer visuals.
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);

                // Create a projectile.
                Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<MaliciousArrow>(), damage, knockback, player.whoAmI);
            }

            return false; // Return false because we don't want tModLoader to shoot projectile
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
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<CypherDust>());
            }
        }
    }
}