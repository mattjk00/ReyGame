using Microsoft.Xna.Framework;
using Rey.Engine;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor
{
    public class OptionFrame : Frame
    {
        public override void Load()
        {
            this.Name = "options";
            this.Scrollable = false;
            this.Height = 100;
            this.Width = 200;
            this.Position = new Vector2(1580, 50);
            this.Background = AssetLoader.LoadTexture("Textures/ui/options.png");

            base.Load();
        }
    }
}
