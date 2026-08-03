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
    public class TerrifierPebble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 300;
            Projectile.light = 0f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
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
            Lighting.AddLight(Projectile.Center, 0.4f, 0.2f, 1f);
            // This makes the projectile rotate to face its velocity direction
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
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
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<TerrifierDust>(), 0f, 0f, 100, default, 1.5f);
            }
        }
    }
}