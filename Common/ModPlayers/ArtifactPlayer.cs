using Atlas.Common.Systems;

using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Common.ModPlayers
{
    public class ArtifactPlayer : ModPlayer
    {
        public bool SnakeEyes = false;

        public int SnakeEyesEffectTimer = 60;

        

        public override void PreUpdate()
        {
            SnakeEyes = false;
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Keybinds.RandomBuffKeybind.JustPressed)
            {
                SoundEngine.PlaySound(SoundID.Item29, Player.Center);
                

                if (SnakeEyes)
                {
                    SnakeEyesEffectTimer = 60;
                }

                
            }

            
        }

        public override void UpdateEquips()
        {
            if(SnakeEyesEffectTimer > 0 && SnakeEyes)
                Player.GetCritChance(DamageClass.Generic) += 200;
        }

        public override void PostUpdate()
        {
            if(SnakeEyesEffectTimer > 0)
            {
                SnakeEyesEffectTimer--;
            }
            
        }

    }

    public class ArtifactTagWord : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return ItemSets.Artifacts.Contains(entity.type);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {

            base.ModifyTooltips(item, tooltips);

            
            //[i:{ModContent.ItemType<ShieldHeart>()}]
            var titletip = new TooltipLine(this.Mod, "ClassTag", $"[c/{Main.DiscoColor.Hex3()}:{ "Artifact" }]");
            tooltips.Insert(1, titletip);

            string text = Keyboard.GetState().IsKeyDown(Keys.LeftShift) ? $"By pressing {Keybinds.RandomBuffKeybind.GetAssignedKeys()[0]}, you activate this artifacts ability" : "[Left-shift for more info]";

            var MoreInfo = new TooltipLine(this.Mod, "MoreInfo", text);
            tooltips.Add(MoreInfo);
        }
    }
}
