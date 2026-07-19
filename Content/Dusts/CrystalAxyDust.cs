using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Dusts
{
    public class CrystalAxyDust: ModDust
    {
        public override void SetStaticDefaults()
        {
            UpdateType = DustID.Smoke;
        }
        public override void OnSpawn(Dust dust)
        {
            // Makes the dust not affected by gravity (optional)
            dust.noGravity = true;
            // Makes the dust light up the surroundings
            dust.noLight = false;
            // Set the initial color intensity (high values = brighter)
            dust.color = new Color(192, 165, 192);
        }
        public override bool Update(Dust dust)
        { // Calls every frame the dust is active
            dust.position += dust.velocity;
            dust.rotation += dust.velocity.X * 0.15f;
            dust.scale *= 0.99f;

            float light = 0.35f * dust.scale;

            Lighting.AddLight(dust.position, light, light, light);

            if (dust.scale < 0.5f)
            {
                dust.active = false;
            }

            return false; // Return false to prevent vanilla behavior.
        }
    }
}