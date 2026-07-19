using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Steamworks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TimeCrusadeMod.Common.Systems;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Items.Consumables;
using TimeCrusadeMod.Content.Items.Placeables.Furniture;
using TimeCrusadeMod.Content.Items.Weapons;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.NPCs.Axy;
using TimeCrusadeMod.Content.Tiles;
using TimeCrusadeMod.Content.Items.Placeables;
using TimeCrusadeMod.Content.Items.Accessories;

namespace TimeCrusadeMod.Content.NPCs.Axy
{
    [AutoloadBossHead]
    public class Axy : ModNPC
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
        public int MinionMaxHealthTotal
        {
            get => (int)NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        public int MinionHealthTotal { get; set; }
        // This property uses NPC.localAI[] instead which doesn't get synced, but because SpawnedMinions is only used on spawn as a flag, this will get set by all parties to true.
        // Knowing what side (client, server, all) is in charge of a variable is important as NPC.ai[] only has four entries, so choose wisely which things you need synced and not synced
        public bool SpawnedMinions
        {
            get => NPC.localAI[0] == 1f;
            set => NPC.localAI[0] = value ? 1f : 0f;
        }
        public static int MinionType()
        {
            return ModContent.NPCType<AxyHelper>();
        }
        // Helper method to determine the amount of minions summoned
        public static int MinionCount()
        {
            int count = 8;

            if (Main.expertMode)
            {
                count += 10; // Increase by 5 if expert or master mode
            }

            if (Main.getGoodWorld)
            {
                count += 100; // Increase by 5 if using the "For The Worthy" seed
            }

            return count;
        }
        private bool secondPhase => state == 1;
        public override void SetStaticDefaults()
        {

            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ironskin] = true;

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                CustomTexturePath = "TimeCrusadeMod/Content/NPCs/Axy/Axy_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
                Position = new Vector2(40f, 24f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 0f
            };
        }
        public override void SetDefaults()
        {
            NPC.width = 110;
            NPC.height = 218;
            NPC.damage = 95;
            NPC.defense = 120;
            NPC.lifeMax = 200000;
            NPC.knockBackResist = 0f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.SpawnWithHigherTime(3);
            NPC.boss = true;
            NPC.npcSlots = 10f;
            NPC.aiStyle = -1;


            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/AxyBossTheme");

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
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<AxyBag>()));

            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<AxyRelic>()));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeables.AxitrentonCrystal>(), 1, 25, 30));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AxyShield>(), 8, 1, 1));
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange([
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A large crystal with alot of power and knowledge. It looks to seem it came from space. It's intent is to test your ability to fight.")
            ]);
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
                int backGoreType = Mod.Find<ModGore>("Axy_Back").Type;
                int frontGoreType = Mod.Find<ModGore>("Axy_Front").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
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
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedAxy, -1);
        }
        public override void AI()
        {
            Visuals();
            Player player = Main.player[NPC.target];

            if (NPC.target == 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            int attackCounter = 0;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attackCounter > 0)
                {
                    attackCounter--; // tick down the attack counter.
                }

                Player targe = Main.player[NPC.target];
                // If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
                if (attackCounter <= 1 && Vector2.Distance(NPC.Center, targe.Center) > 500 && Collision.CanHit(NPC.Center, 1, 1, targe.Center, 1, 1))
                {
                    float rotation = MathHelper.ToRadians(360); // Total spread angle in degrees
                    float numberProjectiles = 1;
                        Vector2 direction = (targe.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                        direction = direction.RotatedByRandom(MathHelper.ToRadians(360));


                        Vector2 perturbedSpeed = NPC.velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, -rotation / (numberProjectiles - 1)));

                        int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 10, ModContent.ProjectileType<AxyCrystal>(), 5, 0, Main.myPlayer);
                    Main.projectile[projectile].timeLeft = 50000;
                    Main.projectile[projectile].damage = 75;
                        attackCounter = 600;
                        NPC.netUpdate = true;
                }
                if (player.dead || !player.active)
                {
                    NPC.velocity.Y -= 0.1f;
                    NPC.EncourageDespawn(10000);
                    return;
                }
                switch (state)
                {
                    case 0:
                        HandleFirstState(player);
                        SpawnMinions();
                        break;
                    case 1:
                        HandleSecondState(player);
                        break;
                }
            }
            Lighting.AddLight(NPC.Center, 1.92f, 1.65f, 1.92f);

        }
        private void SpawnMinions()
        {
            if (SpawnedMinions)
            {
                // No point executing the code in this method again
                return;
            }

            SpawnedMinions = true;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Because we want to spawn minions, and minions are NPCs, we have to do this on the server (or singleplayer, "!= NetmodeID.MultiplayerClient" covers both)
                // This means we also have to sync it after we spawned and set up the minion
                return;
            }

            int count = MinionCount();
            var entitySource = NPC.GetSource_FromAI();

            MinionMaxHealthTotal = 50000;
            for (int i = 0; i < count; i++)
            {
                NPC minionNPC = NPC.NewNPCDirect(entitySource, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<AxyHelper>(), NPC.whoAmI);
                if (minionNPC.whoAmI == Main.maxNPCs)
                    continue; // spawn failed due to spawn cap

                // Now that the minion is spawned, we need to prepare it with data that is necessary for it to work
                // This is not required usually if you simply spawn NPCs, but because the minion is tied to the body, we need to pass this information to it
                AxyHelper minion = (AxyHelper)minionNPC.ModNPC;
                minion.ParentIndex = NPC.whoAmI; // Let the minion know who the "parent" is
                minion.PositionOffset = i / (float)count; // Give it a separate position offset

                MinionMaxHealthTotal += minionNPC.lifeMax; // add the total minion life for boss bar shield text

                // Finally, syncing, only sync on server and if the NPC actually exists (Main.maxNPCs is the index of a dummy NPC, there is no point syncing it)
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, number: minionNPC.whoAmI);
                }
            }

            // sync MinionMaxHealthTotal
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
            }
        }

        private void HandleFirstState(Player player)
        {
            if (subState == 0)
            {
                float baseMoveSpeed = 4f;
                float accelerationSpeed = 0.5f;

                if (Main.expertMode)
                {
                    NPC.damage = 105;
                    baseMoveSpeed = 6f;
                    accelerationSpeed = 1.5f;
                }
                if (Main.masterMode)
                {
                    NPC.damage = 115;
                    baseMoveSpeed = 8f;
                    accelerationSpeed = 2f;
                }
                MoveToTarget(player, baseMoveSpeed, accelerationSpeed, out float distanceToPlayer);

                stateTimer += 1f;

                float threshold = 120f;
                if (Main.expertMode)
                {
                    threshold *= 1f;
                }

                if (stateTimer >= threshold)
                {
                    subState = 1;
                    stateTimer = 0;
                    stateTimer2 = 0;
                    NPC.netUpdate = true;
                    return;
                }
            }

            else if (subState == 1)
            {
                float baseSpeed = 4f;
                if (Main.expertMode)
                {
                    baseSpeed = 6f;
                }

                float deltaX = (player.Center.X - NPC.Center.X);
                float deltaY = (player.Center.Y - NPC.Center.Y);

                float distanceToPlayer = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                float movementSpeed = baseSpeed / distanceToPlayer;
                Vector2 velocity = new Vector2(deltaX, deltaY) * movementSpeed;

                NPC.velocity = velocity;

                subState = 2;

                NPC.netUpdate = true;
                if (NPC.netSpam > 10)
                {
                    NPC.netSpam = 10;
                }
            }

            else if (subState == 2)
            {
                stateTimer += 1f;

                if (stateTimer >= 48f)
                {
                    NPC.velocity *= 1f;

                    if (Main.expertMode)
                    {
                        NPC.velocity *= 1.985f;
                    }

                    if (Math.Abs(NPC.velocity.X) < 0.05) NPC.velocity.X = 0f;
                    if (Math.Abs(NPC.velocity.Y) < 0.05) NPC.velocity.Y = 0f;
                }
                int threshold = 8;
                if (Main.expertMode)
                {
                    threshold = 5;
                }
                if (Main.masterMode)
                {
                    threshold = 3;
                }

                if (stateTimer >= threshold)
                {
                    stateTimer2 += 1f;

                    NPC.target = 255;

                    if (stateTimer2 >= 2f)
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
            if ((float)NPC.life < (float)NPC.lifeMax * lowHealthThreshold)
            {
                state = 1;
                subState = 0;
                stateTimer = 0f;
                stateTimer2 = 0f;

                NPC.netUpdate = true;
                if (NPC.netSpam > 10)
                {
                    NPC.netSpam = 10;
                }
            }
        }
        private void HandleSecondState(Player player)
        {
            if (subState == 0)
            {
                float baseMoveSpeed = 8f;
                float accelerationSpeed = 3f;

                if (Main.expertMode)
                {
                    NPC.damage = 105;
                    baseMoveSpeed = 10f;
                    accelerationSpeed = 5f;
                }
                if (Main.masterMode)
                {
                    NPC.damage = 115;
                    baseMoveSpeed = 14f;
                    accelerationSpeed = 8f;
                }
                MoveToTarget(player, baseMoveSpeed, accelerationSpeed, out float distanceToPlayer);

                stateTimer += 1f;

                float threshold = 10f;
                if (Main.expertMode)
                {
                    threshold *= 5f;
                }

                if (stateTimer >= threshold)
                {
                    subState = 1;
                    stateTimer = 0;
                    stateTimer2 = 0;
                    NPC.netUpdate = true;
                    return;
                }
            }

            else if (subState == 1)
            {
                float baseSpeed = 10f;
                if (Main.expertMode)
                {
                    baseSpeed = 14f;
                }

                float deltaX = (player.Center.X - NPC.Center.X);
                float deltaY = (player.Center.Y - NPC.Center.Y);

                float distanceToPlayer = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                float movementSpeed = baseSpeed / distanceToPlayer;
                Vector2 velocity = new Vector2(deltaX, deltaY) * movementSpeed;

                NPC.velocity = velocity;

                subState = 2;

                NPC.netUpdate = true;
                if (NPC.netSpam > 10)
                {
                    NPC.netSpam = 10;
                }
            }

            else if (subState == 2)
            {
                stateTimer += 1f;

                if (stateTimer >= 40f)
                {
                    NPC.velocity *= 0.98f;

                    if (Main.expertMode)
                    {
                        NPC.velocity *= 2f;
                    }
                    if (Main.masterMode)
                    {
                        NPC.velocity *= 2.5f;
                    }

                    if (Math.Abs(NPC.velocity.X) < 0.5) NPC.velocity.X = 0f;
                    if (Math.Abs(NPC.velocity.Y) < 0.5) NPC.velocity.Y = 0f;
                }
                int threshold = 5;
                if (Main.expertMode)
                {
                    threshold = 3;
                }
                if (Main.masterMode)
                {
                    threshold = 1;
                }

                if (stateTimer >= threshold)
                {
                    stateTimer2 += 1f;

                    NPC.target = 255;

                    if (stateTimer2 >= 2f)
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
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CrystalAxyDust>(), 0f, 0f, 150, default(Color), 1.5f);
            }
        }

        private void MoveToTarget(Player player, float moveSpeed, float accelerationRate, out float distanceToPlayer)
        {
            distanceToPlayer = Vector2.Distance(NPC.Center, player.Center);
            float movementSpeed = moveSpeed * distanceToPlayer;

            float targetVelocityX = (player.Center.X - NPC.Center.X) * movementSpeed;
            float targetVelocityY = (player.Center.Y - NPC.Center.Y) * movementSpeed;

            if (NPC.velocity.X < targetVelocityX)
            {
                NPC.velocity.X += accelerationRate;
                if (NPC.velocity.X < 1f && targetVelocityX > 2f)
                {
                    NPC.velocity.X += accelerationRate;
                }
            }
            if (NPC.velocity.X < targetVelocityX)
            {
                NPC.velocity.X -= accelerationRate;
                if (NPC.velocity.X > 1f && targetVelocityX < 2f)
                {
                    NPC.velocity.X -= accelerationRate;
                }
            }
            if (NPC.velocity.Y < targetVelocityY)
            {
                NPC.velocity.Y += accelerationRate;
                if (NPC.velocity.Y < 1f && targetVelocityY > 2f)
                {
                    NPC.velocity.Y += accelerationRate;
                }
            }
            if (NPC.velocity.Y < targetVelocityY)
            {
                NPC.velocity.Y -= accelerationRate;
                if (NPC.velocity.Y > 1f && targetVelocityY < 2f)
                {
                    NPC.velocity.Y -= accelerationRate;
                }
            }
        }
        private void Visuals()
        {
            // So it will lean slightly towards the direction it's moving
            NPC.rotation = NPC.velocity.X * 0.03f;
        }
    }
}