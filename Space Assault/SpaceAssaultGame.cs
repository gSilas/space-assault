using Microsoft.Xna.Framework;
using SpaceAssault.Screens;
using SpaceAssault.ScreenManagers;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

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
            Global.ContentManager = Content;
            Global.ContentManager.RootDirectory = "Content";

            Global.GraphicsManager.PreferredBackBufferHeight = Global.PreferredBackBufferHeight;
            Global.GraphicsManager.PreferredBackBufferWidth = Global.PreferredBackBufferWidth;
            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen());
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

            //crosshair
            Global.SpriteBatch.Begin();
            Global.SpriteBatch.DrawString(Global.Font, "X", Mouse.GetState().Position.ToVector2() + new Vector2(-6, -7), Color.LightGoldenrodYellow);

            //FPS COUNTER
            Global.SpriteBatch.DrawString(Global.Font, (1 / gameTime.ElapsedGameTime.TotalSeconds).ToString("N1"), new Vector2(3, 3), Color.LightGreen);
            Global.SpriteBatch.End();
            Global.GraphicsManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}