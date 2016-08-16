using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.ScreenManagers;

namespace SpaceAssault.Screens
{
    // Base class for screens that contain a menu of options. The user can
    // move up and down to select an entry, or cancel to back out of the screen.
    abstract class MenuScreen : GameScreen
    {
        List<MenuEntry> menuEntries = new List<MenuEntry>();
        int selectedEntry = 0;
        string menuTitle;


        // Gets the list of menu entries, so derived classes can add
        // or change the menu contents.
        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        // Constructor.
        public MenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        // Responds to user input, changing the selected entry and accepting
        // or cancelling the menu.
        public override void HandleInput(InputState input)
        {

            // mouse click on menu?
            if (input.IsLeftMouseButtonNewPressed())
            {
                Vector2 cornerA;
                Vector2 cornerD;
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    //calculating 2 diagonal corners of current menuEntry (upper left, bottom right)
                    cornerA = menuEntries[i].Position;
                    cornerA.Y -= menuEntries[i].GetHeight() / 2f;

                    cornerD = menuEntries[i].Position;
                    cornerD.Y += menuEntries[i].GetHeight() / 2f;
                    cornerD.X += menuEntries[i].GetWidth();

                    if (cornerA.X < input.MousePosition.X && cornerA.Y < input.MousePosition.Z)
                    {
                        if (cornerD.X > input.MousePosition.X && cornerD.Y > input.MousePosition.Z)
                        {

                            // menuEntry needs a double click
                            /*
                            if (selectedEntry == i)
                            {
                                OnSelectEntry(selectedEntry);
                            }
                            else selectedEntry = i;
                            */

                            // menuEntry needs one click
                            selectedEntry = i;
                            OnSelectEntry(selectedEntry);
                        }
                    }
                    else continue;

                }
            }

            // Move to the previous menu entry?
            if (input.IsMenuUp())
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (input.IsMenuDown())
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }

            // Accept or cancel the menu.
            if (input.IsMenuSelect())
            {
                OnSelectEntry(selectedEntry);
            }
            else if (input.IsMenuCancel())
            {
                OnCancel();
            }
        }


        // Handler for when the user has chosen a menu entry.
        protected virtual void OnSelectEntry(int entryIndex)
        {
            menuEntries[entryIndex].OnSelectEntry();
        }


        // Notifies derived classes that the menu has been cancelled.
        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        // Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        protected virtual void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }


        // Allows the screen the chance to position the menu entries. By default
        // all menu entries are lined up in a vertical list, centered on the screen.
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            Vector2 position = new Vector2(0f, 175f);

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                // each entry is to be centered horizontally
                position.X = Global.GraphicsManager.GraphicsDevice.Viewport.Width / 10;
                //position.X = ScreenManager.GraphicsDevice.Viewport.Width / 5 - menuEntries[i].GetWidth() / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntries[i].Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntries[i].GetHeight();
            }
        }


        // Updates the menu.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }


        // Draws the menu.
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            Global.SpriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title on the screen
            Vector2 titlePosition = new Vector2(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2, 80);
            Vector2 titleOrigin = Global.Font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            Global.SpriteBatch.DrawString(Global.Font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            Global.SpriteBatch.End();
        }
    }
}
