using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Dusts;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class CypheringSlices : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Projectile");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 65;
            Projectile.height = 35;
            Projectile.aiStyle = ProjAIStyleID.ThrownProjectile;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 20;
            Projectile.timeLeft = 600;
            Projectile.light = 0.6f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.alpha = 50;
        }
        public override void OnKill(int timeLeft)
        {
            // This runs when the projectile dies (hits ground/tiles or runs out of time)
            
            // Create 5 dust particles
            for (int i = 0; i < 22; i++)
            {
                // Dust.NewDust(position, width, height, type, speedX, speedY, Alpha, Color, Scale)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CypherDust>(), 0f, 0f, 100, default, 1.5f);
            }
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(0f);
            int dust = Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<Dusts.CypherDust>(), 0f, 0f, 0, default(Color), 1f);
            dust = Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<Dusts.CypherBeam>(), 0f, 0f, 0, default(Color), 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.5f;
            Main.dust[dust].scale *= 2;

        }
        }
    }