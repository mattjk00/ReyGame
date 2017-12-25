using Microsoft.Xna.Framework.Graphics;
using Rey.Engine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public class SceneManager
    {
        private string currentScene = "test"; // the current scene being displayed
        private List<Scene> scenes = new List<Scene>(); // the list of all scenes in the game

        TestScene testScene;

        /// <summary>
        /// Create and load some scenes
        /// </summary>
        public void Load()
        {
            // load a test scene
            this.testScene = new TestScene();
            testScene.Load();
            this.scenes.Add(testScene);
            // done with test scene
        }

        // Update the currently running scene
        public void Update()
        {
            this.scenes.First(x => x.Name == this.currentScene).Update();   
        }

        // draw the current scene
        public void Draw(SpriteBatch sb)
        {
            this.scenes.First(x => x.Name == this.currentScene).Draw(sb);
        }
    }
}
