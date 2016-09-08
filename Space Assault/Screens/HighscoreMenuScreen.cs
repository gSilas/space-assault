using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using IrrKlang;
using SpaceAssault.ScreenManagers;
using SpaceAssault.Utils;

/* TODO: it would be ideal if our textinput were eventbased: http://www.gamedev.net/topic/457783-xna-getting-text-from-keyboard/
 * Question: Do we really need it? Input doesnt have to be perfect, it just has to work.
 */

namespace SpaceAssault.Screens
{
    class HighscoreMenuScreen : MenuScreen
    {
        MenuEntry back;
        bool _enter;
        private bool _nextIterationFalse;

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
            _nextIterationFalse = false;
            back = new MenuEntry("Back");
            inputBoxWidth = 150;
            back.Selected += OnCancel;
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
                                    Global.HighScoreList.Add(EntryText, Global.HighScorePoints);
                                    EntryText = "";
                                    Global.HighScorePoints = 0;
                                    _nextIterationFalse = true;
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
                    SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
                    ISound Accept;
                    Accept = SoundEngine.Play2D(MenuAcceptSound, false, true, false);
                    Accept.Volume = Global.SpeakerVolume / 10;
                    Accept.Paused = false;

                    selectedEntry--;

                    if (selectedEntry < 0)
                        selectedEntry = MenuEntries.Count - 1;
                }

                // Move to the next menu entry?
                if (input.IsMenuDown())
                {
                    //playing the sound
                    SoundEngine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
                    ISound Accept;
                    Accept = SoundEngine.Play2D(MenuAcceptSound, false, true, false);
                    Accept.Volume = Global.SpeakerVolume / 10;
                    Accept.Paused = false;

                    selectedEntry++;

                    if (selectedEntry >= MenuEntries.Count)
                        selectedEntry = 0;
                }

                // Accept or cancel the menu.
                if (input.IsMenuSelect())
                {
                    OnSelectEntry(selectedEntry);
                }
                if (MenuEntries[selectedEntry].IsIncreasingSelect && input.IsMenuIncreasingSelect())
                {
                    OnSelectEntry(selectedEntry);
                }
                else if (input.IsMenuCancel())
                {
                    OnCancel();
                }
            }
        }

    }
}
