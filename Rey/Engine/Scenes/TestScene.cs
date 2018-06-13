using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        StatsFrame statsFrame;
    
        // weather for the scene
        Weather weather; 

        protected bool transitioning; // if the scene is in a state of transitiong

        public TestScene(string name) : base(name) { }

        // the path to the guat map
        private string mapPath = "Assets/villages.guat";
        private string lastDoor = "";
        

        
        NPCTalkingFrame npcTalkingFrame = new NPCTalkingFrame();
        DeathFrame deathFrame = new DeathFrame();
        PauseFrame pauseFrame = new PauseFrame();
        BeatMushroomFrame beatMushroomFrame = new BeatMushroomFrame();

        public override void Load()
        {
            try
            {
                mapPath = System.IO.File.ReadAllText("launchoptions.txt");
            }
            catch (FileNotFoundException fnfe)
            { }

            // reset lists
            this.gameObjects = new List<GameObject>();
            this.frames = new List<UI.Frame>();

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

            /*Bat bat = new Bat();
            bat.Transform.Position = new Vector2(0, 0);*/
            //this.AddGameObject(bat);

            /*Bat bat2 = new Bat();
            bat2.Transform.Position = new Vector2(2000, 0);*/
            //this.AddGameObject(bat2);

            /*BabyFishDemon fish = new BabyFishDemon();
            fish.Transform.Position = new Vector2(100, 720);*/
            //this.AddGameObject(fish);

            player = new Player();
            this.AddGameObject(player);

            
            

            fadeScreen = new GameObject("fade");
            fadeScreen.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/UI/fade.png");
            fadeScreen.Sprite.Color = Color.White * 0; // make the screen invisible
            this.AddGameObject(fadeScreen);

            /*upperWall = new GameObject("upperwall");
            upperWall.Sprite.Texture = AssetLoader.LoadTexture("Assets/Textures/blocks/stone_block.png");
            this.AddGameObject(upperWall);*/

            this.inventory = new Inventory();
            this.statsFrame = new StatsFrame();

            /*TestUIFrame testUIframe = new TestUIFrame();
            this.AddFrame(testUIframe);*/

            this.AddFrame(inventory);
            this.AddFrame(npcTalkingFrame);
            this.AddFrame(statsFrame);
            this.AddFrame(deathFrame);
            this.AddFrame(pauseFrame);
            this.AddFrame(beatMushroomFrame);

             //weather = new Weather();
            //this.AddGameObject(weather);

            this.LoadMap(false);

            
            

            base.Load();
            //enemy.SetInterval(1000);

            
        }

        public TestScene():base() { }



        public override void Update(Camera2D camera)
        {
            // check pause frame state
            SceneState lastState = this.State;
            if (this.pauseFrame.Active)
                this.State = SceneState.Paused;

            // check to see if the mushroom king has died
            if (this.mapPath.Contains("mushroom_palace"))
            {
                var mb = this.gameObjects.Find(x => x.GetType() == typeof(MushroomBoss)) as MushroomBoss;
                if (mb == null) // if the mushroom boss is null, he is not in the scene and is therefore dead
                {
                    this.beatMushroomFrame.Active = true;
                }
            }

            if (this.State == SceneState.Normal || this.State == SceneState.NPCTalking || this.State == SceneState.Paused)
                base.Update(camera);

            // resume state from pause
            if (this.State == SceneState.Paused)
                this.State = lastState;

            // double check to stop the npc talking
            if (this.State == SceneState.Normal)
                this.npcTalkingFrame.Active = false;

            // check death frame
            if (player.EntityStats.HP > 0)
                this.deathFrame.Active = false;
            else
                this.deathFrame.Active = true;

            this.UpdateStats();

            


            // check for input to pause the game
            if (this.pauseFrame.Active == false)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    this.pauseFrame.Active = true;
            }
        }
        
        /// <summary>
        /// Sets map path and then loads map
        /// </summary>
        /// <param name="fullReset"></param>
        /// <param name="mapPath"></param>
        /// <param name="atDoor"></param>
        public void GoToMap(bool fullReset, string mapPath, string atDoor = "")
        {
            this.mapPath = mapPath;
            if (atDoor != "")
                this.LoadMap(fullReset, atDoor);
            else
                this.LoadMap(fullReset);
        }

        public string GetMapPath()
        {
            return this.mapPath;
        }

        public void LoadMap(bool fullReset, string atDoor = "")
        {
            var map = Map.LoadFromFile(this.mapPath);

            var doors = map.Tiles.FindAll(x => x.TileType == TileType.Door);

            if (this.mapPath.Contains("mudtrap") || this.mapPath.Contains("mushroom"))
                SceneManager.SoundManager.PlaySong("theme", 0.8f);
            else if (this.mapPath.Contains("dungeon"))
                SceneManager.SoundManager.PlaySong("adventure", 0.7f);
            else
                SceneManager.SoundManager.PlaySong("clora", 0.8f);

            //whacky
            if (atDoor == "LAST")
                atDoor = lastDoor;

            this.lightSources.Clear();
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
                {
                    // check for an animation tile and add it
                    if (tile.Sprite.Texture.Width > 50)
                    {
                        var animtile = new AnimationTile(tile.Name, tile.Transform.Position, tile.Sprite.Texture, tile.TileType);
                        animtile.SetMapCoords(50);
                        this.AddTile(animtile);
                    }
                    else
                    {
                        this.AddTile(tile);
                    }
                }
            }
            // try to find the player's starting position
            var playerStart = map.Markers.Find(x => x.MarkerType == MarkerType.PlayerSpawnPoint);
            if (playerStart != null && atDoor == "")
                player.Transform.Position = playerStart.StartingPosition;
            else if (atDoor != "" || (atDoor == "" && lastDoor != "")) // if loading in at a door
            {
                if (atDoor == "")
                    atDoor = lastDoor;
                else
                    lastDoor = atDoor;
                // find the specific door with the specific given door data name
                var door = doors.Find(x => x.Data != null && x.Data.Contains(atDoor));
                if (door != null)
                    player.Transform.Position = new Vector2(door.Transform.Position.X, door.Transform.Position.Y + player.Sprite.Texture.Height);
            }

            // parse through enemies
            var enemies = MarkerParser.ParseEnemies(map.Markers);
            foreach (Enemy enemy in enemies)
            {
                // if it's a boss, start the attack
                if (enemy.GetType() == typeof(MushroomBoss))
                    enemy.StartAttack(player); 
                this.AddGameObject(enemy);
            }

            

            // parse through npcs
            var npcs = MarkerParser.ParseNPCs(map.Markers);
            foreach (NPC npc in npcs)
                this.AddGameObject(npc);

            // get light sources
            var lights = MarkerParser.ParseLightsources(map.Markers);
            foreach (LightSource ls in lights)
                this.AddLightSource(ls);

            // reset player
            player.Reset();
            if (player.EntityStats != null)
                player.EntityStats.HP = player.EntityStats.FullStats.MaxHP;

            statsFrame.player = player;

            // reload
            base.Load();

            //this.AddLightSource(new LightSource(Vector2.One, Vector2.One, Color.White));

            /*Pickup pickup = new Pickup();
            pickup.Transform.Position = player.Transform.Position;
            pickup.PickupItem = ItemData.ironHelmet;
            this.AddGameObject(pickup);*/
            /*MushroomMinion mush = new MushroomMinion();
            mush.Transform.Position = new Vector2(500, 500);
            mush.Load();
            this.AddGameObject(mush);*/
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
            if (tile.TileType == TileType.Door && tile.Data != "" && tile.Data != null)
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

        // check the inventory to see what the player's stats should be
        void UpdateStats()
        {
            // sum all the stats
            var stats = new StatsBoost();
            if (GameData.EquippedHelmet.Stats != null)
                stats = EntityStats.SumStatsB(GameData.EquippedHelmet.Stats, stats);
            if (GameData.EquippedChest.Stats != null) 
                stats = EntityStats.SumStatsB(GameData.EquippedChest.Stats, stats);
            if (GameData.EquippedLegs.Stats != null)
                stats = EntityStats.SumStatsB(GameData.EquippedLegs.Stats, stats);

            // boost the player's stats
            this.player.EntityStats.Boosts = stats;
        }

        
    }
}
