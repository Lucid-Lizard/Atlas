using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using UtfUnknown.Core.Models.SingleByte.Finnish;

namespace Atlas.Content.Projectiles
{
    public class VortexBall : PongBall
    {
        public override void BallPhysics()
        {
           
            WindResistance = 0.99999f;
            Friction = 0.99999f;
        }

        public override void HitNPCEFfect(NPC npc)
        {
            Projectile.position = Main.player[Projectile.owner].Center;
            Projectile.velocity = new(Main.player[Projectile.owner].velocity.X, Main.player[Projectile.owner].velocity.Y - 4f);
        }

        public override void SetTrail()
        {
            trail.Width = 9;
            trail.Color = Color.Turquoise;
            trail.WidthFallOff = true;
            trail.Center = true;
            Vector2 scale = Projectile.getRect().Size();
            trail.ParentScale = new Vector3(scale, 0);
        }

        public override void PostAI()
        {

            int d = Dust.NewDust(Projectile.Center, 6, 6, DustID.Vortex, 0.0001f, 0.0001f);
            Main.dust[d].velocity = Vector2.Zero;
            Main.dust[d].noGravity = true;
        }
    }
}
