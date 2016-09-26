using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceAssault.ScreenManagers;
using SpaceAssault.Screens.UI;
using SpaceAssault.Utils;

/* TODO: it would be ideal if our textinput were eventbased: http://www.gamedev.net/topic/457783-xna-getting-text-from-keyboard/
 * Question: Do we really need it? Input doesnt have to be perfect, it just has to work.
 */

namespace SpaceAssault.Screens
{
    class HighscoreMenuScreenOnline : MenuScreen
    {
        MenuEntry back;
        bool _enter;
        private bool _nextIterationFalse;
        private string _entryString;
        private float inputBoxWidth;
        private KeyboardState oldKeyboardState;
        private KeyboardState currentKeyboardState;
        private Dialog _highScoreDialog;
        private Dialog _inputDialog;

        private HighScoreListOnline _highScoreListOn;

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
                        if (Global.Font.Characters.Contains(c))
                            filtered += c;
                    }

                    _entryString = filtered;

                    while (Global.Font.MeasureString(_entryString).X > inputBoxWidth)
                    {
                        //to ensure that text cannot be larger than the box
                        _entryString = _entryString.Substring(0, _entryString.Length - 1);
                    }
                }
            }
        }

        // Constructor.
        public HighscoreMenuScreenOnline(bool enter) : base("Highscore")
        {
            // Create our menu entries.
            _nextIterationFalse = false;
            back = new MenuEntry("Back");
            inputBoxWidth = 150;
            back.Selected += OnCancel;
            // Add entries to the menu.
            MenuEntries.Add(back);

            EntryText = "";
            _enter = enter;
            a += "4J3";
            int spawnPointX = 150;
            int spawnPointY = 200;
            _highScoreDialog = new Dialog(spawnPointX, spawnPointY, 250, 500, 6, false, true);
            _inputDialog = new Dialog(spawnPointX, spawnPointY + 300, 30, 500, 6, false, true);

            _highScoreListOn = new HighScoreListOnline();
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Global.MusicEngine.StopAllSounds();
            Global.Music = Global.MusicEngine.Play2D("TruthOfTheLegend", Global.MusicVolume / 10, false);

            //Dialogs + Background + Frame
            _inputDialog.LoadContent();
            _highScoreDialog.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (_nextIterationFalse)
                _enter = false;
            if (_enter)
            {
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
                                if (EntryText.Length > 0) EntryText = EntryText.Substring(0, EntryText.Length - 1);
                                break;

                            case Keys.Space:
                                EntryText += " ";
                                break;

                            case Keys.Enter:
                                if (EntryText.Length > 2)
                                {
                                    //enter, send the string + points to highscores
                                    _highScoreListOn.addScore(EntryText+a);
                                    EntryText = "";
                                    Global.HighScorePoints = 0;
                                    _nextIterationFalse = true;
                                }
                                break;

                            case Keys.OemMinus:
                                EntryText += "-";
                                break;

                            default:
                                char[] keyText = curKey.ToString().ToCharArray();
                                if (keyText.Length == 1)    //wenn der key nur ein charakter hat
                                {
                                    if (pressedKeys.Length > 1)   //wenn mehr als ein Key gleichzeitig gedrueckt wurde
                                    {
                                        if (pressedKeys[1] == Keys.LeftShift || pressedKeys[1] == Keys.RightShift)   //wenn LeftShift ist => Großschreibung
                                            EntryText += curKey.ToString().ToUpper();
                                    }
                                    else EntryText += curKey.ToString().ToLower();   // => kleinschreibung
                                }
                                else if (keyText.Length == 2 && keyText[0] == 'D')
                                {
                                    EntryText += keyText[1];
                                }
                                break;
                        }
                    }
                }
            }

        }

        // Draws the menu.
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (this.TransitionPosition <= 0f)
            {

                _highScoreDialog.Draw("");

                
                int zeilenAbstand = 24;
                int spaltenAbstand = 200;
                float spawnPointX = _highScoreDialog.position.ToVector2().X + _highScoreDialog.size.X;
                float spawnPointY = _highScoreDialog.position.ToVector2().Y + _highScoreDialog.size.Y;
                Global.UIBatch.Begin();

                if (!_highScoreListOn.isReachable)
                {
                    Global.UIBatch.DrawString(Global.Font, "No internet or server not reachable!", new Vector2(spawnPointX+80, spawnPointY-30), Color.White);
                }
                for (int i = 0; i < 10; i++)
                {
                    string text = "";
                    if ((i+1) / 10 >= 1) text += (i + 1) + ". Place";
                    else text += " " + (i + 1) + ". Place";

                    Global.UIBatch.DrawString(Global.Font, text, new Vector2(spawnPointX, spawnPointY + i * zeilenAbstand), Color.White);
                    Global.UIBatch.DrawString(Global.Font, _highScoreListOn._scoresList[i,0], new Vector2(spawnPointX + spaltenAbstand, spawnPointY + i * zeilenAbstand), Color.White);
                    Global.UIBatch.DrawString(Global.Font, _highScoreListOn._scoresList[i,1], new Vector2(spawnPointX + spaltenAbstand * 2, spawnPointY + i * zeilenAbstand), Color.White);
                }
                Global.UIBatch.End();

                if (_enter)
                {
                    Global.UIBatch.Begin();
                    Global.UIBatch.DrawString(Global.Font,"Please register with our Database to continue!", new Vector2(_inputDialog.position.X+10, _inputDialog.position.Y-20), Color.White);
                    Global.UIBatch.End();
                    _inputDialog.Draw("Score: " + Global.HighScorePoints + "   Your Name: " + EntryText);
                }

            }
        }

        public override void HandleInput(InputState input)
        {
            if (!_enter)
            {
                // mouse click on menu?
                if (input.IsLeftMouseButtonNewPressed())
                {
                    Vector2 cornerA;
                    Vector2 cornerD;
                    for (int i = 0; i < MenuEntries.Count; i++)
                    {
                        //calculating 2 diagonal corners of current menuEntry (upper left, bottom right)
                        cornerA = MenuEntries[i].Position;
                        cornerA.Y -= MenuEntries[i].GetHeight() / 2f;

                        cornerD = MenuEntries[i].Position;
                        cornerD.Y += MenuEntries[i].GetHeight() / 2f;
                        cornerD.X += MenuEntries[i].GetWidth();

                        if (cornerA.X < input.MousePosition.X && cornerA.Y < input.MousePosition.Z)
                        {
                            if (cornerD.X > input.MousePosition.X && cornerD.Y > input.MousePosition.Z)
                            {

                                // menuEntry needs a double click
                                /*
                                if (selectedEntry == i)
                                {
                                    OnSelectEntry(selectedEntry);
                                }
                                else selectedEntry = i;
                                */

                                // menuEntry needs one click
                                selectedEntry = i;
                                OnSelectEntry(selectedEntry);
                            }
                        }
                        else continue;

                    }
                }

                // Move to the previous menu entry?
                if (input.IsMenuUp())
                {
                    //playing the sound
                    SoundEngine.Play2D("MenuAcceptSound", Global.SpeakerVolume / 10, false);

                    selectedEntry--;

                    if (selectedEntry < 0)
                        selectedEntry = MenuEntries.Count - 1;
                }

                // Move to the next menu entry?
                if (input.IsMenuDown())
                {
                    //playing the sound
                    SoundEngine.Play2D("MenuAcceptSound", Global.SpeakerVolume / 10, false);

                    selectedEntry++;

                    if (selectedEntry >= MenuEntries.Count)
                        selectedEntry = 0;
                }

                // Accept or cancel the menu.
                if (input.IsMenuSelect())
                {
                    Global.MusicEngine.StopAllSounds();
                    Global.Music = Global.MusicEngine.Play2D("Unrelenting", Global.MusicVolume / 10, false);
                    OnSelectEntry(selectedEntry);
                }
                else if (input.IsMenuCancel())
                {
                    OnCancel();
                    Global.MusicEngine.StopAllSounds();
                    Global.Music = Global.MusicEngine.Play2D("Unrelenting", Global.MusicVolume / 10, false);
                }
            }
        }

    }
}
