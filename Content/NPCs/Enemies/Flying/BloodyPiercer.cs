using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Biomes;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Items.Placeables.Banners;
using TimeCrusadeMod.Content.NPCs;
using TimeCrusadeMod.Content.Projectiles;

namespace TimeCrusadeMod.Content.NPCs.Enemies.Flying
{
    public class BloodyPiercer : ModNPC
    {
        public int Act = 1;
        public int timer = 0;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            Main.npcFrameCount[Type] = 4;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 64;
            NPC.height = 50;
            NPC.damage = 14;
            NPC.defense = 5;
            NPC.lifeMax = 85;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = NPCAIStyleID.FlyingFish;
            NPC.value = 500f;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            if (Main.expertMode)
            {
                NPC.damage = 24;
                NPC.lifeMax = 95;
                NPC.value = 600f;
            }
            if (Main.masterMode)
            {
                NPC.damage = 26;
                NPC.lifeMax = 100;
                NPC.value = 800f;
            }
            Banner = Type;
            // These lines are only needed in the main body part.
            BannerItem = ModContent.ItemType<BloodyPiercerBanner>();
            ItemID.Sets.KillsToBanner[BannerItem] = 50; // Custom kill count required for banner drop and bestiary unlock. Omit this line for the default 50 kill count.
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
                NPC.TargetClosest(true);
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
                float rotation = MathHelper.ToRadians(360); // Total spread angle in degrees
                float numberProjectiles = 1;
                Vector2 direction = (targe.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                direction = direction.RotatedBy(MathHelper.ToRadians(0));


                Vector2 perturbedSpeed = NPC.velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, -rotation / (numberProjectiles - 1)));

                int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 12, ModContent.ProjectileType<BloodyPiercerStinger>(), 5, 0, Main.myPlayer);
                Main.projectile[projectile].timeLeft = 350;
                Main.projectile[projectile].damage = 20;
                attackCounter = 78;
                NPC.netUpdate = true;
            }
            if (NPC.Center.X < player.Center.X)
            {
                NPC.direction = -1;
            }
            if (NPC.Center.X > player.Center.X)
            {
                NPC.direction = 1;
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
                int frontGoreType = Mod.Find<ModGore>("BloodyPiercer_Front").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneCrimson ? 0.06f : 0;
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
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
                new FlavorTextBestiaryInfoElement(this.GetLocalization("Bestiary").Value)
            });
        }
        public override void FindFrame(int frameHeight)
        {
            int startFrame = 0;
            int endFrame = 3;

            int frameSpeed = 2;

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
        private void MoveToTarget(Player player, float speed, float speedUp, out float distance)
        {
            distance = Vector2.Distance(NPC.Center, player.Center);
            float movementSpeed = speed * distance;

            float targetVelocityX = (player.Center.X - NPC.Center.X) * movementSpeed;
            float targetVelocityY = (player.Center.Y - NPC.Center.Y) * movementSpeed;

            if (NPC.velocity.X < targetVelocityX)
            {
                NPC.velocity.X += speedUp;
                if (NPC.velocity.X < 1f && targetVelocityX > 2f)
                {
                    NPC.velocity.X += speedUp;
                }
            }
            if (NPC.velocity.X < targetVelocityX)
            {
                NPC.velocity.X -= speedUp;
                if (NPC.velocity.X > 1f && targetVelocityX < 2f)
                {
                    NPC.velocity.X -= speedUp;
                }
            }
            if (NPC.velocity.Y < targetVelocityY)
            {
                NPC.velocity.Y += speedUp;
                if (NPC.velocity.Y < 1f
                    && targetVelocityY > 2f)
                {
                    NPC.velocity.Y += speedUp;
                }
            }
            if (NPC.velocity.Y < targetVelocityY)
            {
                NPC.velocity.Y -= speedUp;
                if (NPC.velocity.Y > 1f && targetVelocityY < 2f)
                {
                    NPC.velocity.Y -= speedUp;
                }
            }
        }
    }
}
