using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Screens;
using SpaceAssault.ScreenManagers;


namespace SpaceAssault
{
    public class SpaceAssaultGame : Game
    {
        ScreenManager screenManager;

        //#################################
        // Constructor
        //#################################
        public SpaceAssaultGame()
        {
            IsMouseVisible = false;
            Global.GraphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Global.ContentManager = Content;

            Global.GraphicsManager.PreferredBackBufferHeight = Global.PreferredBackBufferHeight;
            Global.GraphicsManager.PreferredBackBufferWidth = Global.PreferredBackBufferWidth;
            // Create the screen manager component.
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            Global.game = this;
        }


        protected override void LoadContent()
        {
            base.LoadContent();

            // Create a new SpriteBatch, which can be used to draw textures.
            Global.SpriteBatch = new SpriteBatch(Global.GraphicsManager.GraphicsDevice);
            Global.BackgroundBatch = new SpriteBatch(Global.GraphicsManager.GraphicsDevice);
            Global.UIBatch = new SpriteBatch(Global.GraphicsManager.GraphicsDevice);
            Global.Font = Global.ContentManager.Load<SpriteFont>("Fonts/modenine");
            Global.MenuFont= Global.ContentManager.Load<SpriteFont>("Fonts/modenine_big");

            Global.MusicEngine.AddSoundSourceFromFile("Unrelenting", "Content/Media/Music/Unrelenting.mp3");
            Global.MusicEngine.AddSoundSourceFromFile("SpaceFighterLoop", "Content/Media/Music/Space Fighter Loop.mp3");
            Global.MusicEngine.AddSoundSourceFromFile("ShinyTech2", "Content/Media/Music/Shiny Tech2.mp3");
            Global.MusicEngine.AddSoundSourceFromFile("CyborgNinja", "Content/Media/Music/Cyborg Ninja.mp3");
            Global.MusicEngine.AddSoundSourceFromFile("TruthOfTheLegend", "Content/Media/Music/Truth of the Legend_Cut.wav");
            Global.MusicEngine.AddSoundSourceFromFile("voice_loss", "Content/Media/Effects/Voice/voice_loss.mp3");
            Global.MusicEngine.AddSoundSourceFromFile("voice_win", "Content/Media/Effects/Voice/voice_win.mp3");
            Global.MusicEngine.AddSoundSourceFromFile("voice_intro", "Content/Media/Effects/Voice/voice_intro.mp3");

            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen());
            screenManager.AddScreen(new MainMenuScreen());
        }

        //#################################
        // Draw
        //#################################
        protected override void Draw(GameTime gameTime)
        {
            Global.GraphicsManager.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }
    }
}