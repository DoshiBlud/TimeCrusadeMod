using TimeCrusadeMod.Content.Items.Consumables;
using TimeCrusadeMod.Common.Systems;
using TimeCrusadeMod.Content.Tiles;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Items.Placeables.Furniture;
using TimeCrusadeMod.Content.Items.Weapons.Magic;
using TimeCrusadeMod.Content.Items.Weapons.Melee;

namespace TimeCrusadeMod.Content.NPCs.Asphalia
{
    [AutoloadBossHead]
    public class Asphalia : ModNPC
    {
        private int state
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        private int subState
        {
            get => (int)NPC.ai[1];
            set => NPC.ai[1] = value;
        }
        private float stateTimer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        private float stateTimer2
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }
        private bool secondPhase => state == 1;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;

            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                CustomTexturePath = "TimeCrusadeMod/Content/NPCs/Asphalia/Asphalia_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
                Position = new Vector2(40f, 24f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 0f
            };
        }
        public override void SetDefaults()
        {
            NPC.width = 100;
            NPC.height = 100;
            NPC.damage = 60;
            NPC.defense = 9;
            NPC.lifeMax = 7500;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.SpawnWithHigherTime(3);
            NPC.boss = true;
            NPC.npcSlots = 10f;
            NPC.aiStyle = -1;
            NPC.rotation = 0f;


            if (!Main.dedServ) {
				Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/AsphaliaBossTheme");

				// If you would like to play alternate music when the otherworld soundtrack enabled, use this logic.
        }
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<AsphaliaBag>()));
            
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StonedSlicer>(), 3));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<RockyStaff>(), 5));
            
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<AsphaliaRelic>()));
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange([
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A giant rock that came from a piece of the ground randomly. Its intent is to squish your body until its mush.")
            ]);
        }

        public override void HitEffect(NPC.HitInfo hit) {
			// If the NPC dies, spawn gore and play a sound
			if (Main.netMode == NetmodeID.Server) {
				// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
				return;
			}

			if (NPC.life <= 0) {
				// These gores work by simply existing as a texture inside any folder which path contains "Gores/"
				int backGoreType = Mod.Find<ModGore>("Asphalia_Back").Type;
                int frontGoreType = Mod.Find<ModGore>("Asphalia_Front").Type;

				var entitySource = NPC.GetSource_Death();

				for (int i = 0; i < 1; i++) {
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
				}

				SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

				// This adds a screen shake (screenshake) similar to Deerclops
				PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 0.5f)).ToRotationVector2(), 20f, 6f, 20, 100f, FullName);
				Main.instance.CameraModifiers.Add(modifier);
			}
		}

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedAsphalia, -1);
        }
        public override void AI()
        {
            Player player = Main.player[NPC.target];

             Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Stone);
             dust.noGravity = true;
             dust.velocity *= 0.3f;
             dust.scale = 2f;
        
            NPC.rotation += NPC.velocity.X * 0.05f;

            if(NPC.target == 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            if(player.dead || !player.active)
            {
                NPC.velocity.Y -= 0.04f;
                NPC.EncourageDespawn(10000);
                return;
            }
            switch(state)
            {
                case 0:
                HandleFirstState(player);
                break;
                case 1:
                HandleSecondState(player);
                break;
            }

        }

        private void HandleFirstState(Player player)
        {
            if(subState == 0)
            {
                float baseMoveSpeed = 5f;
                float accelerationSpeed = 0.04f;

                if(Main.expertMode)
                {
                    NPC.damage = 68;
                    baseMoveSpeed = 7f;
                    accelerationSpeed = 0.15f;
                }
                if(Main.masterMode)
                {
                    NPC.damage = 74;
                    baseMoveSpeed = 9f;
                    accelerationSpeed = 0.2f;
                }
                MoveToTarget(player, baseMoveSpeed, accelerationSpeed, out float distanceToPlayer);

                stateTimer += 1f;

                float threshold = 120f;
                if(Main.expertMode)
                {
                    threshold *= 1f;
                }

                if(stateTimer >= threshold)
                {
                    subState = 1;
                    stateTimer = 0;
                    stateTimer2 = 0;
                    NPC.netUpdate = true;
                    return;
                }
            }

            else if(subState ==1)
            {
                float baseSpeed = 6f;
                if(Main.expertMode)
                {
                    baseSpeed = 7f;
                }

                float deltaX = (player.Center.X - NPC.Center.X);
                float deltaY = (player.Center.Y - NPC.Center.Y);

                float distanceToPlayer = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                float movementSpeed = baseSpeed / distanceToPlayer;
                Vector2 velocity = new Vector2(deltaX, deltaY) * movementSpeed;

                NPC.velocity = velocity;

                subState = 2;

                NPC.netUpdate = true;
                if(NPC.netSpam > 10)
                {
                    NPC.netSpam = 10;
                }
            }

            else if(subState == 2)
            {
                stateTimer += 1f;

                if(stateTimer >= 48f)
                {
                    NPC.velocity *= 0.98f;

                    if(Main.expertMode)
                    {
                        NPC.velocity *= 0.985f;
                    }

                    if(Math.Abs(NPC.velocity.X) < 0.05) NPC.velocity.X = 0f;
                    if(Math.Abs(NPC.velocity.Y) < 0.05) NPC.velocity.Y = 0f;
                }
                int threshold = 25;
                if(Main.expertMode)
                {
                    threshold = 12;
                }
                if(Main.masterMode)
                {
                    threshold = 6;
                }

                if(stateTimer >= threshold)
                {
                    stateTimer2 += 1f;

                    NPC.target =255;

                    if(stateTimer2 >= 2f)
                    {
                        
                        subState = 0;
                        stateTimer2 = 0f;
                    }
                    else
                    {
                        subState = 1;
                    }
                }
            }

            float lowHealthThreshold = 0.50f;
        if((float)NPC.life < (float)NPC.lifeMax * lowHealthThreshold)
            {
                state = 1;
                subState = 0;
                stateTimer = 0f;
                stateTimer2 = 0f;

                NPC.netUpdate = true;
                if(NPC.netSpam > 10)
                {
                    NPC.netSpam = 10;
                }
            }
        }
        private void HandleSecondState(Player player)
        {
            if(subState == 0)
            {
                float baseMoveSpeed = 25f;
                float accelerationSpeed = 4f;

                if(Main.expertMode)
                {
                    NPC.damage = 81;
                    baseMoveSpeed = 35f;
                    accelerationSpeed = 5f;
                }
                if(Main.masterMode)
                {
                    NPC.damage = 87;
                    baseMoveSpeed = 45f;
                    accelerationSpeed = 6f;
                }
                MoveToTarget(player, baseMoveSpeed, accelerationSpeed, out float distanceToPlayer);

                stateTimer += 1f;

                float threshold = 0f;
                if(Main.expertMode)
                {
                    threshold *= 0f;
                }

                if(stateTimer >= threshold)
                {
                    subState = 1;
                    stateTimer = 0;
                    stateTimer2 = 0;
                    NPC.netUpdate = true;
                    return;
                }
            }

            else if(subState ==1)
            {
                float baseSpeed = 6f;
                if(Main.expertMode)
                {
                    baseSpeed = 7f;
                }

                float deltaX = (player.Center.X - NPC.Center.X);
                float deltaY = (player.Center.Y - NPC.Center.Y);

                float distanceToPlayer = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                float movementSpeed = baseSpeed / distanceToPlayer;
                Vector2 velocity = new Vector2(deltaX, deltaY) * movementSpeed;

                NPC.velocity = velocity;

                subState = 2;

                NPC.netUpdate = true;
                if(NPC.netSpam > 10)
                {
                    NPC.netSpam = 10;
                }
            }

            else if(subState == 2)
            {
                stateTimer += 1f;

                if(stateTimer >= 40f)
                {
                    NPC.velocity *= 0.98f;

                    if(Main.expertMode)
                    {
                        NPC.velocity *= 1f;
                    }
                    if(Main.masterMode)
                    {
                        NPC.velocity *= 1.995f;
                    }

                    if(Math.Abs(NPC.velocity.X) < 0.5) NPC.velocity.X = 0f;
                    if(Math.Abs(NPC.velocity.Y) < 0.5) NPC.velocity.Y = 0f;
                }
                int threshold = 0;
                if(Main.expertMode)
                {
                    threshold = 0;
                }
                if(Main.masterMode)
                {
                    threshold = 0;
                }

                if(stateTimer >= threshold)
                {
                    stateTimer2 += 1f;

                    NPC.target =255;

                    if(stateTimer2 >= 2f)
                    {
                        
                        subState = 0;
                        stateTimer2 = 0f;
                    }
                    else
                    {
                        subState = 1;
                    }
                }
            }
        }
        private void MoveToTarget(Player player, float moveSpeed, float accelerationRate, out float distanceToPlayer)
        {
            distanceToPlayer = Vector2.Distance(NPC.Center, player.Center);
            float movementSpeed = moveSpeed / distanceToPlayer;

            float targetVelocityX = (player.Center.X - NPC.Center.X) * movementSpeed;
            float targetVelocityY = (player.Center.Y - NPC.Center.Y) * movementSpeed;

            if(NPC.velocity.X < targetVelocityX)
            {
                NPC.velocity.X += accelerationRate;
                if(NPC.velocity.X < 1f && targetVelocityX > 2f)
                {
                    NPC.velocity.X += accelerationRate;
                }
            }
            if(NPC.velocity.X < targetVelocityX)
            {
                NPC.velocity.X -= accelerationRate;
                if(NPC.velocity.X > 1f && targetVelocityX < 2f)
                {
                    NPC.velocity.X -= accelerationRate;
                }
            }
             if(NPC.velocity.Y < targetVelocityY)
            {
                NPC.velocity.Y += accelerationRate;
                if(NPC.velocity.Y < 1f && targetVelocityY > 2f)
                {
                    NPC.velocity.Y += accelerationRate;
                }
            }
            if(NPC.velocity.Y < targetVelocityY)
            {
                NPC.velocity.Y -= accelerationRate;
                if(NPC.velocity.Y > 1f && targetVelocityY < 2f)
                {
                    NPC.velocity.Y -= accelerationRate;
                }
            }
        }  
        public override void FindFrame(int frameHeight)
        {
            int startFrame = 0;
            int endFrame = 0;

            if(secondPhase)
            {
                startFrame = 1;
                endFrame = 1;

                if(NPC.frame.Y < startFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }

            int frameSpeed = 0;

            NPC.frameCounter += 0f;
            NPC.frameCounter += NPC.velocity.Length() / 100f;

            if(NPC.frameCounter >= frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if(NPC.frame.Y > endFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }
        }
    } 
}