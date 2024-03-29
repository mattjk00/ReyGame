﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rey.Engine.Prefabs;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public enum SceneState
    {
        Normal,
        Paused,
        NPCTalking
    }

    /// <summary>
    /// Holds the data of all game objects in scene
    /// </summary>
    public class Scene
    {
        public List<GameObject> gameObjects { get; protected set; }
        public List<Tile> tiles { get; protected set; } = new List<Tile>();
        public List<Frame> frames { get; protected set; } = new List<Frame>();
        public List<LightSource> lightSources { get; protected set; } = new List<LightSource>();
        private Texture2D lightMask;
        public Texture2D Background { get; protected set; }
        public string Name { get; protected set; }
        protected CollisionManager collisionManager = new CollisionManager();
        public bool CombatScene { get; set; } = true;
        private Player player;
        public SceneState State { get; set; } = SceneState.Normal;

        public ParticleManager ParticleManager { get; private set; } = new ParticleManager();

        private List<GameObject> addQ = new List<GameObject>(); // queue to add go's

        public Scene() { this.gameObjects = new List<GameObject>();  }
        public Scene(string name)
        {
            this.Name = name;
            this.gameObjects = new List<GameObject>();
            this.tiles = new List<Tile>();
        }

        public Scene(string name, Texture2D bg)
        {
            this.Name = name;
            this.Background = bg;
            this.gameObjects = new List<GameObject>();
            this.tiles = new List<Tile>();
        }

        public void AddGameObject(GameObject go)
        {
            //this.gameObjects.Add(go);
            this.addQ.Add(go);
        }

        // puts game object queue into reg list
        void HandleQueue()
        {
            // adds all them
            this.gameObjects.AddRange(this.addQ);
            this.addQ.Clear();
        }

        public void AddFrame(Frame frame)
        {
            this.frames.Add(frame);
        }

        public void AddTile(Tile tile)
        {
            this.tiles.Add(tile);
        }

        public void ClearTiles()
        {
            this.tiles.Clear();
        }

        public void AddLightSource(LightSource lightSource)
        {
            this.lightSources.Add(lightSource);
        }

        /// <summary>
        /// tells the draw engine whether to use the light shaders or not
        /// </summary>
        /// <returns></returns>
        public bool UseLightShaders()
        {
            return (this.lightSources.Count > 0);
        }

        /// <summary>
        /// Put loading logic in here
        /// </summary>
        public virtual void Load()
        {
            this.HandleQueue();
            foreach (GameObject go in this.gameObjects)
                go.Load();
            foreach (Frame frame in this.frames)
                if (frame.Loaded == false)
                    frame.Load();
            foreach (Tile tile in this.tiles)
                tile.Load();

            player = this.gameObjects.Find(x => x.Name == "player") as Player; // find the player

            try
            {
                this.lightMask = AssetLoader.LoadTexture("Assets/Textures/lightmask.png");
            }
            catch (FileNotFoundException fnfe) { }
        }

        /// <summary>
        /// Unloads most data about the scene
        /// </summary>
        public virtual void Unload()
        {
            //this.Background.Dispose();
            this.gameObjects = new List<GameObject>();
            this.tiles = new List<Tile>();
        }

        /// <summary>
        /// Does default drawing 
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            if (this.Background != null)
                sb.Draw(this.Background, Vector2.Zero, Color.White);

            // all the tiles to draw
            List<GameObject> objectBath = new List<GameObject>();

            // look through each tile
            // all tiles under this depth

            
            foreach (Tile tile in this.tiles.FindAll(x => x.Depth < 1))
            {
                if (player != null)
                {
                    // if the tile is close enough to the player, draw it
                    if (Vector2.Distance(player.Transform.Position, tile.Transform.Position) < 800)
                        objectBath.Add(tile);
                }
                else
                {

                    objectBath.Add(tile);
                }
            }

            // add all pickups
            foreach (GameObject go in this.gameObjects.FindAll(x => x.GetType() == typeof(Pickup)))
                objectBath.Add(go);

            // add all non pickups
            foreach (GameObject go in this.gameObjects.FindAll(x => x.GetType() != typeof(Pickup)))
                objectBath.Add(go);

            // all tiles under this depth, draw thtiles again
            foreach (Tile tile in this.tiles.FindAll(x => x.Depth > 0))
            {
                if (player != null)
                {
                    // if the tile is close enough to the player, draw it
                    if (Vector2.Distance(player.Transform.Position, tile.Transform.Position) < 850)
                        objectBath.Add(tile);
                }
                else
                {
                    objectBath.Add(tile);
                }
            }

            // draw all tiles
            try
            {
                foreach (GameObject go in objectBath)
                    go.Draw(sb);
            }
            catch (ArgumentNullException nre) { // fu
            }

            ParticleManager.Draw(sb);

            foreach (Frame frame in frames.FindAll(f => f.LockedPosition == false && f.Active == true))
                frame.Draw(sb);
            
        }

        /// <summary>
        /// Another draaw method that is called after the first one so without camera
        /// </summary>
        /// <param name="sb"></param>
        public virtual void SecondDraw(SpriteBatch sb)
        {
            // draw the remainder of the frames
            foreach (Frame frame in frames.FindAll(f => f.LockedPosition == true && f.Active == true))
                frame.Draw(sb);
        }

        /// <summary>
        /// Draws the light sources in the scnee
        /// </summary>
        /// <param name="sb"></param>
        public void DrawLights(SpriteBatch sb)
        {
            // draw each light source
            foreach (LightSource light in this.lightSources)
            {
                Vector2 newScale = InputHelper.ScaleTexture(lightMask, 1200, 1200);
                Vector2 origin = new Vector2((lightMask.Width ) / 2, (lightMask.Height ) / 2);
                sb.Draw(lightMask, light.Position, null, Color.LightBlue * 1.0f, 0, origin,
                    newScale, SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// Default updating
        /// </summary>
        public virtual void Update(Camera2D camera)
        {
            foreach (Frame frame in this.frames.FindAll(x => x.Active == true))
            {
                frame.Update();
            }

            // prevent update when scene is paused
            if (this.State != SceneState.Paused)
            {
                foreach (GameObject go in this.gameObjects)
                    go.Update();
                this.gameObjects.RemoveAll(x => x.ToBeDestroyed); // remove all objects that should be destroyed


                foreach (Tile tile in this.tiles)
                    tile.Update();
                this.tiles.RemoveAll(x => x.ToBeDestroyed); // remove all objects that should be destroyed

                InputHelper.MouseOnUI = false; // reset it

                if (this.CombatScene)
                {
                    this.CheckForWorldCollisions();
                    this.CheckForAttackCollisions();
                    this.CheckToSeeIfAllAreDead();

                    player = this.gameObjects.Find(x => x.Name == "player") as Player; // find the player
                    camera.Position = new Vector2(player.Transform.Position.X - 1280 / 2, player.Transform.Position.Y - 720 / 2);
                }

                // if this is true
                if (SceneManager.StartingNewCombatScene == true)
                {
                    this.SetTrapdoorState(false);
                    SceneManager.StartingNewCombatScene = false; // let the scene manager know that it is prepared to start new scene

                }
            }

            ParticleManager.Update();

            // fill queue
            this.HandleQueue();
        }

        // turns monsters aggressive
        void Agro(Player player)
        {
            // find all the enemies
            foreach (var go in this.gameObjects.FindAll(x => x.IsEnemy))
            {
                Enemy enemy = go as Enemy; // convert the game object to an enemy
                if (enemy.EntityStats.Aggressive && enemy.State != EnemyState.Dead && Vector2.Distance(enemy.Transform.Position, player.Transform.Position) < 250)
                    enemy.StartAttack(player); // start the attackl
            }
        }
        /// <summary>
        /// Checks to see if every monster in the room is dead
        /// </summary>
        void CheckToSeeIfAllAreDead()
        {
            // if there are no enemies left
            if (this.gameObjects.FindAll(x => x.IsEnemy).Count == 0)
            {
                this.SetTrapdoorState(true);
            }
        }

        // checks for collisions such as the player stabbing an enemy
        void CheckForAttackCollisions()
        {
            var player = this.gameObjects.First(x => x.Name == "player") as Player; // find the player
            this.Agro(player);

            /*if (player.AttackState == PlayerAttackState.MeleeAttack && player.LandedMeleeHit == false) // if player is attacking and it hasnt already been registered
            {
                // iterate over all the gameobjects
                foreach (GameObject gameObject in this.gameObjects)
                {
                    // except for the player
                    if (gameObject.Name != "player" && gameObject.IsEnemy)
                    {
                        // iterate over this gameobject's bounding boxes
                        for (int i =0; i< gameObject.BoundingBoxes.Count; i++)
                        {
                            // if it intersects the sword box, mayve update this later for readability
                            if (gameObject.BoundingBoxes[i].Intersects(player.BoundingBoxes[1]))
                            {
                                // convert to an enemy
                                var enemy = gameObject as Enemy;
                                enemy.GetHit(player.EntityStats); // hit the enemy
                                enemy.StartAttack(player);
                                if (player.Transform.Position.X > enemy.Transform.Position.X)
                                    enemy.Bounce(new Vector2(-1, 0)); // bounces enemy based on direction of incoming hit
                                else
                                    enemy.Bounce(new Vector2(1, 0));
                                player.LandedMeleeHit = true; // log the hit
                            }

                            // if the game object's sword box intersects the player
                            if (gameObject.BoundingBoxes[1].Intersects(player.BoundingBoxes[0]))
                            {

                            }
                        }
                    }
                }

            }*/

            // -> -> -> -> -> -> -> -> ->-> -> -> -> -> -> -> -> ->-> -> -> -> -> -> -> -> ->-> -> -> -> -> -> -> -> ->
            // add this back later
            //

            foreach (GameObject gameObject in this.gameObjects)
            {
                if (gameObject.IsEnemy)
                {
                    // if the enemy is melee attacking
                    var enemy = gameObject as Enemy;
                    if (enemy.State == EnemyState.MeleeAttacking && enemy.LandedMeleeHit == false)
                    {
                        // if the game object's sword box intersects the player
                        if (enemy.BoundingBoxes[1].Intersects(player.BoundingBoxes[0]))
                        {
                            enemy.LandedMeleeHit = true;
                            player.GetHit(enemy.EntityStats);
                            
                            if (player.Transform.Position.X > enemy.Transform.Position.X)
                            {
                                player.Bounce(new Vector2(4, 0)); // bounces player based on direction of incoming hit
                                enemy.Bounce(new Vector2(-4, 0));
                            }
                            else
                            {
                                player.Bounce(new Vector2(-4, 0));
                                enemy.Bounce(new Vector2(4, 0));
                            }
                        }
                    }
                    else if (enemy.BoundingBoxes[0].Intersects(player.BoundingBoxes[0]))
                    {
                        enemy.LandedMeleeHit = true;
                        player.GetHit(enemy.EntityStats);
                        this.ParticleManager.Burst(player.Transform.Position, new Vector2(2, 2), Color.Red, 25, Vector2.One);

                        // calculate velocities
                        float velX1 = 0, velX2 = 0; // velX1: for player, velX2: for monster
                        if (enemy.GetType() != typeof(MushroomBoss))
                        {
                            velX1 = 4;
                            velX2 = 4;
                        }
                        else
                        {
                            velX1 = 25;
                            velX2 = 0;
                        }

                        if (player.Transform.Position.X > enemy.Transform.Position.X && enemy.EntityStats.HP >= 0)
                        {
                           
                            player.Bounce(new Vector2(velX1, 0)); // bounces player based on direction of incoming hit
                            enemy.Bounce(new Vector2(-velX2, 0));
                            
                        }
                        else if (enemy.EntityStats.HP >= 0)
                        {
                            player.Bounce(new Vector2(-velX1, 0));
                            enemy.Bounce(new Vector2(velX2, 0));
                        }
                    }

                }
            }

            foreach (GameObject gameObject in this.gameObjects)
            {
                if (gameObject.IsEnemy)
                {
                    this.collisionManager.HandlePlayerAttackEnemyCollision(player, gameObject as Enemy);

                    // check in the projectiles being fired by the player
                    foreach (Projectile proj in player.projectileManager.Projectiles)
                    {             
                        this.collisionManager.HandlePlayerProjectileAttackEnemyCollision(player, proj, gameObject as Enemy, ParticleManager);
                    }
                    var enemy = gameObject as Enemy; // convert to an enemy
                    foreach (Projectile enemyProj in enemy.projectileManager.Projectiles)
                    {
                        this.collisionManager.HandleEnemyProjectileHittingPlayer(player, enemyProj, ParticleManager);
                    }
                }

                
            }

            
        }

        void CheckForWorldCollisions()
        {
            // loop through the tiles
            var blockTiles = this.tiles.FindAll(x => x.TileType == TileType.Block);

            int leftTile = (int)Math.Floor((float)player.MovementBox.Left / 50);
            int rightTile = (int)Math.Ceiling(((float)player.MovementBox.Right / 50)) - 1;
            int topTile = (int)Math.Floor((float)player.MovementBox.Top / 50);
            int bottomTile = (int)Math.Ceiling(((float)player.MovementBox.Bottom / 50)) - 1;

            // tiles to check
            var checkTiles = new List<Tile>();

            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    // find current tile
                    Tile tile = blockTiles.Find(t => t.MapX == x && t.MapY == y);
                    if (tile != null)
                        checkTiles.Add(tile);
                }
            }

            // if tiles to check is greater than, remove all except the closest tile
            /*if (checkTiles.Count > 1)
            {
                var closestTile = checkTiles[0];
                for (int i = 0; i < checkTiles.Count; i++)
                {
                    if (Vector2.Distance(checkTiles[0].Transform.Position, player.Transform.Position) < Vector2.Distance(closestTile.Transform.Position, player.Transform.Position))
                        closestTile = checkTiles[0];
                }
                checkTiles.Clear();
                checkTiles.Add(closestTile);
            }*/
            
            
            foreach (Tile tile in checkTiles)
            {
                var touchedVerticalBox = false;
                var touchedHorizontalBox = false;

                if (player.MovementBox.Intersects(tile.Box))
                {
                    // if player collides with a block, handle the collision
                    if (player.MovementBox.Intersects(tile.TopBox) && !touchedVerticalBox && !touchedHorizontalBox)//(player.MovementBox.Bottom > tile.Box.Top && player.MovementBox.Top < tile.Box.Top && !touchedHorizontalBox && !touchedVerticalBox)
                    {
                        this.collisionManager.HandlePlayerAndBlock(player, tile, "top");
                        touchedVerticalBox = true;
                    }
                    else if (player.MovementBox.Intersects(tile.BottomBox) && !touchedVerticalBox && !touchedHorizontalBox) //(player.MovementBox.Top < tile.Box.Bottom && player.MovementBox.Bottom > tile.Box.Bottom && !touchedHorizontalBox && !touchedVerticalBox)//
                    {
                        this.collisionManager.HandlePlayerAndBlock(player, tile, "bottom");
                        touchedVerticalBox = true;
                    }
                    else if (player.MovementBox.Intersects(tile.LeftBox) && !touchedHorizontalBox && !touchedVerticalBox)// (player.MovementBox.Right > tile.Box.Left && player.MovementBox.Left < tile.Box.Left && !touchedHorizontalBox && !touchedVerticalBox)//
                    {
                        this.collisionManager.HandlePlayerAndBlock(player, tile, "left");
                        touchedHorizontalBox = true;
                    }
                    else if (player.MovementBox.Intersects(tile.RightBox) && !touchedHorizontalBox && !touchedVerticalBox) //if (player.MovementBox.Left < tile.Box.Right && player.MovementBox.Right > tile.Box.Right && !touchedHorizontalBox && !touchedVerticalBox)//
                    {
                        this.collisionManager.HandlePlayerAndBlock(player, tile, "right");
                        touchedHorizontalBox = true;
                    }
                }
            }

            /// CHECK FOR NPCs
            foreach (Tile tile in blockTiles)
            {
                var npcs = this.gameObjects.FindAll(x => x.Name == "NPC");
                foreach (NPC npc in npcs)
                {
                    var touchedVerticalBox = false;
                    var touchedHorizontalBox = false;

                    if (npc.MovementBox.Intersects(tile.Box))
                    {
                        // if npc collides with a block, handle the collision
                        if (npc.MovementBox.Intersects(tile.TopBox) && !touchedVerticalBox && !touchedHorizontalBox)//(npc.MovementBox.Bottom > tile.Box.Top && npc.MovementBox.Top < tile.Box.Top && !touchedHorizontalBox && !touchedVerticalBox)
                        {
                            this.collisionManager.HandleNPCAndBlock(npc, tile, "top");
                            touchedVerticalBox = true;
                        }
                        else if (npc.MovementBox.Intersects(tile.BottomBox) && !touchedVerticalBox && !touchedHorizontalBox) //(npc.MovementBox.Top < tile.Box.Bottom && npc.MovementBox.Bottom > tile.Box.Bottom && !touchedHorizontalBox && !touchedVerticalBox)//
                        {
                            this.collisionManager.HandleNPCAndBlock(npc, tile, "bottom");
                            touchedVerticalBox = true;
                        }
                        else if (npc.MovementBox.Intersects(tile.LeftBox) && !touchedHorizontalBox && !touchedVerticalBox)// (npc.MovementBox.Right > tile.Box.Left && npc.MovementBox.Left < tile.Box.Left && !touchedHorizontalBox && !touchedVerticalBox)//
                        {
                            this.collisionManager.HandleNPCAndBlock(npc, tile, "left");
                            touchedHorizontalBox = true;
                        }
                        else if (npc.MovementBox.Intersects(tile.RightBox) && !touchedHorizontalBox && !touchedVerticalBox) //if (npc.MovementBox.Left < tile.Box.Right && npc.MovementBox.Right > tile.Box.Right && !touchedHorizontalBox && !touchedVerticalBox)//
                        {
                            this.collisionManager.HandleNPCAndBlock(npc, tile, "right");
                            touchedHorizontalBox = true;
                        }
                    }

                }
            }

            // check for player touching NPCs
            foreach (NPC npc in this.gameObjects.FindAll(x => x.GetType() == typeof(NPC)))
            {
                // if the npc and the player is close enough
                if (Vector2.Distance(npc.Transform.Position, player.Transform.Position) < 100 && this.State != SceneState.NPCTalking)
                {
                    // if key is pressed, initiate interaction
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        npc.State = NPCState.Talking;
                        this.State = SceneState.NPCTalking;
                    }
                }
                if (this.State == SceneState.NPCTalking)
                {
                    // if the player moves too far away, stop the interaction
                    if (Vector2.Distance(npc.Transform.Position, player.Transform.Position) > 100)
                    {
                        this.State = SceneState.Normal;
                    }
                }
            }


            // check for doors
            int doorTouchCount = 0; // the number of doors the player is touching
            foreach (Tile doorTile in this.tiles.FindAll(x => x.TileType == TileType.Door))
            {
                // if they collide, increase the # of boxes touching
                if (player.MovementBox.Intersects(doorTile.Box))
                    doorTouchCount++;
                if (doorTouchCount >= 2) // if the player is touching a bunch of tiles, trigger a message to the scene
                {
                    doorTouchCount = 0;
                    this.HandleTileTrigger(doorTile);
                }
            }

            // check for pickups
            foreach (Pickup pickup in this.gameObjects.FindAll(x => x.GetType() == typeof(Pickup)))
            {
                // if the pickup box interescts the player
                if (pickup.Box.Intersects(player.BoundingBoxes[0]))
                {
                    // handle it
                    collisionManager.HandlePlayerAndPickup(player, pickup);
                }
            }
        }

        void SetTrapdoorState(bool open)
        {
            // find the trapdoor
            /*var trapdoor = this.gameObjects.Find(x => x.Name == "trapdoor") as Trapdoor;
            trapdoor.Open = open;*/ // open the trapdoor

            // find the go button and activate it
            /*var gobutton = this.frames.Find(x => x.Name == "gameui").Find("gobutton") as Button;
            gobutton.IsActive = open;*/

            
        }

        // handles a trigger from a tile. can be used for doors or other things
        protected virtual void HandleTileTrigger(Tile tile)
        {

        }

        /// <summary>
        /// Drops item at player
        /// </summary>
        /// <param name="item"></param>
        public void DropItemAtPlayer(Item item)
        {
            Random r = new Random();
            // create the pickup
            Pickup pickup = new Pickup();
            pickup.PickupItem = item;
            pickup.Transform.Position = this.player.Transform.Position;
            //pickup.Transform.Velocity = new Vector2(r.Next(-5, 5), r.Next(-4, 4));
            pickup.Load();
            this.AddGameObject(pickup);
        }
        /// <summary>
        /// Drops item at position
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pos"></param>
        public void DropItemAtPosition(Item item, Vector2 pos)
        {
            // create the pickup
            Pickup pickup = new Pickup();
            pickup.PickupItem = item;
            pickup.Transform.Position = pos;
            //pickup.Transform.Velocity = new Vector2(r.Next(-5, 5), r.Next(-4, 4));
            pickup.Load();
            this.AddGameObject(pickup);
        }
    }
}
