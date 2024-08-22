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

        
        public override bool PreDrawExtras()
        {
            if (!PlayerHit)
            {
                base.PreDrawExtras();
            } else
            {
                Texture2D effect = ModContent.Request<Texture2D>("Atlas/Content/Projectiles/MeteorBallEffect").Value;

                for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Type]; i++)
                {
                    Main.EntitySpriteDraw(effect, Projectile.oldPos[i] + (effect.Size() / 2f) - Main.screenPosition,
                        null,
                        Color.Lerp(new Color(255, 250, 127) * MathHelper.Lerp(1f, 0f, Projectile.alpha / 255f), new Color(249, 75, 7, 0), (float)i / (float)ProjectileID.Sets.TrailCacheLength[Type]),
                        Projectile.rotation,
                        TextureAssets.Projectile[Type].Value.Size() / 2f,
                        1.1f,
                        Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
                }
            }
            

            return true;
        }

    }
}
