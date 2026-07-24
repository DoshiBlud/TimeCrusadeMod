using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
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
using TimeCrusadeMod.Content.Items.Consumables;
using TimeCrusadeMod.Content.NPCs.Terrifier;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.NPCs;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Items.Placeables.Furniture;
using TimeCrusadeMod.Content.BossBars;
using System.Threading;

namespace TimeCrusadeMod.Content.NPCs.Terrifier
{
    [AutoloadBossHead]
    // These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
    internal class TerrifierHead : WormHead
    {
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
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Slow] = true;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.boss = true;
            NPC.width = 76;
            NPC.height = 94;
            NPC.damage = 150;
            NPC.lifeMax = 185000;
            NPC.defense = 160;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.npcSlots = 10f;
            NPC.realLife = NPC.whoAmI;
            NPC.scale = 1f;
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
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
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
                Player player = Main.player[NPC.target];
                if (NPC.target == 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
                {
                    NPC.TargetClosest();
                }
                if (player.dead || !player.active)
                {
                    NPC.velocity.Y -= 0.04f;
                    NPC.EncourageDespawn(10000);
                    return;
                }
                else
                {
                    Battle(player);
                }
                if (attackCounter > 0)
                {
                    attackCounter--; // tick down the attack counter.
                }

                Player targe = Main.player[NPC.target];
                // If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
                if (attackCounter <= 4 && Vector2.Distance(NPC.Center, targe.Center) > 200 && Collision.CanHit(NPC.Center, 1, 1, targe.Center, 1, 1))
                {
                    Vector2 direction = (targe.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                    direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

                    int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 1, ModContent.ProjectileType<TerrifierPebble>(), 5, 0, Main.myPlayer);
                    Main.projectile[projectile].timeLeft = 500;
                    attackCounter = 70;
                    NPC.netUpdate = true;
                }
                float speed = 12;
                float speedUp = 0.5f;
                int timer = 60;
                if (timer > 0)
                {
                    timer--;
                }
                if (player.velocity.Y == 0 || player.velocity.X == 0)
                {
                    MoveToTarget(player, speed, speedUp, out float distance);
                }
            }
        }
        int firstAct = 1;
        int phase1 = 0;
        int phase2 = 2;
        float healthHalf = 0.5f;
        float healthMax = 100;
        private void Battle(Player player)
        {
            if (phase1 == 0)
            {
                if (firstAct == 1)
                {
                    if(player.velocity.X == 0)
                    {

                    }
                    else
                    {
                        NPC.velocity.X += MoveSpeed;
                    }
                }
            }
            else if(phase2 == 2)
            {

            }
        }
        private void MoveToTarget(Player player, float speed, float speedUp, out float distance)
        {
            distance = Vector2.Distance(NPC.Center, player.Center);
            float moveSpeed = speed + distance;
        }

        internal class TerrifierBody : WormBody
        {
            private int attackCounter;
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
                NPC.width = 44;
                NPC.height = 64;
                NPC.damage = 115;
                NPC.defense = 350;
                NPC.noGravity = true;
                NPC.noTileCollide = true;
                NPC.npcSlots = 100f;
                NPC.scale = 1.3f;
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
                    if (attackCounter > 0)
                    {
                        attackCounter--; // tick down the attack counter.
                    }

                    Player targe = Main.player[NPC.target];
                    // If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
                    if (attackCounter <= 1 && Vector2.Distance(NPC.Center, targe.Center) > 200 && Collision.CanHit(NPC.Center, 1, 1, targe.Center, 1, 1))
                    {
                        Vector2 direction = (targe.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                        direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

                        int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 1, ModContent.ProjectileType<TerrifierPebble>(), 5, 0, Main.myPlayer);
                        Main.projectile[projectile].timeLeft = 500;
                        attackCounter = 120;
                        NPC.netUpdate = true;
                    }
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

                    for (int i = 0; i < 2; i++)
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
                NPC.width = 44;
                NPC.height = 80;
                NPC.damage = 80;
                NPC.defense = 600;
                NPC.noGravity = true;
                NPC.noTileCollide = true;
                NPC.npcSlots = 100f;
                NPC.scale = 1.3f;
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