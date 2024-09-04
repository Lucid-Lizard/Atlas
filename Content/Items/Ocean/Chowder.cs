using Atlas.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace Atlas.Content.Items.Ocean
{
    public class Chowder : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.damage = 28;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.shoot = ModContent.ProjectileType<WhitePearlBall>();
            Item.shootSpeed = 10f;
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Red;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ItemID.Coral, 7)
                .AddIngredient(ItemID.Seashell, 5)
                .AddTile(TileID.Anvils)
                .Register();
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

            int id = ModContent.ProjectileType<WhitePearlBall>();
            int rand = Main.rand.Next(0, 100);

            //Main.NewText(rand);

            if (rand < 25)
            {
                id = ModContent.ProjectileType<BlackPearlBall>();
            }

            if(rand < 5)
            {
                id = ModContent.ProjectileType<PinkPearlBall>();
            }

            if(id == ModContent.ProjectileType<WhitePearlBall>())
            {
                Projectile.NewProjectile(source, position, new Vector2(0, -3), id, damage, knockback);
            } else if (id == ModContent.ProjectileType<BlackPearlBall>())
            {
                Projectile.NewProjectile(source, position, new Vector2(0, -3), id, (int)(damage * 1.2f), knockback);
            } else
            {
                Projectile.NewProjectile(source, position, new Vector2(0, -3), id, (int)(damage * 1.5f), knockback);
            }
           

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

                    Gore.NewGore(player.GetSource_FromAI(), p.Center, dust1, 411);
                    Gore.NewGore(player.GetSource_FromAI(), p.Center, dust2, 411);
/*
                    Dust.NewDust(p.Center, 1, 1, DustID.BubbleBurst_Blue, dust1.X, dust1.Y);
                    Dust.NewDust(p.Center, 1, 1, DustID.BubbleBurst_Blue, dust2.X, dust2.Y);*/

                    PongBall ball = p.ModProjectile as PongBall;
                    if (ball.Served)
                    {
                        ball.Projectile.velocity *= -1;
                    }
                    else
                    {
                        Vector2 direction = Main.MouseWorld - ball.Projectile.Center;

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
