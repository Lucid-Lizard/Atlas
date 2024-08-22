using Atlas.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Common.Prims
{
    public class GenericPrimTrail : PrimTrail
    {
        public GenericPrimTrail(Color color, Vector2[] points, float width, bool widthFallOff = true, Effect shader = default, bool pixelated = false)
        {
            Color = color;
            Points = points;
            Width = width;
            WidthFallOff = widthFallOff;
            Shader = shader;
            this.pixelated = pixelated;
            Initialize();
        }
    }
}
