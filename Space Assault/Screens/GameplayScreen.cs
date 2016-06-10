using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceAssault.Entities;
using SpaceAssault.Entities.Weapon;
using SpaceAssault.ScreenManager;
using SpaceAssault.Utils;

namespace SpaceAssault.Screens
{

    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    class GameplayScreen : GameScreen
    {
        SpriteFont gameFont;
        Vector2 playerPosition = new Vector2(100, 100);
        float pauseAlpha;

        private Station _station;
        private AsteroidBuilder _asteroidField;
        private Drone _drone;
        private Texture2D _background;
        private List<Bullet> _removeBullets;
        private List<Asteroid> _removeAsteroid;

        // Constructor.
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _station = new Station(new Vector3(0, 0, 0), 0);
            _drone = new Drone(new Vector3(0, 0, 20));
            //_asteroidField = new AsteroidBuilder(new Vector3(500, 0, -500));
            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 250, 250), _drone.Position, Vector3.Up);
            _removeAsteroid = new List<Asteroid>();
            _removeBullets = new List<Bullet>();
            _drone.Initialize();
            _station.Initialize();
        }


        // Load graphics content for the game.
        public override void LoadContent()
        {
            gameFont = Global.ContentManager.Load<SpriteFont>("Fonts/gamefont");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
            _station.LoadContent();
            _drone.LoadContent();
        }


        // Unload graphics content used by the game.
        public override void UnloadContent()
        {
            Global.ContentManager.Unload();
        }


        // Updates the state of the game. This method checks the GameScreen.IsActive
        // property, so the game will stop updating when the pause menu is active,
        // or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                //3D Model
                _station.Update(gameTime);
                _drone.Update(gameTime);
                Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, _drone.Position + new Vector3(0, 250, 250), _drone.Position, Vector3.Up);
            }
        }

        // Lets the game respond to player input. Unlike the Update method,
        // this will only be called when the gameplay screen is active.
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];

            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                Vector2 movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                if (movement.Length() > 1)
                    movement.Normalize();

                playerPosition += movement * 2;
            }
        }


        // Draws the gameplay screen.
        public override void Draw(GameTime gameTime)
        {
            Global.GraphicsManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);

            Global.SpriteBatch.Begin();
            //Global.SpriteBatch.DrawString(gameFont, "// TODO", playerPosition, Color.Green);
            Global.SpriteBatch.End();
            Global.GraphicsManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            _station.Draw();
            _drone.Draw();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
