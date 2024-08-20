using Atlas.Common.Systems;
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
    public class SnakestrikeTechnique : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 36;
            Item.accessory = true;

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TechniquePlayer>().Snakestrike = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<Snakeskin>(50)
                .AddIngredient(ItemID.Book)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
