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
            
            this.State = EnemyState.Idle;
            this.AddDefaultBoundingBox();
            this.IsEnemy = true;
            this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
            this.EntityStats.HP = 10;
            this.EntityStats.AttackSpeed = 25;
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

            this.BoundingBoxes[0] = new Rectangle((int)(this.Transform.Position.X - this.Transform.Origin.X), (int)(this.Transform.Position.Y - this.Transform.Origin.Y), 100, 100);
            this.body.Update(this);
            this.body.Animate();
            this.legs.Update(this);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(shadow.Sprite.Texture, shadow.Transform.Position, shadow.Sprite.Color);

            sb.Draw(legs.Sprite.Texture, legs.Transform.Position, Color.White);
            this.body.Draw(sb);
            sb.Draw(this.Sprite.Texture, this.Transform.Position, Color.White);
            
        }
    }
}
