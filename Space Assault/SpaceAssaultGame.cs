using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Screens;

namespace SpaceAssault
{
    public class SpaceAssaultGame : Game
    {
        ScreenManager.ScreenManager screenManager;

        //#################################
        // Constructor
        //#################################
        public SpaceAssaultGame()
        {
            IsMouseVisible = true;
            Global.GraphicsManager = new GraphicsDeviceManager(this);
            Global.ContentManager = Content;
            Global.ContentManager.RootDirectory = "Content";

            Global.GraphicsManager.PreferredBackBufferHeight = Global.PreferredBackBufferHeight;
            Global.GraphicsManager.PreferredBackBufferWidth = Global.PreferredBackBufferWidth;

            // Create the screen manager component.
            screenManager = new ScreenManager.ScreenManager(this);

            Components.Add(screenManager);

            // Activate the first screens.
            /*
             * We are gonna ingore the Menu from now on and gonna focus on the gameplay
            screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());
            */
            screenManager.AddScreen(new GameplayScreen());

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