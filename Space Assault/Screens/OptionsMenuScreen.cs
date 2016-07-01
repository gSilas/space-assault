using System;

namespace SpaceAssault.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    class OptionsMenuScreen : MenuScreen
    {
        MenuEntry enumMenuEntry;
        MenuEntry languageMenuEntry;
        MenuEntry fullscreenMenuEntry;
        MenuEntry staticNumberMenuEntry;

        enum enumMenu
        {
            enum1,
            enum2,
            enum3,
        }

        static enumMenu currentEnum = enumMenu.enum1;
        static string[] languages = { "English", "Deutsch"};
        static int currentLanguage = 0;
        static bool fullscreen = true;
        static int num = 42;

        // Constructor.
        public OptionsMenuScreen() : base("Options")
        {
            // Create our menu entries.
            enumMenuEntry = new MenuEntry(string.Empty);
            languageMenuEntry = new MenuEntry(string.Empty);
            fullscreenMenuEntry = new MenuEntry(string.Empty);
            staticNumberMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();
            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            enumMenuEntry.Selected += enumMenuEntrySelected;
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            fullscreenMenuEntry.Selected += fullscreenMenuEntrySelected;
            staticNumberMenuEntry.Selected += staticNumberMenuEntrySelected;
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(enumMenuEntry);
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(fullscreenMenuEntry);
            MenuEntries.Add(staticNumberMenuEntry);
            MenuEntries.Add(back);
        }


        // Fills in the latest values for the options screen menu text.
        void SetMenuEntryText()
        {
            enumMenuEntry.Text = "Preferred enum: " + currentEnum;
            languageMenuEntry.Text = "Language: " + languages[currentLanguage];
            fullscreenMenuEntry.Text = "Fullscreen: " + (fullscreen ? "on" : "off");
            staticNumberMenuEntry.Text = "Number: " + num;
        }

        // Event handler for when the Ungulate menu entry is selected.
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


        // Event handler for when the Frobnicate menu entry is selected.
        void fullscreenMenuEntrySelected(object sender, EventArgs e)
        {
            fullscreen = !fullscreen;

            SetMenuEntryText();
        }


        // Event handler for when the Elf menu entry is selected.
        void staticNumberMenuEntrySelected(object sender, EventArgs e)
        {
            num++;

            SetMenuEntryText();
        }
    }
}
