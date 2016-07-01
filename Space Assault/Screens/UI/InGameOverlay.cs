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
        int _health;

        //#################################
        // LoadContent - Function
        //#################################

        public InGameOverlay(int health)
        {
            _health = health;
        }

        public void LoadContent()
        {
            //UI
            Labels.Add(new Label("gamefont", "Health: ", 100, Global.GraphicsManager.PreferredBackBufferHeight - 100, Color.White));
            Labels.Add(new Label("gamefont", "Score: ", 220, Global.GraphicsManager.PreferredBackBufferHeight - 100, Color.White));
        }

        // Draws the background screen.
        public void Update(int health)
        {
            _health = health;
        }

        // Draws the background screen.
        public void Draw()
        {
            Labels[0].Draw(_health);
            Labels[1].Draw(Global.HighScorePoints);
        }

    }
}
