using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rey.Engine.Memory;
using Rey.Engine.Prefabs;
using Rey.Engine.Prefabs.UI;
using Rey.Engine.Prefabs.UI.Inventory;
using System;
using System.Collections.Generic;
using System.IO;
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

        // the path to the guat map
        private string mapPath = "Assets/village.guat";

        

        
        NPCTalkingFrame npcTalkingFrame = new NPCTalkingFrame();

        public override void Load()
        {
            try
            {
                mapPath = System.IO.File.ReadAllText("launchoptions.txt");
            }
            catch (FileNotFoundException fnfe)
            { }

            //this.Name = "test";

            //this.Background = AssetLoader.LoadTexture("Assets/Textures/backgrounds/wood1.png");

            Trapdoor trapdoor = new Trapdoor();
            this.AddGameObject(trapdoor);

            GameData.AddItemToBackpack(ItemData.New(ItemData.mushroomHelmet));
            GameData.AddItemToBackpack(ItemData.New(ItemData.mushroomHelmet));
            GameData.AddItemToBackpack(ItemData.New(ItemData.mushroomHelmet));



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
            //this.AddGameObject(bat2);

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
            this.AddFrame(npcTalkingFrame);

             //weather = new Weather();
            //this.AddGameObject(weather);

            this.LoadMap(false);

            
            

            base.Load();
            //enemy.SetInterval(1000);

        }

        public TestScene():base() { }

        public override void Update(Camera2D camera)
        {
            if (this.State == SceneState.Normal || this.State == SceneState.NPCTalking)
                base.Update(camera);

            // double check to stop the npc talking
            if (this.State == SceneState.Normal)
                this.npcTalkingFrame.Active = false;
        }

        void LoadMap(bool fullReset, string atDoor = "")
        {
            var map = Map.LoadFromFile(this.mapPath);

            this.ClearTiles();
            if (fullReset) // use if not the first loading
                this.gameObjects.RemoveAll(x => x.Name != "player");

            // load tiles
            foreach (Tile tile in map.Tiles)
            {
                try
                {
                    // load the texture
                    if (tile.TileType != TileType.Door)
                        tile.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/tiles/" + tile.Name + ".png");
                    else
                        tile.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/tiles/" + tile.Name.Split(';')[0] + ".png");
                }
                catch (System.IO.FileNotFoundException fnfe)
                {
                    var a = 0;
                }

                /*if (tile.Name == "ocean1" || tile.Name == "wood_base" || tile.Name == "wood_wall")
                    tile.SetType(TileType.Block);*/
                tile.SetMapCoords(50);
                if (tile.Name != null && tile.Name != "delete")
                    this.AddTile(tile);
            }
            // try to find the player's starting position
            var playerStart = map.Markers.Find(x => x.MarkerType == MarkerType.PlayerSpawnPoint);
            if (playerStart != null && atDoor == "")
                player.Transform.Position = playerStart.StartingPosition;
            else if (atDoor != "") // if loading in at a door
            {
                // find the specific door with the specific given door data name
                var door = map.Tiles.Find(x => x.TileType == TileType.Door && x.Data.Split(';')[1] == atDoor);
                if (door != null)
                    player.Transform.Position = new Vector2(door.Transform.Position.X, door.Transform.Position.Y + player.Sprite.Texture.Height);
            }

            // parse through enemies
            var enemies = MarkerParser.ParseEnemies(map.Markers);
            foreach (Enemy enemy in enemies)
                this.AddGameObject(enemy);

            // parse through npcs
            var npcs = MarkerParser.ParseNPCs(map.Markers);
            foreach (NPC npc in npcs)
                this.AddGameObject(npc);

            /*Pickup pickup = new Pickup();
            pickup.Transform.Position = player.Transform.Position;
            pickup.PickupItem = ItemData.ironHelmet;
            this.AddGameObject(pickup);*/
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

            //this.weather.SecondDraw(sb);

            // draw npc ui here
            if (this.State == SceneState.NPCTalking)
            {
                this.npcTalkingFrame.Active = true;
                // find the current talking npc
                foreach (NPC npc in this.gameObjects.FindAll(x => x.GetType() == typeof(NPC)))
                {
                    if (npc.State == NPCState.Talking)
                        this.npcTalkingFrame.NPC = npc;
                }
            }
        }

        /// <summary>
        /// handle a door being entered or other
        /// </summary>
        /// <param name="tile"></param>
        protected override void HandleTileTrigger(Tile tile)
        {
            // if it is a door
            if (tile.TileType == TileType.Door && tile.Data != "")
            {
                // transition
                this.mapPath = "Assets/" + tile.Data.Split(';')[0] + ".guat"; // format the map path
                
                //SceneManager.TransitionToScene("test");

                // try and load in at a door posiition
                if (tile.Data.Contains(';'))
                    this.LoadMap(true, tile.Data.Split(';')[1]);
                else
                    this.LoadMap(true);
            }
        }
    }
}
