﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SpaceAssault.Utils;
using SpaceAssault.Entities;
using SpaceAssault.Screens.UI;

namespace SpaceAssault.Screens
{
    class InGameOverlay
    {
        //#################################
        // Variables
        //#################################
        private Station _station;
        List<Label> Labels = new List<Label>();
        List<Bar> Bars = new List<Bar>();
        private UIItem _shields = new UIItem();
        private UIItem _rocketSymbol = new UIItem();

        private Dialog _upgradeVincinityDialog;
        private Dialog _scoreDialog;
        private Dialog _moneyDialog;
        private Dialog _alertDialog;

        //#################################
        // Constructor
        //#################################
        public InGameOverlay(Station _station)
        {
            this._station = _station;
        }

        //#################################
        // LoadContent - Function
        //#################################
        public void LoadContent(DroneBuilder droneFleet)
        {          
            //Shield
            _shields.LoadContent("Images/UI/shield_ui",4);
            //Rocket
            _rocketSymbol.LoadContent("Images/Rocket_Icon",4);

            //Bars
            Bars.Add(new Bar(new Rectangle(new Point(50, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 80), new Point(300, 60)), Color.Red, droneFleet.GetActiveDrone().maxHealth));
            Bars.Add(new Bar(new Rectangle(new Point(50, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 90), new Point(300, 60)), Color.Blue, droneFleet.GetActiveDrone().maxShield));
            Bars.Add(new Bar(new Rectangle(new Point(Global.GraphicsManager.GraphicsDevice.Viewport.Width - 400, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 750), new Point(300, 60)), Color.Green, _station._maxhealth));
            Bars.Add(new Bar(new Rectangle(new Point(Global.GraphicsManager.GraphicsDevice.Viewport.Width - 400, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 740), new Point(300, 60)), Color.White,_station._maxShield));
            foreach (var bar in Bars)
            {
                bar.LoadContent();
            }

            //Dialogs        
            _upgradeVincinityDialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 - 64, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 82, 24, 176, 8, false, true);
            _scoreDialog = new Dialog(260, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 750, 24, 200, 8, false, true);
            _moneyDialog = new Dialog(50, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 750, 24, 200, 8, false, true);
            _alertDialog = new Dialog(Global.GraphicsManager.GraphicsDevice.Viewport.Width /2 - 160, Global.GraphicsManager.GraphicsDevice.Viewport.Height /2 -16, 24, 320, 8, false, true);

            _scoreDialog.LoadContent();
            _alertDialog.LoadContent();
            _moneyDialog.LoadContent();
            _upgradeVincinityDialog.LoadContent();          
        }
        //#################################
        // Draw
        //#################################
        public void Draw(DroneBuilder droneFleet)
        {
            _shields.Draw(new Point(50, Global.GraphicsManager.GraphicsDevice.Viewport.Height - 130), droneFleet._armor, new Color(1f, 1f, 1f, 0.5f));
            _rocketSymbol.Draw(new Point(355, 717),1 , new Color(1f, 1f, 1f, 0.5f));

            Global.UIBatch.Begin();
            Global.UIBatch.DrawString(Global.Font, Global.NumberOfRockets.ToString(), new Vector2(390,725), new Color(58f, 116f, 112f));
            Global.UIBatch.End();

            Bars[0].Draw(droneFleet.GetActiveDrone().health, droneFleet.GetActiveDrone().maxHealth);
            Bars[1].Draw(droneFleet.GetActiveDrone().shield, droneFleet.GetActiveDrone().maxShield);
            /*
            if (Vector3.Distance(this._station.Position, droneFleet.GetActiveDrone().Position) < 300)
            {
                var vec = new Point();
                vec.X = (int)Global.GraphicsManager.GraphicsDevice.Viewport.Project(_station.Position, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity).X - 80;
                vec.Y = (int)Global.GraphicsManager.GraphicsDevice.Viewport.Project(_station.Position, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity).Y - 90;
                Bars[2].Draw(vec, _station._health, _station._maxhealth);
                vec.Y = (int)Global.GraphicsManager.GraphicsDevice.Viewport.Project(_station.Position, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity).Y - 80;
                Bars[3].Draw(vec, _station._shield, _station._maxShield);
            }
            else
            {*/
                Bars[2].Draw(_station._health, _station._maxhealth);
                Bars[3].Draw(_station._shield, _station._maxShield);
            //}
                       
            _scoreDialog.Draw("Score: " + Global.HighScorePoints.ToString());
            _moneyDialog.Draw("Fragments: "+Global.Money);

            float _distance = Vector2.Distance(new Vector2(droneFleet.GetActiveDrone().Position.X, droneFleet.GetActiveDrone().Position.Z), Vector2.Zero);
            if (_distance > Global.MapRingRadius && _distance < Global.MapRingRadius + 80)
                _alertDialog.Draw("ALERT! OUT OF RANGE", Color.OrangeRed);
            else if (_distance > Global.MapRingRadius + 80)
                _alertDialog.Draw("ALERT! SHIP FAILURE", Color.Red);

            if (Global.Money >= 0)
            {
                if ((Vector3.Distance(this._station.Position, droneFleet.GetActiveDrone().Position) - GameplayScreen._stationHeight) < 150)
                {
                    _upgradeVincinityDialog.Draw("Press B for Shop!");
                }                  
            }          
        }
    }
}
