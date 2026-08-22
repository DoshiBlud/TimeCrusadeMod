using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Dusts;

namespace TimeCrusadeMod.Content.Items.Weapons.Magic
{
    public class StarExiler : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Saliquent Blade");
            // Tooltip.SetDefault("The best blade you will ever hear lol.");
        }

        public override void SetDefaults()
        {
            Item.damage = 110;
            Item.DamageType = DamageClass.Magic;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 23;
            Item.useAnimation = 56;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 56;
            Item.value = Item.sellPrice(gold: 2, silver: 20);
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item72;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<StarLight>();
            Item.shootSpeed = 12f;
            Item.mana = 40;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 3; // Number of projectiles per shot
            float rotation = MathHelper.ToRadians(36); // Total spread angle in degrees

            for (int i = 0; i < numberProjectiles; i++)
            {
                // Calculate the angle for each individual projectile
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));

                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<StarLight>(), damage, knockback, player.whoAmI);
            }
            return false; // Return false so vanilla doesn't shoot its own projectile
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarOre, 12);
            recipe.AddIngredient(ItemID.SpectreStaff);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
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
    }
}