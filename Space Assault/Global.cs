using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.Utils;

/// <summary>
/// Everything in here has to have values assigned either in here or on it's first occurence
/// </summary>
namespace Space_Assault
{
    public static class Global
    {
        //core stuff
        public static SpriteBatch SpriteBatch;

        public static SpriteBatch BackgroundBatch;

        public static ContentManager ContentManager;

        public static GraphicsDeviceManager GraphicsManager;

        public static int PreferredBackBufferHeight = 768;
        public static int PreferredBackBufferWidth = 1366;

        public static Controller Controller;

        public static Camera Camera;

        public static HighScoreList HighScore = new HighScoreList();

        //fonts
        public static SpriteFont Arial;

        //highscore points
        public static int HighScorePoints;

    }
}
