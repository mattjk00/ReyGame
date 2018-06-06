using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.UI
{
    public class Button : UIObject
    {
        public Texture2D normalTexture { get; protected set; }
        public Texture2D hoverTexture { get; protected set; }
        public string Text { get; set; } = "";
        public Color TextColor { get; set; } = Color.White;

        /// <summary>
        /// Set up the default button actions
        /// </summary>
        public Button(string name) : base(name)
        {
            this.OnHover += () =>
            {
                this.Sprite.Texture = this.hoverTexture;
            };
            this.OnMouseLeave += () =>
            {
                this.Sprite.Texture = this.normalTexture;
            };
            this.OnClick += () => { };
            
        }

        /// <summary>
        /// Loads the buttons textures
        /// </summary>
        /// <param name="norm"></param>
        /// <param name="hover"></param>
        public void LoadTextures(string norm, string hover)
        {
            this.normalTexture = AssetLoader.LoadTexture(norm);
            this.hoverTexture = AssetLoader.LoadTexture(hover);
            this.Sprite.Texture = normalTexture;
            this.AddDefaultBoundingBox();

        }

        /// <summary>
        /// Loads the buttons textures
        /// </summary>
        /// <param name="norm"></param>
        /// <param name="hover"></param>
        public void LoadTextures(Texture2D norm, Texture2D hover)
        {
            this.normalTexture = norm;
            this.hoverTexture = hover;
            this.Sprite.Texture = normalTexture;
            this.AddDefaultBoundingBox();

        }

        public override void Update()
        {
            //this.UpdateDefaultBox(0);
            base.Update();

            
        }

        public override void DrawUI(SpriteBatch sb, Frame frame)
        {
            if (AssetLoader.Font.MeasureString(this.Text).X > this.Sprite.Texture.Width)
                base.DrawUI(sb, frame, (int)AssetLoader.Font.MeasureString(this.Text).X, this.Sprite.Texture.Height);
            else
                base.DrawUI(sb, frame);
            // try to draw text
            if (AssetLoader.Font != null)
                sb.DrawString(AssetLoader.Font, this.Text, new Vector2(this.Transform.Position.X + this.Sprite.Texture.Width/2 - AssetLoader.Font.MeasureString(this.Text).X/2, this.Transform.Position.Y + this.Sprite.Texture.Height/2 - AssetLoader.Font.MeasureString(this.Text).Y / 2), this.TextColor);
            
        }
    }
}
