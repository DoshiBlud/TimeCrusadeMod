using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Dusts;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class CrusadersMouth : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 2; 
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 55;
            Projectile.height = 55;
            Projectile.aiStyle = ProjAIStyleID.MagicMissile;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 120;
            Projectile.light = 0.9f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.alpha = 50;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(0f);
    // Slows the projectile by 2.5% every frame
    Projectile.velocity *= 0.75f;

        int frameSpeed = 5;
    Projectile.frameCounter++;
    
    if (Projectile.frameCounter >= frameSpeed) {
        Projectile.frameCounter = 0;
        Projectile.frame++;
        
        // Loop back to the first frame
        if (Projectile.frame >= Main.projFrames[Projectile.type]) {
            Projectile.frame = 0;
        }
    }
            int dust = Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<CrusaderDust>(), 0f, 0f, 0, default, 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.3f;
            Main.dust[dust].scale = (float)Main.rand.Next(100, 135) * 2f;
            Main.dust[dust].type = ModContent.DustType<CrusaderDust>();
        }
        public override void OnKill(int timeLeft)
        {
            // This runs when the projectile dies (hits ground/tiles or runs out of time)
            
            // Create 5 dust particles
            for (int i = 0; i < 15; i++)
            {
                // Dust.NewDust(position, width, height, type, speedX, speedY, Alpha, Color, Scale)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CrusaderDust>(), 0f, 0f, 100, default, 1.5f);
            }
        }
        }
    }