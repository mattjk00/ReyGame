using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    public class MushroomMinion : Enemy
    {
        int magicTimer = 0;

        public override void Load()
        {
            this.Name = "mushroomminion";
            this.EntityStats = new EntityStats();
            this.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/demons/mushroom.png"); // load the player texture

            this.speed = 0.075f;
            this.State = EnemyState.Idle;
            this.AddDefaultBoundingBox();
            this.IsEnemy = true;
            this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
            this.EntityStats.MaxHP = 30;
            this.EntityStats.HP = 30;
            this.EntityStats.AttackSpeed = 100;
            this.EntityStats.Aggressive = true;
            this.EntityStats.AttackLevel = 1;
            this.ChooseNewTargetAndInterval();
            this.BoundingBoxes.Add(new Rectangle(0, 0, 0, 0));

            this.shadow.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/player/shadow.png");
            this.shadow.Sprite.Color = new Color(255, 255, 255) * 0.3f;
            this.shadow.LocalPosition = new Vector2(27, 65);

            // load the healthbar and set the stats
            this.healthbar.Load();
            this.healthbar.AssignStats(this.EntityStats);
            this.healthbar.LocalPosition = new Vector2(10, 10);

            // drop table
            this.DropTable.Add(ItemData.New(ItemData.mushroomHelmet), 0.33f);
            this.DropTable.Add(ItemData.New(ItemData.mushroomChest), 0.33f);
            this.DropTable.Add(ItemData.New(ItemData.mushroomLegs), 0.23f);

        }

        public override void Update()
        {
            base.EssentialUpdate(); // do the essentials

            if (this.State == EnemyState.Idle)
                this.HandleIdleMovement();
            else if (this.State == EnemyState.Chasing)
                this.MoveAround();
            else if (this.State == EnemyState.Dead)
                this.HandleDeath();

            this.projectileManager.Update();

            // update only when the bat is alive
           /* if (this.State != EnemyState.Dead)
            {
                this.BoundingBoxes[0] = new Rectangle((int)(this.Transform.Position.X - this.Transform.Origin.X), (int)(this.Transform.Position.Y - this.Transform.Origin.Y), 100, 100);
            }*/

        }

        public override void Draw(SpriteBatch sb)
        {
            this.healthbar.Draw(sb);

            if (shadow.Sprite.Texture != null)
                sb.Draw(shadow.Sprite.Texture, shadow.Transform.Position, shadow.Sprite.Color);

            sb.Draw(this.Sprite.Texture, this.Transform.Position, this.Sprite.Color);

            this.projectileManager.Draw(sb);
        }

        /// <summary>
        /// Handle what happens when the bat dies
        /// </summary>
        protected override void HandleDeath()
        {
            this.Sprite.Color *= 0.9f;
            this.shadow.Sprite.Color *= 0.9f;
            // check to destroy the object when it fully dissapears
            if (this.Sprite.Color.A <= 0)
                this.ToBeDestroyed = true;
        }

        // handles movement when attacking
        protected void MoveAround()
        {
            this.magicTimer++;

            if (Vector2.Distance(playerTarget.Transform.Position, this.Transform.Position) > 300)
            {
                // if the target player is to the right
                if (this.playerTarget.Transform.Position.X - this.playerTarget.Sprite.Texture.Width - (this.playerTarget.Transform.Origin.X * 2) > this.Transform.Position.X)
                {
                    this.direction = Direction.MovingRight;
                    this.velocity += new Vector2(this.speed * 2, 0);
                }
                // if the target player is to the left
                else if (this.playerTarget.Transform.Position.X + this.playerTarget.Sprite.Texture.Width + this.playerTarget.Transform.Origin.X * 2 < this.Transform.Position.X)
                {
                    this.direction = Direction.MovingLeft;
                    this.velocity += new Vector2(-this.speed * 2, 0);
                }
                // if the target is below the enemy
                if (this.playerTarget.Transform.Position.Y + this.playerTarget.Sprite.Texture.Height > this.Transform.Position.Y)
                {
                    this.velocity += new Vector2(0, this.speed * 2);
                }
                // if the target is above the enemy
                if (this.playerTarget.Transform.Position.Y - this.playerTarget.Sprite.Texture.Height < this.Transform.Position.Y)
                {
                    this.velocity += new Vector2(0, -this.speed * 2);
                }
            }
            else if (this.magicTimer > this.EntityStats.AttackSpeed)
            {
                // start attacking if close enough
                this.HandleMagicAttack();
            }

            this.meleeIntervalTimer++;
        }

        /// <summary>
        /// Really Magic Attack
        /// </summary>
        
        protected void HandleMagicAttack()
        {
            

                this.magicTimer = 0;

                // shoot a thing
                this.projectileManager.ShootNew(this.Transform.Position, this.playerTarget.Transform.Position, 8, 3, this.EntityStats, ProjectileType.Mushroom);
            
        }
    }
}
