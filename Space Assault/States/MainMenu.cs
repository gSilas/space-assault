using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Space_Assault.Entities;
using Space_Assault.Utils;

namespace Space_Assault.States
{
    class MainMenu : IGameState, IUpdateableState, IDrawableState
    {

        //#################################
        // Set Variables
        //#################################

        // General
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
            _gameButton = new Button(Global.ContentManager.Load<Texture2D>("Art/game_button"), new Vector2(50, 0));
            _highScoreButton = new Button(Global.ContentManager.Load<Texture2D>("Art/highscore_button"), new Vector2(150, 175));
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
                Global.Controller.Pop(this);
            }
            if (_highScoreButton.Pressed)
            {
                Global.Controller.Push(Controller.EGameStates.HighScore);
                Global.Controller.Pop(this);
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
