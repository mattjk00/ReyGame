using Microsoft.Xna.Framework.Graphics;
using Rey.Engine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rey.Engine
{
    public static class SceneManager
    {
        static private string currentScene = "test"; // the current scene being displayed
        static private List<Scene> scenes = new List<Scene>(); // the list of all scenes in the game

        static private TestScene testScene;
        static private TransitionScene transitionScene;

        public static bool StartingNewCombatScene = false;

        /// <summary>
        /// Create and load some scenes
        /// </summary>
        public static void Load()
        {
            // load a test scene
            testScene = new TestScene();
            testScene.Load();
            scenes.Add(testScene);
            // done with test scene

            transitionScene = new TransitionScene();
            transitionScene.Load();
            scenes.Add(transitionScene);
        }

        // Update the currently running scene
        public static void Update()
        {
            scenes.First(x => x.Name == currentScene).Update();
        }

        // draw the current scene
        public static void Draw(SpriteBatch sb)
        {
            scenes.First(x => x.Name == currentScene).Draw(sb);
        }

        /// <summary>
        /// Attempts to go the next floor
        /// </summary>
        public static void TryToGoToNextFloor()
        {
            // check if it's a combat scene. Test for now
            if (currentScene == "test")
            {
                // find the current scene and get it as the combat scene
                var scene = scenes.Find(x => x.Name == currentScene) as TestScene;

                // go to the next floor
                scene.NextFloor();

                TransitionToScene("test");
            }
        }

        public static void SetScene(string sceneName)
        {
            currentScene = sceneName;
        }

        public static void TransitionToScene(string sceneName)
        {
            currentScene = "transition";
            transitionScene.LoadNewTransition(sceneName);
        }
    }
}
