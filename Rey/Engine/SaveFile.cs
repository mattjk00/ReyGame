using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    /// <summary>
    /// Holds data for a save file
    /// </summary>
    [Serializable]
    public class SaveFile
    {
        [JsonProperty]
        public Vector2 PlayerPosition { get; protected set; } // the player's position
        [JsonProperty]
        public List<string> Backpack { get; protected set; }
        [JsonProperty]
        public List<string> Equipped { get; protected set; }
        [JsonProperty]
        public string Map { get; protected set; }

        public SaveFile(Vector2 pos, List<string> inv, List<string> equipped, string map)
        {
            this.PlayerPosition = pos;
            this.Backpack = inv;
            this.Equipped = equipped;
            this.Map = map;
        }
    }
}
