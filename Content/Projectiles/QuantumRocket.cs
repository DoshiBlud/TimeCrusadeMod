using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Dusts;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class QuantumRocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Projectile");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 20;
            Projectile.timeLeft = 600;
            Projectile.light = 0.6f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }
        public override void OnKill(int timeLeft)
        {
            // This runs when the projectile dies (hits ground/tiles or runs out of time)
            
            // Create 5 dust particles
            for (int i = 0; i < 22; i++)
            {
                // Dust.NewDust(position, width, height, type, speedX, speedY, Alpha, Color, Scale)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CypherDust>(), 0f, 0f, 100, default, 1.5f);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<QuantumDust>(), 0f, 0f, 100, default, 1.5f);
            }
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.9f, 0.1f, 0.3f);
            Projectile.rotation = Projectile.velocity.ToRotation();            // Create dust particles as the projectile moves
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CypherDust>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100, default, 1.2f);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<QuantumDust>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 100, default, 1.2f);
        }
        }
    }