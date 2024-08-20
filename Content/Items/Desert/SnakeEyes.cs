using Atlas.Common;
using Atlas.Common.ModPlayers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Atlas.Content.Items.Desert
{
    public class SnakeEyes : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemSets.Artifacts.Add(Type);
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ArtifactPlayer>().SnakeEyes = true;
            player.luck += 1f;
                
        }
    }
}
