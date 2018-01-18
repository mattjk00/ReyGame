using Microsoft.Xna.Framework;
using Rey.Engine;
using Rey.Engine.Memory;
using Rey.Engine.Prefabs;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor
{
    public class ObjectChooserFrame : Frame
    {
        public override void Load()
        {
            this.Width = 300;
            this.Height = 720;
            this.Scrollable = true;
            this.ScrollLimits = new Vector2(75, 1000);

            this.Background = AssetLoader.LoadTexture("Textures/ui/ui.png");

            // get how many files are in the textures.
            var files = System.IO.Directory.GetFiles("Textures/objects");
            int row = 0; // row to put tiles in 
            int column = 0;

            for (int i = 0; i < files.Length; i++)
            {
                // create a button in the right row
                var button = new Button(files[i].Split('.')[0].Replace("Textures\\Objects\\", ""))
                {
                    LocalPosition = new Vector2((column * 60) + 25, (row * 60) + 25)
                };
                // load the texture
                button.LoadTextures(files[i], files[i]);
                button.OnClick += () =>
                {
                    EditorManager.TileMode = false;
                    EditorManager.currentMarker = new MapMarker("", Vector2.Zero, button.normalTexture, MarkerType.PlayerSpawnPoint);
                };
                this.AddObject(button);

                column++;

                // prevetn overlap, every 3 squares go to the next row
                if (column > 3)
                {
                    column = 0;
                    row++;
                }
            }

            base.Load();
        }
    }
}
