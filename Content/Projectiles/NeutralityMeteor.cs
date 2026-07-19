using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Dusts;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class NeutralityMeteor : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Projectile");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 45;
            Projectile.height =44;
            Projectile.aiStyle = ProjAIStyleID.Powder;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 180;
            Projectile.light = 0.5f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }
        
        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<NeutralDust>(), 0f, 0f, 0, default(Color), 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.3f;
            Projectile.rotation += Projectile.velocity.X * 0.07f;

        int frameSpeed = 7;
    Projectile.frameCounter++;
    
    if (Projectile.frameCounter >= frameSpeed) {
        Projectile.frameCounter = 0;
        Projectile.frame++;
        
        // Loop back to the first frame
        if (Projectile.frame >= Main.projFrames[Projectile.type]) {
            Projectile.frame = 0;
        }
    }
        }

        public override void OnKill(int timeLeft)
        {
            // This runs when the projectile dies (hits ground/tiles or runs out of time)
            
            // Create 5 dust particles
            for (int i = 0; i < 12; i++)
            {
                // Dust.NewDust(position, width, height, type, speedX, speedY, Alpha, Color, Scale)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Granite, 0f, 0f, 100, default, 1.5f);
            }
            if (Main.myPlayer != -1) {
                // Shoot 3 new projectiles
                for (int i = 0; i < 1; i++) {
                    Vector2 newVelocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)) * 0.5f;
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        newVelocity,
                        ModContent.ProjectileType<NeutralityProjectile1>(), // Replace with your projectile type
                        Projectile.damage / 2, // Half damage
                        Projectile.knockBack,
                        Main.myPlayer
                    );
        }
            }
        }
    }
}