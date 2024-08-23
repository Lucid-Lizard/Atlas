using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Common.Systems
{
    public class DesertAmbiance : ModSystem
    {
        public override void PreUpdatePlayers()
        {
            if (Main.LocalPlayer.ZoneDesert && Main.rand.NextBool(300))
            {
                bool LeftOrRight = Main.rand.NextBool(2);

                if (LeftOrRight)
                {

                    Projectile.NewProjectile(null, Main.screenPosition - new Vector2(Main.screenWidth, 0), new Vector2(0, 0), ModContent.ProjectileType<TumbleWeed>(), 0, 0, ai0: Main.rand.NextFloat(2f, 4f));
                } else
                {
                    Projectile.NewProjectile(null, Main.screenPosition + new Vector2(Main.screenWidth * 2, 0), new Vector2(0, 0), ModContent.ProjectileType<TumbleWeed>(), 0, 0, ai0: -Main.rand.NextFloat(2f, 4f));
                }
            }
        }
    }

    public class TumbleWeed : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.timeLeft = 1200;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
        }

        bool fade = false;

        public override void AI()
        {
            if(Projectile.timeLeft <= 1 && !fade)
            {
                fade = true;
                Projectile.timeLeft = 60;
            }

            Projectile.velocity.X = Projectile.ai[0];

            Projectile.rotation += MathHelper.ToRadians(10) * Math.Sign(Projectile.velocity.X);

            Projectile.velocity.Y = Math.Max(-3, Projectile.velocity.Y);

            Projectile.velocity.Y += 0.1f;

            if (fade)
            {
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, Projectile.timeLeft / 60f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.Y -= 2;

            for (int i = 0; i < 3; i++)
            {

                Dust.NewDust(Projectile.position + new Vector2(12, 24), 3, 0, DustID.SandstormInABottle, -Projectile.velocity.X, -0.1f);
            }
            return false;
        }
    }
}
