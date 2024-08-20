using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Atlas.Common.Systems
{
    public class ClericSystem : ModSystem
    {
        public static List<int> HealingItems;

        public override void Load()
        {
            HealingItems = new List<int>();
        }
    }

    public class ClericDamageWord : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return ClericSystem.HealingItems.Contains(entity.type);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(item, tooltips);

            var tooltip = tooltips.FirstOrDefault(line => line.Name == "Damage" && line.Mod == "Terraria");

            if (tooltip == null)
            {
                return;
            }

            var split = tooltip.Text.Split(' ');
            var damage = split.First();
            var Class = split[1];
            

            tooltip.Text = $"{damage} {Class} healing";
            //[i:{ModContent.ItemType<ShieldHeart>()}]
            
        }
    }
}
