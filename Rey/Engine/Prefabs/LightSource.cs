using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    /// <summary>
    /// Holds the data for a light source in the map.
    /// </summary>
    public class LightSource
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Color Color { get; set; }

        public LightSource(Vector2 pos, Vector2 s, Color c)
        {
            this.Position = pos;
            this.Size = s;
            this.Color = c;
        }
    }
}
