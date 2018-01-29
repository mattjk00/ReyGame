using Microsoft.Xna.Framework;
using Rey.Engine;
using Rey.Engine.Prefabs;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor
{
    class TileChooserFrame :Frame
    {
        string path;
        TileType tileType;

        public TileChooserFrame(string fpath, TileType tt)
        {
            this.path = fpath; // which directory to search
            this.tileType = tt; // which type of tiles
        }

        public override void Load()
        {
            
            
            this.Width = 300;
            this.Height = 720;
            this.Scrollable = true;
            this.ScrollLimits = new Vector2(75, 1000);

            this.Background = AssetLoader.LoadTexture("Textures/ui/ui.png");

            // get how many files are in the textures.
            var files = System.IO.Directory.GetFiles(this.path);
            int row = 0; // row to put tiles in 
            int column = 0;

            for (int i = 0; i < files.Length; i++)
            {
                // create a button in the right row
                var button = new Button(files[i].Split('.')[0].Replace(this.path, "").Replace("\\", ""))
                {
                    LocalPosition = new Vector2((column * 60) + 25, (row * 60) + 25)
                };
                // load the texture
                button.LoadTextures(files[i], files[i]);
                button.OnClick += () =>
                {
                    EditorManager.TileMode = true;
                    EditorManager.currentTile = new Tile(button.Name, Vector2.Zero, button.normalTexture, this.tileType);
                    EditorManager.currentTileName = button.Name;
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

            // remove the default tile
            this.objects.RemoveAll(x => x.Name == "default_tile");
            // fix the delete button
            var deleteButton = this.objects.Find(x => x.Name == "delete");
            if (deleteButton != null)
            {
                deleteButton.OnClick += () =>
                {
                    EditorManager.currentTile = new Tile("", Vector2.Zero, EditorManager.defaultTile, TileType.Empty);
                };
            }

            base.Load();
        }
    }
}
