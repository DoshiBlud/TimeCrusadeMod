using System.Drawing;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Common;
using TimeCrusadeMod.Content.Tiles.Furniture;

namespace TimeCrusadeMod.Content.Items.Placeables
{
    internal class NeutralityFragments : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
            ItemID.Sets.ExtractinatorMode[Type] = Item.type;
        }

        public override void SetDefaults()
        {
            Item.width = 25;
            Item.height = 25;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 5);
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            CreateRecipe(25)
                .AddIngredient(ModContent.ItemType<NeutralityBar>(), 1)
                .AddTile(ModContent.TileType<Decomposer>())
                .Register();
        }
    }
}

