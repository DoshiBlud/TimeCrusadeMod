using TimeCrusadeMod.Content.Items.Placeables.Banners;
using TimeCrusadeMod.NPCs;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Projectiles;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Biomes;

namespace TimeCrusadeMod.Content.NPCs
{
	// These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
	internal class NeutralWormHead : WormHead
	{
		public override int BodyType => ModContent.NPCType<NeutralWormBody>();

		public override int TailType => ModContent.NPCType<NeutralWormTail>();

		public override void SetStaticDefaults() {
			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers() { // Influences how the NPC looks in the Bestiary
				CustomTexturePath = "TimeCrusadeMod/Content/NPCs/NeutralWorm_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
				Position = new Vector2(40f, 24f),
				PortraitPositionXOverride = 0f,
				PortraitPositionYOverride = 12f
            };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
		}

		public override void SetDefaults() {
			// Head is 10 defense, body 20, tail 30.
			NPC.CloneDefaults(NPCID.DiggerHead);
			NPC.npcSlots = 10f;
			NPC.aiStyle = -1;
            SpawnModBiomes = [ModContent.GetInstance<NeutralUndergroundBiome>().Type];

            Banner = Type;
			// These lines are only needed in the main body part.
			BannerItem = ModContent.ItemType<NeutralWormBanner>();
			ItemID.Sets.KillsToBanner[BannerItem] = 25; // Custom kill count required for banner drop and bestiary unlock. Omit this line for the default 50 kill count.
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            // Can only spawn in the ExampleSurfaceBiome and if there are no other ExampleZombieThiefs
            if (spawnInfo.Player.InModBiome(ModContent.GetInstance<NeutralUndergroundBiome>()) && !NPC.AnyNPCs(Type)) {
    			return 1f; // Spawn chance, 1f means 100% chance to spawn
            }

            return 0f;
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
                int frontGoreType = Mod.Find<ModGore>("NeutralWormHead_Front").Type;

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
                new FlavorTextBestiaryInfoElement(this.GetLocalization("Bestiary").Value)
            });
        }

        public override void Init() {
			// Set the segment variance
			// If you want the segment length to be constant, set these two properties to the same value
			MinSegmentLength = 12;
			MaxSegmentLength = 12;

			CommonWormInit(this);
		}

		// This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
		internal static void CommonWormInit(Worm worm) {
			// These two properties handle the movement of the worm
			worm.MoveSpeed = 5.8f;
			worm.Acceleration = 0.090f;
		}

		private int attackCounter;
		private bool startDespawning;
		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write(attackCounter);
		}

		public override void ReceiveExtraAI(BinaryReader reader) {
			attackCounter = reader.ReadInt32();
		}
		// Use the default CheckActive behavior from ModNPC/Worm to handle despawning correctly.

		public override void AI() {
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				if (attackCounter > 0) {
					attackCounter--; // tick down the attack counter.
				}

				Player target1 = Main.player[NPC.target];
				// If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
				if (attackCounter <= 0 && Vector2.Distance(NPC.Center, target1.Center) < 20 && Collision.CanHit(NPC.Center, 1, 1, target1.Center, 1, 1)) {
					Vector2 direction = (target1.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
					direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

					int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 1, ModContent.ProjectileType<NeutralityProjectile1>(), 5, 0, Main.myPlayer);
					Main.projectile[projectile].timeLeft = 300;
					attackCounter = 700;
					NPC.netUpdate = true;
				}
			}
					if (!NPC.HasValidTarget) {
						NPC.velocity.Y += 1f;

						MoveSpeed = 0.3f;

						if (!startDespawning) {
							startDespawning = true;

							// Despawn after 90 ticks (1.5 seconds) if the NPC gets far enough away
							NPC.timeLeft = 10;
						}
					}
			

            Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<NeutralDust>());
		}
	}

	internal class NeutralWormBody : WormBody
	{
		public override void SetStaticDefaults() {
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
			NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<NeutralWormHead>();
		}

		public override void SetDefaults() {
			NPC.CloneDefaults(NPCID.DiggerBody);
			NPC.npcSlots = 10f;
			NPC.aiStyle = -1;

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
                int frontGoreType = Mod.Find<ModGore>("NeutralWormBody_Front").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, frontGoreType);
                }
            }
        }

        public override void Init() {
			NeutralWormHead.CommonWormInit(this);
		}
	}

	internal class NeutralWormTail : WormTail
	{
		public override void SetStaticDefaults() {
			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
			NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<NeutralWormHead>();
		}

		public override void SetDefaults() {
			NPC.CloneDefaults(NPCID.DiggerTail);
			NPC.npcSlots = 10f;
			NPC.aiStyle = -1;

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
                int frontGoreType = Mod.Find<ModGore>("NeutralWormTail_Front").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, NPC.velocity, frontGoreType);
                }
            }
        }

        public override void Init() {
			NeutralWormHead.CommonWormInit(this);
		}
	}
}