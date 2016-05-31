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

        public bool IsStopped { get; set; }

        public void Initialize()
        {
            highScore = new HighScoreList();
        }

        public void LoadContent()
        {

        }

        public void Update(GameTime elapsedTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {

            }
        }

        public void Draw(GameTime elapsedTime)
        {
            int zeilenAbstand = 50;
            int spaltenAbstand = 200;
            int spawnPointX = 400;
            int spawnPointY = 100;
            for(int i = 0; i < highScore.listLength; i++)
            {
                Global.SpriteBatch.DrawString(Global.Arial, (i+1) + ". Platz", new Vector2(spawnPointX, spawnPointY+i*zeilenAbstand), Color.Black);
                Global.SpriteBatch.DrawString(Global.Arial, highScore.scoresList[i].Name , new Vector2(spawnPointX + spaltenAbstand, spawnPointY + i * zeilenAbstand), Color.Black);
                Global.SpriteBatch.DrawString(Global.Arial, (highScore.scoresList[i].Points).ToString(), new Vector2(spawnPointX + spaltenAbstand*2, spawnPointY + i * zeilenAbstand), Color.Black);
            }
        }

        /// <summary>
        /// GameState stuff
        /// </summary>
        public void Kill()
        {

        }

        public void Resume()
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