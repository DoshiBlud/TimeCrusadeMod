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

namespace TimeCrusadeMod.Content.NPCs.Terrifier
{
    [AutoloadBossHead]
    // These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
    internal class TerrifierHead : WormHead
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

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Slow] = true;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerHead);
            NPC.aiStyle = -1;
            NPC.boss = true;
            NPC.width = 52;
            NPC.height = 110;
            NPC.damage = 150;
            NPC.lifeMax = 185000;
            NPC.defense = 160;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.npcSlots = 10f;
            NPC.realLife = NPC.whoAmI;
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/LullabiesofDread");

                // If you would like to play alternate music when the otherworld soundtrack enabled, use this logic.
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color glowMaskColor = Color.White;
            Texture2D glowMaskTexture = glowTexture.Value;

            SpriteBatch mySpriteBatch = spriteBatch;
            NPC rCurrentNPC = NPC;

            SpriteEffects spriteEffects = rCurrentNPC.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.FlipHorizontally;

            int type = NPC.type;

            Color npcColor = drawColor;
            npcColor = rCurrentNPC.GetNPCColorTintedByBuffs(npcColor);

            float num36 = Main.NPCAddHeight(rCurrentNPC);

            Vector2 halfSize = new Vector2(TextureAssets.Npc[type].Width() / 2, TextureAssets.Npc[type].Height() / Main.npcFrameCount[type] / 2);

            mySpriteBatch.Draw(glowMaskTexture,
                               rCurrentNPC.Bottom - screenPos + new Vector2((float)(-TextureAssets.Npc[type].Width()) * rCurrentNPC.scale / 2f + halfSize.X * rCurrentNPC.scale, (float)(-TextureAssets.Npc[type].Height()) * rCurrentNPC.scale / (float)Main.npcFrameCount[type] + 4f + halfSize.Y * rCurrentNPC.scale + num36 + rCurrentNPC.gfxOffY),
                               rCurrentNPC.frame,
                               glowMaskColor,
                               rCurrentNPC.rotation,
                               halfSize,
                               rCurrentNPC.scale,
                               spriteEffects, 0f);
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
            MinSegmentLength = 60;
            MaxSegmentLength = 60;

            CommonWormInit(this);
        }

        // This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
        internal static void CommonWormInit(Worm worm)
        {
            // These two properties handle the movement of the worm
            worm.MoveSpeed = 22f;
            worm.Acceleration = 2f;
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
                switch (state)
                {
                    case 0:
                        HandleFirstState(player);
                        break;
                    case 1:
                        HandleSecondState(player);
                        break;
                }

            }
        }

        private void HandleFirstState(Player player)
        {
            if (subState == 0)
            {
                float baseMoveSpeed = 22f;
                float accelerationSpeed = 2f;

                if (Main.expertMode)
                {
                    NPC.damage = 200;
                    baseMoveSpeed = 26f;
                    accelerationSpeed = 3f;
                }
                if (Main.masterMode)
                {
                    NPC.damage = 230;
                    baseMoveSpeed = 28f;
                    accelerationSpeed = 4f;
                }
                MoveToTarget(player, baseMoveSpeed, accelerationSpeed, out float distanceToPlayer);

                stateTimer += 1f;

                float threshold = 12f;
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
                float baseSpeed = 6f;
                if (Main.expertMode)
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
                    NPC.velocity *= 0.98f;

                    if (Main.expertMode)
                    {
                        NPC.velocity *= 0.985f;
                    }

                    if (Math.Abs(NPC.velocity.X) < 0.05) NPC.velocity.X = 0f;
                    if (Math.Abs(NPC.velocity.Y) < 0.05) NPC.velocity.Y = 0f;
                }
                int threshold = 12;
                if (Main.expertMode)
                {
                    threshold = 6;
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
                float baseMoveSpeed = 30f;
                float accelerationSpeed = 5f;

                if (Main.expertMode)
                {
                    NPC.damage = 300;
                    baseMoveSpeed = 32f;
                    accelerationSpeed = 6f;
                }
                if (Main.masterMode)
                {
                    NPC.damage = 325;
                    baseMoveSpeed = 36f;
                    accelerationSpeed = 7f;
                }
                MoveToTarget(player, baseMoveSpeed, accelerationSpeed, out float distanceToPlayer);

                stateTimer += 1f;

                float threshold = 0f;
                if (Main.expertMode)
                {
                    threshold *= 0f;
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
                float baseSpeed = 6f;
                if (Main.expertMode)
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
                    NPC.velocity *= 3f;

                    if (Main.expertMode)
                    {
                        NPC.velocity *= 4f;
                    }
                    if (Main.masterMode)
                    {
                        NPC.velocity *= 5f;
                    }

                    if (Math.Abs(NPC.velocity.X) < 0.5) NPC.velocity.X = 0f;
                    if (Math.Abs(NPC.velocity.Y) < 0.5) NPC.velocity.Y = 0f;
                }
                int threshold = 0;
                if (Main.expertMode)
                {
                    threshold = 0;
                }
                if (Main.masterMode)
                {
                    threshold = 0;
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
        }
        private void MoveToTarget(Player player, float moveSpeed, float accelerationRate, out float distanceToPlayer)
        {
            distanceToPlayer = Vector2.Distance(NPC.Center, player.Center);
            float movementSpeed = moveSpeed / distanceToPlayer;

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
            NPC.width = 52;
            NPC.height = 110;
            NPC.damage = 115;
            NPC.defense = 350;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.npcSlots = 100f;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color glowMaskColor = Color.White;
            Texture2D glowMaskTexture = glowTexture.Value;

            SpriteBatch mySpriteBatch = spriteBatch;
            NPC rCurrentNPC = NPC;

            SpriteEffects spriteEffects = rCurrentNPC.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.FlipHorizontally;

            int type = NPC.type;

            Color npcColor = drawColor;
            npcColor = rCurrentNPC.GetNPCColorTintedByBuffs(npcColor);

            float num36 = Main.NPCAddHeight(rCurrentNPC);

            Vector2 halfSize = new Vector2(TextureAssets.Npc[type].Width() / 2, TextureAssets.Npc[type].Height() / Main.npcFrameCount[type] / 2);

            mySpriteBatch.Draw(glowMaskTexture,
                               rCurrentNPC.Bottom - screenPos + new Vector2((float)(-TextureAssets.Npc[type].Width()) * rCurrentNPC.scale / 2f + halfSize.X * rCurrentNPC.scale, (float)(-TextureAssets.Npc[type].Height()) * rCurrentNPC.scale / (float)Main.npcFrameCount[type] + 4f + halfSize.Y * rCurrentNPC.scale + num36 + rCurrentNPC.gfxOffY),
                               rCurrentNPC.frame,
                               glowMaskColor,
                               rCurrentNPC.rotation,
                               halfSize,
                               rCurrentNPC.scale,
                               spriteEffects, 0f);
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
            NPC.width = 52;
            NPC.height = 110;
            NPC.damage = 80;
            NPC.defense = 600;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.npcSlots = 100f;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color glowMaskColor = Color.White;
            Texture2D glowMaskTexture = glowTexture.Value;

            SpriteBatch mySpriteBatch = spriteBatch;
            NPC rCurrentNPC = NPC;

            SpriteEffects spriteEffects = rCurrentNPC.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.FlipHorizontally;

            int type = NPC.type;

            Color npcColor = drawColor;
            npcColor = rCurrentNPC.GetNPCColorTintedByBuffs(npcColor);

            float num36 = Main.NPCAddHeight(rCurrentNPC);

            Vector2 halfSize = new Vector2(TextureAssets.Npc[type].Width() / 2, TextureAssets.Npc[type].Height() / Main.npcFrameCount[type] / 2);

            mySpriteBatch.Draw(glowMaskTexture,
                               rCurrentNPC.Bottom - screenPos + new Vector2((float)(-TextureAssets.Npc[type].Width()) * rCurrentNPC.scale / 2f + halfSize.X * rCurrentNPC.scale, (float)(-TextureAssets.Npc[type].Height()) * rCurrentNPC.scale / (float)Main.npcFrameCount[type] + 4f + halfSize.Y * rCurrentNPC.scale + num36 + rCurrentNPC.gfxOffY),
                               rCurrentNPC.frame,
                               glowMaskColor,
                               rCurrentNPC.rotation,
                               halfSize,
                               rCurrentNPC.scale,
                               spriteEffects, 0f);
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