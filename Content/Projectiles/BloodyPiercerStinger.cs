using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class BloodyPiercerStinger : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Projectile");
        }
        public override void SetDefaults()
        {
            Projectile.width = 15;
            Projectile.height = 20;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 3500;
            Projectile.light = 0f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
        }
    }
}