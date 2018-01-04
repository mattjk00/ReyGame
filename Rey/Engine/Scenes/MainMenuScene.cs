using Microsoft.Xna.Framework;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Scenes
{
    /// <summary>
    /// the main menu
    /// </summary>
    public class MainMenuScene : Scene
    {
        private Frame ui;
        private Button startButton;
        private Button quitButton;

        public override void Load()
        {
            this.Name = "mainmenu";
            this.CombatScene = false;
            this.Background = AssetLoader.LoadTexture("Assets/Textures/UI/main_menu.png");

            this.ui = new Frame();

            // the start button
            this.startButton = new Button("start");
            this.startButton.LoadTextures("Assets/Textures/UI/main_menu_button_normal.png", "Assets/Textures/UI/main_menu_button_hover.png");
            this.startButton.Text = "Start!";
            this.startButton.Transform.Position = new Vector2(640, 360);
            this.startButton.OnClick += () =>
            {
                SceneManager.TransitionToScene("test");
            };

            // the quit button
            this.quitButton = new Button("quit");
            this.quitButton.LoadTextures("Assets/Textures/UI/main_menu_button_normal.png", "Assets/Textures/UI/main_menu_button_hover.png");
            this.quitButton.Text = "Quit!";
            this.quitButton.Transform.Position = new Vector2(640, 420);
            this.quitButton.OnClick += () =>
            {
                SceneManager.Quit = true;
            };

            // add the buttons and load
            this.ui.AddObject(this.startButton);
            this.ui.AddObject(this.quitButton);
            this.ui.Load();

            this.AddFrame(this.ui);
            base.Load();
        }
    }
}
