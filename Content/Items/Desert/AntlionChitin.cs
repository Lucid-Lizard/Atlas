using Atlas.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Content.Items.Desert
{
    public class AntlionChitin : ModItem
    {
       
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
        }
    }

    public class AntlionChitinDrop : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.Antlion || entity.type == NPCID.FlyingAntlion || entity.type == NPCID.GiantFlyingAntlion || entity.type == NPCID.WalkingAntlion || entity.type == NPCID.GiantWalkingAntlion;
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // This is where we add item drop rules, here is a simple example:
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AntlionChitin>(), 10, 1, 3));
        }
       
    }
}
