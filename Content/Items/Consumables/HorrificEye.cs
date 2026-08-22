using TimeCrusadeMod.Content.NPCs.Terrifier;
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
using TimeCrusadeMod.Content.Items.Placeables;

namespace TimeCrusadeMod.Content.Items.Consumables
{
    public class HorrificEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.maxStack = 20;
            Item.rare = ModContent.RarityType<TerrifierRarity>();
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<TerrifierHead>());
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                // Play Boss Roar
                SoundEngine.PlaySound(SoundID.Roar, player.position);
                int type = ModContent.NPCType<TerrifierHead>();
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
            recipe.AddIngredient(ModContent.ItemType<RottenLens>());
            recipe.AddIngredient(ItemID.LunarOre, 10);
            recipe.AddIngredient(ModContent.ItemType<SoulofArum>(), 3);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();

            Recipe recipes = CreateRecipe();
            recipes.AddIngredient(ModContent.ItemType<FleshySubstance>());
            recipes.AddIngredient(ItemID.LunarOre, 10);
            recipes.AddIngredient(ModContent.ItemType<SoulofArum>(), 3);
            recipes.AddTile(TileID.DemonAltar);
            recipes.Register();
        }
    }
}