using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Dusts;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class AxyArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 50;
            Projectile.height = 26;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 8;
            Projectile.timeLeft = 1200;
            Projectile.light = 0.9f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true; // This makes the projectile behave like a standard bullet (you can change this to make it behave differently)
        }
        public override void AI()
        {
            UpdateAlpha();
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, 1.92f, 1.65f, 1.92f);
            // This makes the projectile rotate to face its velocity direction
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(0f);

            // Optional: Adjust sprite rotation if it's still wrong (e.g., + MathHelper.ToRadians(90f))

            // Optional: Add trail effect
            // Projectile.spriteDirection = Projectile.direction;
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Type];
                if (++Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;
            }
        }

        public override void OnKill(int timeLeft)
        {
            // This runs when the projectile dies (hits ground/tiles or runs out of time)

            // Create 5 dust particles
            for (int i = 0; i < 5; i++)
            {
                // Dust.NewDust(position, width, height, type, speedX, speedY, Alpha, Color, Scale)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AxitrentonDust>(), 0f, 0f, 100, default, 1.5f);
            }
        }
        private const int AlphaFadeInSpeed = 20;
        private void UpdateAlpha()
        {
            // Slowly remove alpha as it is present
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= AlphaFadeInSpeed;
            }

            // If alpha gets lower than 0, set it to 0
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
        }
    }
}