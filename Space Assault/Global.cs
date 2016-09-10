﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Screens;
using SpaceAssault.Utils;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using IrrKlang;

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
        public static SpriteFont Font;
        public static SpriteFont MenuFont;

        //highscore points
        public static int HighScorePoints;
        public static int Money;

        //OPTIONS 
        // TODO: should be read from a settings file
        public static bool FrameCounterIsEnabled = false;
        public static float SpeakerVolume = 2;
        public static float MusicVolume = 1;

        //MUSIC
        public static ISoundEngine MusicEngine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);
        public static ISound Music;
        


        //RocketCount
        public static int NumberOfRockets=1;

        //SETTINGS
        public static int FleetSpawnTime = 5; // 1 Fleet/5 sec
        public static int MapRingRadius = 800;
        public static int MapSpawnRadius = MapRingRadius + 300;
        public static int MapDespawnRadius = MapSpawnRadius + 500;
        public static Vector3 CameraPosition = new Vector3(0,350,350);

        public static Color UIColor = new Color(30,110,166,255);
        //Color for Entity
        public static Color DroneColor = Color.Aquamarine;
        public static Color DroneBulletColor = Color.LightGreen;
        public static Color EnemyBulletColor = Color.LightGreen;
        public static Color EnemyColor = Color.Crimson;
        public static Color AsteroidColor = Color.LightGray;
        public static Color StationColor = new Color(82, 139, 139);

        public static SpaceAssaultGame game;
    }
}
