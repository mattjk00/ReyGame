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
        protected MouseState mouse;
        protected MouseState oldMouse;
        public string Name { get; set; } = "";

        protected List<UIObject> objects = new List<UIObject>();

        public virtual void Update()
        {
            // get the mouse state
            mouse = Mouse.GetState();
            // update the ui objects
            foreach (UIObject ui in this.objects)
            {
                ui.Update();
                ui.UpdateUI(mouse, oldMouse);
            }
            // update old mouse
            oldMouse = mouse;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            foreach (UIObject ui in this.objects)
            {
                if (ui.IsActive)
                    ui.Draw(sb);
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
    }
}
