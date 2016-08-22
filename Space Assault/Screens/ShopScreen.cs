using System;
using SpaceAssault.Entities;
using SpaceAssault.Utils;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceAssault.Screens.UI;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.ScreenManagers;

namespace SpaceAssault.Screens
{
    class ShopScreen : MenuScreen
    {

        //#################################
        // Variables
        //#################################
        //Drone
        MenuEntry damageMenuEntry;
        MenuEntry healthMenuEntry;
        MenuEntry armorMenuEntry;
        MenuEntry shieldMenuEntry;
        //Station
        MenuEntry sShieldMenuEntry;
        MenuEntry sHealthMenuEntry;
        MenuEntry stationlaserMenuEntry;

        List<Label> Labels = new List<Label>();
        public static int _droneDamageLevel = 1;
        public static int _droneHealthLevel = 1;
        public static int _droneArmorLevel = 1;
        public static int _droneShieldLevel = 1;
        public static int _StationHealthLevel = 1;
        public static int _StationShieldLevel = 1;
        public static int _StationLaserLevel = 0;

        private DroneBuilder _droneFleet;
        private Station _station;

        private Dialog _itemDialog;

        //#################################
        // Constructor
        //#################################
        public ShopScreen(DroneBuilder droneFleet,Station station)
            : base("Shop")
        {
            this._droneFleet = droneFleet;
            this._station = station;

            // Create our menu entries.
            damageMenuEntry = new MenuEntry(string.Empty);
            healthMenuEntry = new MenuEntry(string.Empty);
            armorMenuEntry = new MenuEntry(string.Empty);
            shieldMenuEntry=new MenuEntry(string.Empty);

            sHealthMenuEntry=new MenuEntry(string.Empty);
            sShieldMenuEntry=new MenuEntry(string.Empty);
            stationlaserMenuEntry=new MenuEntry(String.Empty);
            

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Close Shop");

            // Hook up menu event handlers.
            damageMenuEntry.Selected += damageMenuEntrySelected;
            healthMenuEntry.Selected += healthMenuEntrySelected;
            armorMenuEntry.Selected += armorMenuEntrySelected;
            shieldMenuEntry.Selected += shieldMenuEntrySelected;

            sHealthMenuEntry.Selected += sHealthMenuEntrySelected;
            sShieldMenuEntry.Selected += sShieldMenuEntrySelected;
            stationlaserMenuEntry.Selected += stationlaserhMenuEntrySelected;

            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(damageMenuEntry);
            MenuEntries.Add(healthMenuEntry);
            MenuEntries.Add(armorMenuEntry);
            MenuEntries.Add(shieldMenuEntry);

            MenuEntries.Add(sHealthMenuEntry);
            MenuEntries.Add(sShieldMenuEntry);
            MenuEntries.Add(stationlaserMenuEntry);
            MenuEntries.Add(back);

            _itemDialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 - 150, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 750, 672, 704, 8, false, false);

            Global.ShopText.Add(0, "New Laser");
            Global.ShopText.Add(1, "New Health");
            Global.ShopText.Add(2, "New Armor");
            Global.ShopText.Add(3, "New Shield");
            Global.ShopText.Add(4, "New Station Health");
            Global.ShopText.Add(5, "New Station Shield");
            Global.ShopText.Add(6, "New Station Laser");
            Global.ShopText.Add(7, "Go Back");
        }


        //#################################
        // LoadContent - Function
        //#################################
        public override void LoadContent()
        {
            //UI
            Labels.Add(new Label("gamefont", "Points: ", 50, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            _itemDialog.LoadContent();
            _frame.LoadContent();
        }

        //#################################
        // MenuItems
        //#################################
        void SetMenuEntryText()
        {
            damageMenuEntry.Text = "Damage Level: " + _droneDamageLevel;
            healthMenuEntry.Text = "Health Level: " + _droneHealthLevel;
            armorMenuEntry.Text = "Armor Level: " + _droneArmorLevel;
            shieldMenuEntry.Text = "Shield Level: " + _droneShieldLevel;
            sHealthMenuEntry.Text = "Station Health Level: " + _StationHealthLevel;
            sShieldMenuEntry.Text = "Station Shield Level: " + _StationShieldLevel;
            stationlaserMenuEntry.Text = "Station Laser Level: " + _StationLaserLevel;
        }

        // Event handler for when the Damage menu entry is selected.
        void damageMenuEntrySelected(object sender, EventArgs e)
        {
            if (this._droneFleet._updatePoints > 0)
            {
                _droneDamageLevel++;
                this._droneFleet._makeDmg += 10;
                this._droneFleet._updatePoints--;
                SetMenuEntryText();
            }
        }
        // Event handler for when the Health menu entry is selected.
        void healthMenuEntrySelected(object sender, EventArgs e)
        {
            if (this._droneFleet._updatePoints > 0)
            {
                _droneHealthLevel++;
                this._droneFleet._maxHealth += 100;
                this._droneFleet._updatePoints--;
                SetMenuEntryText();
            }
        }

        void armorMenuEntrySelected(object sender, EventArgs e)
        {
            if (this._droneFleet._updatePoints > 0)
            {
                _droneArmorLevel++;
                this._droneFleet._armor+=1;
                this._droneFleet._updatePoints--;
                SetMenuEntryText();
            }
        }
        void shieldMenuEntrySelected(object sender, EventArgs e)
        {
            if (this._droneFleet._updatePoints > 0)
            {
                _droneShieldLevel++;
                this._droneFleet._maxShield+=50;   
                this._droneFleet._updatePoints--;
                SetMenuEntryText();
            }
        }
        void sShieldMenuEntrySelected(object sender, EventArgs e)
        {
            if (this._droneFleet._updatePoints > 0)
            {
                _StationShieldLevel++;
                this._station._maxShield += 500;
                this._droneFleet._updatePoints--;
                SetMenuEntryText();
            }
        }
        void sHealthMenuEntrySelected(object sender, EventArgs e)
        {
            if (this._droneFleet._updatePoints > 0)
            {
                _StationHealthLevel++;
                this._station._health += 1000;
                this._droneFleet._updatePoints--;
                SetMenuEntryText();
            }
        }
        void stationlaserhMenuEntrySelected(object sender, EventArgs e)
        {
            if (this._droneFleet._updatePoints > 1)
            {
                _StationLaserLevel++;
                this._station.makeDmg += 50;       
                this._droneFleet._updatePoints-=2;
                SetMenuEntryText();
            }
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
                    selectedEntry = MenuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (input.IsMenuDown())
            {
                selectedEntry++;

                if (selectedEntry >= MenuEntries.Count)
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

        //#################################
        // Draw
        //#################################
        public override void Draw(GameTime gameTime)
        {

            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                MenuEntry menuEntry = MenuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);
                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title on the screen
            Vector2 titlePosition = new Vector2(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2, 80);
            Vector2 titleOrigin = Global.GameFont.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;


            Global.SpriteBatch.DrawString(Global.GameFont, menuTitle, titlePosition, titleColor, 0,titleOrigin, titleScale, SpriteEffects.None, 0);

            Global.SpriteBatch.End();
            Global.SpriteBatch.Begin();

            Labels[0].Draw(this._droneFleet._updatePoints);

            string entry;
            Global.ShopText.TryGetValue(selectedEntry, out entry);
            _itemDialog.Draw(entry);

            _frame.Draw(false);
        }

    }
}

