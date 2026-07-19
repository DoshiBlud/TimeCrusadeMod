using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Dusts;
using System.Collections.Generic;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class BalisticBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Projectile");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 340;
            Projectile.light = 0.4f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.damage = 198;
        }
        public override void AI()
        {
            UpdateAlpha();
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, 0.1f, 0.1f, 0.9f);
            Projectile.rotation += Projectile.velocity.X + 0.6f;
            Projectile.velocity.Y = Projectile.velocity.Y + 0.6f; // 0.1f for arrow gravity, 0.4f for knife gravity
            if (Projectile.velocity.Y > 22f) // This check implements "terminal velocity". We don't want the projectile to keep getting faster and faster. Past 16f this projectile will travel through blocks, so this check is useful.
            {
                Projectile.velocity.Y = 22f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            // This runs when the projectile dies (hits ground/tiles or runs out of time)

            // Create 5 dust particles
            for (int i = 0; i < 22; i++)
            {
                // Dust.NewDust(position, width, height, type, speedX, speedY, Alpha, Color, Scale)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<GhostDust>(), 0f, 0f, 100, default, 1.5f);
            }
        }
        private const int AlphaFadeInSpeed = 30;
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