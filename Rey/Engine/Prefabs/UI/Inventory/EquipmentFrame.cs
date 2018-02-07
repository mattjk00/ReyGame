using Microsoft.Xna.Framework;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Rey.Engine.Prefabs.UI.Inventory
{
    public class EquipmentFrame : Frame
    {
        Button helmetButton = new Button("helmet");
        Button chestButton = new Button("chest");

        public override void Load()
        {
            this.Name = "equipment";

            this.Width = 250;
            this.Height = 400;
            this.LockedPosition = true;
            //this.Background = AssetLoader.LoadTexture("Assets/Textures/ui/backpack_grid.png");

            helmetButton.LoadTextures("Assets/Textures/ui/equipment_slot.png", "Assets/Textures/ui/equipment_slot_hover.png");
            helmetButton.OnClick += () =>
            {
                // unequip the helmet
                if (GameData.EquippedHelmet.ID != null)
                {
                    GameData.AddItemToBackpack(GameData.EquippedHelmet);
                    GameData.EquippedHelmet = new Item();
                }
            };
            helmetButton.LocalPosition = new Vector2(100, 50);
            this.AddObject(helmetButton);

            chestButton.LoadTextures("Assets/Textures/ui/equipment_slot.png", "Assets/Textures/ui/equipment_slot_hover.png");
            chestButton.OnClick += () =>
            {
                // unequip the helmet
                if (GameData.EquippedChest.ID != null)
                {
                    GameData.AddItemToBackpack(GameData.EquippedChest);
                    GameData.EquippedChest = new Item();
                }
            };
            chestButton.LocalPosition = new Vector2(100, 125);
            this.AddObject(chestButton);

            base.Load();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            // draw the helmet in the equipment screen if valid
            if (GameData.EquippedHelmet.ID != null)
                sb.Draw(GameData.EquippedHelmet.Texture, helmetButton.Transform.Position, null, Color.White, 0, Vector2.Zero, 
                    InputHelper.ScaleTexture(GameData.EquippedHelmet.Texture, 50, 50), SpriteEffects.None, 0);

            // draw the chest in the equipment screen if valid
            if (GameData.EquippedChest.ID != null)
                sb.Draw(GameData.EquippedChest.Texture, chestButton.Transform.Position, null, Color.White, 0, Vector2.Zero,
                    InputHelper.ScaleTexture(GameData.EquippedChest.Texture, 50, 50), SpriteEffects.None, 0);
        }
    }
}
