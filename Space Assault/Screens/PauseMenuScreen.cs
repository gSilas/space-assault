using System;
using IrrKlang;

namespace SpaceAssault.Screens
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    class PauseMenuScreen : MenuScreen
    {
        // Constructor.
        public PauseMenuScreen()
            : base("Interrupted")
        {


            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Mission");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Station");

            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
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
            Accept = SoundEngine.Play3D(OkClick, 0, 0 + 15f, 0, false, true, false);
            Accept.Volume = Global.SpeakerVolume / 10;
            Accept.Paused = false;
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
            Accept = SoundEngine.Play3D(GoBack, 0, 0 + 15f, 0, false, true, false);
            Accept.Volume = Global.SpeakerVolume / 10;
            Accept.Paused = false;
        }

    }
}
