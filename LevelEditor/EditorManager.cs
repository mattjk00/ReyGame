using Microsoft.Xna.Framework.Graphics;
using Rey.Engine;
using Rey.Engine.Memory;
using Rey.Engine.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor
{
    public static class EditorManager
    {
        public static Texture2D defaultTile;

        public static bool TileMode = true;

        public static Tile currentTile; // the current tiel
        public static string currentTileName;
        public static MapMarker currentMarker;

        public static void Load()
        {
            defaultTile = AssetLoader.LoadTexture("Textures/default_tile.png");
        }
    }
}
