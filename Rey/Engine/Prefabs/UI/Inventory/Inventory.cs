using Microsoft.Xna.Framework;
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
        BackpackFrame backpack = new BackpackFrame();

        public override void Load()
        {
            this.Name = "inventory";

            this.Width = 250;
            this.Height = 400;
            this.LockedPosition = true;

            this.Position = new Vector2(InputHelper.GDM.PreferredBackBufferWidth - 250, InputHelper.GDM.PreferredBackBufferHeight - 380);//new Microsoft.Xna.Framework.Vector2(1030, 320);
            this.Background = AssetLoader.LoadTexture("Assets/Textures/UI/ui_back.png");

            this.AddTab(backpack, "Assets/Textures/UI/main_menu_button_normal.png", "Assets/Textures/UI/main_menu_button_hover.png");

            base.Load();
        }
    }
}
