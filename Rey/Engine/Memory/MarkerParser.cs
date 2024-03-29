﻿using Microsoft.Xna.Framework;
using Rey.Engine.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Memory
{
    // parses markers
    public class MarkerParser
    {
        /// <summary>
        /// Returns a list of enemies to b e loaded
        /// </summary>
        /// <param name="markers"></param>
        /// <returns></returns>
        public static List<Enemy> ParseEnemies(List<MapMarker> markers)
        {
            List<Enemy> enemies = new List<Enemy>();
            // iterate through each marker of enemy type
            foreach (MapMarker enemyMarker in markers.FindAll(x => x.MarkerType == MarkerType.SpawnPoint))
            {
                // create a new enemy and set its position
                Enemy newEnemy = new Enemy();
                // get the correct enemy type
                switch (enemyMarker.Name)
                {
                    case "bat_head":
                        newEnemy = new Bat();
                        break;
                    case "fish_demon":
                        newEnemy = new BabyFishDemon();
                        break;
                    case "mushroom":
                        newEnemy = new MushroomMinion();
                        break;
                    case "gargoyle_head":
                        newEnemy = new Gargoyle();
                        break;
                    case "skull":
                        newEnemy = new SkullDemon();
                        break;
                    case "mushroomboss":
                        newEnemy = new MushroomBoss();
                        break;
                }
                newEnemy.Transform.Position = enemyMarker.Position;
                enemies.Add(newEnemy);
            }

            return enemies;
        }

        public static List<NPC> ParseNPCs(List<MapMarker> markers)
        {
            List<NPC> npcs = new List<NPC>();
            // iterate through each marker of npcs type
            foreach (MapMarker npcMarker in markers.FindAll(x => x.MarkerType == MarkerType.NPCSpawnPoint))
            {
                // create a new npc and set its position
                NPC newNPC = new NPC("npc");
                // get the correct enemy type
                switch (npcMarker.Name)
                {
                    case "npc_clarice":
                        newNPC = new NPC("npc", "clarice.guat");
                        break;
                    case "npc_jenkins":
                        newNPC = new NPC("jenkins", "jenkins.guat");
                        break;
                    case "npc_jenkins2":
                        newNPC = new NPC("jenkins", "jenkins2.guat");
                        break;
                }
                newNPC.Transform.Position = npcMarker.Position;
                npcs.Add(newNPC);
            }

            return npcs;
        }

        public static List<LightSource> ParseLightsources(List<MapMarker> markers)
        {
            List<LightSource> ls = new List<LightSource>();
            // iterate through
            foreach (MapMarker lsMarker in markers.FindAll(x => x.MarkerType == MarkerType.LightSource))
            {
                LightSource newLs = new LightSource(lsMarker.Position, Vector2.One, Color.White);
                ls.Add(newLs);
            }
            return ls;
        }
    }
}
