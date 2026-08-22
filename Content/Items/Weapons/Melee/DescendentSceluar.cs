using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Rarities;
using TimeCrusadeMod.Content.Tiles.Furniture;

namespace TimeCrusadeMod.Content.Items.Weapons.Melee
{
    public class DescendentSceluar : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 9));
            ItemID.Sets.AnimatesAsSoul[Type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation
            // DisplayName.SetDefault("Saliquent Blade");
            // Tooltip.SetDefault("The best blade you will ever hear lol.");
        }

        public override void SetDefaults()
        {
            Item.damage = 295;
            Item.DamageType = DamageClass.Melee;
            Item.width = 68;
            Item.height = 68;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = false;
            Item.knockBack = 9;
            Item.value = Item.sellPrice(gold: 3, silver: 20);
            Item.rare = ModContent.RarityType<IllusionaryRarity>();
            Item.UseSound = SoundID.Item15 with { Pitch = 0.04f, PitchVariance = 0.02f };
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SaturnCopy>();
            Item.shootSpeed = 10f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberProjectiles = 2; // Number of projectiles per shot
            float rotation = MathHelper.ToRadians(0); // Total spread angle in degrees
            float rotations = MathHelper.ToRadians(360); // Total spread angle in degrees

            for (int i = 0; i < numberProjectiles; i++)
            {
                // Calculate the angle for each individual projectile
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));

                Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<WhiteLight>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<SaturnCopy>(), damage, knockback, player.whoAmI);
            }
            return false; // Return false so vanilla doesn't shoot its own projectile
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ElementalBlade>());
            recipe.AddIngredient(ModContent.ItemType<IllusionaryDescendentBar>(), 10);
            recipe.AddTile(ModContent.TileType<AxitrentonObserver>());
            recipe.Register();
        }
    }
}