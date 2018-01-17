using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rey.Engine.Prefabs;
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

        protected bool transitioning; // if the scene is in a state of transitiong

        public override void Load()
        {


            this.Name = "test";

            //this.Background = AssetLoader.LoadTexture("Assets/Textures/backgrounds/wood1.png");

            Trapdoor trapdoor = new Trapdoor();
            this.AddGameObject(trapdoor);

            

            /*Enemy enemy = new Enemy();
            enemy.Transform.Position = new Vector2(500, 500);
            this.AddGameObject(enemy);

            Enemy enemy2 = new Enemy();
            enemy2.Transform.Position = new Vector2(800, 200);
            this.AddGameObject(enemy2);*/

            Bat bat = new Bat();
            bat.Transform.Position = new Vector2(0, 0);
            this.AddGameObject(bat);

            BabyFishDemon fish = new BabyFishDemon();
            fish.Transform.Position = new Vector2(100, 720);
            this.AddGameObject(fish);

            player = new Player();
            this.AddGameObject(player);

            fadeScreen = new GameObject("fade");
            fadeScreen.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/UI/fade.png");
            fadeScreen.Sprite.Color = Color.White * 0; // make the screen invisible
            this.AddGameObject(fadeScreen);

            upperWall = new GameObject("upperwall");
            upperWall.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/blocks/stone_block.png");
            this.AddGameObject(upperWall);

            TestUIFrame testUIframe = new TestUIFrame();
            this.AddFrame(testUIframe);

            for (int i = 0; i < 1280/50; i++)
            {
                for (int j = 0; j < 720/50; j++)
                {
                    Tile tile = new Tile(new Vector2(i * 50, j * 50), AssetLoader.LoadTexture("Assets/Textures/tiles/grass1.png"), TileType.Normal);
                    this.AddTile(tile);
                }
            }

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
    }
}
