using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Audio;
using TimeCrusadeMod.Content.Dusts;
using TimeCrusadeMod.Content.Projectiles;

namespace TimeCrusadeMod.Content.Projectiles
{
    public class SphericalBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Crystal Projectile");
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 340;
            Projectile.light = 0.4f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
			// This part makes the projectile do a shime sound every 10 ticks as long as it is moving.
			if (Projectile.soundDelay == 0 && Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) > 2f) {
				Projectile.soundDelay = 10;
				SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
			}

			Vector2 dustPosition = Projectile.Center + new Vector2(Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
			Dust d = Dust.NewDustPerfect(dustPosition, ModContent.DustType<CrusaderDust>(), null, 100, Color.Lime, 0.8f);

			// In Multi Player (MP) This code only runs on the client of the projectile's owner, this is because it relies on mouse position, which isn't the same across all clients.
			if (Main.myPlayer == Projectile.owner && Projectile.ai[0] == 0f) {

				Player player = Main.player[Projectile.owner];
				// If the player channels the weapon, do something. This check only works if item.channel is true for the weapon.
				if (player.channel) {
					float maxDistance = 18f; // This also sets the maximun speed the projectile can reach while following the cursor.
					Vector2 vectorToCursor = Main.MouseWorld - Projectile.Center;
					float distanceToCursor = vectorToCursor.Length();

					// Here we can see that the speed of the projectile depends on the distance to the cursor.
					if (distanceToCursor > maxDistance) {
						distanceToCursor = maxDistance / distanceToCursor;
						vectorToCursor *= distanceToCursor;
					}

					int velocityXBy1000 = (int)(vectorToCursor.X * 1000f);
					int oldVelocityXBy1000 = (int)(Projectile.velocity.X * 1000f);
					int velocityYBy1000 = (int)(vectorToCursor.Y * 1000f);
					int oldVelocityYBy1000 = (int)(Projectile.velocity.Y * 1000f);

					// This code checks if the precious velocity of the projectile is different enough from its new velocity, and if it is, syncs it with the server and the other clients in MP.
					// We previously multiplied the speed by 1000, then casted it to int, this is to reduce its precision and prevent the speed from being synced too much.
					if (velocityXBy1000 != oldVelocityXBy1000 || velocityYBy1000 != oldVelocityYBy1000) {
						Projectile.netUpdate = true;
					}

					Projectile.velocity = vectorToCursor;

				}
				// If the player stops channeling, do something else.
				else if (Projectile.ai[0] == 0f) {

					// This code block is very similar to the previous one, but only runs once after the player stops channeling their weapon.
					
                    Projectile.netUpdate = true;

					float maxDistance = 14f; // This also sets the maximun speed the projectile can reach after it stops following the cursor.
					Vector2 vectorToCursor = Main.MouseWorld - Projectile.Center;
					float distanceToCursor = vectorToCursor.Length();

					//If the projectile was at the cursor's position, set it to move in the oposite direction from the player.
					if (distanceToCursor == 0f) {
						vectorToCursor = Projectile.Center - player.Center;
						distanceToCursor = vectorToCursor.Length();
					}

					distanceToCursor = maxDistance / distanceToCursor;
					vectorToCursor *= distanceToCursor;

					Projectile.velocity = vectorToCursor;

					if (Projectile.velocity == Vector2.Zero) {
						Projectile.Kill();
					}

					Projectile.ai[0] = 1f;
				}
			}
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(Projectile.Center, 0.9f, 0.1f, 0.3f);
            Projectile.rotation += Projectile.velocity.X + 0.6f;
            int dust = Dust.NewDust(Projectile.Center, 1, 1, ModContent.DustType<CrusaderDust>(), 0f, 0f, 0, default(Color), 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity *= 0.3f;
            	for (int loops = 0; loops < 1; loops++) {
				for (int i = 0; i < 2; i++) {
					Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
					Dust di = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<CrusaderDust>(), speed * 2 * (loops + 1), Scale: 0.7f);
					di.noGravity = false;
				}
			}

			// Set the rotation so the projectile points towards where it's going.
			if (Projectile.velocity != Vector2.Zero) {
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			}
        }

        public override void OnKill(int timeLeft)
        {
            // This runs when the projectile dies (hits ground/tiles or runs out of time)
            
            // Create 5 dust particles
            for (int i = 0; i < 13; i++)
            {
                // Dust.NewDust(position, width, height, type, speedX, speedY, Alpha, Color, Scale)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CrusaderDust>(), 0f, 0f, 100, default, 1.5f);
            }
        }
        }
    }