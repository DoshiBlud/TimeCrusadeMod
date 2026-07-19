using TimeCrusadeMod.Content.NPCs.Asphalia;
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

namespace TimeCrusadeMod.Content.Items.Consumables
{
    public class BrittleRock : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 14;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Blue;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<Asphalia>());
        }
        public override bool? UseItem(Player player)
        {
            if(player.whoAmI == Main.myPlayer)
            {
                // Play Boss Roar
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                int type = ModContent.NPCType<Asphalia>();
                if(Main.netMode != NetmodeID.MultiplayerClient)
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
			recipe.AddIngredient(ItemID.StoneBlock, 250);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
    }
}