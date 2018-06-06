using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs.UI
{
    /// <summary>
    /// Displays when the user pauses the game
    /// </summary>
    public class PauseFrame : Frame
    {
        Button resumeButton = new Button("resume");
        Button quitToMenuButton = new Button("quitToMenu");
        Button quitToDesktopButton = new Button("quitToDesktop");

        public override void Load()
        {
            this.Active = false;
            this.LockedPosition = true;

            this.Background = AssetLoader.LoadTexture("Assets/Textures/UI/pause_back.png");

            // set the size
            this.Width = AssetLoader.Graphics.PreferredBackBufferWidth;
            this.Height = AssetLoader.Graphics.PreferredBackBufferHeight;
            this.Position = new Vector2(0, 0);

            resumeButton.LoadTextures("Assets/Textures/UI/main_menu_button_normal2.png", "Assets/Textures/UI/main_menu_button_hover2.png");
            resumeButton.Text = "Resume";
            resumeButton.LocalPosition = new Vector2(this.Width/3, 200);
            resumeButton.TextColor = Color.Black;
            resumeButton.OnClick += () => {
                this.Active = false;
                SceneManager.SoundManager.PlaySound("ui", 0.15f, 1.0f, 0.0f);
            };

            quitToMenuButton.LoadTextures("Assets/Textures/UI/main_menu_button_normal2.png", "Assets/Textures/UI/main_menu_button_hover2.png");
            quitToMenuButton.Text = "Save & Quit to Menu";
            quitToMenuButton.LocalPosition = new Vector2(this.Width / 3, 300);
            quitToMenuButton.TextColor = Color.Black;
            quitToMenuButton.OnClick += () => {
                this.Active = false;
                // add save feature
                SceneManager.TransitionToScene("mainmenu");
                SceneManager.SoundManager.PlaySound("ui", 0.15f, 1.0f, 0.0f);
            };

            quitToDesktopButton.LoadTextures("Assets/Textures/UI/main_menu_button_normal2.png", "Assets/Textures/UI/main_menu_button_hover2.png");
            quitToDesktopButton.Text = "Quit to Desktop";
            quitToDesktopButton.LocalPosition = new Vector2(this.Width / 3, 400);
            quitToDesktopButton.TextColor = Color.Black;
            quitToDesktopButton.OnClick += () => {
                this.Active = false;
                // add save feature
                SceneManager.Quit = true;
                SceneManager.SoundManager.PlaySound("ui", 0.15f, 1.0f, 0.0f);
            };

            this.AddObject(resumeButton);
            this.AddObject(quitToMenuButton);
            this.AddObject(quitToDesktopButton);

            base.Load();
        }
    }
}
