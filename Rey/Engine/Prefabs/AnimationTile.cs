using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    public class AnimationTile : Tile
    {
        public Vector2 FrameSize { get; set; }
        public int AnimationSpeed { get; set; }
        public int FrameCount { get; set; }
        private int timer = 0;
        private int currentFrame = 0;

        public AnimationTile(string name, Vector2 position, Texture2D texture, TileType typ) :base(name, position, texture, typ)
        {
            this.FrameCount = (texture.Width / 50) - 1;
            this.FrameSize = new Vector2(50, 50);
            this.AnimationSpeed = 5;
            this.Box = new Rectangle(this.Transform.Position.ToPoint(), new Point(50, 50));
        }

        /// <summary>
        /// Animates by going through each frame of the spritesheet
        /// </summary>
        /// <param name="speed"></param>
        public void Animate(int speed = -1)
        {
            // change optional animation speed if too slow
            if (speed <= 0)
                speed = this.AnimationSpeed;

            this.timer++;
            if (this.timer > this.AnimationSpeed)
            {
                this.currentFrame++;
                if (this.currentFrame > this.FrameCount)
                {
                    this.currentFrame = 0;
                }
                this.timer = 0;
            }

            this.Transform.Bounds = new Rectangle(this.currentFrame * (int)this.FrameSize.X, 0, (int)this.FrameSize.X, (int)this.FrameSize.Y); // set the sprite bounds to match the animation
        }

        // sets to a certain frame
        public void SetFrame(int frame)
        {
            this.currentFrame = frame;
            this.Transform.Bounds = new Rectangle(this.currentFrame * (int)this.FrameSize.X, 0, (int)this.FrameSize.X, (int)this.FrameSize.Y); // set the sprite bounds to match the animation
        }

        public override void Update()
        {
            this.Animate();
            //base.Update();

            this.Box = new Rectangle((int)this.Transform.Position.X, (int)this.Transform.Position.Y, (int)this.FrameSize.X, (int)this.FrameSize.Y);
            var a = 0;
            this.TopBox = new Rectangle((int)this.Transform.Position.X + 5, (int)this.Transform.Position.Y, (int)this.FrameSize.X - 10, 1);
            this.BottomBox = new Rectangle((int)this.Transform.Position.X + 5, (int)this.Transform.Position.Y + (int)this.FrameSize.Y - 1, (int)this.FrameSize.X - 10, 1);
            this.LeftBox = new Rectangle((int)this.Transform.Position.X, (int)this.Transform.Position.Y + 2, 1, (int)this.FrameSize.Y - 4);
            this.RightBox = new Rectangle((int)this.Transform.Position.X + (int)this.FrameSize.X - 1, (int)this.Transform.Position.Y + 2, 1, (int)this.FrameSize.Y - 4);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this.Sprite.Texture, this.Transform.Position, this.Transform.Bounds, this.Sprite.Color);
        }
    }
}
