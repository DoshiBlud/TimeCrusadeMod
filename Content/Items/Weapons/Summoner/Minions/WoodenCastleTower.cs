using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Weapons.Summoner.Minions;
using TimeCrusadeMod.Content.Projectiles;

namespace TimeCrusadeMod.Content.Items.Weapons.Summoner.Minions
{
    // ExampleSentry is an example sentry.
    // ExampleSentry demonstrates both floating and grounded sentry behaviors. Use ExampleSentryItem to the left of the player spawn a grounded sentry and use it to the right to spawn a floating sentry.
    // Sentries are similar to Minions, but typically don't move, are limited by the sentry limit instead of the minion limit, don't have a corresponding buff, and last for 10 minutes instead of despawning when the player dies.
    // The most critical fields necessary for a projectile to count as a sentry will be noted in this file and in ExampleSentryItem.cs. See also ExampleSentryShot.cs.
    public class WoodenCastleTowerStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 4;
            Item.DamageType = DamageClass.Summon;
            Item.sentry = true;
            Item.mana = 3;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 35;
            Item.useAnimation = 35;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 1;
            Item.value = Item.buyPrice(gold: 30);
            Item.rare = ItemRarityID.White;
            Item.UseSound = SoundID.Item83;
            Item.shoot = ModContent.ProjectileType<WoodenCastleTower>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.Wood, 20)
            .AddIngredient(ItemID.Topaz, 3)
            .AddTile(TileID.WorkBenches)
            .Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            bool canPlaceInAir = false;
            // This is just to let modders experiment with a sentry that places anywhere and one that snaps to the ground.
            if (player.direction == 1)
            {
                canPlaceInAir = false;
            }

            position = Main.MouseWorld;
            player.LimitPointToPlayerReachableArea(ref position);
            int halfProjectileHeight = (int)Math.Ceiling(ContentSamples.ProjectilesByType[type].height * 2f);

            if (!canPlaceInAir)
            {
                // This code will "snap" the sentry to the floor.
                // FindSentryRestingSpot returns the coordinates for the sentry to be placed on solid ground below the cursor position.
                player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
                position = new Vector2(worldX, worldY - halfProjectileHeight);

                // If, for some reason, you need custom placement logic (extra wide, hanging from the ceiling, etc), the following can be used as a guide for implementing that:
                /*
                    // This loop travels down until it finds a solid tile to rest on.
                    (int i, int j) = position.ToTileCoordinates();
                    while (j < Main.maxTilesY - 10) {
                        // This code checks a 3 tile wide area, this will need to be adjusted if the sentry's with is larger than 48.
                        if (WorldGen.SolidTile2(i, j) || WorldGen.SolidTile2(i - 1, j) || WorldGen.SolidTile2(i + 1, j)) {
                            break;
                        }
                        j++;
                    }

                    position = new Vector2(i * 16 + 8, j * 16 - halfProjectileHeight);
                    // Also, replace "i * 16 + 8" with "position.X" if you don't want the sentry to "snap" to the center of tiles like the newer Tavernkeep sentries do.
                    */
            }
            else
            {
                position.Y -= halfProjectileHeight; // Adjust in-air option to spawn with bottom at cursor.
            }

            // Spawn the sentry projectile at the calculated location.
            Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, Main.myPlayer, ai2: canPlaceInAir ? 0 : 1);

            // Kills older sentry projectiles according to player.maxTurrets
            player.UpdateMaxTurrets();

            return false;
        }
    }
    public class WoodenCastleTower : ModProjectile
    {
        public ref float ShootTimer => ref Projectile.ai[0];

        public bool Floating => Projectile.ai[2] == 0;

        public bool JustSpawned
        {
            get => Projectile.localAI[0] == 0;
            set => Projectile.localAI[0] = value ? 0 : 1;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 60;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.sentry = true; // Sets the weapon as a sentry for sentry accessories to properly work.
            Projectile.timeLeft = Projectile.SentryLifeTime; // Sentries last 10 minutes
            Projectile.ignoreWater = true;
            Projectile.netImportant = true; // Sentries need this so they are synced to newly joining players

            // The texture is 54 pixels wide, but we set width to 42 and DrawOffsetX to -6 so it doesn't look weird hanging off the edge of tiles (because it is oval shaped).
            DrawOffsetX = 0;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false; // Allow this projectile to collide with platforms
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false; // Prevent tile collision from killing the projectile
        }

        public override Color? GetAlpha(Color lightColor)
        {
            // Always draw fully bright. This is important because sentries can usually be placed inside tiles where it would be dark.
            return Color.White;
        }

        // This AI will function as a static sentry, and will not move. If you would like to know how to do more advanced minion AI, check out ExampleSimpleMinion.cs.
        public override void AI()
        {
            const int ShootFrequency = 30; // How long the sentry waits between shots.
            const int TargetingRange = 125 * 28; // The sentry's targeting range, 50 tiles.
            const float FireVelocity = 13f; // The velocity the sentry's shot projectile will travel.

            // Code to run when spawned
            if (JustSpawned)
            {
                JustSpawned = false;
                ShootTimer = ShootFrequency * 1.7f; // Delay the first shot slightly

                // The sound that Frost Hydra, Spider Turret, and Houndius Shootius play when spawned. Optional.
                SoundEngine.PlaySound(SoundID.Item46, Projectile.position);

                // Dust indicating the sentry spawned. Optional.
            }

            // Spawn dust randomly
            if (Main.rand.NextBool(10))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.OrangeTorch, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
                dust.noGravity = true;
                dust.velocity *= 0.8f;
            }

            // Gravity
            Projectile.velocity.X = 0;
            if (!Floating)
            { // Only apply gravity if canPlaceInAir from ExampleSentryItem.Shoot was false
                Projectile.velocity.Y += 8f;
                if (Projectile.velocity.Y > 16f)
                {
                    Projectile.velocity.Y = 16f;
                }
            }


            // Find an enemy to target.
            float closestTargetDistance = TargetingRange;
            NPC targetNPC = null;
            // Prioritize the owner's minion attack target. (Right click or whip feature)
            if (Projectile.OwnerMinionAttackTargetNPC != null)
            {
                TryTargeting(Projectile.OwnerMinionAttackTargetNPC, ref closestTargetDistance, ref targetNPC);
            }

            // If no minion attack target or if it was out of range, find the closest enemy to target.
            if (targetNPC == null)
            {
                foreach (var npc in Main.ActiveNPCs)
                {
                    TryTargeting(npc, ref closestTargetDistance, ref targetNPC);
                }
            }

            if (targetNPC != null)
            {
                float shootDegrees = MathHelper.ToRadians(90);
                if (ShootTimer <= 0)
                {
                    ShootTimer = ShootFrequency;

                    // Play a shoot sound
                    SoundEngine.PlaySound(SoundID.Item102 with { Volume = 0.4f }, Projectile.Center);

                    // Actually spawning the projectile only runs if the local player is the owner
                    if (Main.myPlayer == Projectile.owner)
                    {
                        // The direction the projectile will fire.
                        Vector2 shootDirection = (targetNPC.Center - Projectile.Center).SafeNormalize(Vector2.UnitX);
                        // The final velocity vector
                        Vector2 shootVelocity = shootDirection * FireVelocity;

                        // The type of projectile the sentry will shoot. It is important that sentry shots are included in ProjectileID.Sets.SentryShot, so reusing unrelated vanilla projectiles as-is won't work 100%.
                        int type = ModContent.ProjectileType<WoodenCastlePebble>();

                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X  + 5f, Projectile.Center.Y + 5), shootVelocity, type, Projectile.damage, 3, Projectile.owner);
                        // Note that Projectile.damage will take into account current equipment damage bonuses automatically for sentries and minions, so there is no need to calculate that here to take advantage of current equipment bonuses.
                        // See Projectile.ContinuouslyUpdateDamageStats docs for more information.
                    }
                }
            }

            // Count down the shoot timer
            ShootTimer--;
        }

        // Checks if npc is closer than current targetNPC. If so, adjust targetNPC and closestTargetDistance.
        private void TryTargeting(NPC npc, ref float closestTargetDistance, ref NPC targetNPC)
        {
            if (npc.CanBeChasedBy(this))
            {
                float distanceToTargetNPC = Vector2.Distance(Projectile.Center, npc.Center);
                // Is this enemy closer than others? Is it in line of sight?
                if (distanceToTargetNPC < closestTargetDistance && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                {
                    closestTargetDistance = distanceToTargetNPC; // Set a new closest distance value
                    targetNPC = npc;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            // Some sentries play a sound when despawned:
            //SoundEngine.PlaySound(SoundID.Item27, Projectile.position);

            // Dust indicating the sentry despawned
        }
    }
}