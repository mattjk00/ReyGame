using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

public enum Direction
{
    MovingLeft,
    MovingRight
}

public enum PlayerAttackState
{
    None,
    MeleeAttack,
    MagicAttack,
    Dead
}

namespace Rey.Engine.Prefabs
{
    // The player object
    public class Player : GameObject
    {
        // player variables
        KeyboardState keyboard;
        KeyboardState lastKeyboard;
        MouseState mouse;
        MouseState lastMouse;
        float speed = 0.6f;
        ChildObject arm = new ChildObject(); // the player's arm
        ChildObject weapon = new ChildObject();
        ChildAnimation legs = new ChildAnimation(48, 6, 3);
        ChildObject head = new ChildObject();
        
        public Direction direction = Direction.MovingRight;
        public PlayerAttackState AttackState { get; set; } // the attacking state of the player
        public EntityStats EntityStats { get; set; } // the player's stats
        public bool LandedMeleeHit { get; set; } // used to tell if the player has landed a melee hit in the swing

        // timers for attacking
        float meleeAttackTimer = 0;
        float magicAttackTimer = 0;

        Texture2D defaultBody;
        Texture2D magicAttackBody;

        ProjectileManager projectileManager = new ProjectileManager();

        /// <summary>
        /// Set the player's properties
        /// </summary>
        public override void Load()
        {
            this.Name = "player";
            this.EntityStats = new EntityStats();
            this.lastKeyboard = Keyboard.GetState();

            this.defaultBody = AssetLoader.LoadTexture("Assets/Textures/Player/default_body.png"); // load the player texture
            this.magicAttackBody = AssetLoader.LoadTexture("Assets/Textures/Player/default_magic_body.png"); // load the player texture
            this.Sprite.Texture = this.defaultBody;
            this.AddDefaultBoundingBox();
            this.Transform.Position = new Vector2(1280 / 2, 720 / 2);
            this.BoundingBoxes.Add(new Rectangle(0, 0, 0, 0));
            this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
            this.IsEnemy = false;

            this.arm.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Player/iron_arm.png"); // load the player texture
            this.arm.Load();

            this.weapon.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Player/iron_sword.png"); // load the weapon texture
            this.weapon.Load();
            this.weapon.LocalPosition = new Vector2(13, 62);
            this.weapon.Transform.Origin = new Vector2(5, this.weapon.Sprite.Texture.Height / 2);

            this.legs.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/player/legs_animation.png");
            this.legs.LocalPosition = new Vector2(0, 55);

            this.head.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/player/default_head.png");
            this.head.Load();
            this.head.LocalPosition = new Vector2(5, -42);
        }

        /// <summary>
        /// The update loop for the player
        /// </summary>
        public override void Update()
        {
            this.Transform.Velocity *= 0.85f;
            if (this.AttackState != PlayerAttackState.Dead)
            {
                this.HandleInput();
                this.TiltWithVelocity();
            }

            // handle melee attacking if attacking
            if (this.AttackState == PlayerAttackState.MeleeAttack && this.AttackState != PlayerAttackState.Dead)
                this.HandleMeleeAttack();
            else if (this.AttackState == PlayerAttackState.MagicAttack && this.AttackState != PlayerAttackState.Dead)
                this.HandleMagicAttack();
            else if (this.AttackState == PlayerAttackState.Dead)
                this.HandleDeath();
           

            base.Update(); // important so velocity works

            // maybe put this in the child class??
            this.arm.Update(this);
            this.weapon.Update(this);
            this.legs.Update(this);
            this.head.Update(this);
            this.projectileManager.Update();

            // animate the legs if the velocity is greater than 1
            if (Math.Abs(this.Transform.VelX) > 1 || Math.Abs(this.Transform.VelY) > 1)
                this.legs.Animate();
            else
                this.legs.SetFrame(6); // stop the animation and set to the default frame if not moving

            if (this.EntityStats.HP <= 0)
                this.Die();

            this.UpdateDefaultBox(0);
            // update the sword bounding box
            if (this.direction == Direction.MovingRight)
                this.BoundingBoxes[1] = new Rectangle((int)(this.Transform.Position.X + this.Sprite.Texture.Width - this.Transform.Origin.X),
                    (int)(this.Transform.Position.Y - this.Transform.Origin.Y), 45, this.Sprite.Texture.Height);
            else if (this.direction == Direction.MovingLeft)
                this.BoundingBoxes[1] = new Rectangle((int)(this.Transform.Position.X - this.Transform.Origin.X) - 45, (int)(this.Transform.Position.Y - this.Transform.Origin.Y), 45, this.Sprite.Texture.Height);
        }

        // Handle user input
        void HandleInput()
        {
            this.keyboard = Keyboard.GetState();
            this.mouse = Mouse.GetState();

            /* movement input */
            if (this.keyboard.IsKeyDown(Keys.D))
            {
                this.Transform.VelX += speed;
                this.arm.LocalPosition = new Vector2(13, 40);
            }
            if (this.keyboard.IsKeyDown(Keys.A))
            {
                this.Transform.VelX += -speed;
                this.direction = Direction.MovingLeft;
                this.arm.LocalPosition = new Vector2(26, 40);
            }
            if (this.keyboard.IsKeyDown(Keys.W))
                this.Transform.VelY += -speed;
            if (this.keyboard.IsKeyDown(Keys.S))
                this.Transform.VelY += speed;

            if (this.mouse.Position.X > this.Transform.Position.X)
                this.direction = Direction.MovingRight;
            else
                this.direction = Direction.MovingLeft;


            /* attack input */
            if (this.keyboard.IsKeyDown(Keys.Space) && this.lastKeyboard.IsKeyDown(Keys.Space) == false)
            {
                this.StartMeleeAttack(); // start attacking
            }

            /* Magic/Ranged input */
            if (this.mouse.LeftButton == ButtonState.Pressed && this.lastMouse.LeftButton == ButtonState.Released)
            {
                this.StartMagicAttack();
            }

            lastKeyboard = this.keyboard;
            lastMouse = this.mouse;
        }

        /// <summary>
        /// Initiates the magic attack
        /// </summary>
        void StartMagicAttack()
        {
            // if the player is ready to attack
            if (this.magicAttackTimer == 0 && this.AttackState == PlayerAttackState.None)
            {
                this.projectileManager.ShootNew(this.Transform.Position, new Vector2(mouse.Position.X, mouse.Position.Y)); // shoot a new projectile
                this.AttackState = PlayerAttackState.MagicAttack;
                this.LandedMeleeHit = false;
            }
        }

        /// <summary>
        /// Handles the magic attack
        /// </summary>
        void HandleMagicAttack()
        {
            // handle the timer stuff
            this.magicAttackTimer++;
            if (this.magicAttackTimer >= this.EntityStats.MagicSpeed)
            {
                this.magicAttackTimer = 0;
                this.AttackState = PlayerAttackState.None;
            }
        }

        /// <summary>
        /// Tilts the player towards the direction they're moving
        /// </summary>
        void TiltWithVelocity()
        {

            if (this.keyboard.IsKeyDown(Keys.D))
                this.Transform.Rotation = MathHelper.ToRadians(2);
            else if (this.keyboard.IsKeyDown(Keys.A))
                this.Transform.Rotation = MathHelper.ToRadians(-2);
            else
                this.Transform.Rotation = 0;
        }

        /// <summary>
        /// Initiates the melee attack attack
        /// </summary>
        void StartMeleeAttack()
        {
            // if the player is ready to attack
            if (this.meleeAttackTimer == 0 && this.AttackState != PlayerAttackState.MeleeAttack)
            {
                this.AttackState = PlayerAttackState.MeleeAttack; // set the player to attack state
                this.LandedMeleeHit = false;
            }
        }

        /// <summary>
        /// handles the animation and such for the attack
        /// </summary>
        void HandleMeleeAttack()
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
            
            this.Transform.Velocity *= 0.2f; // slow movement

            // once the attack is complete, reset the arm
            if (this.meleeAttackTimer >= this.EntityStats.AttackSpeed)
            {
                this.meleeAttackTimer = 0;
                this.AttackState = PlayerAttackState.None;
                this.arm.Transform.Rotation = 0; // rotate the arm forward
                this.weapon.Transform.Rotation = MathHelper.ToRadians(0);

            }
        }


        /// <summary>
        /// override so you can draw the arm
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            // draw hp
            sb.DrawString(AssetLoader.Font, this.EntityStats.HP.ToString() + "/" + this.EntityStats.MaxHP.ToString(), 
                new Vector2(this.Transform.Position.X, this.Transform.Position.Y - 20 - this.Transform.Origin.Y), Color.LightGreen);
            this.legs.Draw(sb);

            if (this.direction == Direction.MovingRight)
            {
                if (this.AttackState == PlayerAttackState.MeleeAttack || this.AttackState == PlayerAttackState.None)
                {
                    sb.Draw(this.Sprite.Texture, this.Transform.Position, null, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f, SpriteEffects.None, 0);
                    this.weapon.LocalPosition = new Vector2(20, 65);
                    sb.Draw(this.weapon.Sprite.Texture, this.weapon.Transform.Position, null, this.weapon.Sprite.Color, this.weapon.Transform.Rotation, this.weapon.Transform.Origin, 1.0f, SpriteEffects.None, 0);
                }
                else if (this.AttackState == PlayerAttackState.MagicAttack)
                {
                    sb.Draw(this.magicAttackBody, this.Transform.Position + new Vector2(0, 0), null, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else if (this.direction == Direction.MovingLeft)
            {
                if (this.AttackState == PlayerAttackState.MeleeAttack || this.AttackState == PlayerAttackState.None)
                {
                    sb.Draw(this.Sprite.Texture, this.Transform.Position, null, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f, SpriteEffects.FlipHorizontally, 0);
                    this.weapon.LocalPosition = new Vector2(-37, 65);
                    sb.Draw(this.weapon.Sprite.Texture, this.weapon.Transform.Position, null, this.weapon.Sprite.Color, this.weapon.Transform.Rotation, this.weapon.Transform.Origin, 1.0f, SpriteEffects.FlipHorizontally, 0);
                }
                else if (this.AttackState == PlayerAttackState.MagicAttack)
                {
                    sb.Draw(this.magicAttackBody, this.Transform.Position + new Vector2(-60, 0), null, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f, SpriteEffects.None, 0);
                }
            }

            sb.Draw(head.Sprite.Texture, head.Transform.Position, Color.White);

            this.arm.Draw(sb);
            this.projectileManager.Draw(sb);

        }

        /// <summary>
        /// Use to handle taking damage from the attacking player
        /// </summary>
        /// <param name="playerStats"></param>
        /// <returns></returns>
        public int GetHit(EntityStats enemyStats)
        {
            int damageDealt = enemyStats.AttackLevel;
            //this.Sprite.Color = Color.Red;
            this.EntityStats.HP -= damageDealt; // deal damage to the enemy
            return damageDealt;
        }

        void HandleDeath()
        {
            //this.Sprite.Color = new Color(this.Sprite.Color.R, this.Sprite.Color.G, this.Sprite.Color.B, this.Sprite.Color.A - 100);
            this.Sprite.Color = Color.Beige;
        }

        public void Bounce(Vector2 bv)
        {
            this.Transform.Velocity += bv;
        }

        /// <summary>
        /// Kills the enemy
        /// </summary>
        void Die()
        {
            this.AttackState = PlayerAttackState.Dead;
            this.Sprite.Color = new Color(255, 0, 0, 80);
            this.Transform.Rotation = MathHelper.ToRadians(90);
        }
    }
}
