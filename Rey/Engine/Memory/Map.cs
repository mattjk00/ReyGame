using Newtonsoft.Json;
using Rey.Engine.Prefabs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Memory
{
    [Serializable]
    public class Map
    {
        [JsonProperty]
        public List<Tile> Tiles = new List<Tile>();
        [JsonProperty]
        public List<MapMarker> Markers = new List<MapMarker>();
        [JsonProperty]
        public int Width { get; set; } = 75;
        [JsonProperty]
        public int Height { get; set; } = 75;

        public static Map LoadFromFile(string filename)
        {
            Map loadedMap = new Map(); 

            // load the file
            string maptext = File.ReadAllText(filename);

            // deserialize it
            loadedMap = JsonConvert.DeserializeObject<Map>(maptext);

            var y = loadedMap.Tiles.FindAll(x => x.TileType == TileType.Door);

            return loadedMap;
        }

        public void SetSize(int w, int h)
        {
            this.Width = w;
            this.Height = h;
        }
    }
}
