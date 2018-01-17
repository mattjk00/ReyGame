using Microsoft.Xna.Framework.Graphics;
using Rey.Engine;
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

        public static Tile currentTile; // the current tiel

        public static void Load()
        {
            defaultTile = AssetLoader.LoadTexture("Textures/default_tile.png");
        }
    }
}
