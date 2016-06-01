using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.UI;
using System.Collections.Generic;


namespace Space_Assault.States
{
    class MainMenu : IGameState, IUpdateableState, IDrawableState
    {

        //#################################
        // Set Variables
        //#################################

        // General
        List<Button> Buttons = new List<Button>();
        private Button _gameButton;
        private Button _highScoreButton;

        //#################################
        // Constructor
        //#################################
        public MainMenu()
        {
            IsStopped = false;
        }

        //#################################
        // LoadContent - Function
        //#################################
        public void LoadContent()
        {
            //Button
            _gameButton = new Button(Global.ContentManager.Load<Texture2D>("UI/play"), new Vector2(150, 150));
            _highScoreButton = new Button(Global.ContentManager.Load<Texture2D>("UI/highscore"), new Vector2(150, 250));
            
            int init_x = 100;
            int init_y = 100;
            Buttons.Add(new Button("Arial", "StartGame", init_x, init_y));
            Buttons.Add(new Button("Arial", "Highscore", init_x, init_y * (Buttons.Count+1)));
            Buttons.Add(new Button("Arial", "Credits", init_x, init_y * (Buttons.Count + 1)));
            Buttons.Add(new Button("Arial", "Exit", init_x, init_y * (Buttons.Count + 1)));
        }

        public void Kill()
        {
            IsStopped = true;
        }

        public void Resume()
        {
            if (IsStopped)
            {
                IsStopped = false;
            }
        }
        //#################################
        // Draw - Function
        //#################################
        public void Draw(GameTime elapsedTime)
        {
            /*
            foreach (var button in Buttons)
            {
                button.Draw();
            }
            */

            _gameButton.Draw();
            _highScoreButton.Draw();
        }

        //#################################
        // Update - Function
        //#################################

        public void Update(GameTime elapsedTime)
        {
            _gameButton.Update();
            _highScoreButton.Update();

            if (_gameButton.Pressed)
            {
                Global.Controller.Push(Controller.EGameStates.EndlessModeScene);
                Global.Controller.Pop(Controller.EGameStates.MenuBackground);
                Global.Controller.Pop(Controller.EGameStates.MainMenu);
            }
            if (_highScoreButton.Pressed)
            {
                Global.Controller.Push(Controller.EGameStates.HighScore);
                Global.Controller.Pop(Controller.EGameStates.MainMenu);
            }
        }

        public bool IsStopped { get; set; }

        public void Initialize()
        {
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


    }
}
