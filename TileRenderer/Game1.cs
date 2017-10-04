using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileRenderer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public const float Scale = 1f;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int RendererIndex;
        IReadOnlyList<IRenderer> Renderers;
        Texture2D Pixel;

        public Game1(Func<Game1, GraphicsDeviceManager> gfxFactory = null)
        {
            graphics = gfxFactory?.Invoke(this) ?? new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720,
                SynchronizeWithVerticalRetrace = false,
                SupportedOrientations = DisplayOrientation.Default,
                IsFullScreen = false
            };
            IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
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
            Pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Pixel.SetData(new[] { Color.Black });

            var map = Content.Load<TileMap>("Maps/map");
            Renderers = new IRenderer[]
            {
                new RendererVB(map, GraphicsDevice),
                new RendererSB(map, spriteBatch)
            };
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
            var keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();


            if (keyboardState.IsKeyDown(Keys.D1))
            {
                RendererIndex = 0;
                ResetFpsCounter(gameTime);
            }
            else if (keyboardState.IsKeyDown(Keys.D2))
            {
                RendererIndex = 1;
                ResetFpsCounter(gameTime);
            }
            else if (keyboardState.IsKeyDown(Keys.Space) || Microsoft.Xna.Framework.Input.Touch.TouchPanel.GetState().Count > 0)
                ResetFpsCounter(gameTime);

            base.Update(gameTime);
        }

        long frameCount;
        TimeSpan StartTime;
        void ResetFpsCounter(GameTime gameTime)
        {
            frameCount = 0;
            StartTime = gameTime.TotalGameTime;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var renderer = Renderers[RendererIndex];
            renderer.Draw();

            spriteBatch.Begin(transformMatrix: Matrix.Identity);
            {
                var dt = gameTime.TotalGameTime.TotalSeconds - StartTime.TotalSeconds;
                var fps = ++frameCount / Math.Max(dt, 0.000000001);
                var text = renderer.Name + "\n" + "FPS: " + fps.ToString("#.##");

                var font = Content.Load<SpriteFont>("Font0");
                spriteBatch.Draw(Pixel, new Rectangle(0, 0, 300, font.LineSpacing * 2), Color.Black);
                spriteBatch.DrawString(font, text, Vector2.UnitX * 10f, Color.Red);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
