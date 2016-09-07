using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Screens;
using SpaceAssault.Utils;
using System.Collections.Generic;

/// <summary>
/// Everything in here has to have values assigned either in here or on it's first occurence
/// </summary>
namespace SpaceAssault
{
    public static class Global
    {
        //core stuff
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
        public static int Money;

        //OPTIONS 
        // TODO: should be read from a settings file
        public static bool FrameCounterIsEnabled = false;
        public static float SpeakerVolume = 10;


        //SETTINGS
        public static int FleetSpawnTime = 5; // 1 Fleet/5 sec
        public static int AsteroidSpawnTime = 2; // 1/2 AsteroidChunk/sec
        public static int MapRingRadius = 800;
        public static int MapSpawnRadius = MapRingRadius + 300;
        public static int MapDespawnRadius = MapSpawnRadius + 500;
        public static Vector3 CameraPosition = new Vector3(0,350,350);

        public static Color UIColor = new Color(30,110,166,255);
    }
}
