using TimeCrusadeMod.Content.NPCs.Asphalia;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Accessories;

namespace TimeCrusadeMod.Content.Items.Consumables
{
    public class AsphaliaBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BossBag[Type] = true;
            ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;

            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
           Item.width = 32;
           Item.height = 32;
           
            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;

            Item.rare = ItemRarityID.Purple;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrittleLifter>()));
            itemLoot.Add(ItemDropRule.Common(ItemID.StoneBlock, 100));

            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<Asphalia>()));
        }
    }
}