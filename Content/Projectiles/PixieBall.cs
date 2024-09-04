using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace Atlas.Content.Projectiles
{
    public class PixieBall : PongBall
    {
        public override void BallPhysics()
        {
            Friction = 1.1f;
            WindResistance = 1.01f;
        }

        public override void SetTrail()
        {
            trail.Width = 7;
            trail.WidthFallOff = true;
            trail.Color = Color.Gold * 0.8f;
            trail.Center = true;
            Vector2 scale = Projectile.getRect().Size();
            trail.ParentScale = new(scale, 0);
        }

        public override void PostAI()
        {
            if(Main.rand.NextBool(5))
                Dust.NewDust(Projectile.Center, 6, 6, DustID.Pixie);
        }
    }
}
