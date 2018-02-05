using Microsoft.Xna.Framework;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs.UI.Inventory
{
    public class BackpackFrame : Frame
    {
        int backpackSize = 25;

        public override void Load()
        {
            this.Name = "backpack";

            this.Width = 250;
            this.Height = 400;
            this.LockedPosition = true;
            this.Background = AssetLoader.LoadTexture("Assets/Textures/ui/backpack_grid.png");

            int columnCount = 0;
            
            int rowCount = 0;
            for (int i = 0; i < backpackSize; i++)
            {
                
                var newBackpackButton = new ItemButton("slot" + i.ToString())
                {
                    
                };
                newBackpackButton.LocalPosition = new Vector2((50 * rowCount), 1 + (80 * columnCount));
                newBackpackButton.LoadTextures("Assets/Textures/ui/backpack_space.png", "Assets/Textures/ui/backpack_space_hover.png");
                newBackpackButton.Sprite.Color = Color.White * 0.3f;
                this.AddObject(newBackpackButton);

                rowCount++;
                if (rowCount == 5)
                {
                    rowCount = 0;
                    columnCount++;
                }
            }

            base.Load();
        }

        public override void Update()
        {
            // iterate through backpack
            for (int index = 0; index < GameData.backpack.Count; index++)
            {
                // find the slot
                var slot = this.objects[index] as ItemButton;
                slot.Item = GameData.backpack[index]; // set the item slot
            }

            base.Update();
        }
    }
}
