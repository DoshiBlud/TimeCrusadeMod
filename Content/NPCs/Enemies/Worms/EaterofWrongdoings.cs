using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Biomes;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Items.Consumables;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Items.Placeables.Banners;
using TimeCrusadeMod.Content.Projectiles;

namespace TimeCrusadeMod.Content.NPCs.Enemies.Worms
{
    // These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
    internal class EaterofWrongdoingsHead : WormHead
    {
        public override int BodyType => ModContent.NPCType<EaterofWrongdoingsBody>();

        public override int TailType => ModContent.NPCType<EaterofWrongdoingsTail>();

        public override void SetStaticDefaults()
        {
            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                CustomTexturePath = "TimeCrusadeMod/Content/NPCs/Enemies/Worms/EaterofWrongdoings_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
                Position = new Vector2(40f, 24f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 12f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override void SetDefaults()
        {
            // Head is 10 defense, body 20, tail 30.
            NPC.CloneDefaults(NPCID.DiggerHead);
            NPC.npcSlots = 5f;
            NPC.aiStyle = NPCAIStyleID.Worm;
            NPC.width = 30;
            NPC.height = 60;
            NPC.lifeMax = 15000;
            NPC.defense = 120;
            NPC.damage = 90;

            Banner = Type;
            // These lines are only needed in the main body part.
            BannerItem = ModContent.ItemType<ConsumerofBiomesBanner>();
            ItemID.Sets.KillsToBanner[BannerItem] = 25; // Custom kill count required for banner drop and bestiary unlock. Omit this line for the default 50 kill count.
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RottenLens>(), 4, 1, 3));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedMoonlord)
            {
                return spawnInfo.Player.ZoneCorrupt ? 0.2f : 0;
            }
            else
            {
                return 0;
            }
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
                int frontGoreType = Mod.Find<ModGore>("EaterofWrongdoingsHead_Front").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, frontGoreType);
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
                new FlavorTextBestiaryInfoElement(this.GetLocalization("Bestiary").Value)
            });
        }

        public override void Init()
        {
            // Set the segment variance
            // If you want the segment length to be constant, set these two properties to the same value
            MinSegmentLength = 32;
            MaxSegmentLength = 34;

            CommonWormInit(this);
        }

        // This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
        internal static void CommonWormInit(Worm worm)
        {
            // These two properties handle the movement of the worm
            worm.MoveSpeed = 8f;
            worm.Acceleration = 0.15f;
        }

        private int attackCounter;
        private int attackCounters;
        private bool startDespawning;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
            writer.Write(attackCounters);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
            attackCounters = reader.ReadInt32();
        }
        // Use the default CheckActive behavior from ModNPC/Worm to handle despawning correctly.

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attackCounter > 0)
                {
                    attackCounter--; // tick down the attack counter.
                }

                Player target1 = Main.player[NPC.target];
                // If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
                if (attackCounter <= 0 && Vector2.Distance(NPC.Center, target1.Center) >= 80 && Collision.CanHit(NPC.Center, 1, 1, target1.Center, 1, 1))
                {
                    Vector2 direction = (target1.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                    direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

                    int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 10, ProjectileID.CursedFlameHostile, 5, 0, Main.myPlayer);
                    Main.projectile[projectile].timeLeft = 300;
                    Main.projectile[projectile].scale = 0.5f;
                    Main.projectile[projectile].damage = 90;
                    attackCounter = 70;
                    NPC.netUpdate = true;
                }
            }
            if (!NPC.HasValidTarget)
            {
                NPC.velocity.Y += 1f;

                MoveSpeed = 0.3f;

                if (!startDespawning)
                {
                    startDespawning = true;

                    // Despawn after 90 ticks (1.5 seconds) if the NPC gets far enough away
                    NPC.timeLeft = 10;
                }
            }
            float halfHealth = 0.5f;
            if((float)NPC.life < (float)NPC.lifeMax * halfHealth)
            {
                MoveSpeed = 11;
                Acceleration = 0.3f;

                if (attackCounters > 0)
                {
                    attackCounters--; // tick down the attack counter.
                }

                Player target1 = Main.player[NPC.target];
                // If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
                if (attackCounters <= 0 && Vector2.Distance(NPC.Center, target1.Center) >= 80 && Collision.CanHit(NPC.Center, 1, 1, target1.Center, 1, 1))
                {
                    Vector2 direction = (target1.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                    direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

                    int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 19, ProjectileID.CursedFlameHostile, 5, 0, Main.myPlayer);
                    Main.projectile[projectile].timeLeft = 300;
                    Main.projectile[projectile].scale = 0.5f;
                    Main.projectile[projectile].damage = 90;
                    attackCounters = 40;
                    NPC.netUpdate = true;

                    Vector2 directions = (target1.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                    direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

                    int projectiles = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, directions * 17, ProjectileID.CursedFlameHostile, 5, 0, Main.myPlayer);
                    Main.projectile[projectiles].timeLeft = 300;
                    Main.projectile[projectiles].scale = 0.8f;
                    Main.projectile[projectiles].damage = 90;
                    attackCounters = 45;
                    NPC.netUpdate = true;
                }
            }
        }
        }

    internal class EaterofWrongdoingsBody : WormBody
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<NeutralWormHead>();
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerBody);
            NPC.npcSlots = 5f;
            NPC.aiStyle = -1;
            NPC.width = 30;
            NPC.height = 60;
            NPC.lifeMax = 15000;
            NPC.defense = 100;
            NPC.damage = 90;

            // Extra body parts should use the same Banner value as the main ModNPC.
            Banner = ModContent.NPCType<NeutralWormHead>();
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
                int frontGoreType = Mod.Find<ModGore>("EaterofWrongdoingsBody_Front").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, frontGoreType);
                }
            }
        }

        public override void Init()
        {
            NeutralWormHead.CommonWormInit(this);
        }
    }

    internal class EaterofWrongdoingsTail : WormTail
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<NeutralWormHead>();
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerTail);
            NPC.npcSlots = 5f;
            NPC.aiStyle = -1;
            NPC.width = 30;
            NPC.height = 60;
            NPC.lifeMax = 15000;
            NPC.defense = 130;
            NPC.damage = 90;

            // Extra body parts should use the same Banner value as the main ModNPC.
            Banner = ModContent.NPCType<NeutralWormHead>();
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
                int frontGoreType = Mod.Find<ModGore>("EaterofWrongdoingsTail_Front").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, frontGoreType);
                }
            }
        }

        public override void Init()
        {
            NeutralWormHead.CommonWormInit(this);
        }
    }
}