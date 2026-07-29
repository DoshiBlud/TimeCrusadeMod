using Microsoft.Build.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Common.Systems;
using TimeCrusadeMod.Content.BossBars;
using TimeCrusadeMod.Content.Items.Consumables;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Items.Placeables.Furniture;
using TimeCrusadeMod.Content.NPCs.Axy;
using TimeCrusadeMod.Content.NPCs.Enemies.Worms;
using TimeCrusadeMod.Content.NPCs.Terrifier;
using TimeCrusadeMod.Content.Projectiles;

namespace TimeCrusadeMod.Content.NPCs.Terrifier
{
    [AutoloadBossHead]
    // These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
    internal class TerrifierHead : WormHead
    {
        public int phase2 = 1;
        public int phase1 = 0;
        public int timer4 = 0;
        public int timer3 = 0;
        public int timerss = 0;
        public int timers = 0;
        public int timer = 0;
        public int Act = 1;
        private static Asset<Texture2D> glowTexture;

        public override void Load()
        {
            glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
        }
        public override int BodyType => ModContent.NPCType<TerrifierBody>();

        public override int TailType => ModContent.NPCType<TerrifierTail>();

        public override void SetStaticDefaults()
        {
            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                CustomTexturePath = "TimeCrusadeMod/Content/NPCs/Terrifier/Terrifier_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
                Position = new Vector2(40f, 24f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 12f
            };
            NPCID.Sets.TrailCacheLength[NPC.type] = 5; // The length of old position to be recorded
            NPCID.Sets.TrailingMode[NPC.type] = 0; // The recording mode
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Slow] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.boss = true;
            NPC.width = 220;
            NPC.height = 55;
            NPC.damage = 150;
            NPC.lifeMax = 185000;
            NPC.defense = 340;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.npcSlots = 10f;
            NPC.realLife = NPC.whoAmI;
            NPC.knockBackResist = 0;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/LullabiesofDread");

                // If you would like to play alternate music when the otherworld soundtrack enabled, use this logic.
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;

            Texture2D Glow = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glow").Value;

            Rectangle sourceRect = NPC.frame;

            Vector2 origin2 = sourceRect.Size() / 2;

            spriteBatch.Draw(
                texture,
                NPC.Center - screenPos,
                sourceRect,
                drawColor,
                NPC.rotation,
                origin2,
                NPC.scale,
                SpriteEffects.None,
                0f
            );

            spriteBatch.Draw(
                Glow,
                NPC.Center - screenPos,
                sourceRect,
                Color.White,
                NPC.rotation,
                origin2,
                NPC.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<TerrifierBag>()));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StrangePiece>()));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Melee.AxeofFear>(), 2, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Summoner.WhipofTorture>(), 2, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Summoner.Minions.BallTerrifierStaff>(), 2, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Ranged.MaliciousNightmares>(), 2, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Weapons.Magic.BalisticTerrorStaff>(), 2, 1, 1));
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeables.Furniture.TerrifierRelic>()));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange([
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A giant worm filled with pure fear and dread, it comes from the souls of many with its intent of making you suffer.")
            ]);
        }

        public override void Init()
        {
            // Set the segment variance
            // If you want the segment length to be constant, set these two properties to the same value
            MinSegmentLength = 30;
            MaxSegmentLength = 30;

            CommonWormInit(this);
        }

        // This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
        internal static void CommonWormInit(Worm worm)
        {
            // These two properties handle the movement of the worm
            worm.MoveSpeed = 8f;
            worm.Acceleration = 0.5f;
            if (Main.expertMode)
            {
                worm.MoveSpeed = 9f;
                worm.Acceleration = 1;
            }
            if (Main.masterMode)
            {
                worm.MoveSpeed = 10f;
                worm.Acceleration = 2;
            }

        }

        private int attackCounter;
        private int attackCounters;
        private int attackCounterss;
        private int attackCounter3;
        private int attackCounter4;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
            writer.Write(attackCounters);
            writer.Write(attackCounterss);
            writer.Write(attackCounter3);
            writer.Write(attackCounter4);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
            attackCounters = reader.ReadInt32();
            attackCounterss = reader.ReadInt32();
            attackCounter3 = reader.ReadInt32();
            attackCounter4 = reader.ReadInt32();
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            // If the NPC dies, spawn gore and play a sound
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }

            if (NPC.life <= 0)
            {
                // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
                int frontGoreType = Mod.Find<ModGore>("TerrifierHead_Front").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, frontGoreType);
                }
            }
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
            return true;
        }
        public override void OnKill()
        {

            // This sets downedMinionBoss to true, and if it was false before, it initiates a lantern night
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedTerrifier, -1);
        }
        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                float speed = 12;
                float speedUp = 0.5f;
                float MoveUp = -5;
                float MoveDown = 6;
                float MoveLeft = -6;
                if (phase1 == 0)
                {
                    Player player = Main.player[NPC.target];
                    if (NPC.target == 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                    {
                        NPC.TargetClosest();
                    }
                    if (player.dead || !player.active)
                    {
                        NPC.velocity.Y -= 0.04f;
                        NPC.EncourageDespawn(1000000);
                        return;
                    }
                    switch (Act)
                        {
                            case 1:
                            {
                                if (player.Center.Y > NPC.Center.Y)
                                {
                                    MoveSpeed = 7;
                                    Acceleration = 0.3f;
                                }
                                else if (player.Center.X > NPC.Center.X)
                                {
                                    MoveSpeed = 7;
                                    Acceleration = 0.3f;
                                }
                                if (attackCounter > 0)
                                {
                                    attackCounter--; // tick down the attack counter.
                                }
                                Player targe = Main.player[NPC.target];
                                // If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
                                if (attackCounter <= 4 && Vector2.Distance(NPC.Center, targe.Center) >= 200 && Collision.CanHit(NPC.Center, 1, 1, targe.Center, 1, 1))
                                {
                                    Vector2 direction = (targe.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                                    direction = direction.RotatedBy(MathHelper.ToRadians(0));

                                    int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 19, ModContent.ProjectileType<TerrifierPebble>(), 5, 0, Main.myPlayer);
                                    Main.projectile[projectile].timeLeft = 500;
                                    Main.projectile[Type].damage = 75;
                                    attackCounter = 70;
                                    SoundEngine.PlaySound(SoundID.Item21 with { Pitch = 0.6f, PitchVariance = 0.2f });
                                    NPC.netUpdate = true;
                                }
                                if (++timer < 300) // Wait for the timer to reach 120
                                {
                                    break; // Ending the state early; timer not done yet.
                                }

                                Act = 2; // Go to next act
                                timer = 0; // Reset timer
                            }
                            break;

                            case 2:
                            {
                                    for (int dog = 0; dog < 2; dog++)
                                    {
                                        if (attackCounters > 0)
                                        {
                                            attackCounters--; // tick down the attack counter.
                                        }
                                        MoveSpeed = 3;
                                        Acceleration = 0.09f;

                                        Player targett = Main.player[NPC.target];
                                        if (attackCounters <= 4 && Vector2.Distance(NPC.Center, targett.Center) >= 200 && Collision.CanHit(NPC.Center, 1, 1, targett.Center, 1, 1))
                                        {
                                            Vector2 direction = (targett.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                                            direction = direction.RotatedBy(MathHelper.ToRadians(0));

                                            int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 22, ModContent.ProjectileType<TerrifierPebble>(), 5, 0, Main.myPlayer);
                                            Main.projectile[projectile].timeLeft = 500;
                                            Main.projectile[projectile].damage = 75;
                                            attackCounters = 30;
                                        SoundEngine.PlaySound(SoundID.Item21 with { Pitch = 0.6f, PitchVariance = 0.2f });
                                        NPC.netUpdate = true;
                                        }
                                    }
                                if (++timers < 180) // Wait for the timer to reach 180
                                {
                                    break; // Ending the state early; timer not done yet.
                                }

                                Act = 3; // Loop back to Act 1
                                timers = 0; // Reset timer
                            }
                            break;

                        case 3:
                            {
                                MoveSpeed = 2;
                                Acceleration = 1;

                                if (attackCounterss > 0)
                                {
                                    attackCounterss--; // tick down the attack counter.
                                }
                                MoveSpeed = 3;
                                Acceleration = 0.09f;

                                Player targett = Main.player[NPC.target];
                                if (attackCounterss <= 4 && Vector2.Distance(NPC.Center, targett.Center) >= 200 && Collision.CanHit(NPC.Center, 1, 1, targett.Center, 1, 1))
                                {
                                    Vector2 direction = (targett.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                                    direction = direction.RotatedBy(MathHelper.ToRadians(0));

                                    int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 30, ModContent.ProjectileType<TerrifierPebble>(), 5, 0, Main.myPlayer);
                                    Main.projectile[projectile].timeLeft = 500;
                                    Main.projectile[projectile].damage = 75;
                                    attackCounterss = 10;
                                    SoundEngine.PlaySound(SoundID.Item21 with { Pitch = 0.6f, PitchVariance = 0.2f });
                                    NPC.netUpdate = true;
                                }

                                if (++timerss < 250) // Wait for the timer to reach 180
                                {
                                    break; // Ending the state early; timer not done yet.
                                }

                                Act = 4; // Loop back to Act 1
                                timerss = 0; // Reset timer
                            }
                            break;

                        case 4:
                            {
                                    MoveSpeed = 10;
                                    Acceleration = 0.5f;
                                MoveToTarget(player, speed, speedUp, out float distance, MoveUp, MoveDown, MoveLeft, out float MoveRight);
                                if (++timer3 < 250) // Wait for the timer to reach 180
                                {
                                    break; // Ending the state early; timer not done yet.
                                }
                                float halfHealth = 0.5f;
                                float smallHealth = 0.25f;
                                if((float)NPC.life <= (float)NPC.lifeMax * halfHealth)
                                {
                                    Act = 2; //if boss health is 50%, go back to second attack//
                                }
                                else //if not, go back to first attack//
                                {
                                    Act = 1;
                                }
                                if ((float)NPC.life <= (float)NPC.lifeMax * smallHealth)
                                {
                                    Act = 5; //if boss health is 25%, go to fifth attack//
                                }
                                else //if not, go back to first attack//
                                {
                                    Act = 1;
                                }
                                timer3 = 0; // Reset timer
                            }
                            break;

                        case 5:
                            {
                                MoveSpeed = 6;
                                Acceleration = 0.5f;
                                if (attackCounter4 > 0)
                                {
                                    attackCounter4--; // tick down the attack counter.
                                }

                                Player targett = Main.player[NPC.target];
                                if (attackCounter4 <= 4 && Vector2.Distance(NPC.Center, targett.Center) >= 120 && Collision.CanHit(NPC.Center, 1, 1, targett.Center, 1, 1))
                                {
                                    Vector2 direction = (targett.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                                    direction = direction.RotatedByRandom(MathHelper.ToRadians(360));

                                    int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 30, ModContent.ProjectileType<TerrifierPebble>(), 5, 0, Main.myPlayer);
                                    Main.projectile[projectile].timeLeft = 500;
                                    Main.projectile[projectile].damage = 75;
                                    attackCounter4 = 8;
                                    SoundEngine.PlaySound(SoundID.Item21 with { Pitch = 0.6f, PitchVariance = 0.2f });
                                    NPC.netUpdate = true;
                                }
                                if (++timer4 < -1) // Wait for the timer to reach 180
                                {
                                    break; // Ending the state early; timer not done yet.
                                }
                            }
                            break;
                    }
                }
            }
        }
        private void MoveToTarget(Player player, float speed, float speedUp, out float distance, float MoveUp, float MoveDown, float MoveLeft, out float MoveRight)
        {
            MoveUp = NPC.velocity.Y - player.Center.Y;
            MoveDown = NPC.velocity.Y + player.Center.Y;
            MoveLeft = NPC.velocity.X - player.Center.X;
            MoveRight = NPC.velocity.X + player.Center.X;

            distance = Vector2.Distance(NPC.Center, player.Center);
            float moveSpeed = speed + distance;
            float targetVelocityX = (player.Center.X - NPC.Center.X) * moveSpeed;
            float targetVelocityY = (player.Center.Y - NPC.Center.Y) * moveSpeed;
        }

        internal class TerrifierBody : WormBody
        {
            private static Asset<Texture2D> glowTexture;

            public override void Load()
            {
                glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
            }

            public override void SetStaticDefaults()
            {
                NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
                {
                    Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
                };
                NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
                NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<TerrifierHead>();
            }

            public override void SetDefaults()
            {
                NPC.CloneDefaults(NPCID.DiggerBody);
                NPC.aiStyle = -1;
                NPC.boss = true;
                NPC.width = 50;
                NPC.height = 72;
                NPC.damage = 115;
                NPC.defense = 450;
                NPC.noGravity = true;
                NPC.noTileCollide = true;
                NPC.npcSlots = 100f;
                NPC.lifeMax = 185000;
            }
            public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
            {
                Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;

                Texture2D Glow = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glow").Value;

                Rectangle sourceRect = NPC.frame;

                Vector2 origin2 = sourceRect.Size() / 2;

                spriteBatch.Draw(
                    texture,
                    NPC.Center - screenPos,
                    sourceRect,
                    drawColor,
                    NPC.rotation,
                    origin2,
                    NPC.scale,
                    SpriteEffects.None,
                    0f
                );

                spriteBatch.Draw(
                    Glow,
                    NPC.Center - screenPos,
                    sourceRect,
                    Color.White,
                    NPC.rotation,
                    origin2,
                    NPC.scale,
                    SpriteEffects.None,
                    0f
                );

                return false;
            }
            public override bool CheckActive()
            {
                return false;
            }
            public override void AI()
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                }
            }
            public override bool CanHitPlayer(Player target, ref int cooldownSlot)
            {
                cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
                return true;
            }
            public override void HitEffect(NPC.HitInfo hit)
            {
                // If the NPC dies, spawn gore and play a sound
                if (Main.netMode == NetmodeID.Server)
                {
                    // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                    return;
                }

                if (NPC.life <= 0)
                {
                    // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
                    int frontGoreType = Mod.Find<ModGore>("TerrifierBody_Front").Type;

                    var entitySource = NPC.GetSource_Death();

                    for (int i = 0; i < 1; i++)
                    {
                        Gore.NewGore(entitySource, NPC.position, NPC.velocity, frontGoreType);
                    }
                }
            }

            public override void Init()
            {
                TerrifierHead.CommonWormInit(this);
            }
        }

        internal class TerrifierTail : WormTail
        {
            private static Asset<Texture2D> glowTexture;

            public override void Load()
            {
                glowTexture = ModContent.Request<Texture2D>(Texture + "_Glow");
            }
            public override void SetStaticDefaults()
            {
                NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
                {
                    Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
                };
                NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
                NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<TerrifierHead>();
            }

            public override void SetDefaults()
            {
                NPC.CloneDefaults(NPCID.DiggerTail);
                NPC.aiStyle = -1;
                NPC.boss = true;
                NPC.width = 98;
                NPC.height = 188;
                NPC.damage = 80;
                NPC.defense = 600;
                NPC.noGravity = true;
                NPC.noTileCollide = true;
                NPC.npcSlots = 100f;
                NPC.lifeMax = 185000;
            }
            public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
            {
                Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;

                Texture2D Glow = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Glow").Value;

                Rectangle sourceRect = NPC.frame;

                Vector2 origin2 = sourceRect.Size() / 2;

                spriteBatch.Draw(
                    texture,
                    NPC.Center - screenPos,
                    sourceRect,
                    drawColor,
                    NPC.rotation,
                    origin2,
                    NPC.scale,
                    SpriteEffects.None,
                    0f
                );

                spriteBatch.Draw(
                    Glow,
                    NPC.Center - screenPos,
                    sourceRect,
                    Color.White,
                    NPC.rotation,
                    origin2,
                    NPC.scale,
                    SpriteEffects.None,
                    0f
                );

                return false;
            }
            public override bool CheckActive()
            {
                return false;
            }
            public override bool CanHitPlayer(Player target, ref int cooldownSlot)
            {
                cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
                return true;
            }
            public override void HitEffect(NPC.HitInfo hit)
            {
                // If the NPC dies, spawn gore and play a sound
                if (Main.netMode == NetmodeID.Server)
                {
                    // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                    return;
                }

                if (NPC.life <= 0)
                {
                    // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
                    int frontGoreType = Mod.Find<ModGore>("TerrifierTail_Front").Type;

                    var entitySource = NPC.GetSource_Death();

                    for (int i = 0; i < 1; i++)
                    {
                        Gore.NewGore(entitySource, NPC.position, NPC.velocity, frontGoreType);
                    }
                }
            }

            public override void Init()
            {
                TerrifierHead.CommonWormInit(this);
            }
        }
    }
}