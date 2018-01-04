using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    public class Healthbar : ChildObject
    {
        private Texture2D greenbar;
        private EntityStats entityStats;

        public void AssignStats(EntityStats stats)
        {
            this.entityStats = stats;
        }

        public override void Load()
        {
            // load the green health bar
            greenbar = AssetLoader.LoadTexture("assets/textures/ui/healthbar_green.png");

            this.Sprite.Texture = AssetLoader.LoadTexture("assets/textures/ui/healthbar_red.png");

            base.Load();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            var percentHealth = (float)(this.entityStats.HP) / (float)(this.entityStats.MaxHP);
            var barWidth = percentHealth * this.greenbar.Width;

            // draw the green bar on top
            sb.Draw(greenbar, this.Transform.Position, 
                new Rectangle(0, 0, (int)barWidth, greenbar.Height),
                Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
    }
}
