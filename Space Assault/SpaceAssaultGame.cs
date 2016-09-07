using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Screens;
using SpaceAssault.ScreenManagers;


namespace SpaceAssault
{
    public class SpaceAssaultGame : Game
    {
        ScreenManager screenManager;

        //#################################
        // Constructor
        //#################################
        public SpaceAssaultGame()
        {
            IsMouseVisible = false;
            Global.GraphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Global.ContentManager = Content;

            Global.GraphicsManager.PreferredBackBufferHeight = Global.PreferredBackBufferHeight;
            Global.GraphicsManager.PreferredBackBufferWidth = Global.PreferredBackBufferWidth;
            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());
        }


        protected override void LoadContent()
        {   
            // Create a new SpriteBatch, which can be used to draw textures.
            Global.SpriteBatch = new SpriteBatch(Global.GraphicsManager.GraphicsDevice);
            Global.BackgroundBatch = new SpriteBatch(Global.GraphicsManager.GraphicsDevice);
            Global.UIBatch = new SpriteBatch(Global.GraphicsManager.GraphicsDevice);

            //Global.GameFont = Global.ContentManager.Load<SpriteFont>("Fonts/menufont");
            //Global.GameFont = Global.ContentManager.Load<SpriteFont>("Fonts/pc_senior/pcsenior");
            //Global.DialogFont = Global.ContentManager.Load<SpriteFont>("Fonts/pc_senior/pcsenior");
            Global.GameFont = Global.ContentManager.Load<SpriteFont>("Fonts/raider_crusader/raidercrusader");
            Global.DialogFont = Global.ContentManager.Load<SpriteFont>("Fonts/raider_crusader/raidercrusader");
            base.LoadContent();
        }

        //#################################
        // Draw
        //#################################
        protected override void Draw(GameTime gameTime)
        {
            Global.GraphicsManager.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }
    }
}