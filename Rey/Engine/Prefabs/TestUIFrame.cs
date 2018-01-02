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
        Button quitButton = new Button();

        public override void Load()
        {
            quitButton.LoadTextures("Assets/Textures/player/default_head.png", "Assets/Textures/player/shadow.png");
            quitButton.Text = "HEY!";
            quitButton.OnClick += () => {
                quitButton.Text = "QUIT!";
            };

            this.objects.Add(quitButton);

            base.Load();
        }

        
    }
}
