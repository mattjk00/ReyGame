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
    public class EditorScene : Scene
    {
        Frame ui;
        GameObject currentTile;
        Frame fileManagerFrame;

        public override void Load()
        {
            this.Name = "editor";
            this.CombatScene = false;


            ui = new Frame();
            ui.Width = 300;
            ui.Height = 720;
            ui.Scrollable = true;
            ui.ScrollLimits = new Vector2(75, 1000);

            ui.Background = AssetLoader.LoadTexture("Textures/ui.png");

            var delButtonn = new Button("ui_tile")
            {
                LocalPosition = new Vector2(25, 25)
            };
            delButtonn.LoadTextures("Textures/delete.png", "Textures/delete.png");
            delButtonn.OnClick += () =>
            {
                EditorManager.currentTile = new Tile(Vector2.Zero, EditorManager.defaultTile, TileType.Empty);
            };
            ui.AddObject(delButtonn);

            var grassButtonn = new Button("ui_tile")
            {
                LocalPosition = new Vector2(100, 25)
            };
            grassButtonn.LoadTextures("Textures/grass1.png", "Textures/grass1.png");
            grassButtonn.OnClick += () =>
            {
                EditorManager.currentTile = new Tile(Vector2.Zero, grassButtonn.normalTexture, TileType.Normal);
            };
            ui.AddObject(grassButtonn);

            var grassButtonn2 = new Button("ui_tile")
            {
                LocalPosition = new Vector2(175, 25)
            };
            grassButtonn2.LoadTextures("Textures/grass2.png", "Textures/grass2.png");
            grassButtonn2.OnClick += () =>
            {
                EditorManager.currentTile = new Tile(Vector2.Zero, grassButtonn2.normalTexture, TileType.Normal);
            };
            ui.AddObject(grassButtonn2);

            var waterButtonn = new Button("ui_tile")
            {
                LocalPosition = new Vector2(25, 100)
            };
            waterButtonn.LoadTextures("Textures/ocean1.png", "Textures/ocean1.png");
            waterButtonn.OnClick += () =>
            {
                EditorManager.currentTile = new Tile(Vector2.Zero, waterButtonn.normalTexture, TileType.Normal);
            };
            ui.AddObject(waterButtonn);


            this.AddFrame(ui);

            fileManagerFrame = new FileManagerFrame();
            this.AddFrame(fileManagerFrame);

            currentTile = new GameObject("currentTile", new Vector2(300, 25));
            currentTile.Sprite.Texture = grassButtonn.normalTexture;
            this.AddGameObject(currentTile);

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    EditorTile tile = new EditorTile(new Vector2(i * 50, j * 50), EditorManager.defaultTile, TileType.Normal);
                    this.AddTile(tile);
                }
            }


            EditorManager.currentTile = new Tile(Vector2.Zero, grassButtonn.normalTexture, TileType.Normal);

           

            base.Load();
        }

    }
}
