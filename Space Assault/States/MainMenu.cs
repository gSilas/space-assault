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
        Button btnPlay;

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
            //btnPlay = new Button(Global.ContentManager.Load<Texture2D>('Button'), _gm.GraphicsDevice);
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
        }

        //#################################
        // Update - Function
        //#################################

        public void Update(GameTime elapsedTime)
        {
            //Pop test
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Global.Controller.Pop(this);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Global.Controller.Push(Controller.EGameStates.EndlessModeScene);
                Global.Controller.Pop(this);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.H))
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
