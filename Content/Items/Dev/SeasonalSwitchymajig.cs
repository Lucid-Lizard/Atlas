using Atlas.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Content.Items.Dev
{
    public class SeasonalSwitchymajig : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 15;
            Item.useAnimation = 15;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if(player.altFunctionUse != 2)
                SeasonSystem.currentSeason = (SeasonSystem.Season)(((int)SeasonSystem.currentSeason + 1) % 4);
            else SeasonSystem.currentSeason = (SeasonSystem.Season)((int)SeasonSystem.currentSeason - 1 >= 0 ? (int)SeasonSystem.currentSeason - 1 : 3);
           
            return true;
        }
    }
}
