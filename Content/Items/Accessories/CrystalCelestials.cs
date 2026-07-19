using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Rarities;
using Terraria.Enums;
using System.Collections.Generic;
using TimeCrusadeMod.Content.Tiles.Furniture;
using Microsoft.Xna.Framework;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Items.Placeables;

namespace TimeCrusadeMod.Content.Items.Accessories
{
    // This example attempts to showcase most of the common boot accessory effects.
    // Of particular note is a showcase of the correct approaches to various movement speed modifications.
    [AutoloadEquip(EquipType.Shoes)]
    public class CrystalCelestials : ModItem
    {
        public static readonly int MoveSpeedBonus = 14;
        public static readonly int LavaImmunityTime = 10;
        public static readonly int RocketBootsType = 4;

        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MoveSpeedBonus, LavaImmunityTime);

        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 42;

            Item.accessory = true;
            Item.rare = ModContent.RarityType<AxitrentonRarity>();
            Item.value = Item.buyPrice(gold: 20); // Equivalent to Item.buyPrice(0, 1, 0, 0);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CelestialLifter>());
            recipe.AddIngredient(ItemID.TerrasparkBoots);
            recipe.AddIngredient(ModContent.ItemType<AxitrentonCrystal>(), 5);
            recipe.AddTile(ModContent.TileType<AxitrentonObserver>());
            recipe.Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // These 2 stat changes are equal to the Lightning Boots
            player.moveSpeed += MoveSpeedBonus / 100f; // Modifies the player movement speed bonus.
            player.accRunSpeed = 9.25f; // Sets the players sprint speed in boots.

            // player.maxRunSpeed and player.runAcceleration are usually not set by boots and should not be changed in UpdateAccessory due to the logic order. See ExampleStatBonusAccessoryPlayer.PostUpdateRunSpeeds for an example of adjusting those speed stats.

            // Determines whether the boots count as rocket boots
            // 0 - These are not rocket boots
            // Anything else - These are rocket boots
            player.rocketBoots = 4;

            // Sets which dust and sound to use for the rocket flight
            // 1 - Rocket Boots
            // 2 - Fairy Boots, Spectre Boots, Lightning Boots
            // 3 - Frostspark Boots
            // 4 - Terrraspark Boots
            // 5 - Hellfire Treads
            player.vanityRocketBoots = 4;

            player.waterWalk2 = true; // Allows walking on all liquids without falling into it
            player.waterWalk = true; // Allows walking on water, honey, and shimmer without falling into it
            player.iceSkate = true; // Grant the player improved speed on ice and not breaking thin ice when falling onto it
            player.desertBoots = true; // Grants the player increased movement speed while running on sand
            player.fireWalk = true; // Grants the player immunity from Meteorite and Hellstone tile damage
            player.noFallDmg = true; // Grants the player the Lucky Horseshoe effect of nullifying fall damage
            player.lavaRose = true; // Grants the Lava Rose effect
            player.lavaMax += LavaImmunityTime * 60; // Grants the player 2 additional seconds of lava immunity
            player.GetJumpState<SimpleExtraJumpboot>().Enable();

            // player.DoBootsEffect(player.DoBootsEffect_PlaceFlowersOnTile); // Spawns flowers when walking on normal or Hallowed grass

            // These effects are visual only. These are replicated in UpdateVanity below so they apply for vanity equipment.
            if (!hideVisual)
            {
                player.CancelAllBootRunVisualEffects(); // This ensures that boot visual effects don't overlap if multiple are equipped

                // Hellfire Treads sprint dust. For more info on sprint dusts see Player.SpawnFastRunParticles() method in Player.cs
                // Other boot run visual effects include: sailDash, coldDash, desertDash, fairyBoots
            }
        }
        public class SimpleExtraJumpboot : ExtraJump
        {
            public override Position GetDefaultPosition() => new After(FartInAJar);

            public override float GetDurationMultiplier(Player player)
            {
                // Use this hook to set the duration of the extra jump
                // The XML summary for this hook mentions the values used by the vanilla extra jumps
                return 6f;
            }

            public override void UpdateHorizontalSpeeds(Player player)
            {
                // Use this hook to modify "player.runAcceleration" and "player.maxRunSpeed"
                // The XML summary for this hook mentions the values used by the vanilla extra jumps
                player.runAcceleration *= 6f;
                player.maxRunSpeed *= 4f;
            }

            public override void OnStarted(Player player, ref bool playSound)
            {
                // Use this hook to trigger effects that should appear at the start of the extra jump
                // This example mimics the logic for spawning the puff of smoke from the Cloud in a Bottle
                int offsetY = player.height;
                if (player.gravDir == -3f)
                    offsetY = 22;

                offsetY -= 22;
            }

            public override void ShowVisuals(Player player)
            {
                // Use this hook to trigger effects that should appear throughout the duration of the extra jump
                // This example mimics the logic for spawning the dust from the Blizzard in a Bottle
                int offsetY = player.height - 12;
                if (player.gravDir == -3f)
                    offsetY = 22;
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
                Dust dust = Dust.NewDustDirect(spawnPos, player.width, 12, ModContent.DustType<AxitrentonDust>(), player.velocity.X * 0.3f, player.velocity.Y * 0.3f, newColor: Color.Gray);
                dust.fadeIn = 1.5f;
                dust.velocity *= dustVelocityMultiplier;
                dust.velocity += player.velocity * playerVelocityMultiplier;
                dust.noGravity = true;
                dust.noLight = true;
                dust.scale = 1f;
            }
        }

        public override void UpdateVanity(Player player)
        {
            // This code is a copy of the visual effects code in UpdateAccessory above
            player.CancelAllBootRunVisualEffects();
            player.vanityRocketBoots = 2;
            player.hellfireTreads = true;
            if (!player.mount.Active || player.mount.Type != MountID.WallOfFleshGoat)
            {
            }
        }
    }
}