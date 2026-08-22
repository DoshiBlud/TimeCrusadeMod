using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Placeables;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Tiles.Furniture;
using TimeCrusadeMod.Content.Rarities;

namespace TimeCrusadeMod.Content.Items.Placeables
{
    internal class ErelesentBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 59;
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(platinum: 9999);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.ErelesentBar>();
            Item.placeStyle = 0;
            Item.rare = ModContent.RarityType<ErelesentRarity>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SpectreBar, 12)
                .AddIngredient(ModContent.ItemType<IllusionaryDescendentBar>(), 3)
                .AddIngredient(ModContent.ItemType<SoulofDread>(), 2)
                .AddTile(ModContent.TileType<AxitrentonObserver>())
                .Register();
        }
    }
}