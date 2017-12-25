using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rey.Engine.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public class ProjectileManager
    {
        public List<Projectile> Projectiles { get; private set; } = new List<Projectile>();

        // creates and sends a new projectile at a position
        public void ShootNew(Vector2 from, Vector2 to)
        {
            // create a projectile and add it to the projectile array
            var proj = new Projectile();
            proj.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Projectiles/wind_strike.png");
            proj.Load();
            proj.Transform.Position = from;
            proj.LookAt(to);
            this.Projectiles.Add(proj);
        }

        public void Update()
        {
            foreach (Projectile proj in this.Projectiles)
            {
                proj.Update();
                proj.UpdateDefaultBox(0);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Projectile proj in this.Projectiles)
            {
                sb.Draw(proj.Sprite.Texture, proj.Transform.Position, null, Color.White, proj.Transform.Rotation, proj.Transform.Origin, 1.0f, SpriteEffects.None, 0);
                //  sb.Draw(this.Sprite.Texture, this.Transform.Position, null, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f, SpriteEffects.FlipHorizontally, 0);

            }
        }
    } 
}
