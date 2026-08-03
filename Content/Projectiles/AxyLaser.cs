using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class AxyLaser : ModProjectile
    {
        // A helpful math constant for performing beam angling calculations.

        // How much more damage the beams do when the Prism is fully charged. Damage smoothly scales up to this multiplier.
        private const float MaxDamageMultiplier = 1.5f;

        // Beams increase their scale from 0 to this value as the Prism charges up.
        private const float MaxBeamScale = 0.8f;

        // Beams reduce their spread to zero as the Prism charges up. This controls the maximum spread.
        private const float MaxBeamSpread = 2f;

        // The maximum possible range of the beam. Don't set this too high or it will cause significant lag.
        private const float MaxBeamLength = 2400f;

        // The width of the beam in pixels for the purposes of tile collision.
        // This should generally be left at 1, otherwise the beam tends to stop early when touching tiles.
        private const float BeamTileCollisionWidth = 1f;

        // The width of the beam in pixels for the purposes of entity hitbox collision.
        // This gets scaled with the beam's scale value, so as the beam visually grows its hitbox gets wider as well.
        private const float BeamHitboxCollisionWidth = 22f;

        // The number of sample points to use when performing a collision hitscan for the beam.
        // More points theoretically leads to a higher quality result, but can cause more lag. 3 tends to be enough.
        private const int NumSamplePoints = 3;

        // How quickly the beam adjusts to sudden changes in length.
        // Every frame, the beam replaces this ratio of its current length with its intended length.
        // Generally you shouldn't need to change this.
        // Setting it too low will make the beam lazily pass through walls before being blocked by them.
        private const float BeamLengthChangeFactor = 0.75f;

        // The charge percentage required on the host prism for the beam to begin visual effects (e.g. impact dust).
        private const float VisualEffectThreshold = 0.1f;

        // Each Last Prism beam draws two lasers separately: an inner beam and an outer beam. This controls their opacity.
        private const float OuterBeamOpacityMultiplier = 0.75f;
        private const float InnerBeamOpacityMultiplier = 0.1f;

        // The maximum brightness of the light emitted by the beams. Brightness scales from 0 to this value as the Prism's charge increases.
        private const float BeamLightBrightness = 0.75f;

        // These variables control the beam's potential coloration.
        // As a value, hue ranges from 0f to 1f, both of which are pure red. The laser beams vary from 0.57 to 0.75, which winds up being a blue-to-purple gradient.
        // Saturation ranges from 0f to 1f and controls how greyed out the color is. 0 is fully grayscale, 1 is vibrant, intense color.
        // Lightness ranges from 0f to 1f and controls how dark or light the color is. 0 is pitch black. 1 is pure white.
        private const float BeamColorHue = 0.95f;
        private const float BeamHueVariance = 0.23f;
        private const float BeamColorSaturation = 0.68f;
        private const float BeamColorLightness = 0.80f;

        // This property encloses the internal AI variable Projectile.ai[0]. It makes the code easier to read.
        private float BeamID
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        // This property encloses the internal AI variable Projectile.ai[1].
        private float HostPrismIndex
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        // This property encloses the internal AI variable Projectile.localAI[1].
        // Normally, localAI is not synced over the network. This beam manually syncs this variable using SendExtraAI and ReceiveExtraAI.
        private float BeamLength
        {
            get => Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 18;
            Projectile.penetrate = -1;
            // The beam itself still stops on tiles, but its invisible "source" Projectile ignores them.
            // This prevents the beams from vanishing if the player shoves the Prism into a wall.
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;

            // Using local NPC immunity allows each beam to strike independently from one another.
        }

        // Send beam length over the network to prevent hitbox-affecting and thus cascading desyncs in multiplayer.
        public override void SendExtraAI(BinaryWriter writer) => writer.Write(BeamLength);
        public override void ReceiveExtraAI(BinaryReader reader) => BeamLength = reader.ReadSingle();

        public override void AI()
        {
            // If something has gone wrong with either the beam or the host Prism, destroy the beam.
            Projectile hostPrism = Main.projectile[(int)HostPrismIndex];
            if (Projectile.type != ModContent.ProjectileType<AxyLaser>())
            {
                Projectile.Kill();
                return;
            }
            // If the host Prism is already at max charge, don't calculate anything. Just use the max values.
            else
            {
                Projectile.scale = MaxBeamScale;
                Projectile.Opacity = 1f;
            }

            // The amount to which the angle changes reduces over time so that the beams look like they are focusing.
            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();

            // Update the beam's length by performing a hitscan collision check.

            // This Vector2 stores the beam's hitbox statistics. X = beam length. Y = beam width.
            Vector2 beamDims = new Vector2(Projectile.velocity.Length() * BeamLength, Projectile.width * Projectile.scale);

            // Only produce dust and cause water ripples if the beam is above a certain charge level.
            Color beamColor = GetOuterBeamColor();

            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * BeamLength, beamDims.Y, new Utils.TileActionAttempt(DelegateMethods.CastLight));
        }

        // Determines whether the specified target hitbox is intersecting with the beam.
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // If the target is touching the beam's hitbox (which is a small rectangle vaguely overlapping the host Prism), that's good enough.
            if (projHitbox.Intersects(targetHitbox))
            {
                return true;
            }

            // Otherwise, perform an AABB line collision check to check the whole beam.
            float _ = float.NaN;
            Vector2 beamEndPos = Projectile.Center + Projectile.velocity * BeamLength;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, beamEndPos, BeamHitboxCollisionWidth * Projectile.scale, ref _);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // If the beam doesn't have a defined direction, don't draw anything.
            if (Projectile.velocity == Vector2.Zero)
            {
                return false;
            }

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 centerFloored = Projectile.Center.Floor() + Projectile.velocity * Projectile.scale * 10.5f;
            Vector2 drawScale = new Vector2(Projectile.scale);

            // Reduce the beam length proportional to its square area to reduce block penetration.
            float visualBeamLength = BeamLength - 14.5f * Projectile.scale * Projectile.scale;

            DelegateMethods.f_1 = 1f; // f_1 is an unnamed decompiled variable whose function is unknown. Leave it at 1.
            Vector2 startPosition = centerFloored - Main.screenPosition;
            Vector2 endPosition = startPosition + Projectile.velocity * visualBeamLength;

            // Draw the outer beam.
            DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, GetOuterBeamColor() * OuterBeamOpacityMultiplier * Projectile.Opacity);

            // Draw the inner beam, which is half size.
            drawScale *= 0.5f;
            DrawBeam(Main.spriteBatch, texture, startPosition, endPosition, drawScale, GetInnerBeamColor() * InnerBeamOpacityMultiplier * Projectile.Opacity);

            // Returning false prevents Terraria from trying to draw the Projectile itself.
            return false;
        }

        private void DrawBeam(SpriteBatch spriteBatch, Texture2D texture, Vector2 startPosition, Vector2 endPosition, Vector2 drawScale, Color beamColor)
        {
            Utils.LaserLineFraming lineFraming = new Utils.LaserLineFraming(DelegateMethods.RainbowLaserDraw);

            // c_1 is an unnamed decompiled variable which is the render color of the beam drawn by DelegateMethods.RainbowLaserDraw.
            DelegateMethods.c_1 = beamColor;
            Utils.DrawLaser(spriteBatch, texture, startPosition, endPosition, drawScale, lineFraming);
        }

        private Color GetOuterBeamColor()
        {
            // This hue calculation produces a unique color for each beam based on its Beam ID.
            float hue = (BeamID / ModContent.ProjectileType<AxyLaser>()) % BeamHueVariance + BeamColorHue;

            // Main.hslToRgb converts Hue, Saturation, Lightness into a Color for general purpose use.
            Color c = Main.hslToRgb(hue, BeamColorSaturation, BeamColorLightness);

            // Manually reduce the opacity of the color so beams can overlap without completely overwriting each other.
            c.A = 0;
            return c;
        }

        // Inner beams are always pure white so that they act as a "blindingly bright" center to each laser.
        private Color GetInnerBeamColor() => Color.Pink;


        // Automatically iterates through every tile the laser is overlapping to cut grass at all those locations.
        public override void CutTiles()
        {
            // tilecut_0 is an unnamed decompiled variable which tells CutTiles how the tiles are being cut (in this case, via a Projectile).
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.TileActionAttempt cut = new Utils.TileActionAttempt(DelegateMethods.CutTiles);
            Vector2 beamStartPos = Projectile.Center;
            Vector2 beamEndPos = beamStartPos + Projectile.velocity * BeamLength;

            // PlotTileLine is a function which performs the specified action to all tiles along a drawn line, with a specified width.
            // In this case, it is cutting all tiles which can be destroyed by Projectiles, for example grass or pots.
            Utils.PlotTileLine(beamStartPos, beamEndPos, Projectile.width * Projectile.scale, cut);
        }
    }
}