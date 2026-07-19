using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Dusts;
using System;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class AxyCrystal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 40;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300000;
            Projectile.light = 0.3f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.damage = 201;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1.92f, 1.65f, 1.92f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f); // projectile faces sprite right
            // This makes the projectile rotate to face its velocity direction
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
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CrystalAxyDust>(), 0f, 0f, 100, default, 1.5f);
            }
        }
    }
}