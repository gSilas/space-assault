using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        HighScoreList highScore;

        public void Initialize()
        {
            highScore = new HighScoreList();
        }

        public void LoadContent()
        {

        }

        public void Update(GameTime elapsedTime)
        {

        }

        public void Draw(GameTime elapsedTime)
        {
            int zeilenAbstand = 50;
            int spaltenAbstand = 200;
            int spawnPointX = 80;
            int spawnPointY = 80;
            for(int i = 0; i < highScore._listLength; i++)
            {
                Global.SpriteBatch.DrawString(Global.Arial, (i+1) + ". Platz", new Vector2(spawnPointX, spawnPointY+i*zeilenAbstand), Color.BurlyWood);
                Global.SpriteBatch.DrawString(Global.Arial, highScore._scoresList[i].Name , new Vector2(spawnPointX + spaltenAbstand, spawnPointY + i * zeilenAbstand), Color.BurlyWood);
                Global.SpriteBatch.DrawString(Global.Arial, (highScore._scoresList[i].Points).ToString(), new Vector2(spawnPointX + spaltenAbstand*2, spawnPointY + i * zeilenAbstand), Color.BurlyWood);
            }
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