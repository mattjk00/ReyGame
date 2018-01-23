﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rey.Engine
{
    /// <summary>
    /// Holds Data for a game object
    /// </summary>
    public class GameObject
    {
        
        public string Name { get; protected set; }
        public Transform Transform { get; set; }
        public Sprite Sprite { get; set; }
        public List<Rectangle> BoundingBoxes { get; set; }
        public bool IsEnemy { get; protected set; }
        public bool ToBeDestroyed { get; set; } // the status of the game object
        public List<Behavior> Behaviors { get; set; } = new List<Behavior>();

        public GameObject()
        {
            this.Transform = new Transform();
            this.Sprite = new Sprite();
            this.BoundingBoxes = new List<Rectangle>();
        }

        /// <summary>
        /// Genertic Constructor, requires name
        /// </summary>
        public GameObject(string name)
        {
            this.Transform = new Transform();
            this.Sprite = new Sprite();
            this.Name = name;
            this.BoundingBoxes = new List<Rectangle>();
        }

        /// <summary>
        /// Constructor with a position option
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        public GameObject(string name, Vector2 pos)
        {
            this.Transform = new Transform(pos);
            this.Sprite = new Sprite();
            this.Name = name;
            this.BoundingBoxes = new List<Rectangle>();
        }

        /// <summary>
        /// Load textures and such, set properties. Could be used for prefabs
        /// </summary>
        public virtual void Load()
        {
            // create default bounds
            if (this.Sprite.Texture != null)
                this.Transform.Bounds = new Rectangle(0, 0, this.Sprite.Texture.Width, this.Sprite.Texture.Height);
            foreach (Behavior be in this.Behaviors)
                be.Load(this);
        }

        public virtual void Update()
        {
            this.Transform.Position += this.Transform.Velocity;

            // update the behaviors
            foreach (Behavior be in this.Behaviors)
                be.Update(this);
        }

        /// <summary>
        /// Default drawing options
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(this.Sprite.Texture, this.Transform.Position, this.Transform.Bounds, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, 1.0f,  SpriteEffects.None, 0);

            // draw the behaviors
            foreach (Behavior be in this.Behaviors)
                be.Draw(this, sb);
        }

        // adds a default bounding box based on sprite size
        public void AddDefaultBoundingBox()
        {
            this.BoundingBoxes.Add(
                new Rectangle((int)(this.Transform.Position.X - this.Transform.Origin.X), (int)(this.Transform.Position.Y - this.Transform.Origin.Y),
                    this.Sprite.Texture.Width, this.Sprite.Texture.Height));
        }
        // updates a given index to the default
        public void UpdateDefaultBox(int index)
        {
            this.BoundingBoxes[index] = new Rectangle((int)(this.Transform.Position.X - this.Transform.Origin.X), (int)(this.Transform.Position.Y - this.Transform.Origin.Y),
                this.Sprite.Texture.Width, this.Sprite.Texture.Height);
        }

        /// <summary>
        /// Adds a new behavior
        /// </summary>
        /// <param name="br"></param>
        public void AddBehavior(Behavior br)
        {
            this.Behaviors.Add(br);
        }
    }
}
