using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    public class Trapdoor : GameObject
    {
        /// <summary>
        /// Set the player's properties
        /// </summary>
        public override void Load()
        {
            this.Name = "trapdoor";

            this.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/backgrounds/trapdoor.png"); // load the player texture
            this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
            this.AddDefaultBoundingBox();
            this.Transform.Position = new Vector2(1280 / 2, 720 / 2);
            this.IsEnemy = false;
        }
    }
}
