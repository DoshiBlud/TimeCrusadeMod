using Microsoft.Xna.Framework;
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
    public class ZebraMK12 : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Gun");
            // Tooltip.SetDefault("A gun made from crystal, it shoots crystal bullets.");
        }

        public override void SetDefaults()
        {
            Item.damage = 155;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 100;
            Item.height = 24;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(platinum: 500);
            Item.rare = ModContent.RarityType<BrightBlue>();
            Item.UseSound = new SoundStyle($"TimeCrusadeMod/Assets/Sounds/Guns/ZebraGun"); // Sound file from this mod
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<ZebraBullet>();
            Item.shootSpeed = 50f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 2; // Number of projectiles per shot
            float rotation = MathHelper.ToRadians(1); // Total spread angle in degrees


            for (int i = 0; i < numberProjectiles; i++)
            {
                // Calculate the angle for each individual projectile
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(rotation, rotation, i / (numberProjectiles - 1)));

                Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<ZebraBullet>(), damage, knockback, player.whoAmI);
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
            recipe.AddIngredient(ItemID.SniperRifle);
            recipe.AddIngredient(ItemID.Leather, 3);
            recipe.AddIngredient(ItemID.LunarOre, 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}