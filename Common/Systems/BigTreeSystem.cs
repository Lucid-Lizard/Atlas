/*using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Atlas.Common.Systems
{
    public class BigTreeSystem : ModSystem
    {
        public override void Load()
        {
            Terraria.On_Main.DrawBackground += On_Main_DrawBackground;
        }

        private void On_Main_DrawBackground(Terraria.On_Main.orig_DrawBackground orig, Terraria.Main self)
        {
            for(int i = 0; i < Main.maxTilesX; i++)
            {
                for (int j = 0; j < Main.maxTilesY; j++)
                {
                    Tile t = Framing.GetTileSafely(i, j);

                    if(t.TileType == TileID.Trees)
                    {
                        Vector2 drawPos = new(i, j);
                        drawPos *= 16f;

                        
                    }
                }
            }
        }
    }
}
*/