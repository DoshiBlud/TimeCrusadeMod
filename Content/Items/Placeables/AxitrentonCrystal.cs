using Microsoft.Xna.Framework;
using System.Drawing;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Rarities;

namespace TimeCrusadeMod.Content.Items.Placeables
{
    internal class AxitrentonCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(platinum: 500);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = ModContent.RarityType<AxitrentonRarity>();
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.AxitrentonCrystal>();
            Item.useTime = 10;
            Item.useAnimation = 15;
            Item.autoReuse = true;
        }
    }
}
