using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Space_Assault.Global;

namespace Space_Assault
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SpaceAssault : Game
    {
        GraphicsDeviceManager graphics;
        private SpriteFont _font;
        private Controller _controller;

        public SpaceAssault()
        {
            graphics = new GraphicsDeviceManager(this);
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
            //Initialize window
            Window.Title = "Space Assault";
            Window.AllowAltF4 = true;
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1366;
            graphics.ApplyChanges();

            // TODO: Add your initialization logic here
            _controller = new Controller(graphics, Content);

            // Create a new SpriteBatch, which can be used to draw textures.

            Global.SpriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _font = Content.Load<SpriteFont>("Fonts/Arial");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _controller.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            Global.SpriteBatch.Begin();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Global.SpriteBatch.DrawString(_font, (1/gameTime.ElapsedGameTime.TotalSeconds).ToString("N1"), new Vector2(3, 3), Color.LightGreen);
            _controller.Draw(gameTime);
            Global.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
