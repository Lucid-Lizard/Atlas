using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Atlas.Common.Systems
{

    public class TechniqueRegistry : ModSystem
    {
        public static Color TechniqueColor;
        public enum TechniqueType
        {
            Spear,
            BattlePaddle,
            Sword,
            Bow,
            Hammer,
            Axe,
            Boomerang,
            Whip,
            Yoyo
        }

        int timer = 1;
        int switchTime = 80;
        bool Switch = false;
        public override void PostUpdateEverything()
        {
            if (!Switch)
            {
                if(timer++ >= switchTime) { Switch = true; }
            } else 
            {
                if(timer-- <= 0) { Switch = false; }
            }

            TechniqueColor = Color.Lerp(Color.DarkOrange, Color.Gold, timer / (float)switchTime);
        }
        public static string GetText(TechniqueType type)
        {
            if(type == TechniqueType.BattlePaddle)
            {
                return "Battle Paddle";
            } else
            {
                return type.ToString();
            }
        }

        public static Dictionary<int, TechniqueType> Registry = new Dictionary<int, TechniqueType>();

        public class TechniqueWord : GlobalItem
        {
            public override bool AppliesToEntity(Item entity, bool lateInstantiation)
            {
                return TechniqueRegistry.Registry.ContainsKey(entity.type);
            }

            public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
            {

                base.ModifyTooltips(item, tooltips);


                //[i:{ModContent.ItemType<ShieldHeart>()}]
                var titletip = new TooltipLine(this.Mod, "TechniqueTag", $"[c/{TechniqueColor.Hex3()}:{$"-{GetText(Registry[item.type])} Technique-"}]");
                tooltips.Insert(1, titletip);

               
            }
        }
    }
}
