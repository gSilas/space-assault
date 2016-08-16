using System;
using SpaceAssault.Entities;
using SpaceAssault.Utils;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceAssault.Screens
{
    class ShopScreen : MenuScreen
    {

        //#################################
        // Variables
        //#################################
        MenuEntry damageMenuEntry;
        MenuEntry healthMenuEntry;
        MenuEntry armorMenuEntry;
        MenuEntry shieldMenuEntry;

        List<Label> Labels = new List<Label>();
        public static int _droneDamageLevel = 1;
        public static int _droneHealthLevel = 1;
        public static int _droneArmorLevel = 1;
        public static int _droneShieldLevel = 1;

        private DroneBuilder _droneFleet;

        //#################################
        // Constructor
        //#################################
        public ShopScreen(DroneBuilder droneFleet)
            : base("Shop")
        {
            this._droneFleet = droneFleet;

            // Create our menu entries.
            damageMenuEntry = new MenuEntry(string.Empty);
            healthMenuEntry = new MenuEntry(string.Empty);
            armorMenuEntry = new MenuEntry(string.Empty);
            shieldMenuEntry=new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Close Shop");

            // Hook up menu event handlers.
            damageMenuEntry.Selected += damageMenuEntrySelected;
            healthMenuEntry.Selected += healthMenuEntrySelected;
            armorMenuEntry.Selected += armorMenuEntrySelected;
            shieldMenuEntry.Selected += shieldMenuEntrySelected;

            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(damageMenuEntry);
            MenuEntries.Add(healthMenuEntry);
            MenuEntries.Add(armorMenuEntry);
            MenuEntries.Add(shieldMenuEntry);
            MenuEntries.Add(back);
        }


        //#################################
        // LoadContent - Function
        //#################################
        public override void LoadContent()
        {
            //UI
            Labels.Add(new Label("gamefont", "Points: ", 50, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
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
                _droneArmorLevel++;
                this._droneFleet._maxShield+=50;
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

