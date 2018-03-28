using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Rey.Engine;
using Rey.Engine.Memory;
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
        Button optionButton = new Button("option");

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
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "Rey Map File|*.guat";
                ofd.Title = "Save the map";
                System.Windows.Forms.DialogResult dialogResult = ofd.ShowDialog();

                if (ofd.FileName != "" && dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    Load(ofd.FileName);
                }
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

            optionButton.LoadTextures("Textures/ui/options_button.png", "Textures/ui/options_button_hover.png");
            optionButton.LocalPosition = new Vector2(200, 0);
            optionButton.OnClick += () =>
            {
                OptionForm optionForm = new OptionForm();
                optionForm.ShowDialog();
            };

            this.AddObject(optionButton);


            base.Load();
        }

        public override void Update()
        {
            base.Update();
            if (EditorManager.SelectedTile == null)
                optionButton.IsActive = false;
            else
                optionButton.IsActive = true;
        }

        void Save(string filename)
        {

            Map map = new Map();

            List<Tile> exportTiles = new List<Tile>();
            foreach(Tile tile in parent.tiles)
            {
                if (tile.TileType != TileType.Empty)
                {
                    var stile = tile as EditorTile;
                    // creat the new tile
                    var newTile = new Tile(stile.Name, stile.StartingPosition, null, stile.TileType);
                    newTile.Depth = stile.Depth;
                    newTile.Data = stile.Data;
                    
                    exportTiles.Add(newTile);
                }
            }

            map.Tiles = exportTiles;

            List<MapMarker> markers = new List<MapMarker>();
            foreach (Tile tile in parent.tiles)
            {
                // convert to an editor tile
                var stile = tile as EditorTile;
                if (stile.marker != null)
                {
                    var newMarker = new MapMarker(stile.marker.Name, stile.marker.StartingPosition, null, stile.marker.MarkerType);
                    
                    markers.Add(newMarker);
                }
            }
            map.Markers = markers;

            string json = JsonConvert.SerializeObject(map);

            File.WriteAllText(filename, json);
            
        }

        void Load(string filename)
        {
            // load the map
            Map map = Map.LoadFromFile(filename);
            this.parent.ClearTiles(); // clear the parent tiles

            foreach(Tile tile in map.Tiles)
            {
                EditorTile newTile = new EditorTile("", Vector2.Zero, EditorManager.defaultTile, TileType.Empty);
                // handle normal tiles
                if (tile.TileType == TileType.Normal)
                    newTile = new EditorTile(tile.Name, tile.Transform.Position, AssetLoader.LoadTexture("Textures/" + tile.Name + ".png"), tile.TileType);
                else if (tile.TileType == TileType.Block)
                    newTile = new EditorTile(tile.Name, tile.Transform.Position, AssetLoader.LoadTexture("Textures/blocks/" + tile.Name + ".png"), tile.TileType);
                else if (tile.TileType == TileType.Door)
                    newTile = new EditorTile(tile.Name, tile.Transform.Position, AssetLoader.LoadTexture("Textures/doors/" + tile.Name + ".png"), tile.TileType);

                // set depth
                newTile.Depth = tile.Depth;
                // set data
                newTile.Data = tile.Data;

                // add the new tile
                if (newTile.Name != "")
                    this.parent.AddTile(newTile);
            }

            foreach (MapMarker marker in map.Markers)
            {
                // try and find the tile owner
                var tileOwner = this.parent.tiles.Find(x => x.Transform.Position == marker.StartingPosition) as EditorTile;

                if (tileOwner != null)
                {
                    // find the type of tile and load the map marker
                    if (marker.MarkerType == MarkerType.PlayerSpawnPoint)
                        tileOwner.SetMarker(new MapMarker(marker.Name, tileOwner.StartingPosition, AssetLoader.LoadTexture("textures/objects/" + marker.Name + ".png"), marker.MarkerType));
                    else if (marker.MarkerType == MarkerType.SpawnPoint)
                        tileOwner.SetMarker(new MapMarker(marker.Name, tileOwner.StartingPosition, AssetLoader.LoadTexture("textures/monsters/" + marker.Name + ".png"), marker.MarkerType));
                }
            }
        }
    }
}
