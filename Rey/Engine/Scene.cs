using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rey.Engine.Prefabs;
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
        public Texture2D Background { get; protected set; }
        public string Name { get; protected set; }

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

        /// <summary>
        /// Put loading logic in here
        /// </summary>
        public virtual void Load()
        {
            foreach (GameObject go in this.gameObjects)
                go.Load();
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
            sb.Draw(this.Background, Vector2.Zero, Color.White);
            foreach (GameObject go in this.gameObjects)
                go.Draw(sb);
        }

        /// <summary>
        /// Default updating
        /// </summary>
        public virtual void Update()
        {
            foreach (GameObject go in this.gameObjects)
                go.Update();
            this.CheckForAttackCollisions();
        }

        // checks for collisions such as the player stabbing an enemy
        void CheckForAttackCollisions()
        {
            var player = this.gameObjects.First(x => x.Name == "player") as Player; // find the player
            if (player.AttackState == PlayerAttackState.MeleeAttack && player.LandedMeleeHit == false) // if player is attacking and it hasnt already been registered
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

            }

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
                                player.Bounce(new Vector2(10, 0)); // bounces player based on direction of incoming hit
                            else
                                player.Bounce(new Vector2(-10, 0));
                        }
                    }

                }
            }
        }

    }
}
