using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Rey.Engine;
using Rey.Engine.Prefabs;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor
{
    public class FileManagerFrame : Frame
    {
        Button loadButton = new Button("load");
        Button saveButton = new Button("save");

        public EditorScene parent;

        public override void Load()
        {
            this.Name = "fileui";
            this.Scrollable = false;
            this.Background = AssetLoader.LoadTexture("Textures/ui/fileui.png");

            this.Width = 1280;
            this.Height = 20;

            loadButton.LoadTextures("Textures/ui/load_button.png", "Textures/ui/load_button_hover.png");
            loadButton.LocalPosition = new Vector2(0, 0);
            loadButton.OnClick += () =>
            {

            };

            saveButton.LoadTextures("Textures/ui/save_button.png", "Textures/ui/save_button_hover.png");
            saveButton.LocalPosition = new Vector2(100, 0);
            saveButton.OnClick += () =>
            {
                // save the file
                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
                sfd.Filter = "Rey Map File|*.guat";
                sfd.Title = "Save the map";
                System.Windows.Forms.DialogResult dialogResult = sfd.ShowDialog();
                
                if (sfd.FileName != "" && dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    Save(sfd.FileName);
                }
            };

            this.AddObject(loadButton);
            this.AddObject(saveButton);

            base.Load();
        }

        void Save(string filename)
        {

            List<Tile> exportTiles = new List<Tile>();
            foreach(Tile tile in parent.tiles)
            {
                if (tile.TileType != TileType.Empty)
                {
                    // creat the new tile
                    var newTile = new Tile(tile.Transform.Position, null, tile.TileType);
                    exportTiles.Add(newTile);
                }
            }

            string json = JsonConvert.SerializeObject(exportTiles);

            File.WriteAllText(filename, json);
            
        }
    }
}
