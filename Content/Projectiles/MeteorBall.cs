using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using System;

namespace Atlas.Content.Projectiles
{
    public class MeteorBall : PongBall
    {
        

        public override void BallPhysics()
        {
            Gravity += 0.1f;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 16;
            Projectile.height = 16;
        }

        /*public override void PlayerHitEffect()
        {
            Array.Clear(Projectile.oldPos);
        }*/

        public override void HitNPCEFfect(NPC npc)
        {
            if (PlayerJustHit)
            {
                npc.AddBuff(BuffID.OnFire, 120);

            }
           
        }



        public override void SetTrail()
        {
            //trail.pixelated = true;
            trail.Center = true;
            Vector2 scale = Projectile.getRect().Size();
            trail.ParentScale = new Vector3(scale, 0);
            trail.Width = 5;
            trail.Color = (PlayerJustHit ? Color.Yellow : Color.White ) * 0.25f;
            trail.WidthFallOff = true;
        }

    }
}
