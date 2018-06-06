using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Rey.Engine.Prefabs;
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
        static private string currentScene = "mainmenu"; // the current scene being displayed
        static private string lastScene = "";
        static private List<Scene> scenes = new List<Scene>(); // the list of all scenes in the game

        static private TestScene testScene;
        static private TransitionScene transitionScene;
        static private MainMenuScene mainMenuScene;

        public static bool StartingNewCombatScene = false;
        static public bool DrawLastScene = false; // if this is true, the scene manager will draw the last scene as well as the current scene. can be used for transitions
        public static bool Quit = false;

        // sound manager
        public static SoundManager SoundManager { get; private set; } = new SoundManager();

        /// <summary>
        /// Create and load some scenes
        /// </summary>
        public static void Load(ContentManager content)
        {
            
            // load a test scene
            testScene = new TestScene("test");
            testScene.Load();
            scenes.Add(testScene);
            // done with test scene

            // transition
            transitionScene = new TransitionScene("transition");
            transitionScene.Load();
            scenes.Add(transitionScene);

            // main menu
            mainMenuScene = new MainMenuScene("mainmenu");
            mainMenuScene.Load();
            scenes.Add(mainMenuScene);

            SoundManager.Load(content);

            SetScene("test");
        }

        // Update the currently running scene
        public static void Update(Camera2D camera)
        {
            scenes.First(x => x.Name == currentScene).Update(camera);
        }

        // draw the current scene
        public static void Draw(SpriteBatch sb)
        {
            // if drawing the last scene, find and draw the last scene
            if (DrawLastScene)
                scenes.Find(x => x.Name == lastScene).Draw(sb);

            // draw the current scene
            scenes.First(x => x.Name == currentScene).Draw(sb);
        }

        // do the second draw call for the current scene
        public static void SecondDraw(SpriteBatch sb)
        {
            // if drawing the last scene, find and draw the last scene
            if (DrawLastScene)
                scenes.Find(x => x.Name == lastScene).SecondDraw(sb);

            // draw the current scene
            scenes.First(x => x.Name == currentScene).SecondDraw(sb);
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
            lastScene = currentScene;
            currentScene = sceneName;
            if (InputHelper.Camera != null)
                InputHelper.Camera.Position = Vector2.Zero;
            if (sceneName == "test")
            {
                testScene.LoadMap(true);
            }
                // load new scene and unload old scene
            /*scenes.Find(x => x.Name == currentScene).Load();
            if (DrawLastScene == false)
                scenes.Find(x => x.Name == lastScene).Unload();*/
        }

        // gets the current scene
        public static Scene GetCurrentScene()
        {
            return scenes.Find(x => x.Name == currentScene);
        }

        public static void TransitionToScene(string sceneName)
        {
            DrawLastScene = true; // draw the last scene while transitioning
            SetScene("transition");
            transitionScene.LoadNewTransition(sceneName);
        }

        public static Vector2 TryToGetPlayerPosition()
        {
            try
            {
                var player = GetCurrentScene().gameObjects.First(x => x.GetType() == typeof(Player));
                return player.Transform.Position;
            }
            catch (InvalidOperationException ioe)
            {
                return new Vector2(1280/2, 720/2);
            }
        }
    }
}
