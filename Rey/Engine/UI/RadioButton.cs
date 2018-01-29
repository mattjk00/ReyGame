using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.UI
{
    public class RadioButton : UIObject
    {
        public RadioButton(string name) : base(name) { }

        private List<Button> options = new List<Button>();
        private int optionChosen = 0; // the chosen option

        public Texture2D SelectedTexture { get; protected set; }
        public Texture2D NotSelectedTexture { get; protected set; }

        /// <summary>
        /// add a new radio button option
        /// </summary>
        /// <param name="opt"></param>
        public void AddOption(string opt)
        {
            // create a button whose name is its index
            var button = new Button(options.Count.ToString())
            {
                Text = opt,
            };
            button.Transform.Position = new Vector2(1, options.Count * button.Sprite.Texture.Height);
            button.LoadTextures(this.NotSelectedTexture, this.NotSelectedTexture);
            // select the button when clicked
            button.OnClick += () =>
            {
                // should get the index of the button
                var index = int.Parse(button.Name);

            };
        }
    }
}
