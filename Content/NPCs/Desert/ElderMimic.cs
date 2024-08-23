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
    public class ElderMimic : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 128;
            NPC.height = 128;
            NPC.boss = true;
            NPC.damage = 12;
            NPC.defense = 10;
            NPC.lifeMax = 2000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0f;
            NPC.value = Item.buyPrice(gold: 5);
            NPC.npcSlots = 10f;

            NPC.aiStyle = -1;



        }

        public enum AttackPhase
        {
            JumpAround,
            SpewCoins
        }

        public AttackPhase phase = AttackPhase.JumpAround;

        public int jumpCount = 0;
        public override void AI()
        {
            NPC.TargetClosest();

            

            Player player = Main.player[NPC.target];

            if(player != null)
            {
                if (!player.active || player.statLife <= 0)
                {
                    NPC.EncourageDespawn(1);
                } else
                {
                    if (phase == AttackPhase.JumpAround)
                    {
                        NPC.frame = new(0, 0, 128, 128);

                        if (NPC.ai[0]++ > 140)
                        {
                            NPC.ai[0] = 0;
                            NPC.velocity.Y = -7;
                            NPC.velocity.X = 6 * (Math.Sign(player.Center.X - NPC.Center.X));
                            jumpCount++;

                        }
                        else
                        {
                            NPC.velocity.X *= 0.95f;
                            if(NPC.velocity.X < 0.1f && jumpCount >= 3)
                            {
                                phase = AttackPhase.SpewCoins;
                                NPC.ai[0] = 0;
                                jumpCount = 0;
                            }
                        }
                    }

                    if(phase == AttackPhase.SpewCoins)
                    {
                        NPC.frame = new(0, 130, 128, 128);
                        NPC.velocity.X = 0;
                        if (NPC.ai[0]++ > 60)
                        {
                            if (NPC.ai[0] % 15 == 0)
                            {
                                Vector2 velocity = Vector2.UnitX * 5;
                                Vector2 angle = player.Center - NPC.Center;
                                velocity = velocity.RotatedBy(angle.ToRotation());

                                Projectile coin = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, velocity, ProjectileID.GoldCoin, 30, 1f);
                                coin.friendly = false;
                                coin.hostile = true;

                                if (NPC.ai[0] - 60 > 360)
                                {
                                    NPC.ai[0] = 0;
                                    phase = AttackPhase.JumpAround;
                                }
                            }
                        }
                    }
                }
            }
        }

        /*public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Main.EntitySpriteDraw(TextureAssets.Npc[Type].Value, NPC.position,)

            return false;
        }*/
    }
}
