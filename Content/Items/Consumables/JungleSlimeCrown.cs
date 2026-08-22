using TimeCrusadeMod.Content.NPCs.Axy;
using TimeCrusadeMod.Content.Items.Consumables;
using TimeCrusadeMod.Content.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Rarities;
using TimeCrusadeMod.Content.NPCs.JungleLordSlime;
using TimeCrusadeMod.Content.Items.Placeables;

namespace TimeCrusadeMod.Content.Items.Consumables
{
    public class JungleSlimeCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 30;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Green;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.ZoneJungle)
            {
                NPC.AnyNPCs(ModContent.NPCType<JungleLordSlime>());
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                // Play Boss Roar
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                int type = ModContent.NPCType<JungleLordSlime>();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                    return true;
                }
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
            }
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Gel, 20);
            recipe.AddIngredient(ItemID.JungleSpores, 4);
            recipe.AddIngredient(ModContent.ItemType<JungleCrown>());
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}