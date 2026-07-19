using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Dusts;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class NeutralityBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            // DisplayName.SetDefault("Crystal Projectile");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 80;
            Projectile.height = 2;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.light = 0.9f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet; // This makes the projectile behave like a standard bullet (you can change this to make it behave differently)
        }
        public override void AI() {
            // This makes the projectile rotate to face its velocity direction
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(0f);
            
            // Optional: Adjust sprite rotation if it's still wrong (e.g., + MathHelper.ToRadians(90f))
            
            // Optional: Add trail effect
            // Projectile.spriteDirection = Projectile.direction;
        }
        
        public override void OnKill(int timeLeft)
        {
            // This runs when the projectile dies (hits ground/tiles or runs out of time)
            
            // Create 5 dust particles
            for (int i = 0; i < 5; i++)
            {
                // Dust.NewDust(position, width, height, type, speedX, speedY, Alpha, Color, Scale)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<NeutralDust>(), 0f, 0f, 100, default, 1.5f);
            }
        }
        }
    }