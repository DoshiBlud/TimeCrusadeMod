using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Threading;
using System.Timers;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Common.Systems;
using TimeCrusadeMod.Content.Biomes;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Items.Placeables.Banners;
using TimeCrusadeMod.Content.NPCs;
using TimeCrusadeMod.Content.Projectiles;

namespace TimeCrusadeMod.Content.NPCs.JungleLordSlime
{
    [AutoloadBossHead]
    public class JungleLordSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            Main.npcFrameCount[Type] = 5;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 5f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 42;
            NPC.damage = 35;
            NPC.boss = true;
            NPC.defense = 3;
            NPC.lifeMax = 11000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 500f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = -1;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            NPC.scale = 1.155555555f;
            if (Main.expertMode)
            {
                NPC.damage = 40;
                NPC.lifeMax = 12000;
                NPC.value = 1400f;
            }
            if (Main.masterMode)
            {
                NPC.damage = 50;
                NPC.lifeMax = 13000;
                NPC.value = 1400f;
            }

            AIType = -1;
            AnimationType = NPCID.BlueSlime;
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
        public override void AI()
        {
            Player player = Main.player[NPC.target];

            if (NPC.target == 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
                if (player.dead || !player.active)
                {
                    NPC.EncourageDespawn(100);
                    NPC.velocity.Y = -2;
                    return;
                }
            else
            {
                Battle(player);
            }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (attackCounter > 0)
                    {
                        attackCounter--; // tick down the attack counter.
                    }

                    Player targe = Main.player[NPC.target];
                    // If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
                    if (attackCounter <= 0 && Vector2.Distance(NPC.Center, targe.Center) <= 1000 && Collision.CanHit(NPC.Center, 1, 1, targe.Center, 1, 1))
                    {
                        float rotation = MathHelper.ToRadians(360); // Total spread angle in degrees
                        float numberProjectiles = 1;
                        Vector2 direction = (targe.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                        direction = direction.RotatedBy(MathHelper.ToRadians(0));


                        Vector2 perturbedSpeed = NPC.velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, -rotation / (numberProjectiles - 1)));

                        int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 12, ModContent.ProjectileType<JungleSlimeBlob>(), 5, 0, Main.myPlayer);
                        Main.projectile[projectile].timeLeft = 350;
                        Main.projectile[projectile].damage = 20;
                        attackCounter = 60;
                        NPC.netUpdate = true;
                    }
                }
            if (timer > 0)
            {
                timer--;
            }

            if (timer <= 0)
            {
                NPC.velocity.Y = -10;
                if (player.dead)
                {
                    NPC.velocity.Y = -25;
                }
                timer = 120;
                if (NPC.Center.X < player.Center.X)
                {
                    NPC.velocity.X = 6;
                }
                else if (NPC.Center.X > player.Center.X)
                {
                    NPC.velocity.X = -6;
                }
                if (Main.expertMode)
                {
                    if (NPC.Center.X < player.Center.X)
                    {
                        NPC.velocity.X = 4;
                    }
                    else if (NPC.Center.X > player.Center.X)
                    {
                        NPC.velocity.X = -4;
                    }
                    timer = 105;
                }
                if (Main.masterMode)
                {
                    if (NPC.Center.X < player.Center.X)
                    {
                        NPC.velocity.X = 5;
                    }
                    else if (NPC.Center.X > player.Center.X)
                    {
                        NPC.velocity.X = -5;
                    }
                    timer = 90;
                }
                NPC.netUpdate = true;
            }
            if (NPC.velocity.Y == 0)
            {
                NPC.velocity.X /= 1.1f;
            }
        }
        public bool bossBattle;
        public bool shoot;
        public bool jump2;
        int timer = 60;
        private void Battle(Player player)
        {

        }
        public override void OnKill()
        {
            for (int i = 0; i < 10; i++)
            {
                int dustType = DustID.t_Slime;
                var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, dustType, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, Color.Green);

                dust.noGravity = true;
                dust.scale *= 1.75f;
                dust.velocity.X *= 2f;
                dust.velocity.Y *= 2f;
            }
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedJungleLordSlime, -1);
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
                int backGoreType = Mod.Find<ModGore>("JungleCrown_Back").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
            new FlavorTextBestiaryInfoElement(this.GetLocalization("Bestiary").Value)
        });
        }
        public override void FindFrame(int frameHeight)
        {
            int startFrame = 0;
            int endFrame = 4;

            int frameSpeed = 26;

            NPC.frameCounter += 0f;

            if (NPC.frameCounter >= frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y > endFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }
        }
    }
}
