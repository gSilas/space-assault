using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities;
using SpaceAssault.ScreenManagers;
using SpaceAssault.Utils;
using IrrKlang;

namespace SpaceAssault.Screens
{
    // The background screen sits behind all the other menu screens.
    // It draws a background image that remains fixed in place regardless
    // of whatever transitions the screens on top of it may be doing.
    class BackgroundScreen : GameScreen
    {
        Texture2D backgroundTexture;
        private Station _station;
        private Background _back;
        private ISoundEngine _engine;

        // Constructor.
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        // Loads graphics content for this screen. The background texture is quite
        // big, so we use our own local ContentManager to load it. This allows us
        // to unload before going from the menus into the game itself, wheras if we
        // used the shared ContentManager provided by the Game class, the content
        // would remain loaded forever.
        public override void LoadContent()
        {

            backgroundTexture = Global.ContentManager.Load<Texture2D>("Images/background");

            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 40, 150)*1.7f, new Vector3(-100, 0, 0), Vector3.Up);

            _station = new Station(Vector3.Zero, 0);
            _station.LoadContent();
            _back = new Background();

            _engine = new ISoundEngine();
            ISound music = _engine.Play3D("Content/Media/Music/SUBSET_-_05_-_Nazca.mp3", 0, 0, 0, true);

        }


        // Unloads graphics content for this screen.
        public override void UnloadContent()
        {
            //Global.ContentManager.Unload();
            _engine.Dispose();
        }


        // Updates the background screen. Unlike most screens, this should not
        // transition off even if it has been covered by another screen: it is
        // supposed to be covered, after all! This overload forces the
        // coveredByOtherScreen parameter to false in order to stop the base
        // Update method wanting to transition off.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            _station.Update(gameTime);
            
        }


        // Draws the background screen.
        public override void Draw(GameTime gameTime)
        {
            _station.Draw();
            _back.Draw(0,new Vector3(-9000,-8000,-5000));
        }

    }
}
