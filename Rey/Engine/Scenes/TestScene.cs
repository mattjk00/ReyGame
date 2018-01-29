using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rey.Engine.Memory;
using Rey.Engine.Prefabs;
using Rey.Engine.Prefabs.UI.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Scenes
{
    public class TestScene : Scene
    {
        Player player;
        GameObject fadeScreen;
        GameObject upperWall;

        Inventory inventory;
    
        // weather for the scene
        Weather weather; 

        protected bool transitioning; // if the scene is in a state of transitiong

        public TestScene(string name) : base(name) { }

        public override void Load()
        {


            //this.Name = "test";

            //this.Background = AssetLoader.LoadTexture("Assets/Textures/backgrounds/wood1.png");

            Trapdoor trapdoor = new Trapdoor();
            this.AddGameObject(trapdoor);

            var map = Map.LoadFromFile("Assets/new_test.guat");

            /*Enemy enemy = new Enemy();
            enemy.Transform.Position = new Vector2(500, 500);
            this.AddGameObject(enemy);

            Enemy enemy2 = new Enemy();
            enemy2.Transform.Position = new Vector2(800, 200);
            this.AddGameObject(enemy2);*/

            Bat bat = new Bat();
            bat.Transform.Position = new Vector2(0, 0);
            //this.AddGameObject(bat);

            Bat bat2 = new Bat();
            bat2.Transform.Position = new Vector2(2000, 0);
            this.AddGameObject(bat2);

            BabyFishDemon fish = new BabyFishDemon();
            fish.Transform.Position = new Vector2(100, 720);
            //this.AddGameObject(fish);

            player = new Player();
            this.AddGameObject(player);

            fadeScreen = new GameObject("fade");
            fadeScreen.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/UI/fade.png");
            fadeScreen.Sprite.Color = Color.White * 0; // make the screen invisible
            this.AddGameObject(fadeScreen);

            upperWall = new GameObject("upperwall");
            upperWall.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/blocks/stone_block.png");
            this.AddGameObject(upperWall);

            this.inventory = new Inventory();

            TestUIFrame testUIframe = new TestUIFrame();
            this.AddFrame(testUIframe);

            this.AddFrame(inventory);

            weather = new Weather();
            this.AddGameObject(weather);

            // load tiles
            foreach (Tile tile in map.Tiles)
            {
                // load the texture
                tile.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/tiles/" + tile.Name + ".png");
                /*if (tile.Name == "ocean1" || tile.Name == "wood_base" || tile.Name == "wood_wall")
                    tile.SetType(TileType.Block);*/
                tile.SetMapCoords(50);
                this.AddTile(tile);
            }
            // try to find the player's starting position
            var playerStart = map.Markers.Find(x => x.MarkerType == MarkerType.PlayerSpawnPoint);
            if (playerStart != null)
                player.Transform.Position = playerStart.StartingPosition;
            
            // parse through enemies
            var enemies = MarkerParser.ParseEnemies(map.Markers);
            foreach (Enemy enemy in enemies)
                this.AddGameObject(enemy);

            base.Load();
            //enemy.SetInterval(1000);

        }

        public TestScene():base() { }

        public override void Update(Camera2D camera)
        {
            base.Update(camera);
        }

        /// <summary>
        /// Moves to the next floor of the dungeon
        /// </summary>
        public void NextFloor()
        {
            //this.gameObjects.RemoveAll(x => x.IsEnemy); // remove everything except the player
            /*Bat bat = new Bat();
            
            bat.Transform.Position = new Vector2(0, 0);
            bat.Load();
            this.AddGameObject(bat);*/

            // the enemies to spawn
            var enemiesSpawning = new int[] { 1, 2 };
            var maxEnemiesToSpawn = 3;

            player.Reset();

            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                var rand = r.Next(0, 100);
                if (rand <= 25)
                {
                    Bat bat = new Bat();
                    bat.Transform.Position = new Vector2(r.Next(200, 1000), r.Next(100, 600));
                    bat.Load();
                    this.AddGameObject(bat);
                }
            }

            // load the scene again
            SceneManager.StartingNewCombatScene = true;
        }

        public override void SecondDraw(SpriteBatch sb)
        {
            base.SecondDraw(sb);

            this.weather.SecondDraw(sb);
        }
    }
}
