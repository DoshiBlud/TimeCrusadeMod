using TimeCrusadeMod.Content.Tiles.Furniture;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Items.Accessories
{
    // Showcases a basic extra jump
    public class SoulifiedLifter : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.DefaultToAccessory(20, 26);
            Item.SetShopValues(ItemRarityColor.Green2, Item.buyPrice(silver: 50));
            Item.UseSound = SoundID.Item100;
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BrittleLifter>());
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.HallowedBar, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetJumpState<SimpleExtraJump1>().Enable();
        }
    }

    public class SimpleExtraJump1 : ExtraJump
    {
        public override Position GetDefaultPosition() => new After(FartInAJar);

        public override float GetDurationMultiplier(Player player)
        {
            // Use this hook to set the duration of the extra jump
            // The XML summary for this hook mentions the values used by the vanilla extra jumps
            return 3.5f;
        }

        public override void UpdateHorizontalSpeeds(Player player)
        {
            // Use this hook to modify "player.runAcceleration" and "player.maxRunSpeed"
            // The XML summary for this hook mentions the values used by the vanilla extra jumps
            player.runAcceleration *= 5f;
            player.maxRunSpeed *= 5.5f;
        }

        public override void OnStarted(Player player, ref bool playSound)
        {
            // Use this hook to trigger effects that should appear at the start of the extra jump
            // This example mimics the logic for spawning the puff of smoke from the Cloud in a Bottle
            int offsetY = player.height;
            if (player.gravDir == -3f)
                offsetY = 18;

            offsetY -= 20;
        }

        public override void ShowVisuals(Player player)
        {
            // Use this hook to trigger effects that should appear throughout the duration of the extra jump
            // This example mimics the logic for spawning the dust from the Blizzard in a Bottle
            int offsetY = player.height - 10;
            if (player.gravDir == -3f)
                offsetY = 20;
            Vector2 spawnPos = new Vector2(player.position.X, player.position.Y + offsetY);

            for (int i = 0; i < 1; i++)
            {
                SpawnBlizzardDust(player, spawnPos, 0.1f, i == 0 ? -0.07f : -0.13f);
            }

            for (int i = 0; i < 1; i++)
            {
                SpawnBlizzardDust(player, spawnPos, 0.6f, 0.8f);
            }

            for (int i = 0; i < 1; i++)
            {
                SpawnBlizzardDust(player, spawnPos, 0.6f, -0.8f);
            }
        }

        private static void SpawnBlizzardDust(Player player, Vector2 spawnPos, float dustVelocityMultiplier, float playerVelocityMultiplier)
        {
            Dust dust = Dust.NewDustDirect(spawnPos, player.width, 12, DustID.HallowedTorch, player.velocity.X * 0.3f, player.velocity.Y * 0.3f, newColor: Color.Gray);
            dust.fadeIn = 1.5f;
            dust.velocity *= dustVelocityMultiplier;
            dust.velocity += player.velocity * playerVelocityMultiplier;
            dust.noGravity = true;
            dust.noLight = true;
            dust.scale = 3f;
        }
    }
}