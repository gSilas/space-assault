using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Assault.Entities;
using Space_Assault.Utils;

namespace Space_Assault.States
{
    class EndlessMode : IGameState, IUpdateableState, IDrawableState
    {

        //#################################
        // Set Variables
        //#################################

        // General
        private Controller _sc;
        private GraphicsDeviceManager _gm;
        private ContentManager _cm;
        private SoundEffectInstance _stationSound;

        // Sound
        List<SoundEffect> soundEffects;

        // 3D Model
        private Camera _camera;
        private Station _station;
        private Asteroid _asteroid;
        private Drone _drone;
        private SpriteBatch _spriteBatch;
        private Texture2D _background;


            //#################################
            // Constructor
            //#################################
            public EndlessMode(Controller controller)
            {
                _sc = controller;
                _gm = _sc.gm;
                _cm = _sc.cm;
                soundEffects = new List<SoundEffect>();
                _camera = new Camera(_gm.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0,500,-500), new Vector3(0, 0, 0), Vector3.Up);
                _station = new Station(new Vector3(-20,0,20), 0);
                _asteroid = new Asteroid(Vector3.Zero, 0, Vector3.Forward, new Vector3(0.05f, 0.05f, 0.05f));
                IsStopped = false;
            }

        //#################################
        // LoadContent - Function
        //#################################
        public void LoadContent()
        {
            // Sound
            soundEffects.Add(_cm.Load<SoundEffect>("Sounds/stationSound"));

            // Play that can be manipulated after the fact
            _stationSound = soundEffects[0].CreateInstance();
            _stationSound.Volume = 0.1f;
            _stationSound.IsLooped = true;
            _stationSound.Play();

            _station.LoadContent(_cm);
            _asteroid.LoadContent(_cm);


        }

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

        //#################################
        // Draw - Function
        //#################################
        public void Draw(GameTime elapsedTime)
        {
            _station.Draw(_camera);
            _asteroid.Draw(_camera);
        }

        //#################################
        // Update - Function
        //#################################

        public void Update(GameTime elapsedTime)
        {
            //3D Model
            _station.Update(elapsedTime);
            _asteroid.Update(elapsedTime);
            //_drone.Update(elapsedTime);
            _asteroid.Move(Mousehandler.Position);
            Console.WriteLine(Mousehandler.Position.ToString());
            //Pop test
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _sc.Pop(this);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _sc.Push(Controller.EGameStates.EndlessModeScene);
            }
        }

        public bool IsStopped { get; set; }

        public void Initialize()
        {
            _station.Initialize();
            _asteroid.Initialize();
            //_drone.Initialize();
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
