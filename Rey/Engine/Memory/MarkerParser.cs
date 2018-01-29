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
        /// Returns a list of enemies to be loaded
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
                }
                newEnemy.Transform.Position = enemyMarker.Position;
                enemies.Add(newEnemy);
            }

            return enemies;
        }
    }
}
