using System;

namespace SpaceAssault.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    class OptionsMenuScreen : MenuScreen
    {
        //MenuEntry enumMenuEntry;
        //MenuEntry languageMenuEntry;
        //MenuEntry staticNumberMenuEntry;
        MenuEntry fullscreenMenuEntry;
        MenuEntry frameCounterMenuEntry;

        enum enumMenu
        {
            enum1,
            enum2,
            enum3,
        }

        //static enumMenu currentEnum = enumMenu.enum1;
        //static string[] languages = { "English", "Deutsch" };
        //static int currentLanguage = 0;
        //static int num = 42;

        // Constructor.
        public OptionsMenuScreen() : base("Options")
        {
            // Create our menu entries.me
            //enumMenuEntry = new MenuEntry(string.Empty);
            //languageMenuEntry = new MenuEntry(string.Empty);
            //staticNumberMenuEntry = new MenuEntry(string.Empty);
            fullscreenMenuEntry = new MenuEntry(string.Empty);
            frameCounterMenuEntry = new MenuEntry(string.Empty);
            SetMenuEntryText();
            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            //enumMenuEntry.Selected += enumMenuEntrySelected;
            //languageMenuEntry.Selected += LanguageMenuEntrySelected;
            //staticNumberMenuEntry.Selected += staticNumberMenuEntrySelected;
            fullscreenMenuEntry.Selected += fullscreenMenuEntrySelected;
            frameCounterMenuEntry.Selected += frameCounterMenuEntrySelected;
            back.Selected += OnCancel;

            // Add entries to the menu.
            //MenuEntries.Add(enumMenuEntry);
            //MenuEntries.Add(languageMenuEntry);
            //MenuEntries.Add(staticNumberMenuEntry);
            MenuEntries.Add(fullscreenMenuEntry);
            MenuEntries.Add(frameCounterMenuEntry);
            MenuEntries.Add(back);
        }


        // Fills in the latest values for the options screen menu text.
        void SetMenuEntryText()
        {
            //enumMenuEntry.Text = "Preferred enum: " + currentEnum;
            //languageMenuEntry.Text = "Language: " + languages[currentLanguage];
            //staticNumberMenuEntry.Text = "Number: " + num;
            fullscreenMenuEntry.Text = "Fullscreen: " + (Global.GraphicsManager.IsFullScreen ? "on" : "off");
            frameCounterMenuEntry.Text = "FPS Counter: " + (Global.FrameCounterIsEnabled ? "on" : "off");
        }

        // Event handler for when the Fullscreen menu entry is selected.
        void fullscreenMenuEntrySelected(object sender, EventArgs e)
        {
            Global.GraphicsManager.ToggleFullScreen();
            SetMenuEntryText();
        }

        void frameCounterMenuEntrySelected(object sender, EventArgs e)
        {
            Global.FrameCounterIsEnabled = !Global.FrameCounterIsEnabled;
            SetMenuEntryText();
        }

        /*
        // Event handler for when the Enum menu entry is selected.
        void enumMenuEntrySelected(object sender, EventArgs e)
        {
            currentEnum++;

            if (currentEnum > enumMenu.enum3)
                currentEnum = 0;

            SetMenuEntryText();
        }


        // Event handler for when the Language menu entry is selected.
        void LanguageMenuEntrySelected(object sender, EventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;
            SetMenuEntryText();
        }

        // Event handler for when the Number menu entry is selected.
        void staticNumberMenuEntrySelected(object sender, EventArgs e)
        {
            num++;
            SetMenuEntryText();
        }
        */
    }
}
