using Atlas.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Content.Items.Desert
{
    public class PadPaddle : ModItem
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
            Item.shoot = ModContent.ProjectileType<PongBall>();
            Item.shootSpeed = 6f;
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
            for(int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].ModProjectile is PongBall && hitbox.Intersects(Main.projectile[i].getRect()) && player.altFunctionUse < 2)
                {
                    PongBall ball = Main.projectile[i].ModProjectile as PongBall;
                    if (ball.Served)
                    {
                        ball.Projectile.velocity *= -1;
                    } else
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
