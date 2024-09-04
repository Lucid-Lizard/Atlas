using Atlas.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Atlas.Content.Items.Misc
{
    public class TopspinTechnique : ModItem
    {
        public override void SetStaticDefaults()
        {
            TechniqueRegistry.Registry.Add(Type, TechniqueRegistry.TechniqueType.BattlePaddle);
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TopspinTechniquePlayer>().Active = true;
        }
    }

    public class TopspinTechniquePlayer : ModPlayer
    {
        public bool Active = false;

        public override void PreUpdate()
        {
            Active = false;
        }
    }
}
