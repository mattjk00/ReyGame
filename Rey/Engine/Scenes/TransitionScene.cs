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

        public override void Load()
        {
            this.Name = "transition";
            this.CombatScene = false;

            base.Load();
        }

        public void LoadNewTransition(string nxtScene)
        {

            this.nextScene = nxtScene;

            //this.Background = AssetLoader.LoadTexture("Assets/Textures/backgrounds/transition_default.png");

            screen.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/UI/transition_default.png");
            //this.AddGameObject(screen);

            base.Load();
            //enemy.SetInterval(1000);

        }

        public override void Update()
        {
            screen.Sprite.Color *= 0.97f;

            // after the loading screen is faded
            if (screen.Sprite.Color.A == 0)
            {
                // go to the next scene
                SceneManager.SetScene(this.nextScene);
                this.screen.Sprite.Color = Color.White;
            }

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(screen.Sprite.Texture, Vector2.Zero, screen.Sprite.Color);
        }
    }
}
