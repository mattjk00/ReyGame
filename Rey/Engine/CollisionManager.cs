using Microsoft.Xna.Framework;
using Rey.Engine.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public class CollisionManager
    {
        /// <summary>
        /// Handles the collisions between a player and a bat
        /// </summary>
        /// <param name="player"></param>
        /// <param name="bat"></param>
        public void HandlePlayerAttackEnemyCollision(Player player, Enemy enemy)
        {
            /* Melee Attacks */
            if (player.AttackState == PlayerAttackState.MeleeAttack && player.LandedMeleeHit == false)
            {
                // if player weapon intersects bat cumulative bounding box
                switch (enemy.Name)
                {
                    // if it's a bat just check the regular bounding box
                    case "bat":
                    case "babyfishdemon":
                        if (player.BoundingBoxes[1].Intersects(enemy.BoundingBoxes[0]))
                            this.HandleEnemyGettingHitByMelee(player, enemy, new Vector2(2, 0));
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the collision between an enemy and a projectile created by the player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        public void HandlePlayerProjectileAttackEnemyCollision(Player player, Projectile projectile, Enemy enemy)
        {
            // if player weapon intersects enemy bounding box
            switch (enemy.Name)
            {
                // if it's a bat just check the regular bounding box
                case "bat":
                case "babyfishdemon":
                    if (projectile.BoundingBoxes[0].Intersects(enemy.BoundingBoxes[0]))
                        HandleEnemyGettingHitByProjectile(player, projectile, enemy, new Vector2(5, 0));
                    break;
            }
        }

        // handles the event where an enemy gets hit by a player's melee attack
        void HandleEnemyGettingHitByMelee(Player player, Enemy enemy, Vector2 bounce)
        {
            // convert to an enemy
            enemy.GetHitMelee(player.EntityStats); // hit the enemy
            enemy.StartAttack(player);
            if (player.Transform.Position.X > enemy.Transform.Position.X)
                enemy.Bounce(-bounce); // bounces enemy based on direction of incoming hit
            else
                enemy.Bounce(bounce);
            player.LandedMeleeHit = true; // log the hit
        }

        void HandleEnemyGettingHitByProjectile(Player player, Projectile projectile, Enemy enemy, Vector2 bounce)
        {
            // convert to an enemy
            enemy.GetHitMagic(player.EntityStats, projectile); // hit the enemy
            enemy.StartAttack(player);
            if (player.Transform.Position.X > enemy.Transform.Position.X)
                enemy.Bounce(-bounce); // bounces enemy based on direction of incoming hit
            else
                enemy.Bounce(bounce);
            projectile.ToBeDestroyed = true; // destroy the projectile
        }
    }
}
