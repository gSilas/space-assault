using System;
using System.Collections.Generic;
using IrrKlang;
using Microsoft.Xna.Framework;
using SpaceAssault.Entities;
using SpaceAssault.Utils;
using SpaceAssault.Screens.UI;
using SpaceAssault.ScreenManagers;

namespace SpaceAssault.Screens
{
    class ShopScreen : MenuScreen
    {

        //#################################
        // Variables
        //#################################
        //Drone
        List<Label> Labels = new List<Label>();

        MenuEntry damageMenuEntry;
        MenuEntry healthMenuEntry;
        MenuEntry armorMenuEntry;
        MenuEntry shieldMenuEntry;
        MenuEntry sShieldMenuEntry;
        MenuEntry sHealthMenuEntry;
        MenuEntry rocketMenuEntry;

        private int _priceDMG=500;
        private  int _priceDroneHealth=500;
        private int _priceArmor=2000;
        private int _priceDroneShield=500;
        private int _priceStationShield=2000;
        private int _priceStationHealth = 2000;

        public int _droneDamageLevel = 1;
        public int _droneHealthLevel = 1;
        public int _droneArmorLevel = 1;
        public int _droneShieldLevel = 1;
        public int _StationHealthLevel = 1;
        public int _StationShieldLevel = 1;

        private Random _rand = new Random();
        private DroneBuilder _droneFleet;
        private Station _station;

        private Dialog _itemDialog;
        private Dialog _levelDialog;
        private Dialog _priceDialog;

        public SortedDictionary<int, string> ShopText;
        public SortedDictionary<int, string> PriceText;
        public SortedDictionary<int, string> LevelText;

        private ISound _accept;
        private ISound _denie;
        private ISound _UpAndDown;
        //#################################
        // Constructor
        //#################################
        public ShopScreen(DroneBuilder droneFleet,Station station)
            : base("Shop")
        {
            this._droneFleet = droneFleet;
            this._station = station;

            ShopText = new SortedDictionary<int, string>();
            PriceText = new SortedDictionary<int, string>();
            LevelText = new SortedDictionary<int, string>();

        // Create our menu entries.
            damageMenuEntry = new MenuEntry(string.Empty);
            healthMenuEntry = new MenuEntry(string.Empty);
            armorMenuEntry = new MenuEntry(string.Empty);
            shieldMenuEntry=new MenuEntry(string.Empty);

            sHealthMenuEntry=new MenuEntry(string.Empty);
            sShieldMenuEntry=new MenuEntry(string.Empty);
            rocketMenuEntry=new MenuEntry(string.Empty);

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
            rocketMenuEntry.Selected += rocketMenuEntrySelected;

            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(damageMenuEntry);
            MenuEntries.Add(healthMenuEntry);
            MenuEntries.Add(armorMenuEntry);
            MenuEntries.Add(shieldMenuEntry);

            MenuEntries.Add(sHealthMenuEntry);
            MenuEntries.Add(sShieldMenuEntry);
            MenuEntries.Add(rocketMenuEntry);
            MenuEntries.Add(back);

            _itemDialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 - 150, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 650, 320, 320, 8, false, true);
            _priceDialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 - 150, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 320, 48, 320, 8, false, true);
            _levelDialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 - 150, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 262, 48, 320, 8, false, true);
            SetShopText();
        }

        void SetShopText()
        {
            ShopText.Clear();
            ShopText.Add(0, "Your Base Damage is: " + _droneFleet._makeDmg.ToString() + "\n\nFor every upgrade your\nGundamage is increased by 10");
            ShopText.Add(1, "Your Base Health is: " + _droneFleet._maxHealth.ToString() + " Health\n\nIf your Health goes down to 0,\nyour Drone will die.\nFor every Upgrade the maximum\nHealth increases by 100.");
            ShopText.Add(2, "New Armor\n" + _droneFleet._armor.ToString() + " Armor\n\nFor every Upgrade in Armor,\nthe incoming Damage is reduced\nby Damage*Armor/10.");
            ShopText.Add(3, "New Shield\n" + _droneFleet._maxShield.ToString() + " Shield\n\nIf your Shield goes down to 0,\nyour Health will get Damage.\n\nAfter a short delay\nthe shield restores itself.\n\nFor every upgrade the maximum\nShield increases by 50.");
            ShopText.Add(4, "New Station Health\n" + _station._maxhealth.ToString() + " Health\n\nIf the Stationhealth goes down\nto 0, your Game is Over.\nThe Station gets slowly\nrepaired over time.\n\nFor every Upgrade the maximum\nHealth increases by 1000.");
            ShopText.Add(5, "New Station Shield\n" + _station._maxShield.ToString() + " Shield\n\nIf the Stationshield goes down\nto 0,the Station will get real\nDamage.\nThe Shield regenerates, \nif the Station doesnt get hit.\n\nFor every Upgrade the maximum\nShield increases by 500.");
            ShopText.Add(6, "Buy your own Fragment rocket\nIn One Word: BOOOOM\n\nYou have: " + Global.NumberOfRockets + " Fragment Missiles\nfor 500 each");
            ShopText.Add(7, "Close Shop");
            SetPriceText();
            SetLevelText();
        }
        void SetPriceText()
        {
            PriceText.Clear();
            PriceText.Add(0, "Price: " + _priceDMG + " Fragments");
            PriceText.Add(1, "Price: " + _priceDroneHealth + " Fragments");
            PriceText.Add(2, "Price: " + _priceArmor + " Fragments");
            PriceText.Add(3, "Price: " + _priceDroneShield + " Fragments");
            PriceText.Add(4, "Price: " + _priceStationHealth + " Fragments");
            PriceText.Add(5, "Price: " + _priceStationShield + " Fragments");
            PriceText.Add(6, "Price: 500 Fragments");
            PriceText.Add(7, "");
        }
        void SetLevelText()
        {
            LevelText.Clear();
            LevelText.Add(0, "Level: " + _droneDamageLevel);
            LevelText.Add(1, "Level: " + _droneHealthLevel);
            LevelText.Add(2, "Level: " + _droneArmorLevel);
            LevelText.Add(3, "Level: " + _droneShieldLevel);
            LevelText.Add(4, "Level: " + _StationHealthLevel);
            LevelText.Add(5, "Level: " + _StationShieldLevel);
            LevelText.Add(6, "Level: " + Global.NumberOfRockets);
            LevelText.Add(7, "");
        }

        //#################################
        // LoadContent - Function
        //#################################
        public override void LoadContent()
        {
            //UI
            Labels.Add(new Label("Fragments: ", 50, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            _itemDialog.LoadContent();
            _levelDialog.LoadContent();
            _priceDialog.LoadContent();
            _frame.LoadContent();

            //Sound
            SoundEngine = new ISpaceSoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);
            MenuAcceptSound = SoundEngine.AddSoundSourceFromFile("Content/Media/Effects/Blip_Select.wav", StreamMode.AutoDetect, true);
            MenuDenieSound = SoundEngine.AddSoundSourceFromFile("Content/Media/Effects/MenuPointDenie.wav", StreamMode.AutoDetect, true);
            OkClick = SoundEngine.AddSoundSourceFromFile("Content/Media/Effects/MenuPointAccept.wav", StreamMode.AutoDetect, true);
            GoBack = SoundEngine.AddSoundSourceFromFile("Content/Media/Effects/GoBack2.wav", StreamMode.AutoDetect, true);
        }

        //#################################
        // MenuItems
        //#################################
        void SetMenuEntryText()
        {
            damageMenuEntry.Text = "Damage ";
            healthMenuEntry.Text = "Health";
            armorMenuEntry.Text = "Armor";
            shieldMenuEntry.Text = "Shield";
            sHealthMenuEntry.Text = "Station Health";
            sShieldMenuEntry.Text = "Station Shield";
            rocketMenuEntry.Text = "Fragment Missiles";
        }

        // Event handler for when the Damage menu entry is selected.
        void damageMenuEntrySelected(object sender, EventArgs e)
        {


            if (Global.Money >= _priceDMG)
            {
                _droneDamageLevel++;
                Global.Money -= _priceDMG;

                this._droneFleet._makeDmg += 10;
                Global.DroneBulletColor = new Color(_rand.Next(100,200),_rand.Next(100,200),_rand.Next(100,200),255);
                SetMenuEntryText();
                SetShopText();
                
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));

                _accept = SoundEngine.Play2D(MenuAcceptSound, false, true, false);
                _accept.Volume = Global.SpeakerVolume / 10;
                _accept.Paused = false;
            }
            else
            {
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
               
                _denie = SoundEngine.Play2D(MenuDenieSound, false, true, false);
                _denie.Volume = Global.SpeakerVolume / 10;
                _denie.Paused = false;
            }
        }
        // Event handler for when the Health menu entry is selected.
        void healthMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money >= _priceDroneHealth)
            {
                _droneHealthLevel++;
                Global.Money -= _priceDroneHealth;

                this._droneFleet._maxHealth += 100;
                _droneFleet.GetActiveDrone().health += 100;

                SetMenuEntryText();
                SetShopText();

                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));

                _accept = SoundEngine.Play2D(MenuAcceptSound, false, true, false);
                _accept.Volume = Global.SpeakerVolume / 10;
                _accept.Paused = false;
            }
            else
            {
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
              
                _denie = SoundEngine.Play2D(MenuDenieSound, false, true, false);
                _denie.Volume = Global.SpeakerVolume / 10;
                _denie.Paused = false;
            }
        }

        void armorMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money >= _priceArmor)
            {       
                if(_droneFleet._armor < 5)
                {
                    _droneArmorLevel++;
                    Global.Money -= _priceArmor;
                    this._droneFleet._armor++;
                    SetMenuEntryText();
                    ShopText.Remove(2);
                    ShopText.Add(2, "Your Drone has now:\n" + _droneFleet._armor.ToString() + " Armor\n\nFor every Upgrade in Armor,\n\nthe incoming Damage is reduced by Damage*Armor/10.");
                }
                else
                {
                    ShopText.Remove(2);
                    ShopText.Add(2, "Your Drone has now:\n" + _droneFleet._armor.ToString() + "\n\nThis is the maximum armor capacity!");
                }
                
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));

                _accept = SoundEngine.Play2D(MenuAcceptSound, false, true, false);
                _accept.Volume = Global.SpeakerVolume / 10;
                _accept.Paused = false;
            }
            else
            {
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
         
                _denie = SoundEngine.Play2D(MenuDenieSound, false, true, false);
                _denie.Volume = Global.SpeakerVolume / 10;
                _denie.Paused = false;
            }
        }
        void shieldMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money >= _priceDroneShield)
            {
                _droneShieldLevel++;
                Global.Money -= _priceDroneShield;
      
                this._droneFleet._maxShield+=50;   
            
                SetMenuEntryText();
                SetShopText();

                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));

                _accept = SoundEngine.Play2D(MenuAcceptSound, false, true, false);
                _accept.Volume = Global.SpeakerVolume / 10;
                _accept.Paused = false;
            }
            else
            {
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
   
                _denie = SoundEngine.Play2D(MenuDenieSound, false, true, false);
                _denie.Volume = Global.SpeakerVolume / 10;
                _denie.Paused = false;
            }
        }
        void sHealthMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money >= _priceStationHealth)
            {
                _StationHealthLevel++;
                Global.Money -= _priceStationHealth;
             
                this._station._health += 1000;
                this._station._maxhealth += 1000;

                SetMenuEntryText();
                SetShopText();

                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));

                _accept = SoundEngine.Play2D(MenuAcceptSound, false, true, false);
                _accept.Volume = Global.SpeakerVolume / 10;
                _accept.Paused = false;
            }
            else
            {
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
    
                _denie = SoundEngine.Play2D(MenuDenieSound, false, true, false);
                _denie.Volume = Global.SpeakerVolume / 10;
                _denie.Paused = false;
            }
        }
        void sShieldMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money >= _priceStationShield)
            {
                _StationShieldLevel++;
                Global.Money -= _priceStationShield;
           
                this._station._maxShield += 500;
    
                SetMenuEntryText();
                SetShopText();

                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));

                _accept = SoundEngine.Play2D(MenuAcceptSound, false, true, false);
                _accept.Volume = Global.SpeakerVolume / 10;
                _accept.Paused = false;
            }
            else
            {
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
   
                _denie = SoundEngine.Play2D(MenuDenieSound, false, true, false);
                _denie.Volume = Global.SpeakerVolume / 10;
                _denie.Paused = false;
            }
        }
        void rocketMenuEntrySelected(object sender, EventArgs e)
        {
            if (Global.Money >= 500)
            {                
                Global.Money -= 500;
                Global.NumberOfRockets += 1;

                SetMenuEntryText();
                SetShopText();

                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));

                _accept = SoundEngine.Play2D(MenuAcceptSound, false, true, false);
                _accept.Volume = Global.SpeakerVolume / 10;
                _accept.Paused = false;
            }
            else
            {
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
          
                _denie = SoundEngine.Play2D(MenuDenieSound, false, true, false);
                _denie.Volume = Global.SpeakerVolume / 10;
                _denie.Paused = false;
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
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));

                _UpAndDown = SoundEngine.Play2D(OkClick, false, true, false);
                _UpAndDown.Volume = Global.SpeakerVolume / 10;
                _UpAndDown.Paused = false;

                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = MenuEntries.Count - 1;
            }
            // Move to the next menu entry?
            if (input.IsMenuDown())
            {
                //playing the sound
                SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
               
                _UpAndDown = SoundEngine.Play2D(OkClick, false, true, false);
                _UpAndDown.Volume = Global.SpeakerVolume / 10;
                _UpAndDown.Paused = false;

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
            drawMenuEntries(gameTime);
            Labels[0].Draw(Global.Money);

            string entry;
            ShopText.TryGetValue(selectedEntry, out entry);
            string entry2;
            PriceText.TryGetValue(selectedEntry, out entry2);
            string entry3;
            LevelText.TryGetValue(selectedEntry, out entry3);
            Global.SpriteBatch.End();
            Global.SpriteBatch.Begin();
            _itemDialog.Draw(entry);
            _priceDialog.Draw(entry2);
            _levelDialog.Draw(entry3);
            _frame.Draw();
        }
    }
}

