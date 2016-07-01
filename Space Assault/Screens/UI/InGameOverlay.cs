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
            Labels.Add(new Label("gamefont", "Health: ", 100, Global.GraphicsManager.PreferredBackBufferHeight - 100, Color.White));
            Labels.Add(new Label("gamefont", "Score: ", 220, Global.GraphicsManager.PreferredBackBufferHeight - 100, Color.White));
            Labels.Add(new Label("gamefont", "StationHealth: ", Global.GraphicsManager.PreferredBackBufferWidth-200, Global.GraphicsManager.PreferredBackBufferHeight - 100, Color.White));
            Labels.Add(new Label("gamefont", "Press B for Shop", Global.GraphicsManager.PreferredBackBufferWidth / 2, Global.GraphicsManager.PreferredBackBufferHeight - 100, Color.White));
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

            if (_distance < 150)
                Labels[3].Draw();
        }

    }
}
