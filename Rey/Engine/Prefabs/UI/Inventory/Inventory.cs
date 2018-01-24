using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs.UI.Inventory
{
    public class Inventory : TabFrame
    {
        public override void Load()
        {
            this.Name = "inventory";

            this.Width = 250;
            this.Height = 400;
            this.LockedPosition = true;

            this.Position = new Microsoft.Xna.Framework.Vector2(1030, 320);
            this.Background = AssetLoader.LoadTexture("Assets/Textures/UI/ui_back.png");

            base.Load();
        }
    }
}
