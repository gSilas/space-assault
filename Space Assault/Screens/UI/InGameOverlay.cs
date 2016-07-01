using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.ScreenManager;
using SpaceAssault.Utils;
using System.Collections.Generic;
using System;

namespace SpaceAssault.Screens
{
    class InGameOverlay
    {

        //UI
        List<Label> Labels = new List<Label>();
        int _droneHealth;
        int _stationHealth;
        float _distance;

        //#################################
        // LoadContent - Function
        //#################################

        public InGameOverlay(int droneHealth, int stationHealth, float distance)
        {
            _droneHealth = droneHealth;
            _stationHealth = stationHealth;
            _distance = distance;
        }

        public void LoadContent()
        {
            //UI
            Labels.Add(new Label("gamefont", "Health: ", 50, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            Labels.Add(new Label("gamefont", "Score: ", 170, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            Labels.Add(new Label("gamefont", "StationHealth: ", Global.GraphicsManager.PreferredBackBufferWidth-200, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            Labels.Add(new Label("gamefont", "Press B for Shop", Global.GraphicsManager.PreferredBackBufferWidth / 2, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            Labels.Add(new Label("gamefont", "Upgrade available", Global.GraphicsManager.PreferredBackBufferWidth - 180, 50, Color.GreenYellow));
        }

        // Draws the background screen.
        public void Update(int droneHealth, int stationHealth, float distance)
        {
            _droneHealth = droneHealth;
            _stationHealth = stationHealth;
            _distance = distance;
        }

        // Draws the background screen.
        public void Draw()
        {
            Labels[0].Draw(_droneHealth);
            Labels[1].Draw(Global.HighScorePoints);
            Labels[2].Draw(_stationHealth);

            if (_distance < 150 && Global.HighScorePoints > 1000 && GameplayScreen._dronepdate)
                Labels[3].Draw();

            if (Global.HighScorePoints > 1000 &&  GameplayScreen._dronepdate)
                Labels[4].Draw();
        }

    }
}
