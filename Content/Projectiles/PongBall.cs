using Atlas.Common.Prims;
using Atlas.Common.Systems;
using Atlas.Content.Items.Desert;
using Atlas.Content.Items.Misc;
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

        public bool PlayerJustHit = false;
        public Player LastHitter;

        public PrimTrail trail;

        public float Gravity = 0.16f;
        public float WindResistance = 0.99f;
        public float Friction = 0.85f;

        public virtual void BallPhysics()
        {

        }
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

            BallPhysics();
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
                
            }
        }

        /*public void TileHit(Vector2 oldVelocity)
        {
             oldVelocity = Projectile.oldVelocity;

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
                 Projectile.velocity.Y = -oldVelocity.Y ;
             }

           


            LastHitter = null;

        }*/

        public void PlayerHit(Player player, Item item)
        {
            if (fadeOut)
            {
                return;
            }

            PlayerJustHit = true;

            Vector2 direction = Main.MouseWorld - player.Center;

            Vector2 velocity = new(item.shootSpeed, 0);
            velocity = velocity.RotatedBy(direction.ToRotation());

            HitCount++;

            SoundEngine.PlaySound(new SoundStyle("Atlas/Sounds/PongBall") { PitchVariance = 0.2f }, Projectile.position);

            Projectile.velocity = velocity;
            Projectile.timeLeft = 180;

            LastHitter = player;

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


            Projectile.velocity *= -1 * Friction;

            Projectile.timeLeft = 180;

            HitNPCEFfect(npc);

            PlayerJustHit = false;

            LastHitter = null;
        }

        public override void AI()
        {
            if (LastHitter != null)
            {
                if (LastHitter.GetModPlayer<TopspinTechniquePlayer>().Active) { TopspinAI(); }
                else { CommonAI(); }
            }
            else
            {
                CommonAI();
            }
        }

        private NPC HomingTarget
        {
            get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0] - 1];
            set
            {
                Projectile.ai[0] = value == null ? 0 : value.whoAmI + 1;
            }
        }

        public void TopspinAI()
        {
            if (HomingTarget == null)
            {
                HomingTarget = FindClosestNPC(400f);
            }

            if (HomingTarget != null && !IsValidTarget(HomingTarget))
            {
                HomingTarget = null;
            }

            if (HomingTarget == null)
            {
                LastHitter = null;
                return;
            }

            float length = Projectile.velocity.Length();
            float targetAngle = Projectile.AngleTo(HomingTarget.Center);
            Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(targetAngle, MathHelper.ToRadians(10)).ToRotationVector2() * length;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;

            // Using squared values in distance checks will let us skip square root calculations, drastically improving this method's speed.
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            // Loop through all NPCs
            foreach (var target in Main.ActiveNPCs)
            {
                // Check if NPC able to be targeted. 
                if (IsValidTarget(target))
                {
                    // The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
                    float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

                    // Check if it is within the radius
                    if (sqrDistanceToTarget < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToTarget;
                        closestNPC = target;
                    }
                }
            }

            return closestNPC;
        }

        public bool IsValidTarget(NPC target)
        {
            // This method checks that the NPC is:
            // 1. active (alive)
            // 2. chaseable (e.g. not a cultist archer)
            // 3. max life bigger than 5 (e.g. not a critter)
            // 4. can take damage (e.g. moonlord core after all it's parts are downed)
            // 5. hostile (!friendly)
            // 6. not immortal (e.g. not a target dummy)
            // 7. doesn't have solid tiles blocking a line of sight between the projectile and NPC
            return target.CanBeChasedBy() && Collision.CanHit(Projectile.Center, 1, 1, target.position, target.width, target.height);
        }

        public void CommonAI()
        {
            Vector2 vector = Projectile.velocity;

            Projectile.rotation += MathHelper.ToRadians(10) * Math.Sign(Projectile.velocity.X);

            Projectile.velocity.Y += Gravity;
            Projectile.velocity.X *= WindResistance;

            if ((Projectile.velocity.X != vector.X && (vector.X < -3f || vector.X > 3f)) || (Projectile.velocity.Y != vector.Y && (vector.Y < -3f || vector.Y > 3f)))
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
