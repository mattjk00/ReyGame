using Microsoft.Xna.Framework;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Prefabs
{
    public class TestUIFrame : Frame
    {
        Button quitButton = new Button("gobutton");

        public override void Load()
        {
            this.Name = "gameui";

            quitButton.LoadTextures("Assets/Textures/UI/go_button_normal.png", "Assets/Textures/UI/go_button_hover.png");
            quitButton.Transform.Position = new Vector2(640, 220);
            quitButton.IsActive = false;
            quitButton.OnClick += () => {
                SceneManager.TryToGoToNextFloor();
            };

            this.objects.Add(quitButton);

            base.Load();
        }

        
    }
}
