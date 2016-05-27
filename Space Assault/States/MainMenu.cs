
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Space_Assault.Entities;
using Space_Assault.Utils;

namespace Space_Assault.States
{
    class MainMenu : IGameState, IUpdateableState, IDrawableState
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
        private SpriteBatch _spriteBatch;
        private Texture2D _background;


        //#################################
        // Constructor
        //#################################
        public MainMenu(Controller controller)
        {
            _sc = controller;
            _gm = _sc.gm;
            _cm = _sc.cm;
            soundEffects = new List<SoundEffect>();
            _camera = new Camera(800f / 480f, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 45, 60), new Vector3(-30, 0, 0), Vector3.Up);
            _station = new Station(Vector3.Zero, 0);
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
        }

        //#################################
        // Update - Function
        //#################################

        public void Update(GameTime elapsedTime)
        {
            //3D Model
            _station.Update(elapsedTime);

            //Pop test
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _sc.Pop(this);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _sc.Push(Controller.EGameStates.EndlessModeScene);
                _sc.Pop(this);
            }
        }

        public bool IsStopped { get; set; }

        public void Initialize()
        {
            _station.Initialize();
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
