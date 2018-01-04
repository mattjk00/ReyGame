using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    public class BabyFishDemon : Enemy
    {
        ChildAnimation aura = new ChildAnimation(90, 90, 3, 5);

        public override void Load()
        {
            this.Name = "babyfishdemon";
            
            this.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/demons/fish_demon.png"); // load the player texture

            this.speed = 0.1f;
            this.AddDefaultBoundingBox();
            this.IsEnemy = true;
            this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
            this.EntityStats.HP = 25;
            this.EntityStats.MaxHP = 25;
            this.EntityStats.AttackSpeed = 50;
            this.EntityStats.Aggressive = true;
            this.ChooseNewTargetAndInterval();
            this.BoundingBoxes.Add(new Rectangle(0, 0, 0, 0));


            this.shadow.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/player/shadow.png");
            this.shadow.Sprite.Color = new Color(255, 255, 255) * 0.3f;
            this.shadow.LocalPosition = new Vector2(25, 120);

            this.aura.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/demons/fish_demon_aura.png");
            this.aura.LocalPosition = new Vector2(12, 30);
            this.aura.Sprite.Color = Color.White * 0.4f;

            // load the healthbar and set the stats
            this.healthbar.Load();
            this.healthbar.AssignStats(this.EntityStats);
            this.healthbar.LocalPosition = new Vector2(10, 0);
        }

        public override void Update()
        {
            base.Update();

            // update only when the bat is alive
            if (this.State != EnemyState.Dead)
            {
                this.BoundingBoxes[0] = new Rectangle((int)(this.Transform.Position.X - this.Transform.Origin.X), (int)(this.Transform.Position.Y - this.Transform.Origin.Y), 75, 100);
                this.aura.Update(this);
                this.aura.Animate();
            }
            /*else if (this.State == EnemyState.Dead)
            {
                this.HandleDeath();
            }*/
            
        }

        public override void Draw(SpriteBatch sb)
        {
            this.healthbar.Draw(sb);

            if (shadow.Sprite.Texture != null)
                sb.Draw(shadow.Sprite.Texture, shadow.Transform.Position, shadow.Sprite.Color);

            sb.Draw(this.Sprite.Texture, this.Transform.Position, this.Sprite.Color);

            this.aura.Draw(sb);
        }

        /// <summary>
        /// Handle what happens when the bat dies
        /// </summary>
        protected override void HandleDeath()
        {
            this.Sprite.Color *= 0.9f;
            //this.body.Sprite.Color *= 0.9f;
            this.shadow.Sprite.Color *= 0.9f;
            // check to destroy the object when it fully dissapears
            if (this.Sprite.Color.A <= 0)
                this.ToBeDestroyed = true;
        }
    }
}
