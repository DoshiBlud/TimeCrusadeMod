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
    public class EmeraldGemilizer : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Saliquent Blade");
            // Tooltip.SetDefault("The best blade you will ever hear lol.");
        }

        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Melee;
            Item.width = 34;
            Item.height = 36;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = false;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(gold: 3, silver: 20);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Emerald, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}