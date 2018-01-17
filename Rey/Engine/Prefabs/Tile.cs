using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    /// <summary>
    /// The type of tile 
    /// </summary>
    public enum TileType
    {
        Empty = 0,
        Normal = 1
    }

    /// <summary>
    /// a world tile
    /// </summary>
    public class Tile : GameObject
    {
        public TileType TileType { get; protected set; }

        /// <summary>
        /// Constructor for making tile a bit easier to make
        /// </summary>
        /// <param name="position"></param>
        /// <param name="texture"></param>
        public Tile(Vector2 position, Texture2D texture, TileType typ)
        {
            this.Transform.Position = position;
            this.Sprite.Texture = texture;
            this.TileType = typ;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this.Sprite.Texture, this.Transform.Position, new Rectangle(0, 0, 50, 50), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
    }
}
