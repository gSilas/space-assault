using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceAssault.Screens
{
    class HighscoreMenuScreen : MenuScreen
    {
        MenuEntry back;
        bool _enter;

        private string _entryString;
        private int _elapsedTimeMilliseconds;
        private float inputBoxWidth;
        private KeyboardState oldKeyboardState;
        private KeyboardState currentKeyboardState;

        public string EntryText
        {
            get
            {
                return _entryString;
            }
            set
            {
                _entryString = value;

                if (_entryString != "")
                {
                    //if you attempt to display a character that is not in your font
                    //you will get an exception, so we filter the characters
                    //remove the filtering if you're using a default character in your spritefont
                    string filtered = "";
                    foreach (char c in value)
                    {
                        if (Global.GameFont.Characters.Contains(c))
                            filtered += c;
                    }

                    _entryString = filtered;

                    while (Global.GameFont.MeasureString(_entryString).X > inputBoxWidth)
                    {
                        //to ensure that text cannot be larger than the box
                        _entryString = _entryString.Substring(0, _entryString.Length - 1);
                    }
                }
            }
        }

        // Constructor.
        public HighscoreMenuScreen(bool enter) : base("Highscore")
        {
            // Create our menu entries.
            back = new MenuEntry("Back");
            inputBoxWidth = 150;
            // Add entries to the menu.
            MenuEntries.Add(back);

            EntryText = "";
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

                oldKeyboardState = currentKeyboardState;
                currentKeyboardState = Keyboard.GetState();

                Keys[] pressedKeys;
                pressedKeys = currentKeyboardState.GetPressedKeys();

                foreach (Keys curKey in pressedKeys)
                {
                    if (oldKeyboardState.IsKeyUp(curKey))
                    {
                        switch (curKey)
                        {
                            case Keys.Back:
                                if(EntryText.Length > 0) EntryText = EntryText.Substring(0, EntryText.Length - 1);
                                break;

                            case Keys.Space:
                                EntryText += " ";
                                break;

                            case Keys.Enter:
                                if (EntryText.Length > 2)
                                {
                                    //enter, send the string + points to highscores
                                    Global.HighScoreList.Add(EntryText, Global.HighScorePoints);
                                    EntryText = "";
                                    Global.HighScorePoints = 0;
                                    _enter = false;
                                }
                                break;

                            case Keys.OemMinus:
                                EntryText += "-";
                                break;

                            default:
                                if (curKey.ToString().ToCharArray().Length == 1)    //wenn der key nur ein charakter hat
                                {
                                    if (pressedKeys.Length > 1)   //wenn mehr als ein Key gleichzeitig gedrueckt wurde
                                    {
                                        if (pressedKeys[1] == Keys.LeftShift)   //wenn LeftShift ist => Großschreibung
                                            EntryText += curKey.ToString().ToUpper();
                                    }
                                    else EntryText += curKey.ToString().ToLower();   // => kleinschreibung
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                // Hook up menu event handlers.
                back.Selected += OnCancel;
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
                    Global.SpriteBatch.DrawString(Global.GameFont, EntryText, new Vector2(spawnPointX, spawnPointY + 11 * zeilenAbstand), Color.White);
                }
            }
        }

    }
}
