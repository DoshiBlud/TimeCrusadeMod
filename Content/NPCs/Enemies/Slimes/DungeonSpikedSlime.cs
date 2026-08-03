using TimeCrusadeMod.Content.Items.Placeables.Banners;
using TimeCrusadeMod.Content.NPCs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Biomes;

namespace TimeCrusadeMod.Content.NPCs.Enemies.Slimes
{
    public class DungeonSpikedSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            Main.npcFrameCount[Type] = 2;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 32;
            NPC.damage = 40;
            NPC.defense = 3;
            NPC.lifeMax = 200;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 800f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = NPCAIStyleID.Slime;
            if (Main.expertMode)
            {
                NPC.damage = 45;
                NPC.lifeMax = 215;
                NPC.value = 1000f;
            }
            if (Main.masterMode)
            {
                NPC.damage = 50;
                NPC.lifeMax = 230;
                NPC.value = 1100f;
            }
            AIType = NPCID.BlueSlime;
            Banner = Type;
            // These lines are only needed in the main body part.
            BannerItem = ModContent.ItemType<DungeonSpikedSlimeBanner>();
            ItemID.Sets.KillsToBanner[BannerItem] = 20; // Custom kill count required for banner drop and bestiary unlock. Omit this line for the default 50 kill count.
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
            if (attackCounter > 0)
                {
                    attackCounter--; // tick down the attack counter.
                }

                Player targe = Main.player[NPC.target];
                // If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
                if (attackCounter <= 0 && Vector2.Distance(NPC.Center, targe.Center) <= 1000 && Collision.CanHit(NPC.Center, 1, 1, targe.Center, 1, 1))
                {
                for (int g = 0; g < 3; g++)
                {
                    Vector2 direction = (targe.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                    direction = direction.RotatedByRandom(MathHelper.ToRadians(55));

                    int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 12, ModContent.ProjectileType<Spike>(), 5, 0, Main.myPlayer);
                    Main.projectile[projectile].timeLeft = 350;
                    Main.projectile[projectile].damage = 20;
                    attackCounter = 80;
                    NPC.netUpdate = true;
                }
                }
            int timer = 60;
            if (timer > 0)
            {
                timer--;
            }

            if (timer <= 0)
            {
                NPC.velocity.Y = -13;
                if (player.dead)
                {
                    NPC.velocity.Y = -25;
                }
                if (NPC.Center.X < player.Center.X)
                {
                    NPC.velocity.X = 9;
                }
                else if (NPC.Center.X > player.Center.X)
                {
                    NPC.velocity.X = -9;
                }
                timer = 130;
                if (Main.expertMode)
                {
                    if (NPC.Center.X < player.Center.X)
                    {
                        NPC.velocity.X = 10;
                    }
                    else if (NPC.Center.X > player.Center.X)
                    {
                        NPC.velocity.X = -10;
                    }
                    timer = 105;
                }
                if (Main.masterMode)
                {
                    if (NPC.Center.X < player.Center.X)
                    {
                        NPC.velocity.X = 12;
                    }
                    else if (NPC.Center.X > player.Center.X)
                    {
                        NPC.velocity.X = -12;
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

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(NPC.downedBoss3)
            {
                return spawnInfo.Player.ZoneDungeon ? 0.4f : 0;
            }
            else
            {
                return 0;
            }
        }
        public override void OnKill()
        {
            for (int i = 0; i < 10; i++)
            {
                int dustType = DustID.t_Slime;
                var dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, dustType, NPC.velocity.X * 0.2f, NPC.velocity.Y * 0.2f, 100, Color.DarkSlateGray);

                dust.noGravity = true;
                dust.scale *= 1.75f;
                dust.velocity.X *= 2f;
                dust.velocity.Y *= 2f;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement(this.GetLocalization("Bestiary").Value)
            });
        }
        public override void FindFrame(int frameHeight)
        {
            int startFrame = 0;
            int endFrame = 1;

            int frameSpeed = 5;

            NPC.frameCounter += 0.4f;

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
