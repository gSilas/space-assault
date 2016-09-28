using System;
using IrrKlang;
using SpaceAssault.Utils;
using SpaceAssault.ScreenManagers;

namespace SpaceAssault.Screens.Demo
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    class DemoPauseMenuScreen : MenuScreen
    {

        // Constructor.
        public DemoPauseMenuScreen() : base("Demo Mode")
        {
            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Demo");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Demo");

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
            
            const string message = "Are you sure you want to exit the demo?";
            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);
            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmQuitMessageBox);
            
            //playing the sound
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);
        }


        // Event handler for when the user selects ok on the "are you sure
        // you want to quit" message box. This uses the loading screen to
        // transition from the game back to the main menu screen.
        void ConfirmQuitMessageBoxAccepted(object sender, EventArgs e)
        {
            //playing the sound
            SoundEngine.Play2D("GoBack", Global.SpeakerVolume / 10, false);
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),new DemoModeMenuScreen());
        }

    }
}
