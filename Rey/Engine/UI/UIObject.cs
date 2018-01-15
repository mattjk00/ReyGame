using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Rey.Engine.UI
{
    // a ui object is in the scene and holds basic actions for clicks and scaling
    public class UIObject : GameObject
    {
        // a button click action
        public Action OnClick;
        public Action OnHover;
        public Action OnMouseLeave;

        protected bool isMouseOn = false;
        public bool IsActive { get; set; } = true;
        public string Name { get; set; } = "";

        public Vector2 LocalPosition { get; set; }

        public UIObject(string name)
        {
            // add some default delegates
            this.OnClick += () => { };
            this.OnHover += () => { };
            this.OnMouseLeave += () => { };
            this.Name = name;
        }

        // rescale the ui object
        public void Scale(int width, int height)
        {
            // actually do this sometime
        }

        public virtual void DrawUI(SpriteBatch sb, Frame frame)
        {
            Rectangle bounds = new Rectangle(0, 0, 0, 0);
            if (this.Transform.Position.Y + this.Sprite.Texture.Height  < frame.Position.Y + frame.Height && this.Transform.Position.Y > frame.Position.Y)
                bounds = new Rectangle(0, 0, this.Sprite.Texture.Width, this.Sprite.Texture.Height);
            sb.Draw(this.Sprite.Texture, this.Transform.Position, bounds, Color.White);
        }

        /// <summary>
        /// Update the UI with the given mouse coordinates
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        public void UpdateUI(MouseState mouse, MouseState oldMouse, Frame frame)
        {
            // create a hitbox for the mouse
            Rectangle mouseBox = new Rectangle(InputHelper.MousePosition.ToPoint(), new Point(1, 1));

            // set the UI's position based off the local position, the position of the parent frame, and the scroll
            this.Transform.Position = this.LocalPosition + frame.Position + new Vector2(0, frame.ScrollDistance);
            
            if (this.IsActive)
            {
                // check all bounding boxes
                foreach (Rectangle boundingBox in this.BoundingBoxes)
                {
                    // if the button and the mouse intersect
                    if (boundingBox.Intersects(mouseBox))
                    {
                        // trigger the hover method
                        if (this.isMouseOn == false)
                        {
                            this.OnHover();
                            this.isMouseOn = true;
                        }

                        // if clicked
                        if (mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton == ButtonState.Released)
                        {
                            // trigger the click event
                            this.OnClick();
                        }
                    }
                    else
                    {
                        // if the mouse leaves the button, trigger the on mouse leave event
                        if (this.isMouseOn == true)
                        {
                            this.OnMouseLeave();
                            this.isMouseOn = false;
                        }
                    }
                }

            }        }
    }
}
