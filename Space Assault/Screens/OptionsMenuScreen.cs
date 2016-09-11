using System;
using System.Collections.Generic;
using System.Linq;
using IrrKlang;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceAssault.Utils;
using SpaceAssault.ScreenManagers;

namespace SpaceAssault.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    class OptionsMenuScreen : MenuScreen
    {
        OptionMenuEntry fullscreenMenuEntry;
        OptionMenuEntry frameCounterMenuEntry;
        OptionMenuEntry effectVolumeMenuEntry;
        OptionMenuEntry musicVolumeMenuEntry;
        OptionMenuEntry uiColorRMenuEntry;
        OptionMenuEntry uiColorGMenuEntry;
        OptionMenuEntry uiColorBMenuEntry;
        OptionMenuEntry back;

        List<OptionMenuEntry> optionMenuEntries = new List<OptionMenuEntry>();
        protected int lastSelectedEntry = 0;

        // Constructor.
        public OptionsMenuScreen() : base("Options")
        {

            fullscreenMenuEntry = new OptionMenuEntry(string.Empty);
            frameCounterMenuEntry = new OptionMenuEntry(string.Empty);
            effectVolumeMenuEntry = new OptionMenuEntry(string.Empty);
            musicVolumeMenuEntry = new OptionMenuEntry(string.Empty);
            uiColorRMenuEntry = new OptionMenuEntry(string.Empty);
            uiColorGMenuEntry = new OptionMenuEntry(string.Empty);
            uiColorBMenuEntry = new OptionMenuEntry(string.Empty);
            back = new OptionMenuEntry("Back");

            SetMenuEntryText();

            // Hook up menu event handlers.
            fullscreenMenuEntry.SelectedIncrease += fullscreenMenuEntrySelected;
            frameCounterMenuEntry.SelectedIncrease += frameCounterMenuEntrySelected;
            effectVolumeMenuEntry.SelectedIncrease += effectVolumeMenuEntrySelectedIncrease;
            musicVolumeMenuEntry.SelectedIncrease += musicVolumeMenuEntrySelectedIncrease;
            uiColorRMenuEntry.SelectedIncrease += uiColorRMenuEntrySelectedIncrease;
            uiColorGMenuEntry.SelectedIncrease += uiColorGMenuEntrySelectedIncrease;
            uiColorBMenuEntry.SelectedIncrease += uiColorBMenuEntrySelectedIncrease;

            fullscreenMenuEntry.SelectedDecrease += fullscreenMenuEntrySelected;
            frameCounterMenuEntry.SelectedDecrease += frameCounterMenuEntrySelected;
            effectVolumeMenuEntry.SelectedDecrease += effectVolumeMenuEntrySelectedDecrease;
            musicVolumeMenuEntry.SelectedDecrease += musicVolumeMenuEntrySelectedDecrease;
            uiColorRMenuEntry.SelectedDecrease += uiColorRMenuEntrySelectedDecrease;
            uiColorGMenuEntry.SelectedDecrease += uiColorGMenuEntrySelectedDecrease;
            uiColorBMenuEntry.SelectedDecrease += uiColorBMenuEntrySelectedDecrease;

            back.Selected += OnCancel;

            // Add entries to the menu.
            optionMenuEntries.Add(fullscreenMenuEntry);
            optionMenuEntries.Add(frameCounterMenuEntry);
            optionMenuEntries.Add(musicVolumeMenuEntry);
            optionMenuEntries.Add(effectVolumeMenuEntry);

            optionMenuEntries.Add(uiColorRMenuEntry);
            optionMenuEntries.Add(uiColorGMenuEntry);
            optionMenuEntries.Add(uiColorBMenuEntry);
            optionMenuEntries.Add(back);
            menuEntries = optionMenuEntries.Cast<MenuEntry>().ToList();
        }


        // Fills in the latest values for the options screen menu text.
        void SetMenuEntryText()
        {
            fullscreenMenuEntry.Text = "Fullscreen: " + (Global.GraphicsManager.IsFullScreen ? "on" : "off");
            frameCounterMenuEntry.Text = "FPS Counter: " + (Global.FrameCounterIsEnabled ? "on" : "off");
            effectVolumeMenuEntry.Text = "Effect Volume: " + (Global.SpeakerVolume);
            musicVolumeMenuEntry.Text = "Music Volume: " + (Global.MusicVolume);
            uiColorRMenuEntry.Text = "Color R: " + (Global.UIColor.R);
            uiColorGMenuEntry.Text = "Color G: " + (Global.UIColor.G);
            uiColorBMenuEntry.Text = "Color B: " + (Global.UIColor.B);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            lastSelectedEntry = selectedEntry;
        }

        public override void HandleInput(InputState input)
        {
            // mouse click on menu?
            if (input.IsLeftMouseButtonNewPressed())
            {
                Vector2 cornerA;
                Vector2 cornerD;
                for (int i = 0; i < MenuEntries.Count; i++)
                {
                    //calculating 2 diagonal corners of current menuEntry (upper left, bottom right)
                    cornerA = MenuEntries[i].Position;
                    cornerA.Y -= MenuEntries[i].GetHeight() / 2f;

                    cornerD = MenuEntries[i].Position;
                    cornerD.Y += MenuEntries[i].GetHeight() / 2f;
                    cornerD.X += MenuEntries[i].GetWidth();

                    if (cornerA.X < input.MousePosition.X && cornerA.Y < input.MousePosition.Z)
                    {
                        if (cornerD.X > input.MousePosition.X && cornerD.Y > input.MousePosition.Z)
                        {
                            // menuEntry needs one click
                            selectedEntry = i;
                            if(optionMenuEntries[selectedEntry].Equals(back)) OnSelectEntry(selectedEntry);
                        }
                    }
                    else continue;

                }
            }

            // Move to the previous menu entry?
            if (input.IsMenuUp())
            {
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
                ISound Accept;
                Accept = SoundEngine.Play2D(MenuAcceptSound, false, true, false);
                Accept.Volume = Global.SpeakerVolume / 10;
                Accept.Paused = false;

                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = MenuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (input.IsMenuDown())
            {
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
                ISound Accept;
                Accept = SoundEngine.Play2D(MenuAcceptSound, false, true, false);
                Accept.Volume = Global.SpeakerVolume / 10;
                Accept.Paused = false;

                selectedEntry++;

                if (selectedEntry >= MenuEntries.Count)
                    selectedEntry = 0;
            }

            //cancel the menu.
            if (input.IsMenuSelect())
            {
                OnSelectEntry(selectedEntry);
            }
            else if (input.IsMenuCancel())
            {
                OnCancel();
            }

            // checks for increase or decrease
            if (lastSelectedEntry == selectedEntry)
            {
                if (input.IsNewKeyPress(Keys.Left) || input.IsLeftMouseButtonNewPressed())
                {
                    OnSelectEntryDecrease(selectedEntry);
                }

                if (input.IsNewKeyPress(Keys.Right) || input.IsRightMouseButtonNewPressed())
                {
                    OnSelectEntryIncrease(selectedEntry);
                }
            }
        }
        // Handler for when the user increases menu entry.
        protected virtual void OnSelectEntryIncrease(int entryIndex)
        {
            optionMenuEntries[entryIndex].OnSelectEntryIncrease();
        }

        // Handler for when the user decreases menu entry.
        protected virtual void OnSelectEntryDecrease(int entryIndex)
        {
            optionMenuEntries[entryIndex].OnSelectEntryDecrease();
        }

        /// <summary>
        /// Event Handlers for the menu entries
        /// </summary>
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

        void effectVolumeMenuEntrySelectedIncrease(object sender, EventArgs e)
        {
            if (Global.SpeakerVolume < 10) Global.SpeakerVolume++;
            SetMenuEntryText();
        }

        void effectVolumeMenuEntrySelectedDecrease(object sender, EventArgs e)
        {
            if (Global.SpeakerVolume > 0) Global.SpeakerVolume--;
            SetMenuEntryText();
        }

        void musicVolumeMenuEntrySelectedIncrease(object sender, EventArgs e)
        {
            if (Global.MusicVolume < 10) Global.MusicVolume++;
            Global.Music.Volume = Global.MusicVolume / 10;
            SetMenuEntryText();
        }

        void musicVolumeMenuEntrySelectedDecrease(object sender, EventArgs e)
        {
            if (Global.MusicVolume >0) Global.MusicVolume--;
            Global.Music.Volume = Global.MusicVolume / 10;
            SetMenuEntryText();
        }

        void uiColorRMenuEntrySelectedIncrease(object sender, EventArgs e)
        {
            Global.UIColor.R++;
            SetMenuEntryText();
        }

        void uiColorRMenuEntrySelectedDecrease(object sender, EventArgs e)
        {
            Global.UIColor.R--;
            SetMenuEntryText();
        }

        void uiColorGMenuEntrySelectedIncrease(object sender, EventArgs e)
        {
            Global.UIColor.G++;
            SetMenuEntryText();
        }

        void uiColorGMenuEntrySelectedDecrease(object sender, EventArgs e)
        {
            Global.UIColor.G--;
            SetMenuEntryText();
        }

        void uiColorBMenuEntrySelectedIncrease(object sender, EventArgs e)
        {
            Global.UIColor.B++;
            SetMenuEntryText();
        }

        void uiColorBMenuEntrySelectedDecrease(object sender, EventArgs e)
        {
            Global.UIColor.B--;
            SetMenuEntryText();
        }

    }
}
