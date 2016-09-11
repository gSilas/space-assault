using System;
using IrrKlang;
using SpaceAssault.Utils;
using SpaceAssault.ScreenManagers;

namespace SpaceAssault.Screens
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    class PauseMenuScreen : MenuScreen
    {

        // Constructor.
        public PauseMenuScreen() : base("Interrupted")
        {
            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Mission");
            MenuEntry optionsMenuEntry = new MenuEntry("Station Configuration");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Station");

            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
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
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);
        }

        // Event handler for when the Options menu entry is selected.
        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);

            ScreenManager.AddScreen(new OptionsMenuScreen());
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
            SoundEngine.Play2D("GoBack", Global.SpeakerVolume / 10, false);
        }

    }
}
