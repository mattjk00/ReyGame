﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
            /*if (player.AttackState == PlayerAttackState.MeleeAttack && player.LandedMeleeHit == false)
            {
                // if player weapon intersects bat cumulative bounding box
                switch (enemy.Name)
                {
                    // if it's a bat just check the regular bounding box
                    case "bat":
                    case "babyfishdemon":
                    case "mushroomminion":
                    case "skulldemon":
                    case "gargoyle":
                        if (player.BoundingBoxes[1].Intersects(enemy.BoundingBoxes[0]))
                            this.HandleEnemyGettingHitByMelee(player, enemy, new Vector2(2, 0));
                        break;
                }
            }*/
        }

        /// <summary>
        /// Handles the collision between an enemy and a projectile created by the player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        public void HandlePlayerProjectileAttackEnemyCollision(Player player, Projectile projectile, Enemy enemy, ParticleManager particleManager)
        {
            // if player weapon intersects enemy bounding box
            switch (enemy.Name)
            {
                // if it's a bat just check the regular bounding box
                case "bat":
                case "babyfishdemon":
                case "gargoyle":
                    if (projectile.BoundingBoxes[0].Intersects(enemy.BoundingBoxes[0]))
                    {
                        HandleEnemyGettingHitByProjectile(player, projectile, enemy, new Vector2(5, 0));
                        particleManager.Burst(enemy.Transform.Position, new Vector2(3, 3), Color.Red, 25, Vector2.One);
                    }
                    break;
                case "mushroomminion":
                case "skulldemon":
                    if (projectile.BoundingBoxes[0].Intersects(enemy.BoundingBoxes[0]))
                    {
                        HandleEnemyGettingHitByProjectile(player, projectile, enemy, new Vector2(1, 0));
                        particleManager.Burst(enemy.Transform.Position, new Vector2(3, 3), Color.Red, 25, Vector2.One);
                    }
                    break;
                case "mushroomboss":
                    foreach (Rectangle box in enemy.BoundingBoxes)
                        if (projectile.BoundingBoxes[0].Intersects(box))
                        {
                            HandleEnemyGettingHitByProjectile(player, projectile, enemy, new Vector2(0, 0));
                            particleManager.Burst(projectile.Transform.Position, new Vector2(3, 3), Color.DarkSlateBlue, 100, Vector2.One);
                        }
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
            SceneManager.SoundManager.PlaySound("hit", 0.05f, 0.0f, 0.0f);
        }

        public void HandleEnemyProjectileHittingPlayer(Player player, Projectile projectile, ParticleManager particleManager)
        {
            // if hitting the player
            if (projectile.BoundingBoxes[0].Intersects(player.BoundingBoxes[0]))
            {
                projectile.ToBeDestroyed = true;
                player.GetHit(projectile.Damage);
                particleManager.Burst(player.Transform.Position, new Vector2(2, 2), Color.Red, 25, Vector2.One);
                SceneManager.SoundManager.PlaySound("hit", 0.05f, 0.0f, 0.0f);
            }
        }

        /// <summary>
        /// Handles the collision between a player and the world
        /// </summary>
        /// <param name="player"></param>
        /// <param name="tile"></param>
        public void HandlePlayerAndBlock(Player player, Tile tile, string side)
        {
            if (side == "left")
            {
                player.Transform.Position = new Vector2(tile.Transform.Position.X - player.MovementBox.Width + player.MovementBoxOffset + 1, player.Transform.Position.Y);
                //player.Transform.Position = new Vector2(tile.Box.Left - player.MovementBox.Width/2 - 5, player.Transform.Position.Y);
                player.Transform.VelX = 0;
            }
            else if (side == "right")
            {
                player.Transform.Position = new Vector2(tile.Transform.Position.X + tile.Box.Width + player.MovementBoxOffset, player.Transform.Position.Y);
                //player.Transform.Position = new Vector2(tile.Box.Right + player.MovementBox.Width / 2 - 5, player.Transform.Position.Y);
                player.Transform.VelX = 0;
            }
            else if (side == "bottom")
            {
                player.Transform.Position = new Vector2(player.Transform.Position.X, tile.Transform.Position.Y - 15);
                //player.Transform.Position = new Vector2(player.Transform.Position.X, tile.Box.Bottom - player.Sprite.Texture.Height + 7);
                player.Transform.VelY = 0;
            }
            else if (side == "top")
            {
                player.Transform.Position = new Vector2(player.Transform.Position.X, tile.Box.Top - player.Sprite.Texture.Height - 7);
                player.Transform.VelY = 0;
            }
        }

        public void HandleNPCAndBlock(NPC npc, Tile tile, string side)
        {
            if (side == "left")
            {
                npc.Transform.Position = new Vector2(tile.Transform.Position.X - npc.MovementBox.Width + npc.MovementBoxOffset + 1, npc.Transform.Position.Y);
                //player.Transform.Position = new Vector2(tile.Box.Left - player.MovementBox.Width/2 - 5, player.Transform.Position.Y);
                npc.Transform.VelX = 0;
            }
            else if (side == "right")
            {
                npc.Transform.Position = new Vector2(tile.Transform.Position.X + tile.Box.Width + npc.MovementBoxOffset, npc.Transform.Position.Y);
                //npc.Transform.Position = new Vector2(tile.Box.Right + npc.MovementBox.Width / 2 - 5, npc.Transform.Position.Y);
                npc.Transform.VelX = 0;
            }
            else if (side == "bottom")
            {
                npc.Transform.Position = new Vector2(npc.Transform.Position.X, tile.Transform.Position.Y - 15);
                //npc.Transform.Position = new Vector2(npc.Transform.Position.X, tile.Box.Bottom - npc.Sprite.Texture.Height + 7);
                npc.Transform.VelY = 0;
            }
            else if (side == "top")
            {
                npc.Transform.Position = new Vector2(npc.Transform.Position.X, tile.Box.Top - npc.Sprite.Texture.Height - 7);
                npc.Transform.VelY = 0;
            }
        }

        /// <summary>
        /// handles collision between player and pickup
        /// </summary>
        public void HandlePlayerAndPickup(Player player, Pickup pickup)
        {
            // if space is being pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                pickup.GetPickedUp();
            }
        }
    }
}
