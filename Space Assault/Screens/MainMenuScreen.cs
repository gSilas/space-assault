using System;
using IrrKlang;

namespace SpaceAssault.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    class MainMenuScreen : MenuScreen
    {
        // Constructor fills in the menu contents.
        public MainMenuScreen() : base("Dronecontrol")
        {

            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Current Mission");
            MenuEntry highscoreMenuEntry = new MenuEntry("Personal Record");
            MenuEntry optionsMenuEntry = new MenuEntry("Station Configuration");
            MenuEntry creditsMenuEntry = new MenuEntry("Honorable Mentions");
            MenuEntry exitMenuEntry = new MenuEntry("Quit Dronecontrol");
            MenuEntry tutorialMenuEntry = new MenuEntry("Tutorial");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            highscoreMenuEntry.Selected += HighscoreMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            creditsMenuEntry.Selected += CreditsMenuEntrySelected;
            tutorialMenuEntry.Selected += TutorialMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(tutorialMenuEntry);
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(highscoreMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        // Event handler for when the Play Game menu entry is selected.
        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.SetListenerPosition(new Vector3D(0,0,0), new Vector3D(0, 0, 1));
            ISound Accept;
            Accept = SoundEngine.Play3D(OkClick, 0, 0 + 15f, 0, false, true, false);
            Accept.Volume = 1f;
            Accept.Paused = false;

            LoadingScreen.Load(ScreenManager, true, new GameplayScreen());
        }

        // Event handler for when the Options menu entry is selected.
        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
            ISound Accept;
            Accept = SoundEngine.Play3D(OkClick, 0, 0 + 15f, 0, false, true, false);
            Accept.Volume = 1f;
            Accept.Paused = false;

            ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        // Event handler for when the Highscore menu entry is selected.
        void HighscoreMenuEntrySelected(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
            ISound Accept;
            Accept = SoundEngine.Play3D(OkClick, 0, 0 + 15f, 0, false, true, false);
            Accept.Volume = 1f;
            Accept.Paused = false;

            ScreenManager.AddScreen(new HighscoreMenuScreen());
        }

        // Event handler for when the Credits menu entry is selected.
        void CreditsMenuEntrySelected(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
            ISound Accept;
            Accept = SoundEngine.Play3D(OkClick, 0, 0 + 15f, 0, false, true, false);
            Accept.Volume = 1f;
            Accept.Paused = false;

            ScreenManager.AddScreen(new CreditsMenuScreen());
        }


        // When the user cancels the main menu, ask if they want to exit the sample.
        protected override void OnCancel(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
            ISound Accept;
            Accept = SoundEngine.Play3D(OkClick, 0, 0 + 15f, 0, false, true, false);
            Accept.Volume = 0.5f;
            Accept.Paused = false;

            const string message = "Are you sure you want to exit?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox);
        }

        void TutorialMenuEntrySelected(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
            ISound Accept;
            Accept = SoundEngine.Play3D(OkClick, 0, 0 + 15f, 0, false, true, false);
            Accept.Volume = 0.5f;
            Accept.Paused = false;

            LoadingScreen.Load(ScreenManager, true, new TutorialScreen());
        }

        // Event handler for when the user selects ok on the "are you sure
        // you want to exit" message box.
        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
            ISound Denie;
            Denie = SoundEngine.Play3D(OkClick, 0, 0 + 15f, 0, false, true, false);
            Denie.Volume = 0.5f;
            Denie.Paused = false;
            ScreenManager.Game.Exit();
        }
    }
}