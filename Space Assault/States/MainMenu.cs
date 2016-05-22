
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
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
        private Controller sc;
        private GraphicsDeviceManager gm;
        private ContentManager cm;

        // Sound
        List<SoundEffect> soundEffects;

        // 3D Model
        private Camera _camera;
        private Station _station;
        private bool up;
        private float angle;
        private Vector3 position;
        private Model model;
        private SpriteBatch spriteBatch;
        Texture2D background;


        //#################################
        // Constructor
        //#################################
        public MainMenu(Controller controller)
        {
            sc = controller;
            gm = sc.gm;
            cm = sc.cm;
            soundEffects = new List<SoundEffect>();
            _camera = new Camera(800f/480f,10000f, MathHelper.ToRadians(45),1f, new Vector3(0, 45, 60), new Vector3(-30, 0, 0), Vector3.UnitY);
            _station = new Station(Vector3.Zero,0);

        }

        //#################################
        // LoadContent - Function
        //#################################
        public void LoadContent()
        {       
            // Sound
            soundEffects.Add(cm.Load<SoundEffect>("stationSound"));
            
            // Play that can be manipulated after the fact
            var instance = soundEffects[0].CreateInstance();
            instance.IsLooped = true;
            instance.Play();

            _station.LoadContent(cm);
                    
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
        }

        public void Initialize()
        {
            
        }
    }
}
