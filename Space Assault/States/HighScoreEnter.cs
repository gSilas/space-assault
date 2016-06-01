using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Assault.Utils;
using System;
/// <summary>
/// Handles the input for the highscorelist (for the name)
/// help from: http://community.monogame.net/t/true-keyboard-input/1114
/// </summary>
namespace Space_Assault.States
{
    class HighScoreEnter : IGameState, IUpdateableState, IDrawableState
    {
        private Texture2D _entryField;
        private Vector2 _fieldPos;
        private string _entryString;

        EventHandler<TextInputEventArgs> onTextEntered;
        KeyboardState _prevKeyState;

        public void Initialize()
        {
            _fieldPos = new Vector2(100, Global.PreferredBackBufferHeight - 100);
            _entryString = "";


        }

        public void LoadContent()
        {
            _entryField = Global.ContentManager.Load<Texture2D>("UI/entry");
        }

        public void Update(GameTime elapsedTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            // Print to debug console currently pressed keys
            Keys prevKey = Keys.A;
            foreach (Keys key in keyState.GetPressedKeys())
            {
                if(key != prevKey)
                {
                    _entryString += key;
                }
                prevKey = key;
            }
        }

        public void Draw(GameTime elapsedTime)
        {
            Global.SpriteBatch.Draw(_entryField, _fieldPos, Color.Azure);
            Global.SpriteBatch.DrawString(Global.Arial, _entryString, _fieldPos + new Vector2(20, 20), Color.Black);
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

        public bool IsStopped { get; set; }

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
