using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Assault.Entities;
using Space_Assault.Utils;
using System.Diagnostics;

namespace Space_Assault.States
{
    class EndlessMode : IGameState, IUpdateableState, IDrawableState
    {

        //#################################
        // Set Variables
        //#################################

        // General
        private SoundEffectInstance _stationSound;

        // Sound
        List<SoundEffect> soundEffects;

        // 3D Model
        private Station _station;
        private AsteroidBuilder _asteroidField;
        private Drone _drone;
        private Texture2D _background;


        //#################################
        // Constructor
        //#################################
        public EndlessMode()
        {
            soundEffects = new List<SoundEffect>();

            _station = new Station(new Vector3(0, 0, 0), 0);
            _drone = new Drone(new Vector3(0,0,20));
            _asteroidField = new AsteroidBuilder(20,0,_station.Position);
            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 500, 500), _drone.Position, Vector3.Up);

            IsStopped = false;
            Debug.WriteLine("DisplayModeXY: " + "{" + Global.GraphicsManager.PreferredBackBufferWidth.ToString() + " ;" +  Global.GraphicsManager.PreferredBackBufferHeight.ToString() + "}");
            
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
            _asteroidField.LoadContent();
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

            _drone.turn(new Vector3(Global.GraphicsManager.PreferredBackBufferWidth / 2.0f, 0, Global.GraphicsManager.PreferredBackBufferHeight / 2.0f)- MouseHandler.Position);
            //Console.WriteLine(Mousehandler.Position.ToString() + " ~~ CameraTarget: " + _camera.Position.ToString());
            //Pop test
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Global.Controller.Pop(this);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Global.Controller.Push(Controller.EGameStates.EndlessModeScene);
            }
            Global.Camera.Target = _drone.Position;
            Global.Camera.Position = Global.Camera.Target + new Vector3(0, 500, 500);
            _asteroidField.Update(elapsedTime);
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
