using Microsoft.Xna.Framework;
using System;
using System.Runtime.CompilerServices;

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
        MenuEntry effectVolumeMenuEntry;
        MenuEntry musicVolumeMenuEntry;
        MenuEntry uiColorRMenuEntry;
        MenuEntry uiColorGMenuEntry;
        MenuEntry uiColorBMenuEntry;

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
            effectVolumeMenuEntry = new MenuEntry(string.Empty);
            musicVolumeMenuEntry = new MenuEntry(string.Empty);
            uiColorRMenuEntry = new MenuEntry(string.Empty);
            uiColorGMenuEntry = new MenuEntry(string.Empty);
            uiColorBMenuEntry = new MenuEntry(string.Empty);
            SetMenuEntryText();
            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            //enumMenuEntry.Selected += enumMenuEntrySelected;
            //languageMenuEntry.Selected += LanguageMenuEntrySelected;
            //staticNumberMenuEntry.Selected += staticNumberMenuEntrySelected;
            fullscreenMenuEntry.Selected += fullscreenMenuEntrySelected;
            frameCounterMenuEntry.Selected += frameCounterMenuEntrySelected;
            effectVolumeMenuEntry.Selected += effectVolumeMenuEntrySelected;
            musicVolumeMenuEntry.Selected += musicVolumeMenuEntrySelected;
            
            uiColorRMenuEntry.Selected += uiColorRMenuEntrySelected;
            uiColorGMenuEntry.Selected += uiColorGMenuEntrySelected;
            uiColorBMenuEntry.Selected += uiColorBMenuEntrySelected;
            back.Selected += OnCancel;
            uiColorRMenuEntry.IsIncreasingSelect = true;
            uiColorGMenuEntry.IsIncreasingSelect = true;
            uiColorBMenuEntry.IsIncreasingSelect = true;
            // Add entries to the menu.
            //MenuEntries.Add(enumMenuEntry);
            //MenuEntries.Add(languageMenuEntry);
            //MenuEntries.Add(staticNumberMenuEntry);
            MenuEntries.Add(fullscreenMenuEntry);
            MenuEntries.Add(frameCounterMenuEntry);
            MenuEntries.Add(musicVolumeMenuEntry);
            MenuEntries.Add(effectVolumeMenuEntry);
          
            MenuEntries.Add(uiColorRMenuEntry);
            MenuEntries.Add(uiColorGMenuEntry);
            MenuEntries.Add(uiColorBMenuEntry);
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
            effectVolumeMenuEntry.Text = "Effect Volume: " + (Global.SpeakerVolume);
            musicVolumeMenuEntry.Text = "Music Volume: " + (Global.MusicVolume);
            uiColorRMenuEntry.Text = "Color R: " + (Global.UIColor.R);
            uiColorGMenuEntry.Text = "Color G: " + (Global.UIColor.G);
            uiColorBMenuEntry.Text = "Color B: " + (Global.UIColor.B);

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

        void effectVolumeMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.SpeakerVolume == 10)
                Global.SpeakerVolume = 0;
            else
                Global.SpeakerVolume += 1;
            
            



            SetMenuEntryText();
        }

        void musicVolumeMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.MusicVolume == 10)
                Global.MusicVolume = 0;
            else
                Global.MusicVolume += 1;

            Global.Music.Volume = Global.MusicVolume/10;
            SetMenuEntryText();

        }


        void uiColorRMenuEntrySelected(object sender, EventArgs e)
        {
            Global.UIColor.R+=1;
            SetMenuEntryText();
        }
        void uiColorGMenuEntrySelected(object sender, EventArgs e)
        {
            Global.UIColor.G+=1;
            SetMenuEntryText();
        }
        void uiColorBMenuEntrySelected(object sender, EventArgs e)
        {
            Global.UIColor.B+=1;
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
