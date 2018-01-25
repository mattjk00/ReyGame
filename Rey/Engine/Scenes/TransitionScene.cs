using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Rey.Engine.Scenes
{
    public class TransitionScene : Scene
    {
        GameObject screen = new GameObject("screen");
        private string nextScene = "";
        private int colorMultiplier = 9; // what to add to the color
        private float fadeDelay = 1;
        private float fadeTimer = 0;

        public TransitionScene(string name) : base(name) { }

        public override void Load()
        {
            //this.Name = "transition";
            this.CombatScene = false;

            base.Load();
        }

        public void LoadNewTransition(string nxtScene)
        {

            this.nextScene = nxtScene;

            //this.Background = AssetLoader.LoadTexture("Assets/Textures/backgrounds/transition_default.png");

            screen.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/UI/transition_default.png");
            screen.Sprite.Color = new Color(5, 5, 5, 1);
            //this.AddGameObject(screen);

            base.Load();
            //enemy.SetInterval(1000);

        }

        public override void Update(Camera2D camera)
        {
            //screen.Sprite.Color *= 0.97f;
            //screen.Sprite.Color = new Color(255, 255, 255, screen.Sprite.Color.A + this.colorMultiplier);
            this.fadeTimer++;

            // if ready to incrememnt fade
            if (this.fadeTimer > this.fadeDelay)
            {
                screen.Sprite.Color = new Color(screen.Sprite.Color.R + this.colorMultiplier, screen.Sprite.Color.G + this.colorMultiplier, screen.Sprite.Color.B + this.colorMultiplier, screen.Sprite.Color.A + this.colorMultiplier);
                this.fadeTimer = 0;

            }

            if (screen.Sprite.Color.A == 255)
            {
                this.colorMultiplier *= -1;
                screen.Sprite.Color = new Color(255, 255, 255, screen.Sprite.Color.A -1 ); // subtract one
                SceneManager.DrawLastScene = false; // don't draw the last scene

            }

            // after the loading screen is faded
            if (screen.Sprite.Color.A == 0)
            {
                // go to the next scene
                SceneManager.SetScene(this.nextScene);
                this.screen.Sprite.Color = Color.White * 0;
                this.colorMultiplier = Math.Abs(this.colorMultiplier); // reset color adder
            }

            base.Update(camera);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(screen.Sprite.Texture, Vector2.Zero, screen.Sprite.Color);
        }
    }
}
