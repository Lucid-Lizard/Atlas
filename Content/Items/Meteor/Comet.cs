using Atlas.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Content.Items.Meteor
{
    public class Comet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.shoot = ModContent.ProjectileType<MeteorBall>();
            Item.shootSpeed = 8f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2 && player.ownedProjectileCounts[Item.shoot] >= 5)
            {
                return false;
            }

            return true;
        }

        public override bool CanShoot(Player player)
        {
            return player.altFunctionUse == 2;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, new Vector2(0, -7), type, damage, knockback);

            return false;
        }

        public override bool? CanHitNPC(Player player, NPC target)
        {
            return false;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemRotation -= player.direction > 0 ? MathHelper.ToRadians(45) : MathHelper.ToRadians(-45);
        }



        public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            Vector2 start = player.Center;
            Vector2 length = new(16 * 3f, 0);
            Vector2 end = start + length.RotatedBy(player.direction == 1 ? player.itemRotation : player.itemRotation - MathHelper.ToRadians(180f));

            // Dust.QuickDustLine(start, end, 16, Color.Red);

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];

                if (p.active && p.ModProjectile is PongBall && Collision.CheckAABBvLineCollision(p.position, p.Hitbox.Size(), start, end) && player.altFunctionUse < 2)
                {
                    float rot1 = player.itemRotation - MathHelper.ToRadians(45);
                    float rot2 = player.itemRotation + MathHelper.ToRadians(45);

                    Vector2 dust1 = Vector2.UnitX.RotatedBy(rot1);
                    Vector2 dust2 = Vector2.UnitX.RotatedBy(rot2);

                    Dust.NewDust(p.Center, 1, 1, DustID.MeteorHead, dust1.X, dust1.Y);
                    Dust.NewDust(p.Center, 1, 1, DustID.MeteorHead, dust2.X, dust2.Y);

                    PongBall ball = p.ModProjectile as PongBall;
                    if (ball.Served)
                    {
                        ball.Projectile.velocity *= -1;
                    }
                    else
                    {
                        Vector2 direction = Main.MouseWorld - player.Center;

                        Vector2 velocity = new(Item.shootSpeed, 0);
                        velocity = velocity.RotatedBy(direction.ToRotation());

                        ball.ResetHit(PongBall.HitType.Player, player, Item);

                        ball.Projectile.velocity = velocity;
                    }
                }
            }
        }
    }
}
