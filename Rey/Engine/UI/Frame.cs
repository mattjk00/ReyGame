using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.UI
{
    // It's a UI frame
    public class Frame
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Position { get; set; }
        protected MouseState mouse;
        protected MouseState oldMouse;
        public string Name { get; set; } = "";
        public int ScrollDistance { get; set; } = 0;
        public bool Scrollable { get; set; } = false;
        private int previousScrollValue;
        public Texture2D Background { get; set; }
        public Vector2 ScrollLimits { get; set; } = new Vector2(0, 0); // The limit of the scrolling X = min, Y = max
        public bool LockedPosition { get; set; } = false;
        public List<UIObject> objects = new List<UIObject>();

        

        public virtual void Update()
        {
            // get the mouse state
            mouse = Mouse.GetState();

            // if scrollable, scroll
            if (this.Scrollable)
                this.HandleScroll();

            // update the ui objects
            foreach (UIObject ui in this.objects)
            {
                //ui.Update();
                ui.UpdateUI(mouse, oldMouse, this);

                // if the ui object is locked
                /*if (this.LockedPosition == true)
                {
                    // iterate over the bounding boxes in the frame
                    for (int i = 0; i < ui.BoundingBoxes.Count; i++)
                    {
                        // convert the bounding box so it can be clicked on by the sxcreen
                        var r = ui.BoundingBoxes[i]; // cache
                        var pos = InputHelper.ConvertToWindowPoint(r.Location.ToVector2());
                        ui.BoundingBoxes[i] = new Rectangle((int)pos.X, (int)pos.Y, r.Width, r.Height);
                    }
                }*/
            }
            // update old mouse
            oldMouse = mouse;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            // draw the backgroundif possible
            if (this.Background != null)
            {
                sb.Draw(Background, this.Position, null, Color.White, 0, Vector2.Zero,
                    new Vector2((float)this.Width / (float)Background.Width, (float)this.Height / (float)Background.Height), SpriteEffects.None, 0);
            }
            foreach (UIObject ui in this.objects)
            {
                if (ui.IsActive)
                    ui.DrawUI(sb, this);
            }
        }

        public virtual void Load()
        {

        }

        public UIObject Find(string name)
        {
            var ui = this.objects.Find(x => x.Name == name);
            return ui;
        }

        

        public void AddObject(UIObject ui)
        {
            ui.LockedPosition = this.LockedPosition;
            this.objects.Add(ui);
        }

        /// <summary>
        /// Handles the scrolling action
        /// </summary>
        void HandleScroll()
        {
            // create a bounding box of the frame itself
            Rectangle frameBox = new Rectangle(this.Position.ToPoint(), new Point(this.Width, this.Height));

            if (mouse.ScrollWheelValue != this.previousScrollValue)
            {
                // how much to move the object based on scroll
                var offset = (mouse.ScrollWheelValue - previousScrollValue)/4;
                // update the scroll distance of the frame
                this.ScrollDistance += offset; 
            }

            // stop the scroller from going to infinity
            if (this.ScrollLimits != Vector2.Zero)
            {
                if (this.ScrollDistance < this.ScrollLimits.X)
                    this.ScrollDistance = (int)this.ScrollLimits.X;
                if (this.ScrollDistance > this.ScrollLimits.Y)
                    this.ScrollDistance = (int)this.ScrollLimits.Y;
            }

            // remember the last scroll wheel value
            previousScrollValue = mouse.ScrollWheelValue;
        }
    }
}
