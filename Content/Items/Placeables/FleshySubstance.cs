using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Items.Placeables
{
    public class FleshySubstance : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 25; // Configure the amount of this item that's needed to research it in Journey mode.
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 20;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = 1000; // Makes the item worth 1 gold.
            Item.rare = ItemRarityID.White;
        }
    }
}