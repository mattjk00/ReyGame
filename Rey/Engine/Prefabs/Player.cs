﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Rey.Engine.Behaviors;
using System.Timers;

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
        ChildAnimation legs = new ChildAnimation(48, 50, 6, 3);
        ChildObject head = new ChildObject();
        ChildObject shadow = new ChildObject();
        Healthbar healthbar = new Healthbar();
        
        public Direction direction = Direction.MovingRight;
        public PlayerAttackState AttackState { get; set; } // the attacking state of the player
        public EntityStats EntityStats { get; set; } = new EntityStats();// the player's stats
        public bool LandedMeleeHit { get; set; } // used to tell if the player has landed a melee hit in the swing

        // timers for attacking
        float meleeAttackTimer = 0;
        float magicAttackTimer = 0;

        Texture2D defaultBody;
        Texture2D defaultMagicAttackBody;
        Texture2D currentMagicAttackBody;
        Texture2D currentBody;

        Texture2D defaultLegs;

        public ProjectileManager projectileManager { get; protected set; } = new ProjectileManager();

        public Rectangle MovementBox { get; protected set; } = new Rectangle();
        public int MovementBoxOffset { get; protected set; } = 20;

        /// <summary>
        /// Set the player's properties
        /// </summary>
        public override void Load()
        {
            this.Name = "player";
            //this.EntityStats = new EntityStats();
            this.lastKeyboard = Keyboard.GetState();

            this.defaultBody = AssetLoader.LoadTexture("Assets/Textures/Player/default_body.png"); // load the player texture
            this.defaultMagicAttackBody = AssetLoader.LoadTexture("Assets/Textures/Player/default_magic_body.png"); // load the player texture
            this.currentBody = this.defaultBody;
            this.Sprite.Texture = this.defaultBody;
            this.currentMagicAttackBody = this.defaultMagicAttackBody;
            this.AddDefaultBoundingBox();

            // this.Transform.Position = new Vector2(1280 / 2, 720 / 2);
            //this.BoundingBoxes.Add(new Rectangle(0, 0, 0, 0));
            this.Transform.Origin = new Vector2(this.Sprite.Texture.Width / 2, this.Sprite.Texture.Height / 2);
            this.IsEnemy = false;

            this.arm.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Player/iron_arm.png"); // load the player texture
            this.arm.Load();

            this.weapon.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/Player/iron_sword.png"); // load the weapon texture
            this.weapon.Load();
            this.weapon.LocalPosition = new Vector2(13, 62);
            this.weapon.Transform.Origin = new Vector2(5, this.weapon.Sprite.Texture.Height / 2);

            
            defaultLegs = AssetLoader.LoadTexture("Assets/Textures/player/legs_animation2.png");
            this.legs.Sprite.Texture = defaultLegs;
            this.legs.LocalPosition = new Vector2(0, 55);

            this.head.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/player/default_head.png");
            this.head.Load();
            this.head.LocalPosition = new Vector2(5, -42);

            this.shadow.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/player/shadow.png");
            this.shadow.Sprite.Color = new Color(255, 255, 255) * 0.3f;
            this.shadow.LocalPosition = new Vector2(0, 88);

            // load the healthbar and set the stats
            this.healthbar.Load();
            this.healthbar.AssignStats(this.EntityStats);
            this.healthbar.LocalPosition = new Vector2(-10, -50);

            /*this.AddBehavior(new PlayerMovementBehavior()
            {
                Speed = this.speed,
                Direction = this.direction
            });*/
            
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

            if (this.EntityStats.HP > this.EntityStats.FullStats.MaxHP)
                this.EntityStats.HP = this.EntityStats.FullStats.MaxHP;

            base.Update(); // important so velocity works

            // update legs based on what's equipped
            if (GameData.EquippedLegs.ID != null)
                legs.Sprite.Texture = GameData.EquippedLegs.Texture;
            else
                legs.Sprite.Texture = this.defaultLegs;

            // maybe put this in the child class??
            this.arm.Update(this);
            this.weapon.Update(this);
            this.legs.Update(this);
            this.head.Update(this);
            this.shadow.Update(this);
            this.projectileManager.Update();
            this.healthbar.Update(this);
            this.SlowHeal(); // slow heal

            // animate the legs if the velocity is greater than 1
            if (Math.Abs(this.Transform.VelX) > 1 || Math.Abs(this.Transform.VelY) > 1)
            {
                this.legs.Animate();
                // play sound
                this.MakeWalkingSound();
            }
            else
                this.legs.SetFrame(6); // stop the animation and set to the default frame if not moving

            if (this.EntityStats.HP <= 0)
                this.Die();

            // place the movement box
            this.MovementBox = new Rectangle((int)this.Transform.Position.X - this.MovementBoxOffset, (int)this.Transform.Position.Y + 65, 50, 5);

            this.UpdateDefaultBox(0);
            // update the sword bounding box
            /*if (this.direction == Direction.MovingRight)
                this.BoundingBoxes[1] = new Rectangle((int)(this.Transform.Position.X + this.Sprite.Texture.Width - this.Transform.Origin.X),
                    (int)(this.Transform.Position.Y - this.Transform.Origin.Y), 45, this.Sprite.Texture.Height);
            else if (this.direction == Direction.MovingLeft)
                this.BoundingBoxes[1] = new Rectangle((int)(this.Transform.Position.X - this.Transform.Origin.X) - 45, (int)(this.Transform.Position.Y - this.Transform.Origin.Y), 45, this.Sprite.Texture.Height);*/
        }

        int wSoundTimer = 0; // sound timer for walking
        void MakeWalkingSound()
        {
            wSoundTimer++;
            if (wSoundTimer > 10)
            {
                wSoundTimer = 0;
                SceneManager.SoundManager.PlaySound("footstep", 0.05f, (float)(new Random().Next(45, 60))/100.0f, 0.0f);
            }
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

            if (InputHelper.MousePosition.X> this.Transform.Position.X)
                this.direction = Direction.MovingRight;
            else
                this.direction = Direction.MovingLeft;


            /* attack input */
            if (this.keyboard.IsKeyDown(Keys.Space) && this.lastKeyboard.IsKeyDown(Keys.Space) == false && this.AttackState != PlayerAttackState.MagicAttack)
            {
                this.StartMeleeAttack(); // start attacking
            }

            /* Magic/Ranged input */
            if (this.mouse.LeftButton == ButtonState.Pressed && this.lastMouse.LeftButton == ButtonState.Released && this.AttackState != PlayerAttackState.MeleeAttack)
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
            if (this.magicAttackTimer == 0 && this.AttackState == PlayerAttackState.None && InputHelper.MouseOnUI == false)
            {
                this.projectileManager.ShootNew(this.Transform.Position, InputHelper.MousePosition, 16, 4, this.EntityStats, ProjectileType.Wind); // shoot a new projectile
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
            if (this.magicAttackTimer >= 100 - this.EntityStats.FullStats.MagicSpeed)
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
            /*sb.DrawString(AssetLoader.Font, this.EntityStats.HP.ToString() + "/" + this.EntityStats.MaxHP.ToString(), 
                new Vector2(this.Transform.Position.X - 10, this.Transform.Position.Y - 60 - this.Transform.Origin.Y), Color.LightGreen);*/

            this.healthbar.Draw(sb);

            sb.Draw(shadow.Sprite.Texture, shadow.Transform.Position, shadow.Sprite.Color);

            this.legs.Draw(sb);

            if (this.direction == Direction.MovingRight)
            {
                if (this.AttackState == PlayerAttackState.MeleeAttack || this.AttackState == PlayerAttackState.None)
                {
                    sb.Draw(this.currentBody, this.Transform.Position, null, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f, SpriteEffects.None, 0);
                    //this.weapon.LocalPosition = new Vector2(20, 65);
                    //sb.Draw(this.weapon.Sprite.Texture, this.weapon.Transform.Position, null, this.weapon.Sprite.Color, this.weapon.Transform.Rotation, this.weapon.Transform.Origin, 1.0f, SpriteEffects.None, 0);
                }
                else if (this.AttackState == PlayerAttackState.MagicAttack)
                {
                    sb.Draw(this.currentMagicAttackBody, this.Transform.Position + new Vector2(0, 0), null, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f, SpriteEffects.FlipHorizontally, 0);
                }
            }
            else if (this.direction == Direction.MovingLeft)
            {
                if (this.AttackState == PlayerAttackState.MeleeAttack || this.AttackState == PlayerAttackState.None)
                {
                    sb.Draw(this.currentBody, this.Transform.Position, null, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f, SpriteEffects.FlipHorizontally, 0);
                    //this.weapon.LocalPosition = new Vector2(-37, 65);
                    //sb.Draw(this.weapon.Sprite.Texture, this.weapon.Transform.Position, null, this.weapon.Sprite.Color, this.weapon.Transform.Rotation, this.weapon.Transform.Origin, 1.0f, SpriteEffects.FlipHorizontally, 0);
                }
                else if (this.AttackState == PlayerAttackState.MagicAttack)
                {
                    sb.Draw(this.currentMagicAttackBody, this.Transform.Position + new Vector2(-60, 0), null, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f, SpriteEffects.None, 0);
                }
            }

            // draw the head
            if (this.direction == Direction.MovingLeft)
                sb.Draw(head.Sprite.Texture, head.Transform.Position, null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            else
                sb.Draw(head.Sprite.Texture, head.Transform.Position, null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0);


            //this.arm.Draw(sb);
            this.projectileManager.Draw(sb);

            DrawWornEquipment(sb);

            //this.DrawBoundingBoxes(sb);
            //this.DrawBox(sb, this.MovementBox);
        }

        /// <summary>
        /// Draws the equipment the player is currently wearing
        /// </summary>
        void DrawWornEquipment(SpriteBatch sb)
        {
            if (GameData.EquippedHelmet.ID != null)
                sb.Draw(GameData.EquippedHelmet.Texture, this.head.Transform.Position, Color.White);
            if (GameData.EquippedChest.ID != null)
            {
                this.currentBody = GameData.EquippedChest.Texture;//sb.Draw(GameData.EquippedChest.Texture, this.Transform.Position, Color.White);
                this.currentMagicAttackBody = GameData.EquippedChest.AltTexture;
            }
            else
            {
                this.currentBody = this.defaultBody;
                this.currentMagicAttackBody = this.defaultMagicAttackBody;
            }

        }

        /// <summary>
        /// Use to handle taking damage from the attacking player
        /// </summary>
        /// <param name="playerStats"></param>
        /// <returns></returns>
        public int GetHit(EntityStats enemyStats)
        {
            Random rand = new Random();
            int damageDealt = (enemyStats.AttackLevel - (int)((float)this.EntityStats.FullStats.DefenceLevel / 10) + rand.Next(0, enemyStats.AttackLevel/2)) + 1;
            //this.Sprite.Color = Color.Red;
            this.EntityStats.HP -= damageDealt; // deal damage to the enemy
            return damageDealt;
        }

        /// <summary>
        /// For use from projectiles
        /// </summary>
        /// <param name="rawDamage"></param>
        /// <returns></returns>
        public int GetHit(int rawDamage)
        {
            Random rand = new Random();
            int damageDealt = (rawDamage - (int)((float)this.EntityStats.FullStats.DefenceLevel / 10) + rand.Next(0, rawDamage / 2)) + 1;
            //this.Sprite.Color = Color.Red;
            this.EntityStats.HP -= damageDealt; // deal damage to the enemy
            return damageDealt;
        }

        void HandleDeath()
        {
            //this.Sprite.Color = new Color(this.Sprite.Color.R, this.Sprite.Color.G, this.Sprite.Color.B, this.Sprite.Color.A - 100);
            //this.Sprite.Color = Color.Beige;
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

        int healTimer = 0;
        /// <summary>
        /// A slow heal
        /// </summary>
        void SlowHeal()
        {
            // only heal if below the max hp and above 0
            if (this.EntityStats.HP < this.EntityStats.FullStats.MaxHP && this.EntityStats.FullStats.HP > 0)
            {
                healTimer++;
                int interval = 700 - (100 * EntityStats.FullStats.DefenceLevel);
                if (interval < 200)
                    interval = 200;
                if (healTimer > interval)
                {
                    this.EntityStats.HP += 1;

                    healTimer = 0;
                }
            }
        }

        /// <summary>
        /// Removes projectiles and resets the player's attack state and position
        /// </summary>
        public void Reset()
        {
            this.AttackState = PlayerAttackState.None;
            this.direction = Direction.MovingRight;
            this.Transform.Velocity = Vector2.Zero;
            this.meleeAttackTimer = 0;
            this.magicAttackTimer = 0;
            this.LandedMeleeHit = false;
            this.projectileManager.Projectiles.Clear();
            if (this.EntityStats != null)
                this.EntityStats.HP = this.EntityStats.FullStats.MaxHP;
            this.Sprite.Color = new Color(255, 255, 255, 255);
            //this.Transform.Position = new Vector2(1280 / 2, 720 / 2);
        }
    }
}
