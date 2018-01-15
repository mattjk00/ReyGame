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

        public override void Update()
        {
            this.UpdateDefaultBox(0);
            base.Update();
        }

        public override void DrawUI(SpriteBatch sb, Frame frame)
        {
            base.DrawUI(sb, frame);
            sb.DrawString(AssetLoader.Font, this.Text, this.Transform.Position, this.Sprite.Color);
            
        }
    }
}
