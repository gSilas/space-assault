using System;
using SpaceAssault.Utils;
using SpaceAssault.ScreenManagers;
using SpaceAssault.Screens.Demo;

namespace SpaceAssault.Screens.Demo
{
    // The main menu screen is the first thing displayed when the game starts up.
    class DemoModeMenuScreen : MenuScreen
    {
        // Constructor fills in the menu contents.
        public DemoModeMenuScreen() : base("Demo Mode")
        {

            // Create our menu entries.
            MenuEntry modelsMenuEntry = new MenuEntry("Models & Boids");
            MenuEntry effectsMenuEntry = new MenuEntry("Particles");
            MenuEntry gameplayMenuEntry = new MenuEntry("Gameplay");
            MenuEntry back = new OptionMenuEntry("Back");

            // Hook up menu event handlers.
            modelsMenuEntry.Selected += ModelMenuEntrySelected;
            effectsMenuEntry.Selected += EffectsMenuEntrySelected;
            gameplayMenuEntry.Selected += GameplayMenuEntrySelected;
            back.Selected += OnCancel;

            // Add entries to the menu.

            MenuEntries.Add(modelsMenuEntry);
            MenuEntries.Add(effectsMenuEntry);
            MenuEntries.Add(gameplayMenuEntry);
            MenuEntries.Add(back);
        }

        // Event handler for when the Models Game menu entry is selected.
        void ModelMenuEntrySelected(object sender, EventArgs e)
        {
            Global.DemoMode = "Models";
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);
            LoadingScreen.Load(ScreenManager, true, new DemoGameplayScreen());

        }

        // Event handler for when the Effects menu entry is selected.
        void EffectsMenuEntrySelected(object sender, EventArgs e)
        {
            Global.DemoMode = "Effects";
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);
            LoadingScreen.Load(ScreenManager, true, new DemoGameplayScreen());
        }

        // Event handler for when the Gameplay menu entry is selected.
        void GameplayMenuEntrySelected(object sender, EventArgs e)
        {
            Global.DemoMode = "Gameplay";
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);
            LoadingScreen.Load(ScreenManager, true, new DemoTutorialScreen());
        }

        // When the user cancels the main menu, ask if they want to exit the sample.
        protected override void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

        protected override void OnCancel()
        {
            //playing the sound
            SoundEngine.Play2D("OkClick", Global.SpeakerVolume / 10, false);
            ScreenManager.AddScreen(new MainMenuScreen());
        }
    }
}