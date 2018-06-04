using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Rey.Engine
{
    public static class AssetLoader
    {
        public static GraphicsDeviceManager Graphics { get; set; }
        public static SpriteFont Font { get; set; }
        private static Dictionary<string, Texture2D> cache = new Dictionary<string, Texture2D>(); // caches loaded textures
        public static Texture2D BoundingBoxTexture;

        public static void LoadFont(ContentManager content)
        {
            Font = content.Load<SpriteFont>("gameFont");
            BoundingBoxTexture = AssetLoader.LoadTexture("Assets/Textures/ui/bb.png");
        }
        /// <summary>
        /// Loads a texture from a file rather than XNB
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string fileName)
        {
            try
            {
                // if the cache has not loaded this file before
                if (cache.ContainsKey(fileName) == false)
                {
                    // load a file stream of the texture and then load it into a Texture2D
                    FileStream fs = new FileStream(fileName, FileMode.Open);
                    var t = Texture2D.FromStream(Graphics.GraphicsDevice, fs);
                    fs.Dispose();
                    cache.Add(fileName, t); // cache the texture
                    return t;
                }
                else // if the file has been loaded
                {
                    var t = cache[fileName]; // load the texture from the cache
                    return t;
                }
            }
            catch (FileNotFoundException fnfe)
            {
                return null;
            }
        }
    }
}
