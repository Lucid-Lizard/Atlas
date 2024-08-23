using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Content.Items.Misc
{
    public class HeartPerfume : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.maxStack = 1;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<HeartPerfumePlayer>().Active = true;
        }
    }

    public class HeartPerfumePlayer : ModPlayer
    {
        public bool Active = false;

        public override void PreUpdate()
        {
            Active = false;
        }
    }

    public class HeartPerfumeSystem : GlobalItem
    {
        public static int[] Applicable = new int[]
        {
            ItemID.LesserHealingPotion,
            ItemID.HealingPotion,
            ItemID.GreaterHealingPotion,
            ItemID.SuperHealingPotion
        };

        public int GetHealthValue(int itemID)
        {
            if(itemID == Applicable[0])
            {
                return 25;
            }
            if (itemID == Applicable[0])
            {
                return 50;
            }
            if (itemID == Applicable[0])
            {
                return 75;
            }
            if (itemID == Applicable[0])
            {
                return 100;
            }
            return 0;
        }
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return Applicable.Contains(entity.type);
        }

        public override void OnConsumeItem(Item item, Player player)
        {
            if (player.GetModPlayer<HeartPerfumePlayer>().Active)
            {
                /*for(int i = 0; i < 36; i++)
                {
                    Vector2 velocity = Utils.NextVector2CircularEdge(Main.rand, 2, 2);
                    Dust.NewDust(player.Center, 1, 1, DustID.Cloud, velocity.X, velocity.Y, newColor: Color.LightPink, Scale: 4f);
                }*/

                player.itemAnimation = 0;

                foreach(Player playa in Main.player)
                {
                    if(Vector2.Distance(playa.Center, player.Center) <= 16 * 20)
                    {
                        playa.statLife += item.healLife / 2;
                    }
                }

                foreach (NPC npc in Main.npc)
                {
                    if (Vector2.Distance(npc.Center, player.Center) <= 16 * 20 && npc.friendly)
                    {
                        npc.life += item.healLife / 2;
                    }
                }

                player.potionDelayTime = 0;
                player.potionDelay = 0;

                SoundEngine.PlaySound(new("Atlas/Sounds/Perfume") { PitchVariance = 0.2f }, player.Center);

                Projectile.NewProjectile(player.GetSource_DropAsItem(), player.Center - new Vector2(0, 64 - (player.height / 2f)), Vector2.Zero, ModContent.ProjectileType<HeartPoof>(), 0, 0);
            } else
            {
                base.OnConsumeItem(item, player);
            }
            
        }
        public override bool? UseItem(Item item, Player player)
        {
            if (player.GetModPlayer<HeartPerfumePlayer>().Active)
            {
                player.ClearBuff(BuffID.PotionSickness);
                
            }
            return true;
        }
    }

    public class HeartPoof : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 10;
        }
        public override void SetDefaults()
        {
            Projectile.width = 128;
            Projectile.height = 128;
            Projectile.timeLeft = 60;
            Projectile.penetrate = -1;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            if (Projectile.ai[0]++ % 6 == 0)
            {
                Projectile.frame++;
            }

            
        }
    }
}
