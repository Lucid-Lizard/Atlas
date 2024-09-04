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
        public override void BallPhysics()
        {
            Gravity = 0;
            Friction = 0.95f;
            WindResistance = 0.99f;
        }

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

        
    }
}
