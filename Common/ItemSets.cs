using Atlas.Content.Items.Desert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Atlas.Common
{
    public static class ItemSets
    {
        public static List<int> Spears = new()
        {
            ItemID.AdamantiteGlaive,
            ItemID.ChlorophytePartisan,
            ItemID.CobaltNaginata,
            ItemID.DarkLance,
            3836,
            ItemID.Gungnir,
            ItemID.MushroomSpear,
            ItemID.MythrilHalberd,
            ItemID.NorthPole,
            ItemID.ObsidianSwordfish,
            ItemID.OrichalcumHalberd,
            ItemID.PalladiumPike,
            ItemID.Spear,
            ItemID.ThunderSpear,
            ItemID.Swordfish,
            ItemID.TheRottedFork,
            ItemID.TitaniumTrident,
            ItemID.Trident,
            ModContent.ItemType<ChargerSpear>()
        };

        public static List<int> Artifacts = new();
    }
}
