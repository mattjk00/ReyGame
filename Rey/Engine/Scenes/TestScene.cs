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
            bat.Transform.Position = new Vector2(200, 200);
            this.AddGameObject(bat);

            Player player = new Player();
            this.AddGameObject(player);

            base.Load();
            //enemy.SetInterval(1000);

        }

        public TestScene():base() { }
        
    }
}
