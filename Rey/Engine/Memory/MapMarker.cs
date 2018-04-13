using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine.Memory
{
    public enum MarkerType
    {
        PlayerSpawnPoint,
        SpawnPoint,
        NPCSpawnPoint
    }

    [Serializable]
    public class MapMarker
    {
        public MarkerType MarkerType { get; set; }
        public string Name { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 StartingPosition { get; set; }
        public Texture2D Texture { get; set; }

        public MapMarker(string name, Vector2 pos, Texture2D tex, MarkerType markerType)
        {
            this.Name = name;
            this.StartingPosition = new Vector2(pos.X, pos.Y);
            this.Position = pos;
            this.Texture = tex;
            this.MarkerType = markerType;
        }
    }
}
