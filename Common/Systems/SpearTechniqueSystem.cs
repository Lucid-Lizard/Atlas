using Atlas.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Common.Systems
{
    public class TechniquePlayer : ModPlayer
    {
        public bool Snakestrike = false;

        public override void PreUpdate()
        {
            Snakestrike = false;
        }
    }
    public class SpearTechniqueSystem : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return ItemSets.Spears.Contains(entity.type);
        }

        public override bool AltFunctionUse(Item item, Player player)
        {
            return player.GetModPlayer<TechniquePlayer>().Snakestrike;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, player.Center, velocity * 2, ModContent.ProjectileType<SpearTechniqueProjectile>(), damage, knockback, player.whoAmI, item.shoot);
            }
            return player.altFunctionUse != 2;
        }

    }

   
}
