using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Content.Projectiles
{
    public class PongBall : ModProjectile
    {
        public bool Served = false;

        public bool fadeOut = false;

        public int HitCount = 0;

        public enum HitType
        {
            Player,
            NPC,
            Tile
        }

       
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
        }

        public void ResetHit(HitType type, Entity Source, Item item = null)
        {
            if (!fadeOut)
            {
                if (type == HitType.Player)
                {
                    PlayerHit(Source as Player, item);
                }
                else if (type == HitType.NPC)
                {
                    NPCHit(Source as NPC);
                }
                else
                {
                    TileHit();
                }
            }
        }

        public void TileHit()
        {
            Vector2 oldVelocity = Projectile.oldVelocity;

            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(new SoundStyle("Atlas/Sounds/PongBall") { PitchVariance = 0.2f }, Projectile.position);

            // If the projectile hits the left or right side of the tile, reverse the X velocity
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }

            // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            
        }
        public void PlayerHit(Player player, Item item)
        {
            Vector2 direction = Main.MouseWorld - player.Center;

            Vector2 velocity = new(item.shootSpeed, 0);
            velocity = velocity.RotatedBy(direction.ToRotation());

            HitCount++;

            SoundEngine.PlaySound(new SoundStyle("Atlas/Sounds/PongBall") { PitchVariance = 0.2f }, Projectile.position);

            Projectile.velocity = velocity;
            Projectile.timeLeft = 180;
        }

        public void NPCHit(NPC npc)
        {
            HitCount++;
            /*Projectile.velocity *= -1;*/

            Vector2 oldVelocity = Projectile.oldVelocity;

            /*Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

            // If the projectile hits the left or right side of the tile, reverse the X velocity
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }

            // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }*/

            Projectile.velocity *= -1;

            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Vector2 vector = Projectile.velocity;

            Projectile.velocity.Y += 0.16f;

            if((Projectile.velocity.X != vector.X && (vector.X < -3f || vector.X > 3f)) || (Projectile.velocity.Y != vector.Y && (vector.Y < -3f || vector.Y > 3f)))
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                //Main.PlaySound(0, (int)this.center().X, (int)this.center().Y, 1);
            }

            if ((Projectile.timeLeft <= 1 && !fadeOut) || HitCount > 30)
            {
                Projectile.timeLeft = 15;
                fadeOut = true;
            }
            if (fadeOut)
            {
                Projectile.alpha = (int)MathHelper.Lerp(0, 225, (15 - Projectile.timeLeft) / 15f);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ResetHit(HitType.NPC, target);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            TileHit();

            return false;
        }
    }
}
