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
        public Action OnRightClick;
        public Action OnHover;
        public Action OnMouseLeave;

        protected bool isMouseOn = false;
        public bool IsActive { get; set; } = true;
        public string Name { get; set; } = "";

        public Vector2 LocalPosition { get; set; }

        public bool LockedPosition { get; set; }


        public UIObject(string name)
        {
            // add some default delegates
            this.OnClick += () => { };
            this.OnRightClick += () => { };
            this.OnHover += () => { };
            this.OnMouseLeave += () => { };
            this.Name = name;
            this.Sprite.Color = Color.White;
        }

        // rescale the ui object
        public void Scale(int width, int height)
        {
            // actually do this sometime
        }

        public virtual void DrawUI(SpriteBatch sb, Frame frame)
        {
            Rectangle bounds = new Rectangle(0, 0, 0, 0);



            if ((this.Transform.Position.Y + this.Sprite.Texture.Height < frame.Position.Y + frame.Height && this.Transform.Position.Y > frame.Position.Y) || frame.Scrollable == false)
                bounds = this.Transform.Bounds;
            sb.Draw(this.Sprite.Texture, this.Transform.Position, bounds, this.Sprite.Color);
        }

        public virtual void DrawUI(SpriteBatch sb, Frame frame, int w, int h)
        {
            Rectangle bounds = new Rectangle(0, 0, 0, 0);



            if ((this.Transform.Position.Y + this.Sprite.Texture.Height < frame.Position.Y + frame.Height && this.Transform.Position.Y > frame.Position.Y) || frame.Scrollable == false)
                bounds = this.Transform.Bounds;
            sb.Draw(this.Sprite.Texture, this.Transform.Position, bounds, this.Sprite.Color, 0, Vector2.Zero, InputHelper.ScaleTexture(this.Sprite.Texture, w, h), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Update the UI with the given mouse coordinates
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        public void UpdateUI(MouseState mouse, MouseState oldMouse, Frame frame)
        {
            Rectangle mouseBox;
            // create a hitbox for the mouse
            if (this.LockedPosition == false)
                mouseBox = new Rectangle(InputHelper.MousePosition.ToPoint(), new Point(1, 1));
            else
                mouseBox = new Rectangle(Mouse.GetState().Position, new Point(1, 1));

            // set the UI's position based off the local position, the position of the parent frame, and the scroll
            this.Transform.Position = this.LocalPosition + frame.Position + new Vector2(0, frame.ScrollDistance);

            // in case bounds are empty, fix them
            if (this.Transform.Bounds == new Rectangle(0, 0, 0, 0))
            {
                if (this.Sprite.Texture != null)
                    this.Transform.Bounds = new Rectangle(0, 0, this.Sprite.Texture.Width, this.Sprite.Texture.Height);
                else
                    this.Transform.Bounds = new Rectangle(0, 0, 1, 1); // for labels?
            }
            this.BoundingBoxes[0] = new Rectangle((int)this.Transform.Position.X, (int)this.Transform.Position.Y, this.Transform.Bounds.Width, this.Transform.Bounds.Height);


            if (this.IsActive)
            {
                try
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
                            // if right clicked
                            if (mouse.RightButton == ButtonState.Pressed && oldMouse.RightButton == ButtonState.Released)
                            {
                                // trigger the click event
                                this.OnRightClick();
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
                }
                catch (InvalidOperationException ioe) { }
                }
            }        
    }
}
