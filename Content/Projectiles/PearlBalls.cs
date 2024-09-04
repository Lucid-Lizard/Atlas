using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Atlas.Content.Projectiles
{
    public class WhitePearlBall : PongBall
    {
        public override void SetTrail()
        {
            trail.Center = true;
            Vector2 scale = Projectile.getRect().Size();
            trail.ParentScale = new Vector3(scale, 0);
            trail.Width = 5;
            trail.Color = Color.White * 0.25f;
            trail.WidthFallOff = true;
        }
    }

    public class BlackPearlBall : PongBall
    {
        public override void SetTrail()
        {
            trail.Center = true;
            Vector2 scale = Projectile.getRect().Size();
            trail.ParentScale = new Vector3(scale, 0);
            trail.Width = 5;
            trail.Color = Color.Gray * 0.25f;
            trail.WidthFallOff = true;
        }
    }

    public class PinkPearlBall : PongBall
    {
        public override void SetTrail()
        {
            trail.Center = true;
            Vector2 scale = Projectile.getRect().Size();
            trail.ParentScale = new Vector3(scale, 0);
            trail.Width = 5;
            trail.Color = Color.Plum * 0.25f;
            trail.WidthFallOff = true;
        }
    }
}
