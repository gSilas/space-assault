using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceAssault.Entities;
using SpaceAssault.ScreenManagers;
using SpaceAssault.Utils;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;
using SpaceAssault.Screens.UI;
using IrrKlang;

namespace SpaceAssault.Screens
{
    class TutorialScreen : GameScreen
    {

        //#################################
        // Variables
        //#################################

        // Fade to black
        float _pauseAlpha;

        float _sphereAlpha;

        // Gameplay
        private Station _station;
        private Sphere _sphere;
        private DroneBuilder _droneFleet;
        public static int _stationHeight = 80;

        //UI + Frame + Background
        private InGameOverlay _ui;
        private Frame _frame;
        private Background _back;
        private UIItem _stationSymbol;

        ParticleSystem borderParticles;
        ParticleSystem explosionParticles;
        ParticleSystem dustParticles;
        // Sound
        private ISoundSource _explosionSource;
        private ISoundSource _explosionSource1;
        private ISoundSource _explosionSource2;
        private ISoundSource _explosionSource3;

        private ISoundEngine _engine;
        private ISoundSource _openShop;

        Point dialogPos;
        bool movementAllowed;
        Dialog tutorialDialog;
        string tutorialMessage;
        int nextIndex = 0;
        AsteroidBuilder _asteroidField;
        public SortedDictionary<int, string> TutorialText = new SortedDictionary<int, string>();

        List<ExplosionSystem> explosionList = new List<ExplosionSystem>();
        private double _duration = 0.4;


        // The explosions effect works by firing projectiles up into the
        // air, so we need to keep track of all the active projectiles.
        List<Projectile> projectiles = new List<Projectile>();
        TimeSpan timeToNextProjectile = TimeSpan.Zero;

        // Random number generator for the fire effect.
        Random random = new Random();

        //#################################
        // Constructor
        //#################################
        public TutorialScreen()
        {
            explosionParticles = new ShipExplosionSettings();
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            //actual gameplay objects
            _station = new Station(new Vector3(0, _stationHeight, 0), 0);
            _sphere = new Sphere(new Vector3(0, _stationHeight / 2, 0), 0);
            _droneFleet = new DroneBuilder();
            movementAllowed = false;
            Global.Money = 1000;
            _sphereAlpha = 0.1f;
            //UI + Frame + BG 
            _ui = new InGameOverlay(_station);
            _back = new Background();
            _frame = new Frame();
            dustParticles = new DustParticleSystem();
            tutorialDialog = new Dialog(0, 0, 80, 400, 8, false, true);
            borderParticles = new BorderParticleSettings();
            _asteroidField = new AsteroidBuilder();
            TutorialText.Add(TutorialText.Count, "Do you want to skip the tutorial? (Y/N) \n\nYour Mission will begin immediately if \nyou choose to skip.");
            TutorialText.Add(TutorialText.Count, "This is your drone!\nPress [Space] to\nContinue!");
            TutorialText.Add(TutorialText.Count, "You can go back with\n[Back]!\nTry it!");
            TutorialText.Add(TutorialText.Count, "You have to defend the\nstation against waves of\nenemys!");
            TutorialText.Add(TutorialText.Count, "The red bar is your\ndronehull and the\nblue bar is your shield!");
            TutorialText.Add(TutorialText.Count, "The green bar is the\nstationhull. The white\nbar is the stationshield!");
            TutorialText.Add(TutorialText.Count, "Fragments are the common\ncurrency on this station.");
            TutorialText.Add(TutorialText.Count, "You score is your rating!\nShooting stuff increases\nyour score!");
            TutorialText.Add(TutorialText.Count, "The armor icon shows your\ncurrent amount of armor!\nArmor reduces incoming\nDamage.");
            TutorialText.Add(TutorialText.Count, "You can buy upgrades in\nthe stations store for\nfragments.");
            TutorialText.Add(TutorialText.Count, "W - fly upward\nA - fly left\nS - fly downward\nD - fly right");
            TutorialText.Add(TutorialText.Count, "Left Mouse Button\n-> Shoots your Laser\nRight Mouse Button\n-> Shoots your Torpedo\n");
            TutorialText.Add(TutorialText.Count, "I'll now give you your\ncontrols back! But stay\nin radio range.");
            TutorialText.Add(TutorialText.Count, "I also heard of incoming\nasteroids. The comets\nwith the blue trail\noffer a nice income.");
            TutorialText.Add(TutorialText.Count, "Press [X] to start your\nmission!");

            TutorialText.TryGetValue(0, out tutorialMessage);
        }


        //#################################
        // LoadContent
        //#################################
        public override void LoadContent()
        {
            _stationSymbol = new UIItem();
            _stationSymbol.LoadContent("Images/station_icon");
            _droneFleet.addDrone(new Vector3(150, 0, 100));
            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, Global.CameraPosition, _droneFleet.GetActiveDrone().Position, Vector3.Up);
            _station.LoadContent();
            _sphere.LoadContent();
            _ui.LoadContent(_droneFleet);
            _frame.LoadContent();
            tutorialDialog.LoadContent();

            _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);

            _explosionSource = _engine.AddSoundSourceFromFile("Content/Media/Effects/Explosion.wav", StreamMode.AutoDetect, true);
            _explosionSource1 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/Explosion1.wav", StreamMode.AutoDetect, true);
            _explosionSource2 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/Explosion2.wav", StreamMode.AutoDetect, true);
            _explosionSource3 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/Explosion3.wav", StreamMode.AutoDetect, true);

            _openShop = _engine.AddSoundSourceFromFile("Content/Media/Effects/OkClick.wav", StreamMode.AutoDetect, true);

            _asteroidField.LoadContent();

        }

        //#################################
        // UnloadContent
        //#################################
        public override void UnloadContent()
        {
            _engine.StopAllSounds();
            _engine.Dispose();
        }

        //#################################
        // Update
        //#################################
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (_sphereAlpha > 0.1f)
                _sphereAlpha -= 0.001f;

            dustParticles.Update(gameTime);
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);



            foreach (ExplosionSystem explosion in explosionList)
            {
                explosion.Update(gameTime);
            }

            // calling update of objects where necessary
            _station.Update(gameTime);
            if (movementAllowed)
            {
                _droneFleet.Update(gameTime);
                _asteroidField.Update(gameTime, _station.Position);
            }

            Global.Camera.updateCameraPositionTarget(_droneFleet.GetActiveDrone().Position + Global.CameraPosition, _droneFleet.GetActiveDrone().Position);

            // if station dies go back to MainMenu
            // TODO: change to EndScreen and HighScore list)
            if (_station._health <= 0)
                LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen());
            explosionParticles.Update(gameTime);
            CollisionHandling(gameTime);
            UpdateBorder(gameTime);

            // drone is dead 
            if (!_droneFleet.GetActiveDrone().IsNotDead)
                LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen());
        }

        //#################################
        // Handle input
        //#################################
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the player.
            KeyboardState keyboardState = input.CurrentKeyboardState;

            TutorialText.TryGetValue(nextIndex, out tutorialMessage);
            if (nextIndex == 0)
            {
                if (input.IsNewKeyPress(Keys.Y))
                {
                    ScreenManager.AddScreen(new GameplayScreen());
                    ScreenManager.RemoveScreen(this);
                }
                else if (input.IsNewKeyPress(Keys.N))
                {
                    //playing the sound
                    _engine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
                    ISound Open;
                    Open = _engine.Play2D(_openShop, false, true, false);
                    Open.Volume = Global.SpeakerVolume / 10;
                    Open.Paused = false;

                    nextIndex++;
                    TutorialText.TryGetValue(nextIndex, out tutorialMessage);
                }
            }
            else
            {
                // key for the ship etc.
                if (input.IsNewKeyPress(Keys.B))
                {
                    if ((Vector3.Distance(_station.Position, _droneFleet.GetActiveDrone().Position) - _stationHeight) < 150)
                    {
                        //playing the sound
                        _engine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
                        ISound Open;
                        Open = _engine.Play2D(_openShop, false, true, false);
                        Open.Volume = Global.SpeakerVolume / 10;
                        Open.Paused = false;
                        ScreenManager.AddScreen(new ShopScreen(_droneFleet, _station));
                    }

                }
                if (input.IsNewKeyPress(Keys.Space) && (nextIndex + 1) < TutorialText.Count && (nextIndex + 1) > 0)
                {
                    //playing the sound
                    _engine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
                    ISound Open;
                    Open = _engine.Play2D(_openShop, false, true, false);
                    Open.Volume = Global.SpeakerVolume / 10;
                    Open.Paused = false;

                    nextIndex++;
                    TutorialText.TryGetValue(nextIndex, out tutorialMessage);
                    if (nextIndex == TutorialText.Count - 1)
                        movementAllowed = true;
                }
            }

            if (input.IsNewKeyPress(Keys.Back) && nextIndex < TutorialText.Count && (nextIndex - 1) >= 0)
            {
                //playing the sound
                _engine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
                ISound Open;
                Open = _engine.Play2D(_openShop, false, true, false);
                Open.Volume = Global.SpeakerVolume / 10;
                Open.Paused = false;

                nextIndex--;
                TutorialText.TryGetValue(nextIndex, out tutorialMessage);
            }
            if (movementAllowed)
            {
                if (input.IsNewKeyPress(Keys.X))
                {
                    ScreenManager.AddScreen(new GameplayScreen());
                    ScreenManager.RemoveScreen(this);
                }

            }
            //player hits ESC it pauses the game
            if (input.IsPauseGame())
            {
                ScreenManager.AddScreen(new PauseMenuScreen());
            }
        }


        //#################################
        // Draw
        //#################################
        public override void Draw(GameTime gameTime)
        {
            Global.GraphicsManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // calling draw of objects where necessary
            _back.Draw(90, new Vector3(-5000, -2500, -5000));
            _station.Draw(Global.StationColor);
            _droneFleet.Draw();

            foreach (ExplosionSystem explosion in explosionList)
            {
                explosion.Draw();
            }

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
            if (movementAllowed)
            {
                _asteroidField.Draw();
            }
            borderParticles.Draw();
            dustParticles.Draw();
            DrawDirectionArrow();
            DrawDialog();
            explosionParticles.Draw();

            if (_station._shield > 0)
                _sphere.Draw(new Color(255, 255, 255), _sphereAlpha);

            //FRAME & UI ALWAYS LAST
            _ui.Draw(_droneFleet);
            _frame.Draw();
        }

        //###################################################################################################
        // Helper
        //###################################################################################################

        //#################################
        // Helper Draw - Arrow
        //#################################
        void DrawDirectionArrow()
        {
            if (Vector3.Distance(_droneFleet.GetActiveDrone().Position, _station.Position) > 300)
            {
                var vec3 = _station.Position - _droneFleet.GetActiveDrone().Position;
                vec3.Normalize();
                var vec = new Vector2();
                vec.X = Global.GraphicsManager.GraphicsDevice.Viewport.Project(_droneFleet.GetActiveDrone().Position + vec3 * 100, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity).X;
                vec.Y = Global.GraphicsManager.GraphicsDevice.Viewport.Project(_droneFleet.GetActiveDrone().Position + vec3 * 100, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity).Y;
                _stationSymbol.Draw(vec.ToPoint(), 1, Color.White);
            }
        }

        //#################################
        // Helper RndPoint
        //#################################
        Vector3 RandomPointOnCircle(float radius, float height)
        {
            double angle = random.NextDouble() * Math.PI * 2;
            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);
            return new Vector3(x * radius, 0, y * radius + height);
        }

        //#################################
        // Helper Update - Fire
        //#################################
        void UpdateBorder(GameTime gameTime)
        {
            const int borderParticlesPerFrame = 100;
            // Create a number of fire particles, randomly positioned around a circle.
            for (int i = 0; i < borderParticlesPerFrame; i++)
            {
                borderParticles.AddParticle(RandomPointOnCircle(Global.MapRingRadius, 40), Vector3.Zero);
            }
            // Create one smoke particle per frmae, too.
            //SmokeParticles.AddParticle(RandomPointOnCircle(), Vector3.Zero);
            borderParticles.Update(gameTime);
        }

        void DrawDialog()
        {
            dialogPos.X = (int)Global.GraphicsManager.GraphicsDevice.Viewport.Project(_droneFleet.GetActiveDrone().Position, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity).X + 32;
            dialogPos.Y = (int)Global.GraphicsManager.GraphicsDevice.Viewport.Project(_droneFleet.GetActiveDrone().Position, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity).Y + 32;
            tutorialDialog.Draw(dialogPos, tutorialMessage);
        }
        //#################################
        // Sound
        //#################################
        protected void PlayExplosionSound(Vector3D pos)
        {
            var curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
            _engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
            Random _rand = new Random(); ;
            switch (_rand.Next(0, 3))
            {
                case 0:
                    var _explosionSound = _engine.Play3D(_explosionSource, pos.X, pos.Y, pos.Z, false, true, true);
                    _explosionSound.Volume = Global.SpeakerVolume / 10;
                    _explosionSound.Paused = false;
                    break;
                case 1:
                    var _explosionSound1 = _engine.Play3D(_explosionSource1, pos.X, pos.Y, pos.Z, false, true, true);
                    _explosionSound1.Volume = Global.SpeakerVolume / 10;
                    _explosionSound1.Paused = false;
                    break;
                case 2:
                    var _explosionSound2 = _engine.Play3D(_explosionSource2, pos.X, pos.Y, pos.Z, false, true, true);
                    _explosionSound2.Volume = Global.SpeakerVolume / 10;
                    _explosionSound2.Paused = false;
                    break;
                case 3:
                    var _explosionSound3 = _engine.Play3D(_explosionSource3, pos.X, pos.Y, pos.Z, false, true, true);
                    _explosionSound3.Volume = Global.SpeakerVolume / 10;
                    _explosionSound3.Paused = false;
                    break;
            }

        }

        //#################################
        // Collision
        //#################################
        void CollisionHandling(GameTime gameTime)
        {
            //remove lists for collisions etc 
            List<Asteroid> _removeAsteroid = new List<Asteroid>();
            List<Bullet> _removeBullets = new List<Bullet>();

            /* asteroids with drone(& its bullets) & station & other asteroids & enemy ships (& its bullets)*/
            foreach (var ast in _asteroidField._asteroidList)
            {
                if (Vector3.Distance(ast.Position, _station.Position) > Global.MapDespawnRadius)
                {
                    explosionList.Add(new ExplosionSystem(new AsteroidExplosionSettings(), ast.Position, _duration));
                    _removeAsteroid.Add(ast);
                    continue;
                }

                if (Collider3D.IntersectionSphere(ast, _droneFleet.GetActiveDrone()))
                {
                    _droneFleet.GetActiveDrone().getHit(5);
                    dustParticles.AddParticle(ast.Position, Vector3.Zero);
                    _removeAsteroid.Add(ast);

                    PlayExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));
                    continue;
                }

                if (Collider3D.IntersectionSphere(_station, ast))
                {
                    explosionList.Add(new ExplosionSystem(new AsteroidExplosionSettings(), ast.Position, _duration));
                    ast.IsDead = true;
                    _removeAsteroid.Add(ast);
                    _station.getHit(10);
                    continue;
                }

                foreach (var ast2 in _asteroidField._asteroidList)
                {
                    if (ast != ast2 && Collider3D.IntersectionSphere(ast2, ast))
                    {
                        var newDirection = new Vector3();
                        ast.Reflect(ast2.Direction, ast2.Spheres[0].Center, out newDirection);
                        ast2.Direction = newDirection;
                        dustParticles.AddParticle(ast.Position, Vector3.Zero);
                        //ast.Position += ast.Direction * (2 * ((float)random.NextDouble()) * ast.MaxRadius() + ast.MaxRadius());
                    }
                }
                foreach (var bullet in _droneFleet._bulletList)
                {
                    if (Collider3D.IntersectionSphere(bullet, ast))
                    {
                        explosionList.Add(new ExplosionSystem(new AsteroidExplosionSettings(), ast.Position, _duration));
                        _removeAsteroid.Add(ast);
                        _removeBullets.Add(bullet);

                        PlayExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));
                        if (bullet._bulletType == Bullet.BulletType.BigJoe)
                        {
                            explosionList.Add(new ExplosionSystem(new BombExplosionSettings(), new BombRingExplosionSettings(), ast.Position, _duration, 50, true));
                        }
                        if (ast.IsShiny)
                        {
                            Global.Money += 200;
                        }
                        break;
                    }
                }

            }

            foreach (var ast in _removeAsteroid)
            {
                _asteroidField._asteroidList.Remove(ast);
            }

            foreach (var bullet in _removeBullets)
            {
                _droneFleet._bulletList.Remove(bullet);
            }

        }

        //#################################
        // Helper Update - Explosion
        //#################################
        void UpdateExplosions(GameTime gameTime, Vector3 position, Vector3 velocity)
        {
            // Create a new projectile once per second
            //projectiles.Add(new Projectile(explosionParticles,explosionSmokeParticles,projectileTrailParticles));
            explosionParticles.AddParticle(position, velocity);
        }

    }
}

