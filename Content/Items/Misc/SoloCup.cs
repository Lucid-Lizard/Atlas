using Atlas.Content.Projectiles;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Content.Items.Misc
{
    public class SoloCup : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SoloCupPlayer>().Active = true;

        }
    }

    public class SoloCupPlayer : ModPlayer
    {
        public bool Active = false;
        public int projectile = -1;

        public override void PreUpdate()
        {
            Active = false;
        }

        public override void PostUpdate()
        {
            if (Active)
            {
                if (Player.ownedProjectileCounts[ModContent.ProjectileType<SoloCupProjectile>()] == 0)
                {
                    projectile = Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), Player.Center, Vector2.Zero, ModContent.ProjectileType<SoloCupProjectile>(), 0, 0, Player.whoAmI);

                }


            }
            else
            {
                projectile = -1;
            }
        }
    }

    public class SoloCupBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Ranged) *= 1.8f;
        }
    }

    public class SoloCupProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 24;
            Projectile.timeLeft = 2;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.aiStyle = -1;
        }

        public bool state = false;

        public bool Ability = false;

        public override void AI()
        {
            if (Projectile.ai[0]-- > 0)
            {
                Ability = true;
            }
            else
            {
                Ability = false;
            }

            if (Main.player[Projectile.owner].GetModPlayer<SoloCupPlayer>().Active) { Projectile.timeLeft = 2; }

            Projectile.position = Vector2.SmoothStep(Projectile.position, Main.MouseWorld + new Vector2(0, 30), 0.3f);

            List<Projectile> balls = Main.projectile.Where(p => p.ModProjectile is PongBall && p.active).ToList();

            foreach (Projectile p in balls)
            {
                if (p.getRect().Intersects(Projectile.getRect()))
                {
                    PongBall ball = p.ModProjectile as PongBall;

                    Player pl = Main.player[ball.Projectile.owner];

                    pl.AddBuff(ModContent.BuffType<SoloCupBuff>(), 360);

                    for (int i = 0; i < 5; i++)
                    {
                        Dust.NewDust(Projectile.Center, 0, 0, DustID.DesertWater2, SpeedY: -4);
                    }
                    Projectile.ai[0] = 360;

                    ball.Projectile.Kill();


                }
            }

        }


    }
}
