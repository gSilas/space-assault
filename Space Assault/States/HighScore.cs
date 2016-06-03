using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Assault.UI;
using Space_Assault.Utils;

/// <summary>
/// Dient zum laden und schreiben der Highscoreliste 
/// </summary>
/// TODO: Grafische Oberfläche der HighscoreList im Spiel
/// 
namespace Space_Assault.States
{

    class HighScore : IGameState, IUpdateableState, IDrawableState
    {
        private Button _hauptmenuButton;

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            Global.HighScore.LoadFile();
            _hauptmenuButton = new Button(Global.ContentManager.Load<Texture2D>("UI/hauptmenu"), new Vector2(100, 580));
        }

        public void Update(GameTime elapsedTime)
        {
            _hauptmenuButton.Update();

            if (_hauptmenuButton.Pressed)
            {
                Global.Controller.Push(Controller.EGameStates.MainMenu);
                Global.Controller.Pop(Controller.EGameStates.HighScore);
                Global.Controller.Reset(Controller.EGameStates.HighScoreEnter);
                Global.Controller.Pop(Controller.EGameStates.HighScoreEnter);
            }
        }

        public void Draw(GameTime elapsedTime)
        {
            int zeilenAbstand = 50;
            int spaltenAbstand = 200;
            int spawnPointX = 80;
            int spawnPointY = 80;
            for (int i = 0; i < Global.HighScore._listLength; i++)
            {
                Global.SpriteBatch.DrawString(Global.Arial, (i + 1) + ". Platz", new Vector2(spawnPointX, spawnPointY + i * zeilenAbstand), Color.BurlyWood);
                Global.SpriteBatch.DrawString(Global.Arial, Global.HighScore._scoresList[i].Name, new Vector2(spawnPointX + spaltenAbstand, spawnPointY + i * zeilenAbstand), Color.BurlyWood);
                Global.SpriteBatch.DrawString(Global.Arial, (Global.HighScore._scoresList[i].Points).ToString(), new Vector2(spawnPointX + spaltenAbstand * 2, spawnPointY + i * zeilenAbstand), Color.BurlyWood);
            }

            _hauptmenuButton.Draw();
        }
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