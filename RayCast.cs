using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Atlas
{

    public class RayCast
    {
        public enum Layers
        {
            Tiles,
            Projectiles,
            NPCs,
        }

        public static bool DoesTileCollideWithLine(Vector2 start, Vector2 end)
        {
            for (int i = 0; i < (end - start).Length(); i++)
            {
                Vector2 tile = Vector2.Lerp(start, end, i / (end - start).Length());
                if (!WorldGen.TileEmpty((int)tile.X / 16, (int)tile.Y / 16) && WorldGen.SolidTile((int)tile.X / 16, (int)tile.Y / 16))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DoesTileColliderWithLine(Vector2 start, Vector2 direction, float distance)
        {
            direction.Normalize();
            Vector2 end = direction * distance;
            for (int i = 0; i < (end - start).Length(); i++)
            {
                Vector2 tile = Vector2.Lerp(start, end, i / (end - start).Length());
                if (!WorldGen.TileEmpty((int)tile.X / 16, (int)tile.Y / 16) && WorldGen.SolidTile((int)tile.X / 16, (int)tile.Y / 16))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DoesProjCollideWithLine(Vector2 start, Vector2 end)
        {
            for (int i = 0; i < (end - start).Length(); i++)
            {
                Vector2 projectile = Vector2.Lerp(start, end, i / (end - start).Length());
                if (Main.projectile.Where(proj => proj.getRect().Contains(projectile.ToPoint())).Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DoesProjCollideWithLine(Vector2 start, Vector2 direction, float distance)
        {
            direction.Normalize();
            Vector2 end = direction * distance;
            for (int i = 0; i < (end - start).Length(); i++)
            {
                Vector2 projectile = Vector2.Lerp(start, end, i / (end - start).Length());
                if (Main.projectile.Where(proj => proj.getRect().Contains(projectile.ToPoint())).Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DoesNPCCollideWithLine(Vector2 start, Vector2 end)
        {
            for (int i = 0; i < (end - start).Length(); i++)
            {
                Vector2 NPC = Vector2.Lerp(start, end, i / (end - start).Length());
                if (Main.npc.Where(npc => npc.getRect().Contains(NPC.ToPoint())).Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DoesNPCCollideWithLine(Vector2 start, Vector2 direction, float distance)
        {
            direction.Normalize();
            Vector2 end = direction * distance;
            for (int i = 0; i < (end - start).Length(); i++)
            {
                Vector2 NPC = Vector2.Lerp(start, end, i / (end - start).Length());
                if (Main.npc.Where(npc => npc.getRect().Contains(NPC.ToPoint())).Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DoesLayerCollideWithLine(Vector2 start, Vector2 end, Layers layer)
        {
            if (layer == Layers.Tiles)
            {
                for (int i = 0; i < (end - start).Length(); i++)
                {
                    Vector2 tile = Vector2.Lerp(start, end, i / (end - start).Length());
                    if (!WorldGen.TileEmpty((int)tile.X / 16, (int)tile.Y / 16) && WorldGen.SolidTile((int)tile.X / 16, (int)tile.Y / 16))
                    {
                        return true;
                    }
                }
            }
            else if (layer == Layers.Projectiles)
            {
                for (int i = 0; i < (end - start).Length(); i++)
                {
                    Vector2 projectile = Vector2.Lerp(start, end, i / (end - start).Length());
                    if (Main.projectile.Where(proj => proj.getRect().Contains(projectile.ToPoint())).Count() > 0)
                    {
                        return true;
                    }
                }
            }
            else if (layer == Layers.NPCs)
            {
                for (int i = 0; i < (end - start).Length(); i++)
                {
                    Vector2 NPC = Vector2.Lerp(start, end, i / (end - start).Length());
                    if (Main.npc.Where(npc => npc.getRect().Contains(NPC.ToPoint())).Count() > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool DoesLayerCollideWithLine(Vector2 start, Vector2 direction, float distance, Layers layer)
        {
            direction.Normalize();
            Vector2 end = direction * distance;
            if (layer == Layers.Tiles)
            {
                for (int i = 0; i < (end - start).Length(); i++)
                {
                    Vector2 tile = Vector2.Lerp(start, end, i / (end - start).Length());
                    if (!WorldGen.TileEmpty((int)tile.X / 16, (int)tile.Y / 16) && WorldGen.SolidTile((int)tile.X / 16, (int)tile.Y / 16))
                    {
                        return true;
                    }
                }
            }
            else if (layer == Layers.Projectiles)
            {
                for (int i = 0; i < (end - start).Length(); i++)
                {
                    Vector2 projectile = Vector2.Lerp(start, end, i / (end - start).Length());
                    if (Main.projectile.Where(proj => proj.getRect().Contains(projectile.ToPoint())).Count() > 0)
                    {
                        return true;
                    }
                }
            }
            else if (layer == Layers.NPCs)
            {
                for (int i = 0; i < (end - start).Length(); i++)
                {
                    Vector2 NPC = Vector2.Lerp(start, end, i / (end - start).Length());
                    if (Main.npc.Where(npc => npc.getRect().Contains(NPC.ToPoint())).Count() > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
