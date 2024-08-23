using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Atlas.Content.Projectiles
{
    public class SpiritBall : PongBall
    {
        public override void SetTrail()
        {
            trail.Center = true;
            Vector2 scale = Projectile.getRect().Size();
            trail.Texture = ModContent.Request<Texture2D>("Atlas/Assets/TrailTextures/spark_07").Value;
            //trail.pixelated = true;
            trail.ParentScale = new Vector3(scale, 0);
            trail.Width = 50;
            trail.Color = Color.SkyBlue;
            trail.WidthFallOff = true;
        }

        public override void AI()
        {
            Vector2 vector = Projectile.velocity;

            Projectile.rotation += MathHelper.ToRadians(10) * Math.Sign(Projectile.velocity.X);

            

            if ((Projectile.velocity.X != vector.X && (vector.X < -3f || vector.X > 3f)) || (Projectile.velocity.Y != vector.Y && (vector.Y < -3f || vector.Y > 3f)))
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                //Main.PlaySound(0, (int)this.center().X, (int)this.center().Y, 1);
            }

            if (HitCount > 30)
            {
                Projectile.timeLeft = 15;
                fadeOut = true;
            }

            if (Projectile.timeLeft <= 1 && !fadeOut)
            {
                Projectile.timeLeft = 15;
                fadeOut = true;
            }
            if (fadeOut)
            {
                Projectile.alpha = (int)MathHelper.Lerp(0, 225, (15 - Projectile.timeLeft) / 15f);
            }

            Projectile.velocity *= 0.99f;
        }
    }
}
