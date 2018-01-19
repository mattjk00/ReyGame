using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Rey.Engine;
using Rey.Engine.Prefabs;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor
{
    public class EditorScene : Scene
    {
        Frame ui;
        GameObject currentTile;
        FileManagerFrame fileManagerFrame;
        KeyboardState keyboard;
        Vector2 mapPosition = new Vector2(); // the position of the map

        TabFrame tabFrame = new TabFrame();
        ObjectChooserFrame objChooserFrame = new ObjectChooserFrame();

        public override void Load()
        {
            this.Name = "editor";
            this.CombatScene = false;


            ui = new Frame();
            ui.Width = 300;
            ui.Height = 720;
            ui.Scrollable = true;
            ui.ScrollLimits = new Vector2(75, 1000);

            ui.Background = AssetLoader.LoadTexture("Textures/ui/ui.png");

            // get how many files are in the textures.
            var files = System.IO.Directory.GetFiles("Textures");
            int row = 0; // row to put tiles in 
            int column = 0;

            for (int i = 0; i < files.Length; i++)
            {
                // create a button in the right row
                var button = new Button(files[i].Split('.')[0].Replace("Textures\\", ""))
                {
                    LocalPosition = new Vector2((column * 60) + 25, (row * 60) + 25)
                };
                // load the texture
                button.LoadTextures(files[i], files[i]);
                button.OnClick += () =>
                {
                    EditorManager.TileMode = true;
                    EditorManager.currentTile = new Tile(button.Name, Vector2.Zero, button.normalTexture, TileType.Normal);
                    EditorManager.currentTileName = button.Name;
                };
                ui.AddObject(button);

                column++;

                // prevetn overlap, every 3 squares go to the next row
                if (column > 3)
                {
                    column = 0;
                    row++;
                }
            }

            // remove the default tile
            this.ui.objects.RemoveAll(x => x.Name == "default_tile");
            // fix the delete button
            this.ui.objects.Find(x => x.Name == "delete").OnClick += () => 
            {
                EditorManager.currentTile = new Tile("", Vector2.Zero, EditorManager.defaultTile, TileType.Empty);
            };

            
            fileManagerFrame = new FileManagerFrame();
            fileManagerFrame.parent = this;

            objChooserFrame = new ObjectChooserFrame();

            tabFrame.Width = 1920;
            tabFrame.Height = 720;
            tabFrame.Position = new Vector2(0, 50);
            tabFrame.AddTab(ui);
            tabFrame.AddTab(objChooserFrame);

            this.AddFrame(fileManagerFrame);
            this.AddFrame(tabFrame);


            var grassButton = this.ui.Find("grass1") as Button;

            currentTile = new GameObject("currentTile", new Vector2(300, 25));
            currentTile.Sprite.Texture = grassButton.normalTexture;
            this.AddGameObject(currentTile);

            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    EditorTile tile = new EditorTile("ocean1", new Vector2(i * 50, j * 50), this.ui.Find("ocean1").Sprite.Texture, TileType.Block);
                    this.AddTile(tile);
                }
            }


            EditorManager.currentTile = new Tile(grassButton.Name, Vector2.Zero, grassButton.normalTexture, TileType.Normal);

           

            base.Load();
        }

        public override void Update(Camera2D camera)
        {
            keyboard = Keyboard.GetState();

            int scrollSpeed = 7;

            if (keyboard.IsKeyDown(Keys.LeftShift))
                scrollSpeed = 14;

            // handle inputs
            if (keyboard.IsKeyDown(Keys.D))
            {
                this.mapPosition.X -= scrollSpeed;
            }
            // handle inputs
            if (keyboard.IsKeyDown(Keys.A))
            {
                this.mapPosition.X += scrollSpeed;
            }
            // handle inputs
            if (keyboard.IsKeyDown(Keys.W))
            {
                this.mapPosition.Y += scrollSpeed;
            }
            // handle inputs
            if (keyboard.IsKeyDown(Keys.S))
            {
                this.mapPosition.Y -= scrollSpeed;
            }

            base.Update(camera);
        }

        public override void Draw(SpriteBatch sb)
        {
            foreach (Tile tile in this.tiles)
            {
                var stile = tile as EditorTile;
                
                stile.Transform.Position = this.mapPosition + stile.StartingPosition;
                sb.Draw(stile.Sprite.Texture, stile.Transform.Position, Color.White);
                if (stile.marker != null)
                {
                    sb.Draw(stile.marker.Texture, stile.Transform.Position, Color.White);
                }
            }

            foreach (GameObject go in this.gameObjects)
                go.Draw(sb);

            foreach (Frame frame in this.frames)
                frame.Draw(sb);
        }
    }
}
