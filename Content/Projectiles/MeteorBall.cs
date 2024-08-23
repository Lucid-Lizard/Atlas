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
        public bool PlayerHit;

        public override void SetDefaults()
        {
            base.SetDefaults();

            Projectile.width = 16;
            Projectile.height = 16;
        }

        public override void PlayerHitEffect()
        {
            PlayerHit = true;
            Array.Clear(Projectile.oldPos);
        }

        public override void HitNPCEFfect(NPC npc)
        {
            npc.AddBuff(BuffID.OnFire, 120);
            PlayerHit = false;

           
        }

        public override void TileHitEffect()
        {
            PlayerHit = false;
        }


        public override void SetTrail()
        {
            //trail.pixelated = true;
            trail.Center = true;
            Vector2 scale = Projectile.getRect().Size();
            trail.ParentScale = new Vector3(scale, 0);
            trail.Width = 5;
            trail.Color = (PlayerHit ? Color.Yellow : Color.White ) * 0.25f;
            trail.WidthFallOff = true;
        }

    }
}
