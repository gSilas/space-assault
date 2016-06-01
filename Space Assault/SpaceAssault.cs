using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Assault.Utils;

namespace Space_Assault
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SpaceAssault : Game
    {
        public SpaceAssault()
        {
            Global.GraphicsManager = new GraphicsDeviceManager(this);
            Global.ContentManager = Content;
            Global.ContentManager.RootDirectory = "Content";
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
            Global.GraphicsManager.PreferredBackBufferHeight = 768;
            Global.GraphicsManager.PreferredBackBufferWidth = 1366;
            Global.GraphicsManager.ApplyChanges();

            // TODO: Add your initialization logic here
            Global.Controller = new Controller();

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
            Global.Arial = Global.ContentManager.Load<SpriteFont>("Fonts/Arial");
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
            Global.Controller.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            Global.SpriteBatch.Begin();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Global.SpriteBatch.DrawString(Global.Arial, (1/gameTime.ElapsedGameTime.TotalSeconds).ToString("N1"), new Vector2(3, 3), Color.LightGreen);
            Global.Controller.Draw(gameTime);
            Global.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
