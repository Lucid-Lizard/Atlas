using Atlas.Content.Items.Desert;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Content.NPCs.Desert
{
    public class Snake : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;
        }
        public override void SetDefaults()
        {

            NPC.width = 36;
            NPC.height = 30;
            NPC.aiStyle = -1;
            NPC.lifeMax = 200;
            NPC.HitSound = SoundID.NPCHit23;
            NPC.DeathSound = SoundID.NPCDeath26;
            NPC.friendly = false;

            NPC.damage = 15;
            NPC.defense = 0;


            
        }

        float animFrameTimer = 0;
        int animFrameMax = 10;

        public override void FindFrame(int frameHeight)
        {
            animFrameTimer += 1;
            if(animFrameTimer > animFrameMax)
            {
                animFrameTimer = 0;
                NPC.frameCounter++;
                if(NPC.frameCounter >= Main.npcFrameCount[Type])
                {
                    NPC.frameCounter = 0;
                }
            }

            NPC.frame.Y = (frameHeight) * (int)NPC.frameCounter;
        }

        public override void AI()
        {
            NPC.TargetClosest(false);

            Player target = Main.player[NPC.target];

            int dir = Math.Sign(target.Center.X - NPC.Center.X);

            NPC.velocity.X = MathHelper.SmoothStep(NPC.velocity.X, dir * 2f, 0.09f);
            NPC.spriteDirection = -dir;

            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
        }

        public override void PostAI()
        {
         

            
            NPC.spriteDirection = Math.Sign(-NPC.velocity.X);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // This is where we add item drop rules, here is a simple example:
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Snakeskin>(), 1,3,5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZoneDesert ? 4f : 0;
        }

       /* public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, default, default, default,null, default);
            spriteBatch.Draw(TextureAssets.Npc[Type].Value, NPC.position, NPC.frame, drawColor);


            return false;
        }*/

        public override void OnKill()
        {
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Utils.NextVector2Circular(Main.rand, 5, 5), Mod.Find<ModGore>("SnakeGore0").Type);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Utils.NextVector2Circular(Main.rand, 5, 5), Mod.Find<ModGore>("SnakeGore1").Type);
            Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Utils.NextVector2Circular(Main.rand, 5, 5), Mod.Find<ModGore>("SnakeGore2").Type);
        }
    }
}
