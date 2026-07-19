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
    public class GiantNeutralSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueSlime];

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 0.4f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 32;
            NPC.damage = 30;
            NPC.defense = 3;
            NPC.lifeMax = 300;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 500f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = NPCAIStyleID.Slime;
            if (Main.hardMode)
            {
                NPC.damage = 32;
                NPC.lifeMax = 350;
                NPC.value = 1400f;
            }
            if (NPC.downedMoonlord)
            {
                NPC.damage = 40;
                NPC.lifeMax = 1500;
                NPC.value = 60000f;
            }

            AIType = NPCID.BlueSlime;
            AnimationType = NPCID.BlueSlime;
            SpawnModBiomes = [ModContent.GetInstance<NeutralUndergroundBiome>().Type];
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.InModBiome(ModContent.GetInstance<NeutralUndergroundBiome>()) ? 25 : 0;
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
                new FlavorTextBestiaryInfoElement(this.GetLocalization("Bestiary").Value)
            });
        }
    }
}
