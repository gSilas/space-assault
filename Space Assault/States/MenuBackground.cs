using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Space_Assault.Entities;
using Space_Assault.Utils;
using Space_Assault.Effects;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// This Gamestate draws the background that can be used for every gamestate that is some type of menu and not gameplay
/// See MainMenu.Update() for how this works
/// </summary>
namespace Space_Assault.States
{
    class MenuBackground : IGameState, IUpdateableState, IDrawableState
    {

        //The particle engine object that we will manage our particles
        private ParticleEngine particleEngine;
        //The emitter for the particles. Just a point emitter
        private Vector2 emitter = new Vector2(Global.GraphicsManager.PreferredBackBufferWidth / 2, Global.GraphicsManager.PreferredBackBufferHeight / 2);

        // General
        private SoundEffectInstance _stationSound;

        // Sound
        List<SoundEffect> soundEffects;

        // 3D Model
        private Station _station;

        //constructor
        public MenuBackground()
        {
            soundEffects = new List<SoundEffect>();
            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 45, 60), new Vector3(-30, 0, 0), Vector3.Up);
            _station = new Station(Vector3.Zero, 0);
            IsStopped = false;
        }

        public void Initialize()
        {
            _station.Initialize();
        }

        public void LoadContent()
        {
            // Sound
            soundEffects.Add(Global.ContentManager.Load<SoundEffect>("Sounds/stationSound"));

            // Play that can be manipulated after the fact
            _stationSound = soundEffects[0].CreateInstance();
            _stationSound.Volume = 0.1f;
            _stationSound.IsLooped = true;
            _stationSound.Play();

            _station.LoadContent();

            // Load and add our particle texture to the Particle Engine
            Texture2D texture = Global.ContentManager.Load<Texture2D>("Effects/particle_texture");
            particleEngine = new ParticleEngine(texture, emitter);
        }

        public void Draw(GameTime elapsedTime)
        {
            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 45, 60), new Vector3(-30, 0, 0), Vector3.Up);
            // Drawing the particles 
            Global.BackgroundBatch.Begin();
            particleEngine.Draw(Global.BackgroundBatch);
            Global.BackgroundBatch.End();

            Global.GraphicsManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            _station.Draw();

        }

        public void Update(GameTime elapsedTime)
        {
            //3D Model
            _station.Update(elapsedTime);
            particleEngine.Update();
        }

        /// <summary>
        /// GameState stuff
        /// </summary>
        public void Kill()
        {
            IsStopped = true;
            _stationSound.Stop();
        }

        public void Resume()
        {
            if (IsStopped)
            {
                _stationSound.Resume();
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
