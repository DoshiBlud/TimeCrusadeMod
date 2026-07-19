using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using TimeCrusadeMod.Content.Dusts;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class Lunarang : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Projectile");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 20;
            Projectile.height = 52;
            Projectile.CloneDefaults(ProjectileID.EnchantedBoomerang);
            // projectile.aiStyle = 3; This line is not needed since CloneDefaults sets it already.
            AIType = ProjectileID.EnchantedBoomerang;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 120000;
            Projectile.light = 0.9f;
            Projectile.rotation += 0.4f * (float)Projectile.direction;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true; // This makes the projectile behave like a standard bullet (you can change this to make it behave differently)
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 1f, 0.9f);
            // This makes the projectile rotate to face its velocity direction

            // Optional: Adjust sprite rotation if it's still wrong (e.g., + MathHelper.ToRadians(90f))

            // Optional: Add trail effect
            // Projectile.spriteDirection = Projectile.direction;
        }

        public override void OnKill(int timeLeft)
        {
            // This runs when the projectile dies (hits ground/tiles or runs out of time)

            // Create 5 dust particles
            for (int i = 0; i < 22; i++)
            {
                // Dust.NewDust(position, width, height, type, speedX, speedY, Alpha, Color, Scale)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.LunarOre, 0f, 0f, 100, default, 1.5f);
            }
        }
    }
}