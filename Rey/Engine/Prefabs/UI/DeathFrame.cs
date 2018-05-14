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

        public override void Load()
        {
            this.Active = false;
            this.LockedPosition = true;

            // set the size
            this.Width = AssetLoader.Graphics.PreferredBackBufferWidth / 2;
            this.Height = AssetLoader.Graphics.PreferredBackBufferHeight / 2;
            this.Position = new Vector2(AssetLoader.Graphics.PreferredBackBufferWidth/4, AssetLoader.Graphics.PreferredBackBufferHeight/4);

            this.Background = AssetLoader.LoadTexture("Assets/Textures/UI/death_background.png");

            restartButton.LoadTextures("Assets/Textures/UI/main_menu_button_normal.png", "Assets/Textures/UI/main_menu_button_hover.png");
            restartButton.Text = "Restart?";
            restartButton.LocalPosition = new Vector2(25, 200);
            restartButton.OnClick += () => 
            {
                // try this out
                
                SceneManager.SetScene("test");
                var scene = SceneManager.GetCurrentScene() as TestScene;
                scene.LoadMap(true, "LAST"); // load scene at last door
            };
            this.AddObject(restartButton);

            base.Load();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
