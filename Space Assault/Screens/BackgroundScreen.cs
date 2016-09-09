using System;
using Microsoft.Xna.Framework;
using SpaceAssault.Entities;
using SpaceAssault.ScreenManagers;
using SpaceAssault.Utils;
using IrrKlang;
using SpaceAssault.Screens.UI;

namespace SpaceAssault.Screens
{
    // The background screen sits behind all the other menu screens.
    // It draws a background image that remains fixed in place regardless
    // of whatever transitions the screens on top of it may be doing.
    class BackgroundScreen : GameScreen
    {
        //Station for BG
        private Station _station;

        //Sound
        private ISoundEngine _engine;
        float posOnCircle = 0;

        //Dialogs
        private Dialog _welcomedialog;
        private Dialog _pilotdialog;

        //Background
        private Background _back;

        //Random
        private Random _rand;
        private int _id;
        private int _id2;

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
            //Random IDs
            _rand = new Random();
            _id = _rand.Next(10000, 99999);
            _id2 = _rand.Next(10000, 99999);

            //Dialogs + Background + Frame
            _welcomedialog = new Dialog(100, 700, 32, 544, 4, false, true);
            _pilotdialog = new Dialog(800, 40, 48, 192, 4, false, true);
            _pilotdialog.LoadContent();
            _welcomedialog.LoadContent();
            //Camera
            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 40, 150) * 1.7f, new Vector3(-100, 0, 0), Vector3.Up);

            //Station
            _station = new Station(Vector3.Zero, 0);
            _station.LoadContent();

            //Sound
            _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);
            _engine.SetListenerPosition(new Vector3D(0,0,0), new Vector3D(0, 0, 1));
            Global.Music = _engine.Play2D("Content/Media/Music/Unrelenting.mp3", false);
            Global.Music.Volume = Global.MusicVolume / 10;

            _back = new Background();
        }


        // Unloads content for this screen.
        public override void UnloadContent()
        {
            _engine.StopAllSounds();
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


            posOnCircle += 0.04f;
            
        }


        // Draws the background screen.
        public override void Draw(GameTime gameTime)
        {
            _back.Draw(0, new Vector3(-9000, -8000, -5000));
            _station.Draw();
            _welcomedialog.Draw("Welcome pilot! " + gameTime.TotalGameTime.TotalSeconds.ToString().Remove(5) + " seconds without accidents! :)");
            _pilotdialog.Draw("Pilot ID:   "+ _id +  "\nStation ID: " + _id2);         
        }

    }
}
