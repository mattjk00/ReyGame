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
        public List<Frame> frames { get; protected set; } = new List<Frame>();
        public Texture2D Background { get; protected set; }
        public string Name { get; protected set; }
        protected CollisionManager collisionManager = new CollisionManager();
        public bool CombatScene { get; set; } = true;

        public Scene() { this.gameObjects = new List<GameObject>();  }
        public Scene(string name)
        {
            this.Name = name;
            this.gameObjects = new List<GameObject>();
        }

        public Scene(string name, Texture2D bg)
        {
            this.Name = name;
            this.Background = bg;
            this.gameObjects = new List<GameObject>();
        }

        public void AddGameObject(GameObject go)
        {
            this.gameObjects.Add(go);
        }

        public void AddFrame(Frame frame)
        {
            this.frames.Add(frame);
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
        }

        /// <summary>
        /// Unloads most data about the scene
        /// </summary>
        public virtual void Unload()
        {
            this.Background = null;
            this.gameObjects = new List<GameObject>();
        }

        /// <summary>
        /// Does default drawing 
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            if (this.Background != null)
                sb.Draw(this.Background, Vector2.Zero, Color.White);
            foreach (GameObject go in this.gameObjects)
                go.Draw(sb);
            foreach (Frame frame in this.frames)
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

            foreach (Frame frame in this.frames)
            {
                frame.Update();
            }

            if (this.CombatScene)
            {
                this.CheckForAttackCollisions();
                this.CheckToSeeIfAllAreDead();

                var player = this.gameObjects.First(x => x.Name == "player") as Player; // find the player
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
                                player.Bounce(new Vector2(7, 0)); // bounces player based on direction of incoming hit
                                enemy.Bounce(new Vector2(-7, 0));
                            }
                            else
                            {
                                player.Bounce(new Vector2(-7, 0));
                                enemy.Bounce(new Vector2(7, 0));
                            }
                        }
                    }
                    else if (enemy.BoundingBoxes[0].Intersects(player.BoundingBoxes[0]))
                    {
                        enemy.LandedMeleeHit = true;
                        player.GetHit(enemy.EntityStats);
                        if (player.Transform.Position.X > enemy.Transform.Position.X)
                        {
                            player.Bounce(new Vector2(9, 0)); // bounces player based on direction of incoming hit
                            enemy.Bounce(new Vector2(-9, 0));
                        }
                        else
                        {
                            player.Bounce(new Vector2(-9, 0));
                            enemy.Bounce(new Vector2(9, 0));
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
