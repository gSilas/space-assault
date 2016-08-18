using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceAssault.Entities;
using SpaceAssault.Entities.Weapon;
using SpaceAssault.ScreenManagers;
using SpaceAssault.Utils;
using IrrKlang;
using SpaceAssault.Utils.Particle;
using SpaceAssault.Utils.Particle.Settings;

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

        // Gameplay
        private Station _station;
        private AsteroidBuilder _asteroidField;
        private FleetBuilder _fleet;
        private DroneBuilder _droneFleet;
        private int _deathCounter = 0;
        public static int _stationHeight = 80;
    
        //UI + Frame + Background
        private InGameOverlay _ui;
        private Frame _frame;
        private Background _back;

        // Sound
        private ISoundSource _explosionSource;
        private ISoundEngine _engine;

        //Particle
        ParticleSystem explosionParticles;
        ParticleSystem explosionSmokeParticles;
        ParticleSystem projectileTrailParticles;
        ParticleSystem TrailParticles;
        ParticleSystem SmokeParticles;
        ParticleSystem fireParticles;

        Vector3 explosionPosition = Vector3.Zero;
        double explosionTimeDelta = 0;

        // Switch between three different visual effects.
        enum ParticleState
        {
            Explosions,
            Smoke,
            RingOfFire,
        };

        ParticleState currentState = ParticleState.Smoke;

        // The explosions effect works by firing projectiles up into the
        // air, so we need to keep track of all the active projectiles.
        List<Projectile> projectiles = new List<Projectile>();
        List<Trail> trail = new List<Trail>();
        TimeSpan timeToNextProjectile = TimeSpan.Zero;

        // Random number generator for the fire effect.
        Random random = new Random();

        //#################################
        // Constructor
        //#################################
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            //actual gameplay objects
            _station = new Station(new Vector3(0, _stationHeight, 0), 0);
            _station.Initialize();
            _asteroidField = new AsteroidBuilder();
            _fleet = new FleetBuilder();
            _droneFleet = new DroneBuilder();

            //UI + Frame + BG 
            _ui = new InGameOverlay(_station);
            _back = new Background();
            _frame = new Frame();

            _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);

            
            // Contruct Particle
            explosionParticles = new ExplosionParticleSystem();
            explosionSmokeParticles = new ExplosionSmokeParticleSystem();
            TrailParticles = new TrailParticleSystem();
            projectileTrailParticles = new ProjectileTrailParticleSystem();
            SmokeParticles = new SmokeParticleSystem();
            fireParticles = new FireParticleSystem();

            trail.Add(new Trail(TrailParticles));
        }


        //#################################
        // LoadContent
        //#################################
        public override void LoadContent()
        {
            _droneFleet.addDrone(new Vector3(150, 0, 100));
            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 250, 250), _droneFleet.GetActiveDrone().Position, Vector3.Up);
            _station.LoadContent();
            _asteroidField.LoadContent();
            _ui.LoadContent(_droneFleet);
            _frame.LoadContent();
            _explosionSource = _engine.AddSoundSourceFromFile("Content/Media/Music/Explosion.wav", StreamMode.AutoDetect, true);
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

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            //everything in this scope here what happens when GameplayScreen is active
            if (IsActive)
            {


                // calling update of objects where necessary
                _station.Update(gameTime);
                _droneFleet.Update(gameTime);
                _asteroidField.Update(gameTime, _droneFleet.GetActiveDrone().Position);
                Global.Camera.updateCameraPositionTarget(_droneFleet.GetActiveDrone().Position + new Vector3(0, 250, 250), _droneFleet.GetActiveDrone().Position);
                _fleet.Update(gameTime, _droneFleet.GetActiveDrone().Position);

                //remove lists for collisions etc 
                List<Asteroid>  _removeAsteroid = new List<Asteroid>();
                List<Bullet>  _removeBullets = new List<Bullet>();

                // Particle
                if (_station._health < 10000)
                {
                    UpdateSmoke();
                    SmokeParticles.Update(gameTime);
                }

                
                foreach (var drone in _droneFleet._droneShips)
                {
                    UpdateTrail(gameTime, drone.Position);
                    TrailParticles.Update(gameTime);
                }

                explosionParticles.Update(gameTime);

                // if station dies go back to MainMenu
                // TODO: change to EndScreen and HighScore list)
                if (_station._health <= 0)
                    LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen());

                //upgrading the health of drone and _droneupdate is used by other object to make the game harder
                if (Global.HighScorePoints > 1000 * (_droneFleet._totalUpdates + 1))
                {
                    _droneFleet._updatePoints++;
                    _droneFleet._totalUpdates++;
                }

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

                /// <summary>
                /// EVRYTHING AFTER THIS LINE IS COLLISION HANDLING
                /// </summary>

                /* bullet of drone with enemy ships */
                foreach (var bullet in _droneFleet._bulletList)
                {
                    foreach (var ship in _fleet._enemyShips)
                    {

                        if (Collider3D.IntersectionSphere(bullet, ship))
                        {
                            ship.Health -= _droneFleet.GetActiveDrone().Gun.makeDmg;
                            _removeBullets.Add(bullet);
                            Global.HighScorePoints += 20;

                            if (ship.Health <= 0)
                            {
                                explosionPosition = ship.Position;
                                explosionTimeDelta = gameTime.TotalGameTime.TotalSeconds;
                                PlayExplosionSound(new Vector3D(bullet.Position.X, bullet.Position.Y, bullet.Position.Z));
                            }

                            break;
                        }
                    }

                }

                /* bullet of enemy ships with drone and station */
                foreach (var bullet in _fleet._bulletList)
                {
                    if (Collider3D.IntersectionSphere(bullet, _droneFleet.GetActiveDrone()))
                    {
                        //_drone.getHit(ship.Gun.makeDmg);
                        _droneFleet.GetActiveDrone().getHit(bullet.makeDmg);
                        _removeBullets.Add(bullet);
                    }

                    if (bullet.CanDamageStation && Collider3D.IntersectionSphere(bullet, _station))
                    {
                        _station._health -= bullet.makeDmg;
                        _removeBullets.Add(bullet);
                    }
                }

                /* asteroids with drone(& its bullets) & station & other asteroids & enemy ships (& its bullets)*/
                foreach (var ast in _asteroidField._asteroidList)
                {
                    if (gameTime.TotalGameTime > (ast._originTime.Add(TimeSpan.FromMinutes(0.7d))))
                    {
                        _removeAsteroid.Add(ast);
                    }

                    if (Collider3D.IntersectionSphere(ast, _droneFleet.GetActiveDrone()))
                    {
                        _droneFleet.GetActiveDrone().getHit(5);

                        _removeAsteroid.Add(ast);
                        Global.HighScorePoints -= 50;
                        PlayExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));
                        continue;
                    }

                    if (Collider3D.IntersectionSphere(_station, ast))
                    {
                        ast.IsDead = true;
                        _removeAsteroid.Add(ast);
                        _station._health -= 10;
                        continue;
                    }

                    foreach (var ast2 in _asteroidField._asteroidList)
                    {
                        if (ast != ast2 && Collider3D.IntersectionSphere(ast2, ast))
                        {
                            ast.IsDead = true;
                            explosionPosition = ast.Position;
                            explosionTimeDelta = gameTime.TotalGameTime.TotalSeconds;
                            _removeAsteroid.Add(ast);
                            break;
                        }
                    }
                    foreach (var ship in _fleet._enemyShips)
                    {
                        if (Collider3D.IntersectionSphere(ast, ship))
                        {
                            ship.Health -= 5;
                            _removeAsteroid.Add(ast);
                        }
                    }
                    foreach (var bullet in _fleet._bulletList)
                    {
                        if (Collider3D.IntersectionSphere(bullet, ast))
                        {
                            _removeAsteroid.Add(ast);
                            _removeBullets.Add(bullet);
                            break;
                        }
                    }

                    foreach (var bullet in _droneFleet._bulletList)
                    {
                        if (Collider3D.IntersectionSphere(bullet, ast))
                        {
                            explosionPosition = ast.Position;
                            explosionTimeDelta = gameTime.TotalGameTime.TotalSeconds;
                            _removeAsteroid.Add(ast);
                            _removeBullets.Add(bullet);
                            Global.HighScorePoints += 50;
                            PlayExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));
                            break;
                        }
                    }

                    // Particle-Explosions
                    if (gameTime.TotalGameTime.TotalSeconds - explosionTimeDelta < 0.5)
                    {
                        UpdateExplosions(gameTime, explosionPosition, Vector3.Zero);
                    }

                }

                foreach (var ast in _removeAsteroid)
                {
                    _asteroidField._asteroidList.Remove(ast);
                }

                foreach (var bullet in _removeBullets)
                {
                    _droneFleet._bulletList.Remove(bullet);
                    _fleet._bulletList.Remove(bullet);
                }
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
                if ((Vector3.Distance(_station.Position, _droneFleet.GetActiveDrone().Position) - _stationHeight) < 150 && Global.HighScorePoints > 1000)
                    ScreenManager.AddScreen(new ShopScreen(_droneFleet));
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
            _station.Draw();
            _droneFleet.Draw();
            _asteroidField.Draw();
            _fleet.Draw();
            _ui.Draw(_droneFleet);
            if(_station._health > 1000)
            {
                _frame.Draw(false);
            }
            else { _frame.Draw(true); }

            // Particle
            /*
            explosionParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
            explosionSmokeParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
            TrailParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
            fireParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
            */

            if (_station._health < 10000)
            {
                SmokeParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
                SmokeParticles.Draw(gameTime);
            }

            TrailParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
            TrailParticles.Draw(gameTime);

            explosionParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
            explosionParticles.Draw(gameTime);

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
        }

        //#################################
        // Sound
        //#################################
        protected void PlayExplosionSound(Vector3D pos)
        {
            var curListenerPos = new Vector3D(Global.Camera.Target.X, Global.Camera.Target.Y, Global.Camera.Target.Z);
            _engine.SetListenerPosition(curListenerPos, new Vector3D(0, 0, 1));
            var _explosionSound = _engine.Play3D(_explosionSource, pos.X, pos.Y, pos.Z, false, true, true);
            _explosionSound.Volume = 1f;
            _explosionSound.Paused = false;
        }

        //#################################
        // Helper Update - Explosion
        //#################################
        void UpdateExplosions(GameTime gameTime, Vector3 position, Vector3 velocity)
        {
            timeToNextProjectile -= gameTime.ElapsedGameTime;

            if (timeToNextProjectile <= TimeSpan.Zero)
            {
                // Create a new projectile once per second. The real work of moving
                // and creating particles is handled inside the Projectile class.
                //projectiles.Add(new Projectile(explosionParticles,explosionSmokeParticles,TrailParticles));
                explosionParticles.AddParticle(position, velocity);
               
                timeToNextProjectile += TimeSpan.FromSeconds(0);
            }
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
        // Helper Update - Trail
        //#################################
        void UpdateTrail(GameTime gameTime, Vector3 position)
        {
            for (int i=0;  i < trail.Count; i++)
            {
                trail[i].Update(gameTime, position);
            }
        }

        //#################################
        // Helper Update - Smoke
        //#################################
        void UpdateSmoke()
        {
            // This is trivial: we just create one new smoke particle per frame.
            SmokeParticles.AddParticle(Vector3.Zero, Vector3.Zero);
        }

        //#################################
        // Helper Update - Fire
        //#################################
        void UpdateFire()
        {
            const int fireParticlesPerFrame = 20;

            // Create a number of fire particles, randomly positioned around a circle.
            for (int i = 0; i < fireParticlesPerFrame; i++)
            {
                fireParticles.AddParticle(RandomPointOnCircle(), Vector3.Zero);
            }

            // Create one smoke particle per frmae, too.
            SmokeParticles.AddParticle(RandomPointOnCircle(), Vector3.Zero);
        }

        //#################################
        // Helper RndPoint
        //#################################
        Vector3 RandomPointOnCircle()
        {
            const float radius = 30;
            const float height = 40;

            double angle = random.NextDouble() * Math.PI * 2;

            float x = (float)Math.Cos(angle);
            float y = (float)Math.Sin(angle);

            return new Vector3(x * radius, y * radius + height, 0);
        }

    }
}
