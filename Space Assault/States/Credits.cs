using Microsoft.Xna.Framework;
using System;
using Space_Assault.UI;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Assault.States
{
    class Credits : IGameState, IUpdateableState, IDrawableState
    {

        //#################################
        // Set Variables
        //#################################

        List<Label> Labels = new List<Label>();
        private Button _hauptmenuButton;

        //#################################
        // Constructor
        //#################################
        public Credits()
        {
            IsStopped = false;
        }

        //#################################
        // LoadContent - Function
        //#################################
        public void LoadContent()
        {
            int label_x = 150;
            int label_y = 150;
            int margin_x = 30;

            Labels.Add(new Label("Arial", "Daniel", label_x, label_y + Labels.Count * margin_x, Color.White));
            Labels.Add(new Label("Arial", "Dustin", label_x, label_y + Labels.Count * margin_x, Color.White));
            Labels.Add(new Label("Arial", "Hans-Martin", label_x, label_y + Labels.Count * margin_x, Color.White));
            Labels.Add(new Label("Arial", "Philipp", label_x, label_y + Labels.Count * margin_x, Color.White));

            _hauptmenuButton = new Button(Global.ContentManager.Load<Texture2D>("UI/hauptmenu"), new Vector2(100, 580));
        }

        //#################################
        // Update - Function
        //#################################
        public void Update(GameTime elapsedTime)
        {
            _hauptmenuButton.Update();

            if (_hauptmenuButton.Pressed)
            {
                Global.Controller.Push(Controller.EGameStates.MainMenu);
                Global.Controller.Pop(Controller.EGameStates.Credits);
            }
        }

        //#################################
        // Draw - Function
        //#################################
        public void Draw(GameTime elapsedTime)
        {
            foreach (var label in Labels)
            {
                label.Draw();
            }

            _hauptmenuButton.Draw();
        }


        public bool IsStopped { get; set; }

        public void Kill()
        {
            IsStopped = true;
            _hauptmenuButton.Pressed = false;
        }

        public void Resume()
        {
            if (IsStopped)
            {
                IsStopped = false;
            }
        }

        public bool Equals(IGameState other)
        {
            return other.GetType() == this.GetType();
        }

        public bool Equals(IUpdateable other)
        {
            return other.GetType() == this.GetType();
        }

        public bool Equals(IDrawableState other)
        {
            return other.GetType() == this.GetType();
        }

        public void Initialize(){}
    }
}
