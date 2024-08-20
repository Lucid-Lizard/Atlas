using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace Atlas.Content.Items.Desert
{
    public class AntflingBow : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 48;
            Item.damage = 18;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 45;
            Item.useAnimation = 45;

            Item.UseSound = SoundID.Item5;

            Item.useAmmo = AmmoID.Arrow;
            Item.shoot = ModContent.ProjectileType<AntflingBowArrow>();
            Item.shootSpeed = 7f;

        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AntlionChitin>(), 7);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }


        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            Projectile.NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI);

            return false;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 0);
        }
    }

    public class AntflingBowArrow : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 24;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 1;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<AntflingBowAnt>(), 4, 2);
            SoundEngine.PlaySound(SoundID.NPCHit31, target.Center);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<AntflingBowAnt>(), 4, 2);
            return true;
        }
    }

    public class AntflingBowAnt : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // Total count animation frames
            Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 16;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 360;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
        }


        public override void AI()
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Vector2.Distance(Main.npc[i].Center, Projectile.Center) < Vector2.Distance(Main.npc[(int)Projectile.ai[0]].Center, Projectile.Center))
                {
                    Projectile.ai[0] = i;
                }
            }

            if (++Projectile.frameCounter >= MathHelper.Lerp(5f, 3f, Math.Abs(Projectile.velocity.X) / 2f))
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            int moveDir = Math.Sign(Main.npc[(int)Projectile.ai[0]].Center.X - Projectile.Center.X);
            Projectile.spriteDirection = moveDir;
            Projectile.direction = moveDir;
            Projectile.velocity.X = MathHelper.SmoothStep(Projectile.velocity.X, 2f * moveDir, 0.1f);

            if (DoesTileCollideWithLine(Projectile.Center, Projectile.Center + new Vector2(0, (Projectile.height / 2) + 2)))
            {
                Vector2 jumpCheck = Projectile.position;


                Projectile.velocity.Y = 0;
                Projectile.position.Y = (float)((Math.Floor(Projectile.position.Y / 16) * 16) - (Projectile.height / 2) + (16 - (Projectile.height / 2)));

                //Dust.NewDust(new Vector2(Projectile.position.X, (float)(Math.Floor(Projectile.position.Y / 16) * 16)), 1, 1, DustID.RedTorch);


                jumpCheck.Y = (float)((Math.Floor(Projectile.position.Y / 16) * 16));



                if (DoesTileCollideWithLine(jumpCheck, jumpCheck + new Vector2(32 * Projectile.direction, 0)))
                {
                    Projectile.velocity.Y -= 8;
                }
            }
            else
            {
                Projectile.velocity.Y += 0.3f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath34, Projectile.Center);
        }

        public bool DoesTileCollideWithLine(Vector2 start, Vector2 end)
        {
            for (int i = 0; i < (end - start).Length(); i++)
            {
                Vector2 tile = Vector2.Lerp(start, end, i / (end - start).Length());
                if (!WorldGen.TileEmpty((int)tile.X / 16, (int)tile.Y / 16) && WorldGen.SolidTile((int)tile.X / 16, (int)tile.Y / 16))
                {
                    return true;
                }
            }
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        internal Tile TileAtLocation(Vector2 position)
        {
            int x = (int)position.X / 16;
            int y = (int)position.Y / 16;
            //null-safe
            return Framing.GetTileSafely(x, y);
        }
        internal bool InTheGround(Vector2 position)
        {
            Tile tile = TileAtLocation(position);
            return tile.HasTile && tile.BlockType == BlockType.Solid || Main.tileSolidTop[tile.TileType];
        }
    }
}
