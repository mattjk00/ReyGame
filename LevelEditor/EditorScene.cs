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
        //Frame ui;
        GameObject currentTile;
        FileManagerFrame fileManagerFrame;
        KeyboardState keyboard;
        Vector2 mapPosition = new Vector2(); // the position of the map

        TabFrame tabFrame = new TabFrame();
        ObjectChooserFrame objChooserFrame;

        ObjectChooserFrame monsterChooser;
        ObjectChooserFrame npcChooser;
        ObjectChooserFrame lightChooser;

        TileChooserFrame tileChooser;
        TileChooserFrame blockChooser;
        TileChooserFrame doorChooser;

        OptionFrame optionFrame;

        public LevelEditor game; // the game

        public int Width { get; private set; }
        public int Height { get; private set; }

        Button grassButton;

        public override void Load()
        {
            this.Name = "editor";
            this.CombatScene = false;



            fileManagerFrame = new FileManagerFrame();
            fileManagerFrame.parent = this;

            objChooserFrame = new ObjectChooserFrame("Textures/objects");
            objChooserFrame.Name = "Markers";

            monsterChooser = new ObjectChooserFrame("Textures/monsters");
            monsterChooser.Name = "Monsters";

            npcChooser = new ObjectChooserFrame("Textures/npcs");
            npcChooser.Name = "NPCs";

            lightChooser = new ObjectChooserFrame("Textures/lights");
            lightChooser.Name = "Lights";

            tileChooser = new TileChooserFrame("Textures", TileType.Normal);
            tileChooser.Name = "Tiles";
            tileChooser.Load();

            blockChooser = new TileChooserFrame("Textures/blocks", TileType.Block);
            blockChooser.Name = "Blocks";
            blockChooser.Load();

            doorChooser = new TileChooserFrame("Textures/doors", TileType.Door);
            doorChooser.Name = "Doors";
            doorChooser.Load();

            tabFrame.Width = 300;
            tabFrame.Height = 720;
            tabFrame.Position = new Vector2(0, 50);
            tabFrame.AddTab(tileChooser, "Textures/ui/tab.png", "Textures/ui/tab_hover.png");
            tabFrame.AddTab(blockChooser, "Textures/ui/tab.png", "Textures/ui/tab_hover.png");
            tabFrame.AddTab(monsterChooser, "Textures/ui/tab.png", "Textures/ui/tab_hover.png");
            tabFrame.AddTab(npcChooser, "Textures/ui/tab.png", "Textures/ui/tab_hover.png");
            tabFrame.AddTab(lightChooser, "Textures/ui/tab.png", "Textures/ui/tab_hover.png");

            tabFrame.AddTab(objChooserFrame, "Textures/ui/tab.png", "Textures/ui/tab_hover.png");
            tabFrame.AddTab(doorChooser, "Textures/ui/tab.png", "Textures/ui/tab_hover.png");

            optionFrame = new OptionFrame();
            

            this.AddFrame(fileManagerFrame);
            this.AddFrame(tabFrame);
            this.AddFrame(optionFrame);
            

            grassButton = tileChooser.Find("grass1") as Button;

            currentTile = new GameObject("currentTile", new Vector2(300, 25));
            currentTile.Sprite.Texture = grassButton.normalTexture;
            this.AddGameObject(currentTile);

            StartForm startForm = new StartForm();
            var dialogResult = startForm.ShowDialog();

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.LoadTiles(startForm.Width, startForm.Height);
            }
            else
            {
                game.Exit();
            }
           

            base.Load();
        }

        public void LoadTiles(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    EditorTile tile = new EditorTile("ocean1", new Vector2(i * 50, j * 50), blockChooser.Find("ocean1").Sprite.Texture, TileType.Block);
                    this.AddTile(tile);
                }
            }


            EditorManager.currentTile = new Tile(grassButton.Name, Vector2.Zero, grassButton.normalTexture, TileType.Normal);
        }

        int rotateTimer = 0;

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

            rotateTimer++;
            if (rotateTimer > 25)
            {
                rotateTimer = 0;
                if (keyboard.IsKeyDown(Keys.Right))
                {
                    EditorManager.currentTile.Transform.Rotation += MathHelper.ToRadians(90);
                }
                if (keyboard.IsKeyDown(Keys.Left))
                {
                    EditorManager.currentTile.Transform.Rotation -= MathHelper.ToRadians(90);
                }
            }

            base.Update(camera);
        }

        public override void Draw(SpriteBatch sb)
        {
            foreach (Tile tile in this.tiles)
            {
                var stile = tile as EditorTile;
                
                stile.Transform.Position = this.mapPosition + stile.StartingPosition;
                if (stile.Sprite.Texture != null)
                    sb.Draw(stile.Sprite.Texture, stile.Transform.Position + new Vector2(stile.Sprite.Texture.Width / 2, stile.Sprite.Texture.Height / 2), null, Color.White, stile.Transform.Rotation, new Vector2(stile.Sprite.Texture.Width/2, stile.Sprite.Texture.Height/2), Vector2.One, SpriteEffects.None, 0);
                if (stile.marker != null)
                {
                    sb.Draw(stile.marker.Texture, stile.Transform.Position, Color.White);
                }

                if (stile.Selected)
                {
                    sb.Draw(EditorManager.selectedTileTexture, tile.Transform.Position, Color.White);
                }
            }

            foreach (GameObject go in this.gameObjects)
                go.Draw(sb);

            foreach (Frame frame in this.frames)
                frame.Draw(sb);

            // helper
            
            sb.Draw(EditorManager.currentTile.Sprite.Texture, InputHelper.MousePosition + new Vector2(EditorManager.currentTile.Sprite.Texture.Width / 2, EditorManager.currentTile.Sprite.Texture.Height / 2), null, Color.White * 0.3f, EditorManager.currentTile.Transform.Rotation, new Vector2(EditorManager.currentTile.Sprite.Texture.Width / 2, EditorManager.currentTile.Sprite.Texture.Height / 2), Vector2.One, SpriteEffects.None, 0);
        }
    }
}
