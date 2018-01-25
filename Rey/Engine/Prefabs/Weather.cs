using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    public enum WeatherType
    {
        Rain,
        Snow
    }

    public class Weather : GameObject
    {
        private List<GameObject> drops = new List<GameObject>(); // rain drops, snow flakes, etc.
        public int Intensity { get; set; } = 0;
        public WeatherType WeatherType { get; set; } = WeatherType.Rain;
        private int timer = 0;

        // textures
        private Texture2D rainTexture;

        public override void Load()
        {
            rainTexture = AssetLoader.LoadTexture("Assets/textures/weather/rain_drop.png");

            this.Sprite.Texture = AssetLoader.LoadTexture("Assets/textures/weather/fog.png");
            this.Sprite.Color = Color.White * 0.5f;

            base.Load();
        }

        public override void Update()
        {
            timer++;

            Random rand = new Random();

            // if it is time
            if (timer >= 1 + this.Intensity * 10)
            {
                // if it shoudl rain.
                if (this.WeatherType == WeatherType.Rain)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        var go = new GameObject("rain", new Vector2(rand.Next(0, 1280), -15));
                        go.Transform.Rotation = 90 + MathHelper.ToRadians(rand.Next(-15, 15));
                        go.Sprite.Texture = this.rainTexture;
                        go.Sprite.Color = Color.White * 0.6f;
                        go.Load();

                        this.drops.Add(go);
                    }
                }
                timer = 0;
            }

            foreach (GameObject drop in this.drops)
            {
                Vector2 dir = new Vector2((float)Math.Cos(drop.Transform.Rotation), (float)Math.Sin(drop.Transform.Rotation));
                dir.Normalize();
                drop.Transform.Position += dir * 20;

                if (drop.Transform.Position.Y > 720)
                    drop.ToBeDestroyed = true;
            }

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            // don't do anything here!!
        }

        public void SecondDraw(SpriteBatch sb)
        {
            base.Draw(sb);

            foreach(GameObject drop in this.drops)
            {
                //drop.Draw(sb);
                sb.Draw(drop.Sprite.Texture, drop.Transform.Position, Color.White);
            }

            //base.Draw(sb);
        }
    }
}
