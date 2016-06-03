using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.Entities;
using Space_Assault.Utils;
using System.Diagnostics;
using Space_Assault.Effects;
using Space_Assault.Entities.Weapon;
using Space_Assault.UI;

namespace Space_Assault.States
{
    class EndlessMode : IGameState, IUpdateableState, IDrawableState
    {

        //#################################
        // Set Variables
        //#################################

        private ParticleEngine _particleEngine;
        //The emitter for the particles. Just a point emitter
        private Vector2 _emitter = new Vector2(0, 0);

        // General
        private SoundEffectInstance _stationSound;
        private SoundEffectInstance _explosionSound;

        // Sound
        List<SoundEffect> soundEffects;

        // 3D Model
        private Station _station;
        private AsteroidBuilder _asteroidField;
        private Drone _drone;
        private Texture2D _background;
        private List<Bullet> _removeBullets;
        private List<Asteroid> _removeAsteroid;

        //UI
        List<Label> Labels = new List<Label>();

        //#################################
        // Constructor
        //#################################
        public EndlessMode()
        {
            soundEffects = new List<SoundEffect>();

            _station = new Station(new Vector3(0, 0, 0), 0);
            _drone = new Drone(new Vector3(0, 0, 20));
            _asteroidField = new AsteroidBuilder(new Vector3(500, 0, -500));
            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 500, 500), _drone.Position, Vector3.Up);
            _removeAsteroid = new List<Asteroid>();
            _removeBullets = new List<Bullet>();
            IsStopped = false;
        }

        //#################################
        // LoadContent - Function
        //#################################
        public void LoadContent()
        {
            //UI
            Labels.Add(new Label("Arial", "Life: ", 100, Global.GraphicsManager.PreferredBackBufferHeight - 100, Color.White));
            Labels.Add(new Label("Arial", "Score: ", 200, Global.GraphicsManager.PreferredBackBufferHeight - 100, Color.White));

            // Sound
            soundEffects.Add(Global.ContentManager.Load<SoundEffect>("Sounds/stationSound"));
            soundEffects.Add(Global.ContentManager.Load<SoundEffect>("Sounds/explosion"));

            // Play that can be manipulated after the fact
            _stationSound = soundEffects[0].CreateInstance();
            _stationSound.Volume = 0.1f;
            _stationSound.IsLooped = true;
            _stationSound.Play();

            _explosionSound = soundEffects[1].CreateInstance();
            _explosionSound.Volume = 0.3f;



            _station.LoadContent();
            _drone.LoadContent();
            Texture2D texture = Global.ContentManager.Load<Texture2D>("Effects/particle_texture");
            _particleEngine = new ParticleEngine(texture, new Vector2(0, Global.GraphicsManager.PreferredBackBufferHeight / 2));
        }

        public void Kill()
        {
            IsStopped = true;
            _stationSound.Stop();
            _drone._droneMoveSound.Stop();

        }

        public void Resume()
        {
            if (IsStopped)
            {
                _stationSound.Resume();
                IsStopped = false;
            }
        }

        //#################################
        // Draw - Function
        //#################################
        public void Draw(GameTime elapsedTime)
        {
            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 500, 500), _drone.Position, Vector3.Up);

            Global.BackgroundBatch.Begin();
            _particleEngine.Draw(Global.BackgroundBatch);
            Global.BackgroundBatch.End();
            Global.GraphicsManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            _station.Draw();
            _drone.Draw();
            _asteroidField.Draw();

            Labels[0].Draw(_drone._health);
            Labels[1].Draw(Global.HighScorePoints);

            
        }

        //#################################
        // Update - Function
        //#################################

        public void Update(GameTime elapsedTime)
        {
            //3D Model
            _station.Update(elapsedTime);
            _drone.Update(elapsedTime);

            Global.Camera.Target = _drone.Position;
            Global.Camera.Position = Global.Camera.Target + new Vector3(0, 500, 500);
            _asteroidField.Update(elapsedTime);

            _particleEngine.Update();

            //collision handling
            {
                foreach (var ast in _asteroidField.Asteroids)
                {
                    foreach (var bullet in _drone.GetBulletList())
                    {
                        if (Collider3D.Intersection(ast, bullet))
                        {
                            _explosionSound.Play();
                            _removeAsteroid.Add(ast);
                            _removeBullets.Add(bullet);
                            Global.HighScorePoints+=50;
                        }
                    }
                    if (Collider3D.Intersection(ast, _station))
                    {
                        _explosionSound.Play();
                        _removeAsteroid.Add(ast);
                        _station._health -= 34;
                    }
                    if (Collider3D.Intersection(ast, _drone))
                    {
                        _explosionSound.Play();
                        _removeAsteroid.Add(ast);
                        _drone._health -= 30;
                    }
                }
                foreach (var ast in _removeAsteroid)
                {
                    _asteroidField.Asteroids.Remove(ast);
                }
                foreach (var bullet in _removeBullets)
                {
                    _drone.GetBulletList().Remove(bullet);
                }
            }
            if(_drone._health <= 0 || _station._health <= 0)
            {
                _stationSound.Stop();
                Global.Controller.Push(Controller.EGameStates.MenuBackground);
                Global.Controller.Push(Controller.EGameStates.HighScore);
                Global.Controller.Push(Controller.EGameStates.HighScoreEnter);
                Global.Controller.Pop(Controller.EGameStates.EndlessModeScene);
                _station._health = 100;
                _drone._health = 100;
            }
        }

        public bool IsStopped { get; set; }

        public void Initialize()
        {
            _station.Initialize();
            _drone.Initialize();
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
