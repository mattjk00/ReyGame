using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Rey.Engine.UI
{
    /// <summary>
    /// A UI frame with tabs
    /// </summary>
    public class TabFrame : Frame
    {
        // the tabs
        protected List<Frame> tabs = new List<Frame>();
        protected List<Vector2> tabStartingPositions = new List<Vector2>();
        protected int currentTab = 0;

        /// <summary>
        /// Adds a new tab to this UI frame, as well as a button to click on it
        /// </summary>
        /// <param name="frame"></param>
        public void AddTab(Frame frame, string normalTab, string hoverTab)
        {
            this.tabs.Add(frame);
            this.tabStartingPositions.Add(frame.Position);

            // create tab button
            var tabButton = new Button((this.tabs.Count - 1).ToString())
            {
                LocalPosition = new Vector2(0, -25),
                Text = frame.Name
            };
            tabButton.LoadTextures(normalTab, hoverTab);
            tabButton.Sprite.Color = Color.Black;

            // adjust tabs
            //if (this.tabs.Count > 1)
                

            

            tabButton.OnClick += () =>
            {
                // set the current tab to the tab number. The number of the tab is stored in the button name
                this.currentTab = int.Parse(tabButton.Name);
            };
            this.AddObject(tabButton);
        }

        public override void Load()
        {
            base.Load();
            // load all the frames
            foreach (Frame frame in this.tabs)
                frame.Load();
            int index = 0;
            foreach(Button tabButton in this.objects)
            {
                // stretch the buttons
                tabButton.Transform.Bounds = new Rectangle(0, 0, (int)((float)this.Width / (float)(this.tabs.Count)), tabButton.normalTexture.Height);
                tabButton.LocalPosition = new Vector2(tabButton.Transform.Bounds.Width * index, -25);
                index++;
            }
        }

        public override void Update()
        {
            base.Update();

            // update the current tab
            if (this.tabs.Count > 0)
            {
                tabs[currentTab].Position = this.Position + tabStartingPositions[currentTab];
                tabs[currentTab].Update();
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            // draw the backgroundif possible
            if (this.Background != null)
            {
                sb.Draw(Background, this.Position, null, Color.White, 0, Vector2.Zero,
                    new Vector2((float)this.Width / (float)Background.Width, (float)this.Height / (float)Background.Height), SpriteEffects.None, 0);
            }

            if (this.tabs.Count > 0)
            {
                tabs[currentTab].Draw(sb);
            }

            foreach (UIObject ui in this.objects)
            {
                if (ui.IsActive)
                    ui.DrawUI(sb, this);
            }
        }
    }
}
