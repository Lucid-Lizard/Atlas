using Atlas.Common.Prims;
using Atlas.Common.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Content.Projectiles
{
    public abstract class PongBall : ModProjectile
    {
        public bool Served = false;

        public bool fadeOut = false;

        public int HitCount = 0;

        public PrimTrail trail;
        

        public virtual void SetTrail()
        {

        }

        public enum HitType
        {
            Player,
            NPC,
            Tile
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 0;
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

        public override void OnSpawn(IEntitySource source)
        {
            trail = new(Projectile.oldPos, Color.White, 5);
            trail.Initialize();
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
            if (fadeOut)
            {
                return;
            }

            Vector2 direction = Main.MouseWorld - player.Center;

            Vector2 velocity = new(item.shootSpeed, 0);
            velocity = velocity.RotatedBy(direction.ToRotation());

            HitCount++;

            SoundEngine.PlaySound(new SoundStyle("Atlas/Sounds/PongBall") { PitchVariance = 0.2f }, Projectile.position);

            Projectile.velocity = velocity;
            Projectile.timeLeft = 180;

            PlayerHitEffect();
        }

        public virtual void PlayerHitEffect() { }

        public void NPCHit(NPC npc)
        {
            if (fadeOut)
            {
                return;
            }
            HitCount++;
            

            Vector2 oldVelocity = Projectile.oldVelocity;


            Projectile.velocity *= -1;

            Projectile.timeLeft = 180;

            HitNPCEFfect(npc);
        }

        public override void AI()
        {
            Vector2 vector = Projectile.velocity;

            Projectile.rotation += MathHelper.ToRadians(10) * Math.Sign(Projectile.velocity.X);

            Projectile.velocity.Y += 0.16f;

            if((Projectile.velocity.X != vector.X && (vector.X < -3f || vector.X > 3f)) || (Projectile.velocity.Y != vector.Y && (vector.Y < -3f || vector.Y > 3f)))
            {
                Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
                //Main.PlaySound(0, (int)this.center().X, (int)this.center().Y, 1);
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
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ResetHit(HitType.NPC, target);
        }

        public virtual void HitNPCEFfect(NPC npc) { }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            TileHit();

            return false;
        }

        public virtual void TileHitEffect() { }


        public override bool PreDraw(ref Color lightColor)
        {
            SetTrail();
            

            trail.Draw();

            return true;
        }
        public override void OnKill(int timeLeft)
        {
            if(trail != null)
            {
                trail.kill = true;
            }
        }


    }
}
