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

        public MainMenuScene(string name) : base(name) { }

        public override void Load()
        {
            //this.Name = "mainmenu";
            this.CombatScene = false;
            this.Background = AssetLoader.LoadTexture("Assets/Textures/UI/mainmenu2.png");

            this.ui = new Frame();
            this.ui.Scrollable = false;
            this.ui.Position = new Vector2(1280-500, 720/2 - 200);
            this.ui.Width = 250;
            this.ui.Height = 200;
            //this.ui.Background = AssetLoader.LoadTexture("Assets/Textures/UI/ui_back.png");

            // the start button
            this.startButton = new Button("start");
            this.startButton.LoadTextures("Assets/Textures/UI/main_menu_button_normal2.png", "Assets/Textures/UI/main_menu_button_hover2.png");
            this.startButton.Text = "Start!";
            this.startButton.TextColor = Color.Black;
            this.startButton.LocalPosition = new Vector2(45, 10);
            this.startButton.OnClick += () =>
            {
                MemoryManager.Load();
                //SceneManager.TransitionToScene("test");
            };

            // the quit button
            this.quitButton = new Button("quit");
            this.quitButton.LoadTextures("Assets/Textures/UI/main_menu_button_normal2.png", "Assets/Textures/UI/main_menu_button_hover2.png");
            this.quitButton.Text = "Quit!";
            this.quitButton.TextColor = Color.Black;
            this.quitButton.LocalPosition = new Vector2(45, 70);
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
