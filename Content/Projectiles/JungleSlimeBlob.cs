using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class JungleSlimeBlob : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 5;
            // DisplayName.SetDefault("Crystal Projectile");
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 350;
            Projectile.light = 0f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(0f);
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Type];
                if (++Projectile.frame >= Main.projFrames[Type])
                    Projectile.frame = 0;
            }
        }
    }
}