using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Content.NPCs.Desert
{
    public class ScorchingSun : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = 44;
            NPC.height = 44;
            NPC.lifeMax = 150;
            NPC.defense = 3;
            NPC.aiStyle = NPCAIStyleID.DemonEye;
            NPC.friendly = false;
            NPC.HitSound = SoundID.Item20;
            NPC.DeathSound = SoundID.Item45;
            NPC.damage = 15;
        }

        public override void AI()
        {
            NPC.rotation = 0f;
            NPC.ai[0] += 0.05f;
        }

        public readonly Rectangle sun = new(14, 14, 44, 44);
        public readonly Rectangle ring = new(0, 76, 74, 74);
        public readonly Rectangle eyes = new(24, 178, 24, 14);

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            for(int i = 0; i < 15; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Pixie);
            }
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Pixie);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.OnFire, 320);
        }

        public override void OnKill()
        {
            int amount = 30;
            for (int i = 0; i < amount; i++)
            {
                Vector2 vel = new(7, 0);
                vel = vel.RotatedBy((Math.Tau / amount) * i);
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Pixie, vel.X, vel.Y);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            

            Main.EntitySpriteDraw(TextureAssets.Npc[Type].Value, NPC.Center - Main.screenPosition, ring, Color.White, NPC.ai[0], ring.Size() / 2, 1f, SpriteEffects.None);
            Main.EntitySpriteDraw(TextureAssets.Npc[Type].Value, NPC.Center - Main.screenPosition, sun, Color.White, 0f, sun.Size() / 2, 1f, SpriteEffects.None);

            Vector2 eyeOffset = NPC.velocity;
            eyeOffset.X = Math.Clamp(eyeOffset.X, -5, 5);
            eyeOffset.Y = Math.Clamp(eyeOffset.Y, -5, 5);

            Main.EntitySpriteDraw(TextureAssets.Npc[Type].Value, NPC.Center + eyeOffset - Main.screenPosition, eyes, Color.White, 0f, eyes.Size() / 2, 1f, SpriteEffects.None);
            
            return false;
        }
    }
}
