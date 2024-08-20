using Atlas.Content.Items.Desert;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Content.NPCs.Misc
{
    public class Twister : ModNPC
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
            NPC.HitSound = SoundID.DoubleJump;
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.friendly = false;

            NPC.damage = 15;
            NPC.defense = 0;


            
        }

        float animFrameTimer = 0;
        int animFrameMax = 4;

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

            NPC.velocity.X = MathHelper.SmoothStep(NPC.velocity.X, dir * 3.5f, 0.087f);
            NPC.spriteDirection = -dir;

            Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);
        }

        public override void PostAI()
        {
         

            
            NPC.spriteDirection = 1;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // This is where we add item drop rules, here is a simple example:
            npcLoot.Add(ItemDropRule.Common(ItemID.Cloud, 1,3,5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.Player.ZonePurity ? 4f : 0f;
        }

        public override void OnKill()
        {
            for(int i = 0; i < Main.rand.Next(8,12); i++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Cloud, 5 * Math.Sign(Main.rand.Next(-2, 1)), 0);
            }
        }
    }
}
