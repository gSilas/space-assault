using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SpaceAssault.ScreenManagers
{
    // The screen manager manages one or more GameScreen instances.
    public class ScreenManager : DrawableGameComponent
    {

        List<GameScreen> _screens = new List<GameScreen>();
        List<GameScreen> _screensToUpdate = new List<GameScreen>();
        InputState _input = new InputState();
        Texture2D _blankTexture;
        Texture2D _crosshair;
        bool _isInitialized;


        // Constructs a new screen manager component.
        public ScreenManager(Game game) : base(game)
        {
        }

        // Initializes the screen manager component.
        public override void Initialize()
        {
            base.Initialize();
            _isInitialized = true;
        }


        // Load graphics content.
        protected override void LoadContent()
        {
            _blankTexture = Global.ContentManager.Load<Texture2D>("Images/blank");
            _crosshair = Global.ContentManager.Load<Texture2D>("Images/Fadenkreuz_1");

            // Tell each of the screens to load their content.
            foreach (GameScreen screen in _screens)
            {
                screen.LoadContent();
            }
        }


        // Unload graphics content.
        protected override void UnloadContent()
        {
            base.UnloadContent();

            // Tell each of the screens to unload their content.
            foreach (GameScreen screen in _screens)
            {
                screen.UnloadContent();
            }
        }


        // Allows each screen to run logic.
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            _input.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            _screensToUpdate.Clear();

            foreach (GameScreen screen in _screens)
                _screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (_screensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = _screensToUpdate[_screensToUpdate.Count - 1];

                _screensToUpdate.RemoveAt(_screensToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(_input);
                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }

        // Tells each screen to draw itself.
        public override void Draw(GameTime gameTime)
        {
            Global.SpriteBatch.Begin();
            foreach (GameScreen screen in _screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;
                screen.Draw(gameTime);
            }

            //crosshair
            Global.SpriteBatch.Draw(_crosshair, Mouse.GetState().Position.ToVector2() + new Vector2((_crosshair.Width + 1) / -2, _crosshair.Height / -2), Color.White);

            //FPS COUNTER
            Global.SpriteBatch.DrawString(Global.GameFont, (1.0f / gameTime.ElapsedGameTime.TotalSeconds).ToString("N0"), new Vector2(3, 3), Color.LightGreen);
            Global.SpriteBatch.End();
            Global.GraphicsManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }



        // Adds a new screen to the screen manager.
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;

            // If we have a graphics device, tell the screen to load content.
            _screens.Add(screen);
            if (_isInitialized)
            {
                screen.LoadContent();
            }
           
        }


        // Removes a screen from the screen manager. You should normally
        // use GameScreen.ExitScreen instead of calling this directly, so
        // the screen can gradually transition off rather than just being
        // instantly removed.
        public void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (_isInitialized)
            {
                screen.UnloadContent();
            }

            _screens.Remove(screen);
            _screensToUpdate.Remove(screen);
        }

        // Expose an array holding all the screens. We return a copy rather
        // than the real master list, because screens should only ever be added
        // or removed using the AddScreen and RemoveScreen methods.
        public GameScreen[] GetScreens()
        {
            return _screens.ToArray();
        }


        // Helper draws a translucent black fullscreen sprite, used for fading
        // screens in and out, and for darkening the background behind popups.
        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = Global.GraphicsManager.GraphicsDevice.Viewport;

            Global.SpriteBatch.Draw(_blankTexture, new Rectangle(0, 0, viewport.Width, viewport.Height),Color.Black * alpha);

        }
    }
}
