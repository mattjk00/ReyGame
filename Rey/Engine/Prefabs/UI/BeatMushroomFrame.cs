using Microsoft.Xna.Framework;
using Rey.Engine.Scenes;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs.UI
{
    class BeatMushroomFrame : Frame
    {
        Button okButton = new Button("ok");
        Label msgLbl = new Label("msg");

        public override void Load()
        {
            this.Active = false;
            this.LockedPosition = true;

            this.Background = AssetLoader.LoadTexture("Assets/Textures/UI/ui_back2.png");

            // set the size
            this.Width = 500;
            this.Height = 500;
            this.Position = new Vector2(1280 / 2 - 250, 720 / 2 - 250);

            okButton.LoadTextures("Assets/Textures/UI/main_menu_button_normal2.png", "Assets/Textures/UI/main_menu_button_hover2.png");
            okButton.Text = "Ok";
            okButton.LocalPosition = new Vector2(this.Width / 3, 200);
            okButton.TextColor = Color.Black;
            okButton.OnClick += () => {
                this.Active = false;
                var ts = SceneManager.GetCurrentScene() as TestScene; // this frame should only appear in test scenes
                ts.GoToMap(true, "Assets/rey_house.guat");
                SceneManager.SoundManager.PlaySound("ui", 0.15f, 1.0f, 0.0f);
            };

            msgLbl.Text = "You have defeated the Mushroom King and recovered a \npiece of the Sword of Beldor!! Congratulations.\nYou will now be teleported back to your home";
            msgLbl.LocalPosition = new Vector2(5, 5);

            this.AddObject(msgLbl);
            this.AddObject(okButton);

            base.Load();
        }
    }
}
