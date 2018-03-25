using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.UI
{
    public class Label : UIObject
    {
        public string Text { get; set; } = "PLACEHOLDER";
        public Color TextColor { get; set; } = Color.Black;
        public float TextScale { get; set; } = 1.0f;

        public Label(string name) : base(name)

        {
            this.BoundingBoxes.Add(new Rectangle(0, 0, 1, 1));
        }

        public override void DrawUI(SpriteBatch sb, Frame frame)
        {
            
            // try to draw text
            if (AssetLoader.Font != null)
                sb.DrawString(AssetLoader.Font, this.Text, this.Transform.Position, this.TextColor, 0, Vector2.Zero, this.TextScale, SpriteEffects.None, 0);

        }
    }
}
