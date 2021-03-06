using System;
using SpaceAssault.Utils;
using SpaceAssault.ScreenManagers;
using SpaceAssault.Screens.Demo;

namespace SpaceAssault.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    class MainMenuScreen : MenuScreen
    {
        // Constructor fills in the menu contents.
        public MainMenuScreen() : base("Space Assault")
        {

            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Start Game");
            MenuEntry highscoreMenuEntry = new MenuEntry("Station Records");
            MenuEntry optionsMenuEntry = new MenuEntry("Station Configuration");
            MenuEntry creditsMenuEntry = new MenuEntry("Honorable Mentions");
            MenuEntry tutorialMenuEntry = new MenuEntry("Begin Your Mission");
            MenuEntry demoMenuEntry = new MenuEntry("Demonstration Mode");
            MenuEntry exitMenuEntry = new MenuEntry("Quit Game");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            highscoreMenuEntry.Selected += HighscoreMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            creditsMenuEntry.Selected += CreditsMenuEntrySelected;
            tutorialMenuEntry.Selected += TutorialMenuEntrySelected;
            demoMenuEntry.Selected += DemoMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.

            //MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(tutorialMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(highscoreMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(demoMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }


        // Event handler for when the Credits menu entry is selected.
        void DemoMenuEntrySelected(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);

            ScreenManager.AddScreen(new DemoModeMenuScreen());
        }


        // Event handler for when the Play Game menu entry is selected.
        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);

        }

        // Event handler for when the Options menu entry is selected.
        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);

            ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        // Event handler for when the Highscore menu entry is selected.
        void HighscoreMenuEntrySelected(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);

            ScreenManager.AddScreen(new HighscoreMenuScreenOnline(false));
        }

        // Event handler for when the Credits menu entry is selected.
        void CreditsMenuEntrySelected(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);

            ScreenManager.AddScreen(new CreditsMenuScreen());
        }


        // When the user cancels the main menu, ask if they want to exit the sample.
        protected override void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

        protected override void OnCancel()
        {
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);

            const string message = "Are you sure you want to exit?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox);
        }

        void TutorialMenuEntrySelected(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);

            LoadingScreen.Load(ScreenManager, true, new TutorialScreen());
        }

        // Event handler for when the user selects ok on the "are you sure
        // you want to exit" message box.
        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);
            ScreenManager.Game.Exit();
        }
    }
}