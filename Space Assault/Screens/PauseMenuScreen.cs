using System;
using IrrKlang;

namespace SpaceAssault.Screens
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    class PauseMenuScreen : MenuScreen
    {
        private MenuEntry _fullscreenMenuEntry;
        private MenuEntry _frameCounterMenuEntry;
        private MenuEntry _effectVolumeMenuEntry;
        private MenuEntry _musicVolumeMenuEntry;
        // Constructor.
        public PauseMenuScreen()
            : base("Interrupted")
        {


            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Mission");
            _fullscreenMenuEntry = new MenuEntry(string.Empty);
            _frameCounterMenuEntry = new MenuEntry(string.Empty);
            _effectVolumeMenuEntry = new MenuEntry(string.Empty);
            _musicVolumeMenuEntry = new MenuEntry(string.Empty);
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Station");
            SetMenuEntryText();

            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            _fullscreenMenuEntry.Selected += FullscreenMenuEntrySelected;
            _frameCounterMenuEntry.Selected += FrameCounterMenuEntrySelected;
            _effectVolumeMenuEntry.Selected += EffectVolumeMenuEntrySelected;
            _musicVolumeMenuEntry.Selected += MusicVolumeMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(_fullscreenMenuEntry);
            MenuEntries.Add(_frameCounterMenuEntry);
            MenuEntries.Add(_musicVolumeMenuEntry);
            MenuEntries.Add(_effectVolumeMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        // Event handler for when the Quit Game menu entry is selected.
        void QuitGameMenuEntrySelected(object sender, EventArgs e)
        {
            
            const string message = "Are you sure you want to abandon your Mission?";
            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);
            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmQuitMessageBox);
            //playing the sound
            SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
            ISound Accept;
            Accept = SoundEngine.Play2D(OkClick, false, true, false);
            Accept.Volume = Global.SpeakerVolume / 10;
            Accept.Paused = false;
        }
        void FullscreenMenuEntrySelected(object sender, EventArgs e)
        {
            Global.GraphicsManager.ToggleFullScreen();
            SetMenuEntryText();
        }

        void FrameCounterMenuEntrySelected(object sender, EventArgs e)
        {
            Global.FrameCounterIsEnabled = !Global.FrameCounterIsEnabled;
            SetMenuEntryText();
        }

        void EffectVolumeMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.SpeakerVolume == 10)
                Global.SpeakerVolume = 0;
            else
                Global.SpeakerVolume += 1;
            SetMenuEntryText();
        }

        void MusicVolumeMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.MusicVolume == 10)
                Global.MusicVolume = 0;
            else
                Global.MusicVolume += 1;

            Global.Music.Volume = Global.MusicVolume / 10;

            //Console.WriteLine(Global.Music.Volume + Global.MusicVolume);
            SetMenuEntryText();

        }
        // Event handler for when the user selects ok on the "are you sure
        // you want to quit" message box. This uses the loading screen to
        // transition from the game back to the main menu screen.
        void ConfirmQuitMessageBoxAccepted(object sender, EventArgs e)
        {
           
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),new MainMenuScreen());
            //LoadingScreen.Load(ScreenManager, false, null, new MainMenuScreen());
            //ScreenManager.Game.Exit();
            //playing the sound
            SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
            ISound Accept;
            Accept = SoundEngine.Play2D(GoBack, false, true, false);
            Accept.Volume = Global.SpeakerVolume / 10;
            Accept.Paused = false;
            Global.MusicEngine.StopAllSounds();
        }


        void SetMenuEntryText()
        {
            _fullscreenMenuEntry.Text = "Fullscreen: " + (Global.GraphicsManager.IsFullScreen ? "on" : "off");
            _frameCounterMenuEntry.Text = "FPS Counter: " + (Global.FrameCounterIsEnabled ? "on" : "off");

            _effectVolumeMenuEntry.Text = "Effect Volume: " + (Global.SpeakerVolume);
            _musicVolumeMenuEntry.Text = "Music Volume: " + (Global.MusicVolume);
        }

    }
}
