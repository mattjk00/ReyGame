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
    public enum ProjectileType
    {
        Wind,
        Mushroom,
        SporeBomb
    }

    public class ProjectileManager
    {
        public List<Projectile> Projectiles { get; private set; } = new List<Projectile>();
        Random random = new Random();

        // creates and sends a new projectile at a position
        public void ShootNew(Vector2 from, Vector2 to, int speed, int damage, EntityStats playerStats, ProjectileType projectileType)
        {
            // create a projectile and add it to the projectile array
            var proj = new Projectile();
            proj.Sprite.Texture = GetTexture(projectileType);
            proj.Load();
            proj.Transform.Position = from;
            proj.LookAt(to);
            var buffedDamage = (int)((float)damage * (float)(1 + ((float)playerStats.FullStats.AttackLevel/10)));
            proj.Damage = buffedDamage;
            proj.Speed = speed;
            proj.Sprite.Color = new Color(random.Next(200, 255), random.Next(200, 255), 255);
            this.Projectiles.Add(proj);
        }

        /// <summary>
        /// returns a projectile texture
        /// </summary>
        /// <param name="projectileType"></param>
        Texture2D GetTexture(ProjectileType projectileType)
        {
            if (projectileType == ProjectileType.Mushroom)
                return AssetLoader.LoadTexture("Assets/Textures/Projectiles/mushroom_strike.png");
            else if (projectileType == ProjectileType.SporeBomb)
                return AssetLoader.LoadTexture("Assets/Textures/Projectiles/spore_bomb.png");
            else
                return AssetLoader.LoadTexture("Assets/Textures/Projectiles/wind_strike.png");
        }

        public void Update()
        {
            foreach (Projectile proj in this.Projectiles)
            {
                proj.Update();
                proj.UpdateDefaultBox(0);
            }
            this.Projectiles.RemoveAll(x => x.ToBeDestroyed == true); // remove all projectiles that need to be destroyed
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Projectile proj in this.Projectiles)
            {
                sb.Draw(proj.Sprite.Texture, proj.Transform.Position, null, proj.Sprite.Color, proj.Transform.Rotation, proj.Transform.Origin, 1.0f, SpriteEffects.None, 0);
                //  sb.Draw(this.Sprite.Texture, this.Transform.Position, null, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f, SpriteEffects.FlipHorizontally, 0);

            }
        }
    } 
}
