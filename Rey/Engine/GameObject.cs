using System;
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
        
        public string Name { get; set; } // the game object's name
        public Transform Transform { get; set; } // the position data for the object
        public Sprite Sprite { get; set; } // the drawing data for the object
        public List<Rectangle> BoundingBoxes { get; set; } // Collision detection boxes
        public bool IsEnemy { get; protected set; } // Is this game object an enemy?
        public bool ToBeDestroyed { get; set; } // the status of the game object
        public List<Behavior> Behaviors { get; set; } = new List<Behavior>(); // Scripts for the object
        protected bool hasDefaultBox = false;

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
            try
            {
                sb.Draw(this.Sprite.Texture, this.Transform.Position, this.Transform.Bounds, this.Sprite.Color, this.Transform.Rotation, this.Transform.Origin, this.Transform.Scale, SpriteEffects.None, 0);
            }
            catch (ArgumentNullException ane)
            {

            }
                // draw the behaviors
            foreach (Behavior be in this.Behaviors)
                be.Draw(this, sb);
        }

        protected void DrawBoundingBoxes(SpriteBatch sb)
        {
            foreach (Rectangle rect in this.BoundingBoxes)
            {
                sb.Draw(AssetLoader.BoundingBoxTexture, rect.Location.ToVector2(), null, Color.Green, 0, Vector2.Zero,
                    InputHelper.ScaleTexture(AssetLoader.BoundingBoxTexture, rect.Width, rect.Height),
                    SpriteEffects.None, 0);

                
            }
        }

        protected void DrawBox(SpriteBatch sb, Rectangle rect)
        {
            sb.Draw(AssetLoader.BoundingBoxTexture, rect.Location.ToVector2(), null, Color.Red, 0, Vector2.Zero,
                    InputHelper.ScaleTexture(AssetLoader.BoundingBoxTexture, rect.Width, rect.Height),
                    SpriteEffects.None, 0);
        }

        // adds a default bounding box based on sprite size
        public void AddDefaultBoundingBox()
        {
            if (this.Sprite.Texture != null && this.hasDefaultBox == false)
            {
                this.BoundingBoxes.Add(
                    new Rectangle((int)(this.Transform.Position.X - this.Transform.Origin.X), (int)(this.Transform.Position.Y - this.Transform.Origin.Y),
                        this.Sprite.Texture.Width, this.Sprite.Texture.Height));
                this.hasDefaultBox = true;
            }
        }


        // updates a given index to the default
        public void UpdateDefaultBox(int index)
        {
            if (this.Sprite.Texture != null)
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
