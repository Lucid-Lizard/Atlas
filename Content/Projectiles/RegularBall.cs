using Microsoft.Xna.Framework;
using Terraria;

namespace Atlas.Content.Projectiles
{
    public class RegularBall : PongBall
    {
        public override void SetTrail()
        {
            trail.Center = true;
            Vector2 scale = Projectile.getRect().Size();
            //trail.pixelated = true;
            trail.ParentScale = new Vector3(scale, 0);
            trail.Width = 5;
            trail.Color = Color.White * 0.25f;
            trail.WidthFallOff = true;
        }

        public override void BallPhysics()
        {
            base.BallPhysics();
        }

    }
}
