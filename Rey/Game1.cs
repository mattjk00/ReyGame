using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rey.Engine;
using System;

namespace Rey
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //SceneManager sceneManager = new SceneManager();

        Texture2D mouseTexture;
        Camera2D camera = new Camera2D(1280, 720);
        MouseState mouse;
        KeyboardState keyboard;
        Texture2D vhsFilter;

        // lighting system
        Texture2D lightMask;
        Effect lightEffect;
        RenderTarget2D lightTarget;
        RenderTarget2D mainTarget;
        RenderTarget2D uiTarget;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = false;
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;
            //this.Window.AllowUserResizing = true;
            //this.graphics.ToggleFullScreen();
            //this.graphics.IsFullScreen = true;

            this.camera.Zoom = 1.0f;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            AssetLoader.Graphics = this.graphics;
            AssetLoader.LoadFont(Content);

            vhsFilter = AssetLoader.LoadTexture("Assets/Textures/backgrounds/vhs.png");

            this.graphics.PreferredBackBufferWidth = 1280;//(int)(graphics.GraphicsDevice.DisplayMode.Width * 0.9f);//1280;
            this.graphics.PreferredBackBufferHeight = 720;//(int)(graphics.GraphicsDevice.DisplayMode.Height * 0.9f);//720;
            //this.graphics.IsFullScreen = true;
            this.graphics.ApplyChanges();

            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            InputHelper.GD = this.GraphicsDevice;
            InputHelper.GDM = this.graphics;

            mouseTexture = AssetLoader.LoadTexture("Assets/Textures/Player/mouse.png");

            lightMask = AssetLoader.LoadTexture("Assets/Textures/lightmask.png");

            var pp = GraphicsDevice.PresentationParameters; // cache for ease of use
            lightTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            uiTarget = new RenderTarget2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);

            lightEffect = Content.Load<Effect>("lighteffect");

            SceneManager.Load();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();
            /*if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();*/
            
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                this.graphics.ToggleFullScreen();
            }

            if (SceneManager.Quit)
            {
                Exit();
            }

            InputHelper.MousePosition = Vector2.Transform(mouse.Position.ToVector2(), Matrix.Invert(camera.GetTransformation(GraphicsDevice, graphics)));
            InputHelper.Camera = this.camera;
            

            SceneManager.Update(camera);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            /*RenderTarget2D target = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            GraphicsDevice.SetRenderTarget(target);*/

            //[REAL]
            /*spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,  camera.GetTransformation(GraphicsDevice, graphics));
            SceneManager.Draw(spriteBatch);

            spriteBatch.End();

            

            // second draw
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            SceneManager.SecondDraw(spriteBatch);
            //spriteBatch.Draw(vhsFilter, Vector2.Zero, Color.White * 0.1f);
            spriteBatch.Draw(mouseTexture, Mouse.GetState().Position.ToVector2(), Color.White);
            

            //spriteBatch.DrawString(AssetLoader.Font, "(" + InputHelper.MousePosition.X.ToString() + ", " + InputHelper.MousePosition.Y.ToString() + ")", InputHelper.MousePosition, Color.Red);
            //spriteBatch.DrawString(AssetLoader.Font, "(" + Mouse.GetState().Position.X.ToString() + ", " + Mouse.GetState().Position.Y.ToString() + ")", InputHelper.MousePosition, Color.Red);

            spriteBatch.End();*/
            // [END REAL]

            var playerPos = SceneManager.TryToGetPlayerPosition();

            GraphicsDevice.SetRenderTarget(lightTarget);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, camera.GetTransformation(GraphicsDevice, graphics));
            //spriteBatch.Draw(lightMask, new Vector2(playerPos.X - lightMask.Width/2, playerPos.Y - lightMask.Height/2), Color.White);
            if (SceneManager.GetCurrentScene().UseLightShaders()) // draw the scene's lights if there should be
                SceneManager.GetCurrentScene().DrawLights(spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(mainTarget);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetTransformation(GraphicsDevice, graphics));
            SceneManager.Draw(spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(uiTarget);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            SceneManager.SecondDraw(spriteBatch);
            spriteBatch.Draw(mouseTexture, Mouse.GetState().Position.ToVector2(), Color.White);
            //spriteBatch.Draw(mouseTexture, new Vector2(35, 35), Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            
            // draw the light shaders if there is lights in the scene
            if (SceneManager.GetCurrentScene().UseLightShaders())
            {
                lightEffect.Parameters["lightMask"].SetValue(lightTarget);
                lightEffect.CurrentTechnique.Passes[0].Apply();
            }

            //spriteBatch.Draw(lightTarget, Vector2.Zero, Color.White);
            spriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(uiTarget, Vector2.Zero, Color.White);
            spriteBatch.End();

            /* GraphicsDevice.SetRenderTarget(null);


             spriteBatch.Begin();
             // what to scale the screen by
             float screenScaleFactorX = (float)graphics.PreferredBackBufferWidth/(float)target.Width;
             float screenScaleFactorY = (float)graphics.PreferredBackBufferHeight/(float)target.Height;
             spriteBatch.Draw(target, Vector2.Zero, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White, 0, Vector2.Zero, 
                 new Vector2(1, 1), 
                 SpriteEffects.None, 0);
             spriteBatch.End();*/

            base.Draw(gameTime);
        }
    }
}
