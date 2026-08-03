using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Threading;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Biomes;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Items.Placeables.Banners;
using TimeCrusadeMod.Content.NPCs;
using TimeCrusadeMod.Content.Projectiles;

namespace TimeCrusadeMod.Content.NPCs.Enemies.Rolling
{
    public class CorruptRoller : ModNPC
    {
        public int Act = 1;
        public int timer = 0;
        public override void SetStaticDefaults()
        {
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 20;
            NPC.height = 20;
            NPC.scale = 2f;
            NPC.damage = 22;
            NPC.defense = 8;
            NPC.lifeMax = 130;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 500f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = NPCAIStyleID.Unicorn;
            NPC.noGravity = false;
            NPC.noTileCollide = false;
            if(Main.expertMode)
            {
                NPC.damage = 25;
                NPC.lifeMax = 160;
                NPC.value = 650;
            }
            if (Main.masterMode)
            {
                NPC.damage = 29;
                NPC.lifeMax = 180;
                NPC.value = 760;
            }
            AIType = NPCID.Bunny;
            Banner = Type;
            // These lines are only needed in the main body part.
            BannerItem = ModContent.ItemType<CorruptRollerBanner>();
            ItemID.Sets.KillsToBanner[BannerItem] = 50; // Custom kill count required for banner drop and bestiary unlock. Omit this line for the default 50 kill count.
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
                NPC.velocity.X = -2;
                NPC.rotation += NPC.velocity.X * 0.08f;
                return;
            }
            NPC.rotation += NPC.velocity.X * 0.08f;
            switch(Act)
            {
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture).Value;

            Texture2D Eye = ModContent.Request<Texture2D>(NPC.ModNPC.Texture + "_Eye").Value;

            Vector2 origin = texture.Size() / 2;

            spriteBatch.Draw(
                texture,
                NPC.Center - screenPos,
                null,
                drawColor,
                NPC.rotation,
                origin,
                NPC.scale,
                SpriteEffects.None,
                0f
            );

            spriteBatch.Draw(
                Eye,
                NPC.Center - screenPos,
                null,
                Color.White,
                0,
                origin,
                NPC.scale,
                SpriteEffects.None,
                0f
            );

            return false;
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
                int frontGoreType = Mod.Find<ModGore>("CorruptRoller_Front").Type;

                var entitySource = NPC.GetSource_Death();

                for (int i = 0; i < 1; i++)
                {
                    Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneCorrupt ? 0.8f : 0;
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
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
            BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement(this.GetLocalization("Bestiary").Value)
            });
        }
    }
}
