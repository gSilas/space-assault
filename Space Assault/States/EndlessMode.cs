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
        private List<Asteroid> _asteroidList;
        private Camera _camera;
        private Station _station;
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

            _station = new Station(new Vector3(-20, 0, 20), 0);
            _drone = new Drone(Vector3.Zero, 0, Vector3.Forward, new Vector3(0.5f, 0.5f, 0.5f));
            _asteroidList = new List<Asteroid>();
            _camera = new Camera(_gm.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 500, 500), _drone.Position, Vector3.Up);
            IsStopped = false;
            Console.WriteLine("DisplayModeXY: " + "{" + _gm.PreferredBackBufferWidth.ToString() + " ;" +  _gm.PreferredBackBufferHeight.ToString() + "}");
            
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
            _drone.LoadContent(_cm);


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
            _drone.Draw(_camera);
            foreach (var asteroid in _asteroidList)
            {
                asteroid.Draw(_camera);
            }
            Console.WriteLine(elapsedTime.ElapsedGameTime.ToString());
        }

        //#################################
        // Update - Function
        //#################################

        public void Update(GameTime elapsedTime)
        {
            //3D Model
            _station.Update(elapsedTime);
            _drone.Update(elapsedTime);
            //_drone.Update(elapsedTime);

            Vector3 windowMidpoint = new Vector3(_gm.PreferredBackBufferWidth / 2.0f, 0, _gm.PreferredBackBufferHeight / 2.0f);
            foreach (var asteroid in _asteroidList)
            {
                asteroid.Update(elapsedTime);
            }
            _drone.Move(Mousehandler.Position - windowMidpoint);
            Console.WriteLine(Mousehandler.Position.ToString() + " ~~ CameraTarget: " + _camera.Position.ToString());
            //Pop test
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _sc.Pop(this);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _sc.Push(Controller.EGameStates.EndlessModeScene);
            }
            _camera.Target = _drone.Position;
            _camera.Position = _camera.Target + new Vector3(0, 500, 500);
        }

        public bool IsStopped { get; set; }

        public void Initialize()
        {
            _station.Initialize();
            _drone.Initialize();
            int i = 0;
            while (i <= 20)
            {
                Asteroid ast = new Asteroid(new Vector3(i*20,0,i*35),0,Vector3.Forward, 0.5f);
                ast.Initialize();
                ast.LoadContent(_cm);
                _asteroidList.Add(ast);
                
                i++;
            }
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
