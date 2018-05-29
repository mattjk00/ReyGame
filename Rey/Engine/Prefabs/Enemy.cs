using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Rey.Engine.Prefabs
{
    public enum EnemyState
    {
        Idle,
        Chasing,
        MeleeAttacking,
        MagicAttacking,
        Dead
    }

    public class Enemy : GameObject
    {
        protected float speed = 0.2f;
        protected Vector2 velocity = new Vector2();
        protected PlayerArm arm = new PlayerArm(); // the player's arm
        protected ChildObject weapon = new ChildObject();
        protected ChildObject shadow = new ChildObject();
        protected Healthbar healthbar = new Healthbar();

        protected Direction direction = Direction.MovingRight;
        public PlayerAttackState AttackState { get; set; } // the attacking state of the player
        public EntityStats EntityStats { get; set; } = new EntityStats(); // the player's stats
        public EnemyState State { get; protected set; } = EnemyState.Idle; // the enemy's state

        // the drop table for the enemy
        public Dictionary<Item, float> DropTable { get; protected set; } = new Dictionary<Item, float>();
        

        // timers for attacking
        protected float meleeAttackTimer = 0;
        protected float meleeIntervalTimer = 0;
        protected float meleeAttackInterval = 15;
        protected float movementTimer = 0;
        protected float interval = 1000;
        protected Player playerTarget; // the targeted player
        public ProjectileManager projectileManager { get; protected set; } = new ProjectileManager();

        // for idle movement
        protected bool moving = false;
        protected Vector2 target;

        public bool LandedMeleeHit { get; set; }

        /// <summary>
        /// Set the player's properties
        /// </summary>
        public override void Load()
        {
            this.Name = "enemy";
            this.EntityStats = new EntityStats();
            this.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Enemies/warrior1.png"); // load the player texture
            this.arm.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Player/iron_arm.png"); // load the player texture
            this.arm.Load();
            this.weapon.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Player/iron_sword.png"); // load the weapon texture
            this.weapon.Load();
            this.weapon.LocalPosition = new Vector2(13, 62);
            this.weapon.Transform.Origin = new Vector2(5, this.weapon.Sprite.Texture.Height / 2);
            this.State = EnemyState.Idle;
            this.AddDefaultBoundingBox();
            this.IsEnemy = true;
            this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
            this.EntityStats.HP = 10;
            this.EntityStats.AttackSpeed = 25;
            this.ChooseNewTargetAndInterval();
            this.BoundingBoxes.Add(new Rectangle(0, 0, 0, 0));

            this.shadow.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/player/shadow.png");
            this.shadow.Sprite.Color = new Color(255, 255, 255) * 0.3f;
            this.shadow.LocalPosition = new Vector2(0, 88);

            // load the healthbar and set the stats
            this.healthbar.Load();
            this.healthbar.AssignStats(this.EntityStats);
            this.healthbar.LocalPosition = new Vector2(-10, -40);
        }

        public override void Update()
        {
            if (this.State == EnemyState.Idle)
                HandleIdleMovement();
            else if (this.State == EnemyState.Chasing)
                HandleAttackMovement();
            else if (this.State == EnemyState.MeleeAttacking)
                HandleMeleeAttack();
            else if (this.State == EnemyState.Dead)
                this.HandleDeath();

            this.EssentialUpdate();
        }

        /// <summary>
        /// The essential stuff, nothing to do with combat
        /// </summary>
        protected virtual void EssentialUpdate()
        {
            this.UpdateDefaultBox(0);
            // update the sword bounding box
            /*if (this.direction == Direction.MovingRight)
                this.BoundingBoxes[1] = new Rectangle((int)(this.Transform.Position.X + this.Sprite.Texture.Width - this.Transform.Origin.X),
                    (int)(this.Transform.Position.Y - this.Transform.Origin.Y), 55, this.Sprite.Texture.Height);
            else if (this.direction == Direction.MovingLeft)
                this.BoundingBoxes[1] = new Rectangle((int)(this.Transform.Position.X - this.Transform.Origin.X) - 55, (int)(this.Transform.Position.Y - this.Transform.Origin.Y), 55, this.Sprite.Texture.Height);
            */
            if (this.EntityStats.HP <= 0 && this.State != EnemyState.Dead)
                Die();

            this.Transform.Position += this.velocity; // update position based on velocity
            this.velocity *= 0.95f;


            base.Update();

            this.arm.Transform.Position = this.Transform.Position + this.arm.LocalPosition - this.Transform.Origin;
            this.weapon.Transform.Position = this.Transform.Position + this.weapon.LocalPosition - this.Transform.Origin;
            this.shadow.Update(this);
            this.healthbar.Update(this);
        }

        // handles the movement
        protected virtual void HandleIdleMovement()
        {
            // if the enemy is waiting to move again
            if (moving == false)
            {
                this.movementTimer += 1; // step timer

                if (this.movementTimer > this.interval)
                {
                    this.moving = true;
                }
            }
            else if (this.moving == true)
            { // if the enemy is en route
              // move towards the player horizontally
                if (this.Transform.Position.X > this.target.X)
                {
                    this.velocity += new Vector2(-this.speed, 0);
                    this.direction = Direction.MovingLeft;
                }
                else
                {
                    this.velocity += new Vector2(this.speed, 0);
                    this.direction = Direction.MovingRight;
                }

                // move towards the player vertically
                if (this.Transform.Position.Y > this.target.Y)
                {
                    this.velocity += new Vector2(0, -this.speed);
                }
                else
                {
                    this.velocity += new Vector2(0, this.speed);
                }

                // if the player has stopped moving or arrived at target
                if (Vector2.Distance(this.Transform.Position, this.target) < 1)
                {
                    this.moving = false;
                    this.ChooseNewTargetAndInterval();
                }
            }
        }

        // starts an attack
        public void StartAttack(Player player)
        {
            if (this.State != EnemyState.Dead)
            {
                this.playerTarget = player;
                this.State = EnemyState.Chasing;
            }
        }

        /// <summary>
        /// Use to handle taking damage from the attacking player
        /// </summary>
        /// <param name="playerStats"></param>
        /// <returns></returns>
        public virtual int GetHitMelee(EntityStats playerStats)
        {
            int damageDealt = playerStats.AttackLevel;
            //this.Sprite.Color = Color.Red;
            this.EntityStats.HP -= damageDealt; // deal damage to the enemy
            return damageDealt;
        }

        /// <summary>
        /// Use to handle taking damage from the attacking magic player
        /// </summary>
        /// <param name="playerStats"></param>
        /// <returns></returns>
        public virtual int GetHitMagic(EntityStats playerStats, Projectile projectile)
        {
            int damageDealt = (playerStats.MagicSpeed/100) + projectile.Damage;
            //this.Sprite.Color = Color.Red;
            this.EntityStats.HP -= damageDealt; // deal damage to the enemy
            return damageDealt;
        }

        // handles movement when attacking
        protected virtual void HandleAttackMovement()
        {
            // if the target player is to the right
            if (this.playerTarget.Transform.Position.X - this.playerTarget.Sprite.Texture.Width - (this.playerTarget.Transform.Origin.X*2) > this.Transform.Position.X)
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

            this.meleeIntervalTimer++;

            // if the enemy is very close to the player
            if (Vector2.Distance(this.Transform.Position, this.playerTarget.Transform.Position) < 150 && this.meleeIntervalTimer > this.meleeAttackInterval)
            {
                if (this.playerTarget.direction == Direction.MovingRight)
                    this.direction = Direction.MovingLeft;
                else
                    this.direction = Direction.MovingRight;
                this.State = EnemyState.MeleeAttacking;
                this.meleeIntervalTimer = 0;
            }
        }

        /// <summary>
        /// handles the animation and such for the attack
        /// </summary>
        protected virtual void HandleMeleeAttack()
        {
            this.meleeAttackTimer++;
            // rotate arm based on direction the player is facing
            if (this.direction == Direction.MovingRight)
            {
                this.arm.Transform.Rotation = MathHelper.ToRadians(-45); // rotate the arm forward
                this.weapon.LocalPosition = new Vector2(35, 55);
            }
            if (this.direction == Direction.MovingLeft)
            {
                this.arm.Transform.Rotation = MathHelper.ToRadians(45); // rotate the arm forward
                this.weapon.LocalPosition = new Vector2(-55, 60);
            }

            this.arm.Transform.Rotation += MathHelper.ToRadians(new Random().Next(-5, 5));

            // once the attack is complete, reset the arm
            if (this.meleeAttackTimer >= this.EntityStats.AttackSpeed)
            {
                this.meleeAttackTimer = 0;
                this.AttackState = PlayerAttackState.None;
                this.arm.Transform.Rotation = 0; // rotate the arm forward
                this.State = EnemyState.Chasing;
                this.LandedMeleeHit = false;
            }
        }

        /// <summary>
        /// Chooses the new target and interval for waiting
        /// </summary>
        protected virtual void ChooseNewTargetAndInterval()
        {
            this.movementTimer = 0;

            // choose target location near its starting point
            float x = 0;
            float y = 0;


            x = new Random().Next((int)this.Transform.Position.X - 100, (int)this.Transform.Position.X + 100);
            y = new Random().Next((int)this.Transform.Position.Y - 95, (int)this.Transform.Position.Y + 95);

            if (this.Transform.Position.X < 200)
                x = new Random().Next((int)this.Transform.Position.X - 5, (int)this.Transform.Position.X + 200);
            if (this.Transform.Position.X > 1280 - 200)
                x = new Random().Next((int)this.Transform.Position.X - 200, (int)this.Transform.Position.X + 5);
            if (this.Transform.Position.Y < 200)
                y = new Random().Next((int)this.Transform.Position.Y - 5, (int)this.Transform.Position.Y + 185);
            if (this.Transform.Position.Y > 720 - 200)
                y = new Random().Next((int)this.Transform.Position.Y - 185, (int)this.Transform.Position.Y + 5);

            this.target = new Vector2(x, y);

            this.interval = new Random().Next(100, 250);
        }

        //
        public void SetInterval(int interv)
        {
            this.interval = interv;
        }

        protected virtual void HandleDeath()
        {
            //this.Sprite.Color = new Color(this.Sprite.Color.R, this.Sprite.Color.G, this.Sprite.Color.B, this.Sprite.Color.A - 100);
            //this.Sprite.Color = Color.Beige;
        }

        /// <summary>
        /// Kills the enemy
        /// </summary>
        protected virtual void Die()
        {

            this.State = EnemyState.Dead;
            this.Sprite.Color = new Color(255, 0, 0, 80);
            this.Transform.Rotation = MathHelper.ToRadians(90);
            this.DropItems(); // drop items
            
        }

        /// <summary>
        /// Drop table
        /// </summary>
        protected void DropItems()
        {

            Random random = new Random();
            if (DropTable.Count > 0)
            {
                // get a random index from the drop table
                int randomIndex = random.Next(0, DropTable.Count);
                float randomNum = (float)random.NextDouble(); // gets a random float to compare to
                                                              // compare the random number to the dictionary entry at the given random index. If the randnum is less than or equal to the entry, drop the item.
                if (randomNum <= DropTable.ElementAt(randomIndex).Value)
                {
                    // drop the item at the monster positiion
                    SceneManager.GetCurrentScene().DropItemAtPosition(ItemData.New(DropTable.ElementAt(randomIndex).Key), this.Transform.Position);
                }
            }
        }

        // bounces the enemy based on given velocity
        public void Bounce(Vector2 bv)
        {
            this.velocity += bv;
        }

        public override void Draw(SpriteBatch sb)
        {
            // draw hp
            /*sb.DrawString(AssetLoader.Font, this.EntityStats.HP.ToString() + "/" + this.EntityStats.MaxHP.ToString(), 
                new Vector2(this.Transform.Position.X, this.Transform.Position.Y - 20 - this.Transform.Origin.Y), Color.LightGreen);*/
            this.healthbar.Draw(sb);

            if (this.direction == Direction.MovingRight)
            {
                sb.Draw(this.Sprite.Texture, this.Transform.Position, null, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f, SpriteEffects.None, 0);
                this.weapon.LocalPosition = new Vector2(20, 65);
                sb.Draw(this.weapon.Sprite.Texture, this.weapon.Transform.Position, null, this.weapon.Sprite.Color, this.weapon.Transform.Rotation, this.weapon.Transform.Origin, 1.0f, SpriteEffects.None, 0);
            }
            else if (this.direction == Direction.MovingLeft)
            {
                sb.Draw(this.Sprite.Texture, this.Transform.Position, null, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f, SpriteEffects.FlipHorizontally, 0);
                this.weapon.LocalPosition = new Vector2(-37, 65);
                sb.Draw(this.weapon.Sprite.Texture, this.weapon.Transform.Position, null, this.weapon.Sprite.Color, this.weapon.Transform.Rotation, this.weapon.Transform.Origin, 1.0f, SpriteEffects.FlipHorizontally, 0);
            }
            this.arm.Draw(sb);
            //base.Draw(sb);
        }
    }
}
