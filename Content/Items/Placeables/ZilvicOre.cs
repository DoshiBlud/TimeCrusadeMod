using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Tiles;

namespace TimeCrusadeMod.Content.Items.Placeables
{
    internal class ZilvicOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 58;
        }

        public override void SetDefaults()
        {
            Item.width = 23;
            Item.height = 23;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 1);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.ZilvicOreTile>();
            Item.rare = ItemRarityID.White;
        }
    }
}