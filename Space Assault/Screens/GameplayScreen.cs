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
using IrrKlang;
using SpaceAssault.Screens.UI;
using System.Diagnostics;

namespace SpaceAssault.Screens
{
    class GameplayScreen : GameScreen
    {
        //#################################
        // Variables
        //#################################

        // Fade to black
        float _deadDroneAlpha;
        float _actualDeadDroneAlpha;
        float _pauseAlpha;

        float _sphereAlpha;

        // Gameplay
        private Station _station;
        private Sphere _sphere;
        private AsteroidBuilder _asteroidField;
        private DroneBuilder _droneFleet;
        private int _deathCounter = 0;
        public static int _stationHeight = 80;
        private WaveBuilder _waveBuilder;

        private InputState _input;


        //UI + Frame + Background
        private InGameOverlay _ui;
        private Frame _frame;
        private Background _back;
        private UIItem _stationSymbol;
        private UIItem _enemySymbol;

        // Sound
        private ISoundSource _explosionSource;
        private ISoundSource _explosionSource1;
        private ISoundSource _explosionSource2;
        private ISoundSource _explosionSource3;
        private ISoundSource _openShop;
        private ISoundSource _hitSound;
        private ISoundEngine _engine;

        //Particle
        ParticleSystem borderParticles;
        ParticleSystem dustParticles;
        ParticleSystem hitmarkerParticles;

        List<ExplosionSystem> explosionList = new List<ExplosionSystem>();
        List<ExplosionSystem> explosionRemoveList = new List<ExplosionSystem>();

        //Effects
        private Effect _stationEffect;

        // Keep track of all the active projectiles
        List<Projectile> projectiles = new List<Projectile>();
        TimeSpan timeToNextProjectile = TimeSpan.Zero;

        // Random number generator
        Random random;

        //Created screens
        PauseMenuScreen pause;
        ShopScreen shop;

        //#################################
        // Constructor
        //#################################
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            //actual gameplay objects
            _station = new Station(new Vector3(0, _stationHeight, 0), 0);
            _sphere = new Sphere(new Vector3(0, _stationHeight / 2, 0), 0);
            _asteroidField = new AsteroidBuilder();
            _droneFleet = new DroneBuilder();

            _sphereAlpha = 0.1f;
            random = new Random();
            _waveBuilder = new WaveBuilder(TimeSpan.FromSeconds(10d), 15);
            Global.Money = 0;
            //UI + Frame + BG 
            _ui = new InGameOverlay(_station);
            _back = new Background();
            _frame = new Frame();
            Global.HighScorePoints = 0;
            Global.Money = 0;
            _input = new InputState();

            _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);

            // Construct Particles
            borderParticles = new BorderParticleSettings();
            dustParticles = new DustParticleSystem();
            hitmarkerParticles = new HitMarkerParticleSystem();
        }


        //#################################
        // LoadContent
        //#################################
        public override void LoadContent()
        {
         
            _stationSymbol = new UIItem();
            _enemySymbol = new UIItem();
            _stationSymbol.LoadContent("Images/station_icon");
            _enemySymbol.LoadContent("Images/enemy_icon");
            _droneFleet.addDrone(new Vector3(150, 0, 100));
            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, Global.CameraPosition, _droneFleet.GetActiveDrone().Position, Vector3.Up);
            _station.LoadContent();
            _sphere.LoadContent();
            _asteroidField.LoadContent();
            _ui.LoadContent(_droneFleet);
            _frame.LoadContent();
            _waveBuilder.LoadContent();
            
            //Effects
            _stationEffect = Global.ContentManager.Load<Effect>("Effects/stationEffect");

            //Sounds
           
            Global.Music = _engine.Play2D("Content/Media/Effects/Objects/Explosion3.wav", false);
            Global.Music.Volume = Global.MusicVolume / 10;

            _openShop = _engine.AddSoundSourceFromFile("Content/Media/Effects/OpenShop.wav", StreamMode.AutoDetect, true);
            _explosionSource = _engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/Explosion4.wav", StreamMode.AutoDetect, true);
            _explosionSource1 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/Explosion1.wav", StreamMode.AutoDetect, true);
            _explosionSource2 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/Explosion2.wav", StreamMode.AutoDetect, true);
            _explosionSource3 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/Explosion3.wav", StreamMode.AutoDetect, true);
            _hitSound = _engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/GetHitShips.wav", StreamMode.AutoDetect, true);
        }

        //#################################
        // UnloadContent
        //#################################
        public override void UnloadContent()
        {
            //Global.ContentManager.Unload();
            
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
            SoundDJ();

            if (_sphereAlpha > 0.1f)
                _sphereAlpha -= 0.001f;

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            DebugFunctions();

            //boids
            _waveBuilder.Update(gameTime, ref _asteroidField, ref _droneFleet);


            // calling update of objects where necessary
            _station.Update(gameTime);
            _droneFleet.Update(gameTime);
            _asteroidField.Update(gameTime, _droneFleet.GetActiveDrone().Position);
            _input.Update();
            Global.Camera.updateCameraPositionTarget(_droneFleet.GetActiveDrone().Position + Global.CameraPosition, _droneFleet.GetActiveDrone().Position);


            // Particles
            dustParticles.Update(gameTime);
            UpdateBorder(gameTime);
            hitmarkerParticles.Update(gameTime);

            foreach (ExplosionSystem explosion in explosionList)
            {
                explosion.Update(gameTime);
                if (explosion._state == 2)
                {
                    explosionRemoveList.Add(explosion);
                }
            }

            foreach (var explosion in explosionRemoveList)
            {
                explosionList.Remove(explosion);
            }
            explosionRemoveList.Clear();


            if (_waveBuilder.HasEnded)
                LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen(), new HighscoreMenuScreen(true));
            // if station dies go back to MainMenu
            // TODO: change to EndScreen and HighScore list)
            if (_station._health <= 0)
                LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen(), new HighscoreMenuScreen(true));

            CollisionHandling(gameTime);


            // fading out/in when drone is dead & alive again
            if (!_droneFleet.GetActiveDrone().IsNotDead)
                _deadDroneAlpha = Math.Min(_deadDroneAlpha + 1f / 32, 1);
            else
                _deadDroneAlpha = Math.Max(_deadDroneAlpha - 1f / 32, 0);

            // if fading out is max, respawn
            if (_actualDeadDroneAlpha >= 1f)
            {
                _droneFleet.GetActiveDrone().Reset();
                _deathCounter++;
            }
            
        }

        //#################################
        // Everything related to Debugstuff/input etc. here (only works in debug mode)
        //#################################
        [Conditional("DEBUG")]
        public void DebugFunctions()
        {
            if (_input.IsNewKeyPress(Keys.L) && _input.IsNewKeyPress(Keys.K))
            {
                Global.Money += 10000;
            }

            if (_input.IsNewKeyPress(Keys.F) && _input.IsNewKeyPress(Keys.G))
            {
                _station._health -= 1000;
            }
            if (_input.IsNewKeyPress(Keys.F1))
            {
                Console.WriteLine("Drone Damage: " + _droneFleet.GetActiveDrone().makeDmg);
            }
            if (_input.IsNewKeyPress(Keys.F2))
            {
                Console.WriteLine("Drone Shield: " + _droneFleet.GetActiveDrone().shield);
            }
            if (_input.IsNewKeyPress(Keys.F3))
            {
                Console.WriteLine("Drone Health: " + _droneFleet.GetActiveDrone().health);
            }
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
                    if (shop == null)
                        shop = new ShopScreen(_droneFleet, _station);
                    ScreenManager.AddScreen(shop);
                    //LoadingScreen.Load(this.ScreenManager, false, new ShopScreen(_droneFleet, _station));
                }

            }

            //player hits ESC it pauses the game
            if (input.IsPauseGame())
            {
                //playing the sound
                
                _engine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
                ISound Open;
                Open = _engine.Play2D(_openShop, false, true, false);
                Open.Volume = Global.SpeakerVolume / 10;
                Open.Paused = false;
                if (pause == null)
                    pause = new PauseMenuScreen();
                ScreenManager.AddScreen(pause);
                //LoadingScreen.Load(this.ScreenManager, false, new PauseMenuScreen());
            }
        }


        //#################################
        // Draw
        //#################################
        public override void Draw(GameTime gameTime)
        {
            Global.GraphicsManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // calling draw of objects where necessary
            _back.Draw(90, new Vector3(-15000, -2000, -15000));
            _station.Draw(Global.StationColor);
            _droneFleet.Draw();
            _asteroidField.Draw();
            _waveBuilder.Draw(gameTime);

            // Particle
            dustParticles.Draw();
            borderParticles.Draw();
            hitmarkerParticles.Draw();

            foreach (ExplosionSystem explosion in explosionList)
            {
                explosion.Draw();
            }


            //if drone is dead fade to black
            if (_deadDroneAlpha > 0)
            {
                _actualDeadDroneAlpha = MathHelper.Lerp(0f, 2f, _deadDroneAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(_actualDeadDroneAlpha);
            }


            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }

            DrawStationDirectionArrow();
            DrawShipDirectionArrow();

            if (_station._shield > 0)
                _sphere.Draw(new Color(255, 255, 255), _sphereAlpha);

            //FRAME & UI ALWAYS LAST
            _ui.Draw(_droneFleet);
            _frame.Draw();
        }

        //#################################
        // Sound
        //#################################
        protected void PlayExplosionSound(Vector3D pos)
        {
            var curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
            _engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
            int a = random.Next(0, 3);
            switch (a)
            {
                case 0:
                    var explosionSound = _engine.Play2D(_explosionSource, false, true, false);
                    explosionSound.Volume = Global.SpeakerVolume / 10;
                    explosionSound.Paused = false;
                    break;
                case 1:
                    var explosionSound1 = _engine.Play2D(_explosionSource1,false, true, false);
                    explosionSound1.Volume = Global.SpeakerVolume / 10;
                    explosionSound1.Paused = false;
                    break;
                case 2:
                    var explosionSound2 = _engine.Play2D(_explosionSource2,  false, true, false);
                    explosionSound2.Volume = Global.SpeakerVolume / 10;
                    explosionSound2.Paused = false;
                    break;
                case 3:
                    var explosionSound3 = _engine.Play2D(_explosionSource3, false, true, false);
                    explosionSound3.Volume = Global.SpeakerVolume / 10;
                    explosionSound3.Paused = false;
                    break;
              
            }
            Console.WriteLine(a);
        }
        protected void PlayShipHitSound(Vector3D pos)
        {
            var curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
            _engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
            ISound Hit = _engine.Play2D(_hitSound, false, true, false);
            Hit.Volume = Global.SpeakerVolume / 10;
            Hit.Paused = false;
        }

        //#################################
        // Collision
        //#################################
        void CollisionHandling(GameTime gameTime)
        {
            //remove lists for collisions etc 
            List<Asteroid> _removeAsteroid = new List<Asteroid>();
            List<Bullet> _removeBullets = new List<Bullet>();

            /* bullet of drone with enemy ships */
            foreach (var bullet in _droneFleet._bulletList)
            {
                if (Collider3D.BoundingFrustumIntersection(bullet))
                {
                    foreach (var ship in _waveBuilder.ShipList)
                    {
                        if (Collider3D.BoundingFrustumIntersection(bullet))
                        {
                            if (Collider3D.IntersectionSphere(bullet, ship))
                            {
                                if (bullet._bulletType == Bullet.BulletType.BigJoe)
                                {
                                    explosionList.Add(new ExplosionSystem(new BombExplosionSettings(), new BombRingExplosionSettings(), ship.Position, 0.4, 50, true));
                                }
                                ship.getHit(bullet.makeDmg);
                                //PlayShipHitSound(new Vector3D(ship.Position.X, ship.Position.Y, ship.Position.Z));
                                _removeBullets.Add(bullet);
                                Global.HighScorePoints += 20;
                                if (ship.Health > 0)
                                {
                                    PlayShipHitSound(new Vector3D(ship.Position.X, ship.Position.Y, ship.Position.Z));
                                    hitmarkerParticles.AddParticle(bullet.Position, Vector3.Zero);
                                }
                                else if (ship.Health <= 0)
                                {
                                    if (ship.GetType() == typeof(EnemyBomber))
                                        explosionList.Add(new ExplosionSystem(new ShipBigExplosionSettings(), new ShipRingExplosionSettings(), ship.Position, 0.4, 50, true));
                                    else
                                        explosionList.Add(new ExplosionSystem(new ShipExplosionSettings(), new ShipRingExplosionSettings(), ship.Position, 0.4, 30));
                                    PlayExplosionSound(new Vector3D(bullet.Position.X, bullet.Position.Y, bullet.Position.Z));
                                }
                                break;
                            }
                        }
                    }
                }
            }



            /* bullet of enemy ships with drone and station */
            foreach (var bullet in _waveBuilder.BulletList)
            {
                if (Collider3D.BoundingFrustumIntersection(bullet))
                {
                    if (Collider3D.IntersectionSphere(bullet, _droneFleet.GetActiveDrone()))
                    {
                        //_drone.getHit(ship.Gun.makeDmg);
                        _droneFleet.GetActiveDrone().getHit(bullet.makeDmg);
                        if (bullet._bulletType == Bullet.BulletType.BossGun)
                        {
                            explosionList.Add(new ExplosionSystem(new BombExplosionSettings(), new BombRingExplosionSettings(), bullet.Position, 0.6, 50, true));
                        }
                        _removeBullets.Add(bullet);
                    }

                    if (bullet.CanDamageStation && Collider3D.IntersectionSphere(bullet, _station))
                    {
                        _sphereAlpha = 0.2f;
                        explosionList.Add(new ExplosionSystem(new ShipExplosionSettings(), bullet.Position, 0.4));
                        _station.getHit(bullet.makeDmg);
                        if (bullet._bulletType == Bullet.BulletType.BossGun)
                        {
                            explosionList.Add(new ExplosionSystem(new BombExplosionSettings(), new BombRingExplosionSettings(), bullet.Position, 0.6, 50, true));
                        }
                        _removeBullets.Add(bullet);
                    }
                }
            }

            /* asteroids with drone(& its bullets) & station & other asteroids & enemy ships (& its bullets)*/
            foreach (var ast in _asteroidField._asteroidList)
            {
                if (Collider3D.BoundingFrustumIntersection(ast))
                {
                    if (Vector3.Distance(ast.Position, _station.Position) > Global.MapDespawnRadius)
                    {
                        explosionList.Add(new ExplosionSystem(new AsteroidExplosionSettings(), ast.Position, 0.4));
                        _removeAsteroid.Add(ast);
                        continue;
                    }

                    if (Collider3D.IntersectionSphere(ast, _droneFleet.GetActiveDrone()))
                    {
                        _droneFleet.GetActiveDrone().getHit(5);
                        dustParticles.AddParticle(ast.Position, Vector3.Zero);
                        _removeAsteroid.Add(ast);
                        Global.HighScorePoints -= 50;
                        PlayExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));
                        continue;
                    }

                    if (Collider3D.IntersectionSphere(_station, ast))
                    {
                        _sphereAlpha = 0.2f;
                        explosionList.Add(new ExplosionSystem(new AsteroidExplosionSettings(), ast.Position, 0.4));
                        ast.IsDead = true;
                        _removeAsteroid.Add(ast);
                        _station.getHit(10);
                        continue;
                    }

                    foreach (var ast2 in _asteroidField._asteroidList)
                    {
                        if (Collider3D.BoundingFrustumIntersection(ast2))
                        {
                            if (ast != ast2 && Collider3D.IntersectionSphere(ast2, ast))
                            {
                                var newDirection = new Vector3();
                                ast.Reflect(ast2.Direction, ast2.Spheres[0].Center, out newDirection);
                                ast2.Direction = newDirection;
                                dustParticles.AddParticle(ast.Position, Vector3.Zero);
                                //ast.Position += ast.Direction * (2*((float) random.NextDouble())*ast.MaxRadius() + ast.MaxRadius());
                            }
                        }
                    }
                    foreach (var ship in _waveBuilder.ShipList)
                    {
                        if (Collider3D.BoundingFrustumIntersection(ship))
                        {
                            if (Collider3D.IntersectionSphere(ast, ship))
                            {
                                ship.Health -= 5;
                                dustParticles.AddParticle(ship.Position, Vector3.Zero);
                                _removeAsteroid.Add(ast);
                            }
                        }

                    }
                    foreach (var bullet in _waveBuilder.BulletList)
                    {
                        if (Collider3D.BoundingFrustumIntersection(bullet))
                        {
                            if (Collider3D.IntersectionSphere(bullet, ast))
                            {
                                explosionList.Add(new ExplosionSystem(new AsteroidExplosionSettings(), ast.Position, 0.35));
                                _removeAsteroid.Add(ast);
                                _removeBullets.Add(bullet);
                                break;
                            }
                        }
                    }

                    foreach (var bullet in _droneFleet._bulletList)
                    {
                        if (Collider3D.BoundingFrustumIntersection(bullet))
                        {
                            if (Collider3D.IntersectionSphere(bullet, ast))
                            {
                                explosionList.Add(new ExplosionSystem(new AsteroidExplosionSettings(), ast.Position, 0.35));
                                _removeAsteroid.Add(ast);
                                _removeBullets.Add(bullet);
                                Global.HighScorePoints += 50;
                                PlayExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));

                                if (bullet._bulletType == Bullet.BulletType.BigJoe)
                                {
                                    explosionList.Add(new ExplosionSystem(new BombExplosionSettings(), new BombRingExplosionSettings(), ast.Position, 0.6, 50, true));
                                }
                                if (ast.IsShiny)
                                {
                                    Global.Money += 200;
                                }
                                break;
                            }
                        }
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
                _waveBuilder.BulletList.Remove(bullet);
            }

        }

        //###################################################################################################
        // Helper
        //###################################################################################################

        //#################################
        // Helper Draw - Arrow
        //#################################
        void DrawStationDirectionArrow()
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
        void DrawShipDirectionArrow()
        {
            float minDistance = float.MaxValue;
            Vector3 posS = Vector3.Zero;
            foreach (var ship in _waveBuilder.ShipList)
            {
               var val = Math.Min(Vector3.Distance(_droneFleet.GetActiveDrone().Position, ship.Position), minDistance);
               if ( minDistance != val)
               {
                    minDistance = val;
                    posS = ship.Position;
               }

            }
            var vec3 = posS - _droneFleet.GetActiveDrone().Position;
            vec3.Normalize();
            var vec = new Vector2();
            vec.X = Global.GraphicsManager.GraphicsDevice.Viewport.Project(_droneFleet.GetActiveDrone().Position + vec3 * 50, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity).X;
            vec.Y = Global.GraphicsManager.GraphicsDevice.Viewport.Project(_droneFleet.GetActiveDrone().Position + vec3 * 50, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity).Y;
            _enemySymbol.Draw(vec.ToPoint(), 1, Global.EnemyColor);
         }
        
        //#################################
        // Helper RndPoint
        //#################################
        Vector3 RandomPointOnCircle(float radius)
        {
            double angle = random.NextDouble() * Math.PI * 2;

            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            return new Vector3(x * radius, 0, y * radius);
        }


        //#################################
        // Helper Update - Projectiles
        //#################################
        void UpdateProjectiles(GameTime gameTime)
        {
            int i = 0;

            while (i < projectiles.Count)
            {
                if (!projectiles[i].Update(gameTime))
                {
                    // Remove projectiles at the end of their life.
                    projectiles.RemoveAt(i);
                }
                else
                {
                    // Advance to the next projectile.
                    i++;
                }
            }
        }

        //#################################
        // Helper Update - Smoke
        //#################################
        /**
        void UpdateSmoke(GameTime gameTime)
        {
            // This is trivial: we just create one new smoke particle per frame.
            SmokeParticles.AddParticle(Vector3.Zero, Vector3.Zero);
            SmokeParticles.Update(gameTime);
        }
         **/

        //#################################
        // Helper Update - Border
        //#################################
        void UpdateBorder(GameTime gameTime)
        {
            const int borderParticlesPerFrame = 50;

            // Create a number of border particles, randomly positioned around a circle.
            for (int i = 0; i < borderParticlesPerFrame; i++)
            {
                borderParticles.AddParticle(RandomPointOnCircle(Global.MapRingRadius), Vector3.Zero);
            }
            borderParticles.Update(gameTime);
        }
        //###########################
        // SoundDJ
        //###########################
        void SoundDJ()
        {
            if (Global.Music.Finished)
            {      
                switch (random.Next(1, 4))
                {
                    case 1:
                        Global.Music = Global.MusicEngine.Play2D("Content/Media/Music/Space Fighter Loop.mp3", false);
                        Global.Music.Volume = Global.MusicVolume / 10;
                        break;

                    case 2:
                        Global.Music = Global.MusicEngine.Play2D("Content/Media/Music/Shiny Tech2.mp3", false);
                        Global.Music.Volume = Global.MusicVolume / 10;
                        break;

                    case 3:
                        Global.Music = Global.MusicEngine.Play2D("Content/Media/Music/Cyborg Ninja.mp3", false);
                        Global.Music.Volume = Global.MusicVolume/10;
                        break;

                }
            }
        
        }

    }
}