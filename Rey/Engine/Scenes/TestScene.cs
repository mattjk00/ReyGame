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

        public override void Load()
        {


            this.Name = "test";

            this.Background = AssetLoader.LoadTexture("Assets/Textures/backgrounds/wood1.png");

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

            TestUIFrame testUIframe = new TestUIFrame();
            this.AddFrame(testUIframe);

            base.Load();
            //enemy.SetInterval(1000);

        }

        public TestScene():base() { }
        
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
