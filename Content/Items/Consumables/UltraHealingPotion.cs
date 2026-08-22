using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Rarities;

namespace TimeCrusadeMod.Content.Items.Consumables
{
    public class UltraHealingPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 20;

            // Dust that will appear in these colors when the item with ItemUseStyleID.DrinkLiquid is used
            ItemID.Sets.DrinkParticleColors[Type] = [
                new Color(29, 49, 165),
                new Color(20, 40, 160),
                new Color(22, 44, 166)
            ];
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 38;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item3;
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;
            Item.rare = ModContent.RarityType<BrightBlue>();
            Item.value = Item.buyPrice(gold: 1);
            Item.healLife = 300;
            Item.buffType = BuffID.PotionSickness;
            Item.buffTime = 3600; // 3600 ticks = 60 seconds
            Item.potion = true; // important: marks this as a potion-style item, so Potion Sickness prevents reuse
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SuperHealingPotion);
            recipe.AddIngredient(ModContent.ItemType<AxitrentonCrystal>());
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}