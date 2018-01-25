using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rey.Engine.Prefabs;
using Rey.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    /// <summary>
    /// Holds the data of all game objects in scene
    /// </summary>
    public class Scene
    {
        public List<GameObject> gameObjects { get; protected set; }
        public List<Tile> tiles { get; protected set; } = new List<Tile>();
        public List<Frame> frames { get; protected set; } = new List<Frame>();
        public Texture2D Background { get; protected set; }
        public string Name { get; protected set; }
        protected CollisionManager collisionManager = new CollisionManager();
        public bool CombatScene { get; set; } = true;
        private Player player;

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
            this.gameObjects.Add(go);
        }

        public void AddFrame(Frame frame)
        {
            this.frames.Add(frame);
        }

        public void AddTile(Tile tile)
        {
            this.tiles.Add(tile);
        }

        /// <summary>
        /// Put loading logic in here
        /// </summary>
        public virtual void Load()
        {
            foreach (GameObject go in this.gameObjects)
                go.Load();
            foreach (Frame frame in this.frames)
                frame.Load();
            foreach (Tile tile in this.tiles)
                tile.Load();

            player = this.gameObjects.Find(x => x.Name == "player") as Player; // find the player
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


            // look through each tile
            foreach (Tile tile in this.tiles)
            {
                if (player != null)
                {
                    // if the tile is close enough to the player, draw it
                    if (Vector2.Distance(player.Transform.Position, tile.Transform.Position) < 850)
                        tile.Draw(sb);
                }
                else
                {
                    tile.Draw(sb);
                }
            }
            
            foreach (GameObject go in this.gameObjects)
                go.Draw(sb);
            foreach (Frame frame in frames.FindAll(f => f.LockedPosition == false))
                frame.Draw(sb);
            
        }

        /// <summary>
        /// Another draaw method that is called after the first one so without camera
        /// </summary>
        /// <param name="sb"></param>
        public virtual void SecondDraw(SpriteBatch sb)
        {
            // draw the remainder of the frames
            foreach (Frame frame in frames.FindAll(f => f.LockedPosition == true))
                frame.Draw(sb);
        }

        /// <summary>
        /// Default updating
        /// </summary>
        public virtual void Update(Camera2D camera)
        {
            foreach (GameObject go in this.gameObjects)
                go.Update();
            this.gameObjects.RemoveAll(x => x.ToBeDestroyed); // remove all objects that should be destroyed

            foreach (Tile tile in this.tiles)
                tile.Update();
            this.tiles.RemoveAll(x => x.ToBeDestroyed); // remove all objects that should be destroyed

            foreach (Frame frame in this.frames)
            {
                frame.Update();
            }

            if (this.CombatScene)
            {
                this.CheckForWorldCollisions();
                this.CheckForAttackCollisions();
                this.CheckToSeeIfAllAreDead();

                player = this.gameObjects.Find(x => x.Name == "player") as Player; // find the player
                camera.Position = new Vector2(player.Transform.Position.X - 1280/2, player.Transform.Position.Y - 720/2);
            }

            // if this is true
            if (SceneManager.StartingNewCombatScene == true)
            {
                this.SetTrapdoorState(false);
                SceneManager.StartingNewCombatScene = false; // let the scene manager know that it is prepared to start new scene
                
            }
        }

        // turns monsters aggressive
        void Agro(Player player)
        {
            // find all the enemies
            foreach (var go in this.gameObjects.FindAll(x => x.IsEnemy))
            {
                Enemy enemy = go as Enemy; // convert the game object to an enemy
                if (enemy.EntityStats.Aggressive && enemy.State != EnemyState.Dead)
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
            }

            foreach (GameObject gameObject in this.gameObjects)
            {
                if (gameObject.IsEnemy)
                {
                    this.collisionManager.HandlePlayerAttackEnemyCollision(player, gameObject as Enemy);

                    // check in the projectiles being fired by the player
                    foreach (Projectile proj in player.projectileManager.Projectiles)
                    {
                        this.collisionManager.HandlePlayerProjectileAttackEnemyCollision(player, proj, gameObject as Enemy);
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
        }

        void SetTrapdoorState(bool open)
        {
            // find the trapdoor
            var trapdoor = this.gameObjects.Find(x => x.Name == "trapdoor") as Trapdoor;
            trapdoor.Open = open; // open the trapdoor

            // find the go button and activate it
            var gobutton = this.frames.Find(x => x.Name == "gameui").Find("gobutton") as Button;
            gobutton.IsActive = open;

            
        }

    }
}
