using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Assault.Utils;
using System;
/// <summary>
/// Handles the input for the highscorelist (for the name)
/// Takes Global.HighScorePoints
/// TODO: make it more fancier with http://www.monogame.net/documentation/?page=E_Microsoft_Xna_Framework_GameWindow_TextInput
/// </summary>
namespace Space_Assault.States
{
    class HighScoreEnter : IGameState, IUpdateableState, IDrawableState
    {
        private Texture2D _entryField;
        private Vector2 _fieldPos;
        private string _entryString;
        private int _elapsedTimeMilliseconds;

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
            _elapsedTimeMilliseconds += elapsedTime.ElapsedGameTime.Milliseconds;


            KeyboardState keyState = Keyboard.GetState();
            if (keyState.GetPressedKeys().Length > 0 && _elapsedTimeMilliseconds > 16 * 4)
            {

                Keys curKey = keyState.GetPressedKeys()[0];

                //handling text input
                if (_entryString.Length <= 10)
                {
                    //wenn der key nur ein charakter hat
                    if (curKey.ToString().ToCharArray().Length == 1)
                        //wenn mehr als ein Key gleichzeitig gedrueckt wurde
                        if (keyState.GetPressedKeys().Length > 1)
                        {
                            //wenn LeftShift ist => Großschreibung
                            if (keyState.GetPressedKeys()[1] == Keys.LeftShift)
                                _entryString += curKey.ToString().ToUpper();
                        }
                        // => kleinschreibung
                        else _entryString += curKey.ToString().ToLower();
                    //wenn der key das minuszeichen ist
                    else if (curKey == Keys.OemMinus)
                        _entryString += "-";
                }

                //handling other operations on string
                if (curKey == Keys.Back && _entryString.Length > 0)
                    _entryString = _entryString.Remove(_entryString.Length - 1, 1);

                if (curKey == Keys.Enter && _entryString.Length > 0)
                {
                    Global.HighScore.Add(_entryString, Global.HighScorePoints);
                    Global.HighScorePoints = 0;
                    Global.Controller.Pop(Controller.EGameStates.HighScoreEnter);
                }

                _elapsedTimeMilliseconds = 0;
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
