using Microsoft.Xna.Framework;
using SpaceAssault.Utils;
using System.Collections.Generic;
using SpaceAssault.Entities;
using SpaceAssault.Screens.UI;

namespace SpaceAssault.Screens
{
    class InGameOverlay
    {

        //#################################
        // Variables
        //#################################
        List<Label> Labels = new List<Label>();
        private Drone _drone;
        private Station _station;
        List<Bar> Bars = new List<Bar>();

        //#################################
        // Constructor
        //#################################
        public InGameOverlay(Drone _drone, Station _station)
        {
            this._drone = _drone;
            this._station = _station;
        }

        //#################################
        // LoadContent - Function
        //#################################
        public void LoadContent()
        {
            //UI
            Labels.Add(new Label("gamefont", "Health: ", 50, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            Labels.Add(new Label("gamefont", "Score: ", 170, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            Labels.Add(new Label("gamefont", "StationHealth: ", Global.GraphicsManager.PreferredBackBufferWidth - 200, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            Labels.Add(new Label("gamefont", "Press B for Shop", Global.GraphicsManager.PreferredBackBufferWidth / 2, Global.GraphicsManager.PreferredBackBufferHeight - 50, Color.White));
            Labels.Add(new Label("gamefont", "Upgrade available: ", Global.GraphicsManager.PreferredBackBufferWidth - 180, 50, Color.GreenYellow));

            Bars.Add(new Bar(new Rectangle(new Point(50, Global.GraphicsManager.PreferredBackBufferHeight - 80), new Point(300, 60)), Color.Red, _drone.MaxHealth));
            Bars.Add(new Bar(new Rectangle(new Point(50, Global.GraphicsManager.PreferredBackBufferHeight - 90), new Point(300, 60)), Color.Blue, _drone.MaxShield));
            Bars.Add(new Bar(new Rectangle(new Point(Global.GraphicsManager.PreferredBackBufferWidth-400, Global.GraphicsManager.PreferredBackBufferHeight - 750), new Point(300, 60)), Color.Green, 10000));

            foreach (var bar in Bars)
            {
                bar.LoadContent();
            }
        }
        //#################################
        // Draw
        //#################################
        public void Draw()
        {
            Labels[0].Draw(this._drone.Health);
            Labels[1].Draw(Global.HighScorePoints);
            Labels[2].Draw(this._station._health);

            if (this._drone._updatePoints > 0)
            {
                Labels[4].Draw(this._drone._updatePoints);

                if ((Vector3.Distance(this._station.Position, this._drone.Position) - GameplayScreen._stationHeight) < 150)
                    Labels[3].Draw();
            }

            Bars[0].Draw(_drone.Health, _drone.MaxHealth);
            Bars[1].Draw(_drone.Shield, _drone.MaxShield);
            Bars[2].Draw(_station._health, 10000);

        }

    }
}
