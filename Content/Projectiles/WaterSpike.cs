using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TimeCrusadeMod.Content.Dusts;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class WaterSpike : ModProjectile
    {
        public override void SetStaticDefaults()
        {

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 20;
            Projectile.height = 11;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 12000;
            Projectile.light = 0.8f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            AIType = ProjectileID.Bullet; // This makes the projectile behave like a standard bullet (you can change this to make it behave differently)
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, 0.0f, 0.8f);
            // This makes the projectile rotate to face its velocity direction
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(0f);

            // Optional: Adjust sprite rotation if it's still wrong (e.g., + MathHelper.ToRadians(90f))

            // Optional: Add trail effect
            // Projectile.spriteDirection = Projectile.direction;
        }
        public override void PostAI()
        {
            for(int dust = 0; dust < 1; dust++)
            {
                int dusts = Dust.NewDust(Projectile.Center, 1, 1, DustID.Water, 0f, 0f, 0, default(Color), 1f);
                dust = Dust.NewDust(Projectile.Center, 1, 1, DustID.Water, 0f, 0f, 0, default(Color), 1f);
                Main.dust[dusts].noGravity = true;
                Main.dust[dusts].velocity *= 0.5f;
                Main.dust[dusts].scale *= 3;
            }
            for(int dusts = 0; dusts < 1; dusts++)
            {
                int dustss = Dust.NewDust(Projectile.Center, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                dusts = Dust.NewDust(Projectile.Center, 1, 1, DustID.BlueTorch, 0f, 0f, 0, default(Color), 1f);
                Main.dust[dusts].noGravity = true;
                Main.dust[dusts].velocity *= 0.5f;
                Main.dust[dusts].scale *= 2;
            }
        }

        public override void OnKill(int timeLeft)
        {
            // This runs when the projectile dies (hits ground/tiles or runs out of time)

            // Create 5 dust particles
            for (int i = 0; i < 9; i++)
            {
                // Dust.NewDust(position, width, height, type, speedX, speedY, Alpha, Color, Scale)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Water, 0f, 0f, 100, default, 1.5f);
            }
        }
    }
}