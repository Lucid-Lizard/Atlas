using Atlas.Common.Systems;
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

namespace Atlas.Content.Items.Desert
{
    public class SnakeOil : ModItem
    {
        public override void SetStaticDefaults()
        {
            ClericSystem.HealingItems.Add(Type);
        }
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 30;
            Item.damage = 10;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.autoReuse = false;
            Item.mana = 15;
            Item.UseSound = SoundID.Item1;

            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<SnakeOilProjectile>();
            Item.shootSpeed = 6f;
            
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Snakeskin>(), 15);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Bottles);
            recipe.Register();
        }
    }

    public class SnakeOilProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
        }

        public void HealEffect()
        {
            SoundEngine.PlaySound(SoundID.Item107, Projectile.Center);

            for(int i = 0; i < 25; i++)
            {
                Vector2 velocity = new(6, 0);
                velocity = velocity.RotatedBy((Math.Tau / 25) * i);
                Dust.NewDust(Projectile.Center, 16, 16, DustID.TintableDust, SpeedX: velocity.X, SpeedY: velocity.Y, newColor: new(71, 128, 19), Scale: 1.2f);
            }

            for(int i = 0; i < Main.maxPlayers; i++)
            {
                var player = Main.player[i];

                if (player != Main.LocalPlayer) continue;

                if(player.team == Main.LocalPlayer.team && Vector2.Distance(player.Center,Projectile.Center) < 16 * 10)
                {
                    player.Heal(Projectile.damage);
                    //player.HealEffect(Projectile.damage, true);
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            HealEffect();
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            HealEffect();
        }

        public override bool? CanDamage()
        {
            return false;
        }
    }
}
