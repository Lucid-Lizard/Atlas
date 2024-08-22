using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Content.Items.Desert
{
    public class GruntingStick : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.damage = 25;
            Item.DamageType = DamageClass.Summon;
            Item.useStyle = ItemUseStyleID.RaiseLamp;
            
            Item.useAnimation = 60;
            Item.useTime = 60;
            Item.autoReuse = false;

            Item.mana = 40;
        }

        public override bool? UseItem(Player player)
        {

            for(int i = -5; i <= 5; i++)
            {
                Point pos = Utils.ToTileCoordinates(player.getRect().Bottom());

                pos.Y += 2;

                bool foundGround = false;

                for(int j = 0; j < 25 && !foundGround; j++)
                {
                    if (!Main.tile[pos.X, pos.Y - j].HasTile && Main.tile[pos.X, pos.Y - (j - 1)].HasTile)
                    {
                        foundGround = true;

                        if((i - 1) % 2 == 0)
                            Projectile.NewProjectile(Item.GetSource_FromAI(), new Vector2(((pos.X + i) * 16) + 8, ((pos.Y - j - 1) * 16) + 8), new Vector2(0, -5), ModContent.ProjectileType<AntflingBowArrow>(), 5, 0f, Owner: player.whoAmI);
                    }
                }
            }

            return true;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemRotation = player.direction > 0 ? MathHelper.ToRadians(45) : MathHelper.ToRadians(-45);
            player.itemLocation = player.Center - new Vector2(heldItemFrame.X / 2, 0) + new Vector2(-6 * player.direction, 0);
        }


    }

    
}
