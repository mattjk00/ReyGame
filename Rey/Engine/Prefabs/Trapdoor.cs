using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Rey.Engine.Prefabs
{
    public class Trapdoor : GameObject
    {
        public bool Open { get; set; } = false; // tells whether the trapdoor is open or closed

        Texture2D closedTexture;
        Texture2D openTexture;

        /// <summary>
        /// Set the player's properties
        /// </summary>
        public override void Load()
        {
            this.Name = "trapdoor";

            this.closedTexture = AssetLoader.LoadTexture("Assets/Textures/backgrounds/trapdoor.png");
            this.openTexture = AssetLoader.LoadTexture("Assets/Textures/backgrounds/open_trapdoor.png");
            this.Sprite.Texture = this.closedTexture; // load the player texture
            this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
            this.AddDefaultBoundingBox();
            this.Transform.Position = new Vector2(1280 / 2, 720 / 2);
            this.IsEnemy = false;
        }

        public override void Update()
        {
            if (this.Open)
                this.Sprite.Texture = this.openTexture;
            else
                this.Sprite.Texture = this.closedTexture;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(this.Sprite.Texture, this.Transform.Position, this.Sprite.Color);
        }
    }
}
