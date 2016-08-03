using Microsoft.Xna.Framework;
using SpaceAssault.Screens;
using SpaceAssault.ScreenManagers;
using System;

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
            IsMouseVisible = true;
            Global.GraphicsManager = new GraphicsDeviceManager(this);
            Global.ContentManager = Content;
            Global.ContentManager.RootDirectory = "Content";

            Global.GraphicsManager.PreferredBackBufferHeight = Global.PreferredBackBufferHeight;
            Global.GraphicsManager.PreferredBackBufferWidth = Global.PreferredBackBufferWidth;

            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // Activate the first screens.
            //screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());
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