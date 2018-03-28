﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Rey.Engine.Prefabs
{
    public enum NPCState
    {
        Idle,
        Walking,
        Talking
    }

    /// <summary>
    /// The NPC of the game
    /// </summary>
    public class NPC : GameObject
    {
        // the state of the current NPC
        public NPCState State { get; set; } = NPCState.Idle;

        ChildObject body = new ChildObject();

        // walking variables
        int intervalWalk;
        int walkTimer; // keeps track of when to walk
        Vector2 target;
        Random random = new Random();
        Rectangle movementBounds = new Rectangle();
        Vector2 lastTarget = Vector2.Zero;


        // Collision Variables
        public Rectangle MovementBox { get; set; }
        public int MovementBoxOffset { get; set; } = 5;

        // interaction variables
        public List<string> Script = new List<string>()
        {
            "Hey are you Rey?",
            "I heard that the sword of Beldor is still out there...",
            "And it's the only thing that'll beat Rainus Crainus!"
        };

        // default constructor
        public NPC()
        {
            this.Name = "NPC";
            intervalWalk = random.Next(0, 0);

            this.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/NPCs/npc.png");
            this.AddDefaultBoundingBox();

            this.body.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/NPCs/npc_body.png");
            this.body.Load();

        }

        public override void Load()
        {
            base.Load();

            // create movement bounds
            this.movementBounds = new Rectangle((int)this.Transform.Position.X - 100, (int)this.Transform.Position.Y - 100, 200, 200);
        }

        public override void Update()
        {
            base.Update();
            this.UpdateDefaultBox(0);
            this.Transform.Velocity *= 0.9f;

            this.body.LocalPosition = new Vector2(-5, 40);
            this.body.Update(this);

            // CHANGE LATEr
            this.MovementBox = this.BoundingBoxes[0];

            // Call correct function for the state
            if (State == NPCState.Idle)
                HandleIdle();
            else if (State == NPCState.Walking)
                HandleWalk();
        }

        /// <summary>
        /// Prep to walk around
        /// </summary>
        public void HandleIdle()
        {
            walkTimer++;
            // ready to walk
            if (walkTimer > intervalWalk)
            {
                // reset variables
                walkTimer = 0;
                intervalWalk = random.Next(15, 25);

                var targetX = 0;
                var targetY = 0;

                // control the next target so that where he moves is evened out
                if (lastTarget.X >= 0)
                    targetX = random.Next(-50, 5);
                else
                    targetX = random.Next(-5, 50);

                if (lastTarget.Y >= 0)
                    targetY = random.Next(-51, 4);
                else
                    targetY = random.Next(-4, 51);

                target = new Vector2(this.Transform.Position.X + targetX, this.Transform.Position.Y + targetY); // choose the new target
                this.State = NPCState.Walking;

                this.lastTarget = new Vector2(targetX, targetY);
            }
        }

        /// <summary>
        /// Walk around
        /// </summary>
        public void HandleWalk()
        {
            // speed/direction vars
            float xSpeed = 1;
            float ySpeed = 1;

            // reverse speeds based on direction
            if (this.Transform.Position.X > this.target.X)
                xSpeed = -1;

            if (this.Transform.Position.Y > this.target.Y)
                ySpeed *= -1;

            this.Transform.Position += new Vector2(xSpeed, ySpeed);

            // stop movement
            if (this.Transform.Position.X > this.movementBounds.Right || this.Transform.Position.X < this.movementBounds.Left)
                this.State = NPCState.Idle;
            if (this.Transform.Position.Y > this.movementBounds.Bottom || this.Transform.Position.Y < this.movementBounds.Top)
                this.State = NPCState.Idle;

            // if arrived, set the state to idle
            if (Vector2.Distance(this.Transform.Position, this.target) < 20)
                this.State = NPCState.Idle;
        }

        public override void Draw(SpriteBatch sb)
        {
            this.body.Draw(sb);

            base.Draw(sb);

            // sb.Draw(this.body.Sprite.Texture, this.body.Transform.Position, Color.White);
        }
    }
}