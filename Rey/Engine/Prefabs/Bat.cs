using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Rey.Engine.Prefabs
{
    public class Bat : Enemy
    {
        private ChildObject legs = new ChildObject();
        private ChildAnimation body = new ChildAnimation(99, 60, 2, 3);

        public override void Load()
        {
            this.Name = "bat";
            this.EntityStats = new EntityStats();
            this.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/bats/bat_head.png"); // load the player texture

            this.speed = 0.15f;
            this.State = EnemyState.Idle;
            this.AddDefaultBoundingBox();
            this.IsEnemy = true;
            this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
            this.EntityStats.HP = 10;
            this.EntityStats.AttackSpeed = 50;
            this.EntityStats.Aggressive = true;
            this.ChooseNewTargetAndInterval();
            this.BoundingBoxes.Add(new Rectangle(0, 0, 0, 0));

            this.body.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/bats/bat_flying.png");
            this.body.LocalPosition = new Vector2(-5, 30);

            this.legs.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/bats/bat_legs.png"); // load the player texture
            //this.legs.Load();
            this.legs.LocalPosition = new Vector2(20, 75);

            this.shadow.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/player/shadow.png");
            this.shadow.Sprite.Color = new Color(255, 255, 255) * 0.3f;
            this.shadow.LocalPosition = new Vector2(25, 120);
        }

        public override void Update()
        {
            base.Update();

            // update only when the bat is alive
            if (this.State != EnemyState.Dead)
            {
                this.BoundingBoxes[0] = new Rectangle((int)(this.Transform.Position.X - this.Transform.Origin.X), (int)(this.Transform.Position.Y - this.Transform.Origin.Y), 100, 100);
                this.body.Update(this);
                this.body.Animate();
                this.legs.Update(this);
            }
            /*else if (this.State == EnemyState.Dead)
            {
                this.HandleDeath();
            }*/
            
        }

        public override void Draw(SpriteBatch sb)
        {
            if (shadow.Sprite.Texture != null)
                sb.Draw(shadow.Sprite.Texture, shadow.Transform.Position, shadow.Sprite.Color);

            sb.Draw(legs.Sprite.Texture, legs.Transform.Position, this.Sprite.Color);
            //this.legs.Draw(sb);
            this.body.Draw(sb);
            sb.Draw(this.Sprite.Texture, this.Transform.Position, this.Sprite.Color);
            
        }

        /// <summary>
        /// Handle what happens when the bat dies
        /// </summary>
        protected override void HandleDeath()
        {
            this.Sprite.Color *= 0.9f;
            this.body.Sprite.Color *= 0.9f;
            this.shadow.Sprite.Color *= 0.9f;
            // check to destroy the object when it fully dissapears
            if (this.Sprite.Color.A <= 0)
                this.ToBeDestroyed = true;
        }
    }
}
