﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Rey.Engine;

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

            this.graphics.PreferredBackBufferWidth = 1280;//(int)(graphics.GraphicsDevice.DisplayMode.Width * 0.9f);//1280;
            this.graphics.PreferredBackBufferHeight = 720;//(int)(graphics.GraphicsDevice.DisplayMode.Height * 0.9f);//720;
            //this.graphics.IsFullScreen = true;
            this.graphics.ApplyChanges();

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            /*RenderTarget2D target = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            GraphicsDevice.SetRenderTarget(target);*/

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,  camera.GetTransformation(GraphicsDevice, graphics));
            SceneManager.Draw(spriteBatch);

            spriteBatch.End();

            

            // second draw
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            SceneManager.SecondDraw(spriteBatch);
            //spriteBatch.Draw(vhsFilter, Vector2.Zero, Color.White * 0.1f);
            spriteBatch.Draw(mouseTexture, Mouse.GetState().Position.ToVector2(), Color.White);

            //spriteBatch.DrawString(AssetLoader.Font, "(" + InputHelper.MousePosition.X.ToString() + ", " + InputHelper.MousePosition.Y.ToString() + ")", InputHelper.MousePosition, Color.Red);
            //spriteBatch.DrawString(AssetLoader.Font, "(" + Mouse.GetState().Position.X.ToString() + ", " + Mouse.GetState().Position.Y.ToString() + ")", InputHelper.MousePosition, Color.Red);

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
