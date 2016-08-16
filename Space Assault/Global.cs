using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Utils;

/// <summary>
/// Everything in here has to have values assigned either in here or on it's first occurence
/// </summary>
namespace SpaceAssault
{
    public static class Global
    {
        //core stuff
        public static SpaceAssaultGame SpaceAssault;

        public static SpriteBatch SpriteBatch;

        public static SpriteBatch BackgroundBatch;

        public static SpriteBatch UIBatch;

        public static ContentManager ContentManager;

        public static GraphicsDeviceManager GraphicsManager;

        public static int PreferredBackBufferHeight = 800;
        public static int PreferredBackBufferWidth = 1280;

        public static Camera Camera;

        public static HighScoreList HighScoreList = new HighScoreList();

        //fonts
        public static SpriteFont GameFont;
        public static SpriteFont DialogFont;

        //highscore points
        public static int HighScorePoints;

    }
}
