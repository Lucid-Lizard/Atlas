using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Atlas.Common.Systems
{
    public class SeasonSystem : ModSystem
    {
        public enum Season
        {
            Spring,
            Summer,
            Fall,
            Winter
        }

        public static Texture2D AutumnGrass;
        public static Texture2D SpringGrass;
        public static Texture2D WinterGrass;

        public static Season currentSeason;

        public static bool Fall => currentSeason == Season.Fall;
        public static bool Summer => currentSeason == Season.Summer;
        public static bool Spring => currentSeason == Season.Spring;
        public static bool Winter => currentSeason == Season.Winter;

        public override void SaveWorldData(TagCompound tag)
        {
            tag.Add("Atlas: Season", (int)currentSeason);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if(tag.TryGet<int>("Atlas: Season", out int season))
            {
                currentSeason = (Season)season;
            } else
            {
                currentSeason = Season.Summer;
            }
        }

        public override void SetStaticDefaults()
        {
           
            AutumnGrass = ModContent.Request<Texture2D>("Atlas/Assets/AutumnGrass").Value;
            SpringGrass = ModContent.Request<Texture2D>("Atlas/Assets/SpringGrass").Value;
            WinterGrass = ModContent.Request<Texture2D>("Atlas/Assets/WinterGrass").Value;
        }
       

        public class SeasonDirt : GlobalTile
        {

            public override bool PreDraw(int i, int j, int type, SpriteBatch spriteBatch)
            {
                if (type == TileID.Grass)
                {
                    AutumnGrass = ModContent.Request<Texture2D>("Atlas/Assets/AutumnGrass").Value;
                    SpringGrass = ModContent.Request<Texture2D>("Atlas/Assets/SpringGrass").Value;
                    WinterGrass = ModContent.Request<Texture2D>("Atlas/Assets/WinterGrass").Value;

                    if (Summer)
                    {
                        return true;
                    } else if (Fall)
                    {
                        
                        spriteBatch.Draw(AutumnGrass, new Vector2(i + 12, j + 12) * 16 - Main.screenPosition, new Rectangle(Main.tile[i, j].TileFrameX, Main.tile[i, j].TileFrameY, 16, 16), Lighting.GetColor(i, j));

                    }
                    else if (Spring)
                    {
                        spriteBatch.Draw(SpringGrass, new Vector2(i + 12, j + 12) * 16 - Main.screenPosition, new Rectangle(Main.tile[i, j].TileFrameX, Main.tile[i, j].TileFrameY, 16, 16), Lighting.GetColor(i, j));

                    }
                    else if (Winter)
                    {
                        spriteBatch.Draw(WinterGrass, new Vector2(i + 12, j + 12) * 16 - Main.screenPosition, new Rectangle(Main.tile[i, j].TileFrameX, Main.tile[i, j].TileFrameY, 16, 16), Lighting.GetColor(i, j));

                    }

                   

                    return false;
                }

                return true;
            }
            
        }
    }
}
