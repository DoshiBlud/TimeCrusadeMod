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
    public class LunarTempest : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Saliquent Blade");
            // Tooltip.SetDefault("The best blade you will ever hear lol.");
        }

        public override void SetDefaults()
        {
            Item.damage = 212;
            Item.DamageType = DamageClass.Melee;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = false;
            Item.knockBack = 7;
            Item.value = Item.sellPrice(gold: 3, silver: 20);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LihzahrdTemplePiece>();
            Item.shootSpeed = 22f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 2;
            float rotation = MathHelper.ToRadians(0);
            float rotation1 = MathHelper.ToRadians(20);
            float rotation2 = MathHelper.ToRadians(-20f);
            for (int t = 1; t < numberProjectiles; t++)
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, t / (numberProjectiles - 1)));
                // Shoot projectile 1 (e.g., a custom mod projectile)
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<LihzahrdTemplePiece>(), damage, knockback, player.whoAmI);
            }
            return false; // Prevents the default projectile from shooting
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(7))
            {
                // Creates dust at the player's weapon location
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Lihzahrd);
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarOre, 20);
            recipe.AddIngredient(ItemID.LihzahrdBrick, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}