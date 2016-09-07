using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceAssault.Screens
{
    class HighscoreMenuScreen : MenuScreen
    {
        MenuEntry back;
        bool _enter;

        private Vector2 _fieldPos;
        private string _entryString;
        private int _elapsedTimeMilliseconds;

        // Constructor.
        public HighscoreMenuScreen(bool enter) : base("Highscore")
        {
            // Create our menu entries.
            back = new MenuEntry("Back");

            // Hook up menu event handlers.
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(back);

            _fieldPos = new Vector2(100, Global.PreferredBackBufferHeight - 100);
            _entryString = "";
            _enter = enter;
        }

        public override void LoadContent()
        {
            base.LoadContent();

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (_enter)
            {
                _elapsedTimeMilliseconds += gameTime.ElapsedGameTime.Milliseconds;

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
                        Global.HighScoreList.Add(_entryString, Global.HighScorePoints);
                        _entryString = "";
                        Global.HighScorePoints = 0;
                        _enter = false;
                    }

                    _elapsedTimeMilliseconds = 0;
                }
            }
        }

        // Draws the menu.
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (this.TransitionPosition <= 0f)
            {
                int zeilenAbstand = 20;
                int spaltenAbstand = 200;
                int spawnPointX = 150;
                int spawnPointY = 200;
                for (int i = 0; i < Global.HighScoreList._listLength; i++)
                {
                    Global.SpriteBatch.DrawString(Global.GameFont, (i + 1) + ". Platz", new Vector2(spawnPointX, spawnPointY + i * zeilenAbstand), Color.BurlyWood);
                    Global.SpriteBatch.DrawString(Global.GameFont, Global.HighScoreList._scoresList[i].Name, new Vector2(spawnPointX + spaltenAbstand, spawnPointY + i * zeilenAbstand), Color.BurlyWood);
                    Global.SpriteBatch.DrawString(Global.GameFont, (Global.HighScoreList._scoresList[i].Points).ToString(), new Vector2(spawnPointX + spaltenAbstand * 2, spawnPointY + i * zeilenAbstand), Color.BurlyWood);
                }

                if (_enter)
                {
                    Global.SpriteBatch.DrawString(Global.GameFont, _entryString, _fieldPos + new Vector2(20, 20), Color.Black);
                }
            }
        }

    }
}
