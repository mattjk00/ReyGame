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
        Normal = 1,
        PlayerStart = 2,
        Block = 3
    }

    /// <summary>
    /// a world tile
    /// </summary>
    [Serializable]
    public class Tile : GameObject
    {
        public TileType TileType { get; protected set; }
        public Rectangle Box { get; protected set; }
        public Rectangle LeftBox { get; protected set; }
        public Rectangle RightBox { get; protected set; }
        public Rectangle TopBox { get; protected set; }
        public Rectangle BottomBox { get; protected set; }

        /// <summary>
        /// Constructor for making tile a bit easier to make
        /// </summary>
        /// <param name="position"></param>
        /// <param name="texture"></param>
        public Tile(string name, Vector2 position, Texture2D texture, TileType typ)
        {
            this.Transform.Position = position;
            this.Sprite.Texture = texture;
            this.TileType = typ;
            this.Name = name;
        }

        public override void Update()
        {
            base.Update();

            // update the bounding box
            this.Box = new Rectangle((int)this.Transform.Position.X, (int)this.Transform.Position.Y, this.Sprite.Texture.Width, this.Sprite.Texture.Height);

            this.TopBox = new Rectangle((int)this.Transform.Position.X + 10, (int)this.Transform.Position.Y, this.Sprite.Texture.Width - 20, 1);
            this.BottomBox = new Rectangle((int)this.Transform.Position.X + 5, (int)this.Transform.Position.Y + this.Sprite.Texture.Height - 1, this.Sprite.Texture.Width - 10, 1);
            this.LeftBox = new Rectangle((int)this.Transform.Position.X, (int)this.Transform.Position.Y + 2, 1, this.Sprite.Texture.Height - 4);
            this.RightBox = new Rectangle((int)this.Transform.Position.X + (int)this.Sprite.Texture.Width - 1, (int)this.Transform.Position.Y + 2, 1, this.Sprite.Texture.Height - 4);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this.Sprite.Texture, this.Transform.Position, new Rectangle(0, 0, 50, 50), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }

        public void SetType(TileType tt)
        {
            this.TileType = tt;
        }
    }
}
