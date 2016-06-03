using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Space_Assault.Entities;
using Space_Assault.Utils;
using System.Diagnostics;
using Space_Assault.Effects;
using Space_Assault.Entities.Weapon;

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

        // Sound
        List<SoundEffect> soundEffects;

        // 3D Model
        private Station _station;
        private AsteroidBuilder _asteroidField;
        private Drone _drone;
        private Texture2D _background;
        private int _score;
        private List<Bullet> _removeBullets;
        private List<Asteroid> _removeAsteroid;


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
            // Sound
            soundEffects.Add(Global.ContentManager.Load<SoundEffect>("Sounds/stationSound"));

            // Play that can be manipulated after the fact
            _stationSound = soundEffects[0].CreateInstance();
            _stationSound.Volume = 0.1f;
            _stationSound.IsLooped = true;
            _stationSound.Play();

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
                            _removeAsteroid.Add(ast);
                            _removeBullets.Add(bullet);
                            _score+=50;
                        }
                    }
                    if (Collider3D.Intersection(ast, _station))
                    {
                        _removeAsteroid.Add(ast);
                        _station._health -= 34;
                    }
                    if (Collider3D.Intersection(ast, _drone))
                    {
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
                Global.HighScorePoints = 1500;
                _stationSound.Stop();
                Global.Controller.Push(Controller.EGameStates.MenuBackground);
                Global.Controller.Push(Controller.EGameStates.HighScore);
                Global.Controller.Push(Controller.EGameStates.HighScoreEnter);
                Global.Controller.Pop(Controller.EGameStates.EndlessModeScene);
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
