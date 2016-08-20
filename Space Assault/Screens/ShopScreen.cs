using System;
using SpaceAssault.Entities;
using SpaceAssault.Utils;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;

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
        }


        //#################################
        // LoadContent - Function
        //#################################
        public override void LoadContent()
        {
            //UI
            Labels.Add(new Label("gamefont", "Points: ", 50, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
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
            if (this._droneFleet._updatePoints > 0)
            {
                _StationLaserLevel++;
                //this._station._health += 1000;       
                this._droneFleet._updatePoints--;
                SetMenuEntryText();
            }
        }

        //#################################
        // Draw
        //#################################
        public void Draw()
        {
            Labels[0].Draw(this._droneFleet._updatePoints);
        }

    }
}

