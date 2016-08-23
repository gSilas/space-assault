﻿using System;
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
        private int _priceDMG=1000;
        MenuEntry healthMenuEntry;
        public static int _priceDroneHealth=500;
        MenuEntry armorMenuEntry;
        private int _priceArmor=2000;
        MenuEntry shieldMenuEntry;
        private int _priceDroneShield=500;
        //Station
        MenuEntry sShieldMenuEntry;
        private int _priceStationShield=3000;
        MenuEntry sHealthMenuEntry;
        private int _priceStationHealth = 5000;
        MenuEntry stationlaserMenuEntry;


        List<Label> Labels = new List<Label>();
        public static int _droneDamageLevel = 1;
        public static int _droneHealthLevel = 1;
        public static int _droneArmorLevel = 1;
        public static int _droneShieldLevel = 1;
        public static int _StationHealthLevel = 1;
        public static int _StationShieldLevel = 1;
        public static bool _stationLaser = false;

        private DroneBuilder _droneFleet;
        private Station _station;

        private Dialog _itemDialog;
        public SortedDictionary<int, string> ShopText = new SortedDictionary<int, string>();
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

            ShopText = new SortedDictionary<int, string>();
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

            /*/ADD Costs for Upgrades
            _priceDMG = 1000;
            _priceArmor = 2000;
            //_priceDroneHealth = 500;
            _priceDroneShield = 500;
            _priceStationShield = 5000;
            _priceStationHealth = 5000;
            */

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

            ShopText.Add(0, "Your Base Damage is: " + _droneFleet._makeDmg.ToString()+"\n\nFor Every Upgrade your\nGundamage is increased by 10");
            ShopText.Add(1, "Your Base Health is: " + _droneFleet._maxHealth.ToString() + " Health\n\nIf your Health goes down to 0,\nyour Drone will die.\nCare!!! There is no way to restore it,\nexcept getting a new Drone.\n\nFor every Upgrade the maximum Health\nincreases by 100.");
            ShopText.Add(2, "New Armor\n" + _droneFleet._armor.ToString() + " Armor\n\nFor every Upgrade in Armor, \nthe incoming Damage is reduced by 1.");
            ShopText.Add(3, "New Shield\n" + _droneFleet._maxShield.ToString() + " Shield\n\nIf your Shield goes down to 0, \nyour Health will get Damage.\n\nAfter a short delay\nthe shield restores itself.\n\nFor every upgrade the maximum Shield\nincreases by 50. ");
            ShopText.Add(4, "New Station Health\n" +_station._maxhealth.ToString() + " Health\n\nIf the Stationhealth goes down to 0,\nyour Game is Over.\nThe Station gets slowly repaired over time.\n\nFor every Upgrade the maximum Health\nincreases by 1000." );
            ShopText.Add(5, "New Station Shield\n" + _station._maxShield.ToString() + " Shield\n\nIf the Stationshield goes down to 0,\nthe Station will get real Damage.\nThe Shield regenerates, \nif the Station doesnt get hit.\n\nFor every Upgrade the maximum Shield\nincreases by 500.");
            ShopText.Add(6, "5000 Fragments\nUpgrade the Station,\nso it can destroy Astroids");
            ShopText.Add(7, "Close Shop");
        }


        //#################################
        // LoadContent - Function
        //#################################
        public override void LoadContent()
        {
            //UI
            Labels.Add(new Label("gamefont", "Fragments: ", 50, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            _itemDialog.LoadContent();
            _frame.LoadContent();
        }

        //#################################
        // MenuItems
        //#################################
        void SetMenuEntryText()
        {
            damageMenuEntry.Text = "Damage Level: " + _droneDamageLevel +" for "+ _priceDMG+" Fragments";
            healthMenuEntry.Text = "Health Level: " + _droneHealthLevel +" for "+_priceDroneHealth + " Fragments";
            armorMenuEntry.Text = "Armor Level: " + _droneArmorLevel + " for " + _priceArmor + " Fragments";
            shieldMenuEntry.Text = "Shield Level: " + _droneShieldLevel + " for " + _priceDroneShield + " Fragments";
            sHealthMenuEntry.Text = "Station Health Level: " + _StationHealthLevel + " for " + _priceStationHealth + " Fragments";
            sShieldMenuEntry.Text = "Station Shield Level: " + _StationShieldLevel + " for " + _priceStationShield + " Fragments";
            stationlaserMenuEntry.Text = "Station Laser Online: " + _stationLaser;
        }

        // Event handler for when the Damage menu entry is selected.
        void damageMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money > _priceDMG)
            {
                _droneDamageLevel++;
                Global.Money -= _priceDMG;
                _priceDMG *= 2;

                this._droneFleet._makeDmg += 10;
                
                SetMenuEntryText();
                ShopText.Remove(0);
                ShopText.Add(0, "Your Laser does now " + _droneFleet._makeDmg.ToString() + " Damage\n\nFor Every Upgrade your\nGundamage is increased by 10");
               
            }
        }
        // Event handler for when the Health menu entry is selected.
        void healthMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money > _priceDroneHealth)
            {
                _droneHealthLevel++;
                Global.Money -= _priceDroneHealth;
                _priceDroneHealth *=2;

                this._droneFleet._maxHealth += 100;

                SetMenuEntryText();
                ShopText.Remove(1);
                ShopText.Add(1, "Your Drone has now:\n" + _droneFleet._maxHealth.ToString() + " maximum Health\n\nIf your Health goes down to 0,\nyour Drone will die.\nCare!!! There is no way to restore it,\nexcept getting a new Drone.\n\nFor every Upgrade the maximum Health\nincreases by 100.");
                
            }
        }

        void armorMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money > _priceArmor)
            {
                _droneArmorLevel++;
                 Global.Money -= _priceArmor;
                _priceArmor *= 2;
              
                this._droneFleet._armor+=1;
             
                SetMenuEntryText();
                ShopText.Remove(2);
                ShopText.Add(2, "Your Drone has now:\n" + _droneFleet._armor.ToString() + " Armor\n\nFor every Upgrade in Armor, \nthe incoming Damage is reduced by 1.");
            }
        }
        void shieldMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money > _priceDroneShield)
            {
                _droneShieldLevel++;
                Global.Money -= _priceDroneShield;
                _priceDroneShield *= 2;
      
                this._droneFleet._maxShield+=50;   
        
           
                SetMenuEntryText();
                ShopText.Remove(3);
                ShopText.Add(3, "Your Drone has now:\n" + _droneFleet._maxShield.ToString() + " maximum Shield\n\nIf your Shield goes down to 0, \nyour Health will get Damage.\n\nAfter a short delay\nthe shield restores itself.\n\nFor every upgrade the maximum Shield\nincreases by 50. ");
            }
        }
        void sHealthMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money > _priceStationHealth)
            {
                _StationHealthLevel++;
                Global.Money -= _priceStationHealth;
                _priceStationHealth *= 2;
             
                this._station._health += 1000;
             
                SetMenuEntryText();
                ShopText.Remove(4);
                ShopText.Add(4, "Your Station has now:\n" + _station._maxhealth.ToString() + " maximum Health\n\nIf the Stationhealth goes down to 0,\nyour Game is Over.\nThe Station gets slowly repaired over time.\n\nFor every Upgrade the maximum Health\nincreases by 1000.");
            }
        }
        void sShieldMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money > _priceStationShield)
            {
                _StationShieldLevel++;
                Global.Money -= _priceStationShield;
                _priceStationShield *= 2;
           
                this._station._maxShield += 500;
    
                SetMenuEntryText();
                ShopText.Remove(5);
                ShopText.Add(5, "Your Station has now:\n" + _station._maxShield.ToString() + " maximum Shield\n\nIf the Stationshield goes down to 0,\nthe Station will get real Damage.\nThe Shield regenerates, \nif the Station doesnt get hit.\n\nFor every Upgrade the maximum Shield\nincreases by 500.");
            }
        }
        void stationlaserhMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money > 5000 && _stationLaser==false)
            {
                _stationLaser = true;
                Global.Money -= 5000;

                this._station.makeDmg += 5000;

                SetMenuEntryText();
                ShopText.Remove(6);
                ShopText.Add(6, "The Stationlaser is now ONLINE\n");
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

            Labels[0].Draw(Global.Money);

            string entry;
            ShopText.TryGetValue(selectedEntry, out entry);
            _itemDialog.Draw(entry);

            _frame.Draw(false);
        }
    }
}

