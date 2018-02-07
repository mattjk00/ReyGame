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
        EquipmentFrame equipment = new EquipmentFrame();

        public override void Load()
        {
            this.Name = "inventory";

            this.Width = 250;
            this.Height = 400;
            this.LockedPosition = true;

            this.Position = new Vector2(InputHelper.GDM.PreferredBackBufferWidth - 250, InputHelper.GDM.PreferredBackBufferHeight - 380);//new Microsoft.Xna.Framework.Vector2(1030, 320);
            this.Background = AssetLoader.LoadTexture("Assets/Textures/UI/ui_back.png");

            backpack.Name = "Backpack";
            equipment.Name = "Equipment";

            this.AddTab(backpack, "Assets/Textures/UI/invetory_bar.png", "Assets/Textures/UI/invetory_bar_hover.png");
            this.AddTab(equipment, "Assets/Textures/UI/invetory_bar.png", "Assets/Textures/UI/invetory_bar_hover.png");
            base.Load();
        }
    }
}
