using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rey.Engine
{
    /// <summary>
    /// Holds information for the game object's Texture and other rendering attributes
    /// </summary>
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }

        public Sprite()
        {
            this.Color = Color.White;
        }

        /// <summary>
        /// Allows for inputting of texture and Color
        /// </summary>
        /// <param name="t"></param>
        /// <param name="c"></param>
        public Sprite(Texture2D t, Color c)
        {
            this.Texture = t;
            this.Color = c;
        }
    }
}
