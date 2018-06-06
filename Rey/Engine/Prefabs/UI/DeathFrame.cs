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
    // displays when you have died
    public class DeathFrame : Frame
    {
        Button restartButton = new Button("restart");
        Label messageLbl = new Label("message");

        public override void Load()
        {
            this.Active = false;
            this.LockedPosition = true;

            // set the size
            this.Width = 500;
            this.Height = 500;
            this.Position = new Vector2(1280/2-250, 720/2-250);

            this.Background = AssetLoader.LoadTexture("Assets/Textures/UI/ui_back3.png");

            restartButton.LoadTextures("Assets/Textures/UI/main_menu_button_normal2.png", "Assets/Textures/UI/main_menu_button_hover2.png");
            restartButton.Text = "Restart?";
            restartButton.LocalPosition = new Vector2(25, 200);
            restartButton.TextColor = Color.Black;
            restartButton.OnClick += () => 
            {
                // try this out
                
                SceneManager.SetScene("test");
                var scene = SceneManager.GetCurrentScene() as TestScene;
                scene.LoadMap(true, "LAST"); // load scene at last door
            };
            this.AddObject(restartButton);

            messageLbl.Text = "Rey has died!";
            messageLbl.LocalPosition = new Vector2(25, 50);
            messageLbl.TextColor = Color.Red;
            messageLbl.TextScale = 1.0f;
            this.AddObject(messageLbl);

            base.Load();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
