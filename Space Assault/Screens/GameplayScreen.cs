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
        private DroneBuilder _droneFleet;
        private ExplosionSpawner _explosionSpawner;
        private int _deathCounter = 0;
        public static int _stationHeight = 80;
        private WaveBuilder _waveBuilder;


        //UI + Frame + Background
        private InGameOverlay _ui;
        private Frame _frame;
        private Background _back;
        private UIItem _stationSymbol;

        // Sound
        private ISoundSource _explosionSource;
        private ISoundSource _explosionSource1;
        private ISoundSource _explosionSource2;
        private ISoundSource _explosionSource3;
        private ISoundSource _openShop;
        private ISoundEngine _engine;

        //Particle
        ParticleSystem explosionParticles;
        ParticleSystem dustParticles;
        ParticleSystem explosionSmokeParticles;
        ParticleSystem projectileTrailParticles;
        ParticleSystem SmokeParticles;
        ParticleSystem borderParticles;

        Vector3 explosionPosition = Vector3.Zero;
        double explosionEffectDelta = 0;
        bool explosionTriggered = false;

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
            _asteroidField = new AsteroidBuilder();
            _droneFleet = new DroneBuilder();
            _explosionSpawner = new ExplosionSpawner();

            _waveBuilder = new WaveBuilder(TimeSpan.FromSeconds(15d),2,2);
            Global.Money = 0;
            //UI + Frame + BG 
            _ui = new InGameOverlay(_station);
            _back = new Background();
            _frame = new Frame();

           _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);

            // Construct Particle
            explosionParticles = new ExplosionParticleSystem();
            //explosionSmokeParticles = new ExplosionSmokeParticleSystem();
            //projectileTrailParticles = new ProjectileTrailParticleSystem();
            //SmokeParticles = new SmokeParticleSystem();
            borderParticles = new BorderParticleSystem();
            dustParticles = new DustParticleSystem();
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
            _asteroidField.LoadContent();
            _ui.LoadContent(_droneFleet);
            _frame.LoadContent();
            _waveBuilder.LoadContent();

            //Sounds
            _openShop = _engine.AddSoundSourceFromFile("Content/Media/Effects/OpenShop.wav", StreamMode.AutoDetect, true);
            _explosionSource = _engine.AddSoundSourceFromFile("Content/Media/Effects/Explosion.wav", StreamMode.AutoDetect, true);
            _explosionSource1 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/Explosion1.wav", StreamMode.AutoDetect, true);
            _explosionSource2 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/Explosion2.wav", StreamMode.AutoDetect, true);
            _explosionSource3 = _engine.AddSoundSourceFromFile("Content/Media/Effects/Objects/Explosion3.wav", StreamMode.AutoDetect, true);

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


            //boids
            _waveBuilder.Update(gameTime, ref _asteroidField, ref _droneFleet);


            // calling update of objects where necessary
            _station.Update(gameTime);
            _droneFleet.Update(gameTime);
            _asteroidField.Update(gameTime, _droneFleet.GetActiveDrone().Position);
            Global.Camera.updateCameraPositionTarget(_droneFleet.GetActiveDrone().Position + Global.CameraPosition, _droneFleet.GetActiveDrone().Position);
            _explosionSpawner.Update(gameTime);

            // Particles
            //UpdateSmoke(gameTime);
            //UpdateProjectiles(gameTime);
            dustParticles.Update(gameTime);
            explosionParticles.Update(gameTime);
            UpdateBorder(gameTime);


            // if station dies go back to MainMenu
            // TODO: change to EndScreen and HighScore list)
            if (_station._health <= 0)
                LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen());

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
                    Open = _engine.Play3D(_openShop, 0, 0 + 15f, 0, false, true, false);
                    Open.Volume = 1f;
                    Open.Paused = false;
                    ScreenManager.AddScreen(new ShopScreen(_droneFleet, _station));
                }
                  
            }

            //player hits ESC it pauses the game
            if (input.IsPauseGame())
            {
                //playing the sound
                _engine.SetListenerPosition(new Vector3D(0, 0, 0), new Vector3D(0, 0, 1));
                ISound Open;
                Open = _engine.Play3D(_openShop, 0, 0 + 15f, 0, false, true, false);
                Open.Volume = 1f;
                Open.Paused = false;
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
            _waveBuilder.Draw(gameTime);

            // Particle
            //SmokeParticles.Draw();
            dustParticles.Draw();
            explosionParticles.Draw();
            borderParticles.Draw();
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

            DrawDirectionArrow();

            //FRAME & UI ALWAYS LAST
            _ui.Draw(_droneFleet);
            if (_station._health > 1000)
            {
                _frame.Draw(false);
            }
            else { _frame.Draw(true); }
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
                    _explosionSound.Volume = 5f;
                    _explosionSound.Paused = false;
                    break;
                case 1:
                    var _explosionSound1 = _engine.Play3D(_explosionSource1, pos.X, pos.Y, pos.Z, false, true, true);
                    _explosionSound1.Volume = 5f;
                    _explosionSound1.Paused = false;
                    break;
                case 2:
                    var _explosionSound2 = _engine.Play3D(_explosionSource2, pos.X, pos.Y, pos.Z, false, true, true);
                    _explosionSound2.Volume = 5f;
                    _explosionSound2.Paused = false;
                    break;
                case 3:
                    var _explosionSound3 = _engine.Play3D(_explosionSource3, pos.X, pos.Y, pos.Z, false, true, true);
                    _explosionSound3.Volume = 5f;
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

            /* bullet of drone with enemy ships */
            foreach (var bullet in _droneFleet._bulletList)
            {
                foreach (var ship in _waveBuilder.ShipList)
                {
                        if (Collider3D.IntersectionSphere(bullet, ship))
                        {
                            if (bullet._bulletType == Bullet.BulletType.BigJoe)
                            {
                                float radius = 80;
                                _explosionSpawner.SpawnExplosion(bullet.Position, radius, 100);
                                ExplosionCircle(bullet.Position, new Vector3(0, 0, 0), radius);
                            }
                            ship.getHit(bullet.makeDmg);
                            _removeBullets.Add(bullet);
                            Global.HighScorePoints += 20;

                            if (ship.Health <= 0)
                            {
                                explosionPosition = ship.Position;
                                explosionTriggered = true;
                                PlayExplosionSound(new Vector3D(bullet.Position.X, bullet.Position.Y, bullet.Position.Z));
                            }

                            break;
                        }              
                }

            }

            /* explosions with asteroids, enemy ships */
            foreach (var expl in _explosionSpawner._explList)
            {
                foreach (var ship in _waveBuilder.ShipList)
                {
                        if (Collider3D.IntersectionSphere(ship, expl))
                        {
                            ship.getHit(expl._makeDmg);
                            Global.HighScorePoints += 20;
                            PlayExplosionSound(new Vector3D(ship.Position.X, ship.Position.Y, ship.Position.Z));

                        }
                }

                foreach (var ast in _asteroidField._asteroidList)
                {
                    if (Collider3D.IntersectionSphere(expl, ast))
                    {
                        explosionPosition = ast.Position;
                        explosionTriggered = true;
                        ast.IsDead = true;
                        _removeAsteroid.Add(ast);
                        PlayExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));
                        break;
                    }
                }

            }

            /* bullet of enemy ships with drone and station */
            foreach (var bullet in _waveBuilder.BulletList)
            {
                if (Collider3D.IntersectionSphere(bullet, _droneFleet.GetActiveDrone()))
                {
                    //_drone.getHit(ship.Gun.makeDmg);
                    _droneFleet.GetActiveDrone().getHit(bullet.makeDmg);
                    _removeBullets.Add(bullet);
                }

                if (bullet.CanDamageStation && Collider3D.IntersectionSphere(bullet, _station))
                {
                    _station.getHit(bullet.makeDmg);
                    _removeBullets.Add(bullet);
                }
            }

            /* asteroids with drone(& its bullets) & station & other asteroids & enemy ships (& its bullets)*/
            foreach (var ast in _asteroidField._asteroidList)
            {
                if (Vector3.Distance(ast.Position,_station.Position) > Global.MapDespawnRadius)
                {
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
                    explosionPosition = ast.Position;
                    explosionTriggered = true;
                    ast.IsDead = true;
                    _removeAsteroid.Add(ast);
                    _station.getHit(10);
                    continue;
                }

                foreach (var ast2 in _asteroidField._asteroidList)
                {
                    if (ast != ast2 && Collider3D.IntersectionSphere(ast2, ast))
                    {                   
                            ast.NegateDirection();
                            dustParticles.AddParticle(ast.Position, Vector3.Zero);
                            ast.Position += ast.Direction * (2*((float) random.NextDouble())*ast.MaxRadius() + ast.MaxRadius());
                    }
                }
                foreach (var ship in _waveBuilder.ShipList)
                {

                        if (Collider3D.IntersectionSphere(ast, ship))
                        {
                            ship.Health -= 5;
                            dustParticles.AddParticle(ship.Position, Vector3.Zero);
                            _removeAsteroid.Add(ast);
                        }
                    
                }
                foreach (var bullet in _waveBuilder.BulletList)
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
                        explosionTriggered = true;
                        _removeAsteroid.Add(ast);
                        _removeBullets.Add(bullet);
                        Global.HighScorePoints += 50;
                        PlayExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));
                        if (bullet._bulletType == Bullet.BulletType.BigJoe)
                        {
                            float radius = 80;
                            _explosionSpawner.SpawnExplosion(bullet.Position, radius, 100);
                            ExplosionCircle(bullet.Position, new Vector3(0, 0, 0), radius);
                        }
                        if (ast.IsShiny)
                        {
                            Global.Money += 200;
                        }
                        break;
                    }
                }

                if (explosionTriggered)
                {
                    explosionEffectDelta++;
                    UpdateExplosions(gameTime, explosionPosition, Vector3.Zero);

                    if (explosionEffectDelta > 3)
                    {
                        explosionTriggered = false;
                        explosionEffectDelta = 0;
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
        void DrawDirectionArrow()
        { 
            if (Vector3.Distance(_droneFleet.GetActiveDrone().Position,_station.Position)>300)
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
        // Helper Update - Explosion
        //#################################
        void UpdateExplosions(GameTime gameTime, Vector3 position, Vector3 velocity)
        {
            // Create a new projectile once per second
            //projectiles.Add(new Projectile(explosionParticles,explosionSmokeParticles,projectileTrailParticles));
            explosionParticles.AddParticle(position, velocity);
        }

        //#################################
        // Helper ExplosionCircle
        //#################################
        void ExplosionCircle(Vector3 pos, Vector3 velocity, float radius)
        {
            float angle = 0.0f;
            for (int i = 0; i <= radius; i += 25)
            {
                while (angle < 2 * Math.PI)
                {
                    explosionParticles.AddParticle(pos + new Vector3(i * (float)Math.Cos(angle), 0, i * (float)Math.Sin(angle)), velocity);
                    angle += 0.2f;
                }
                angle = 0;
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
        // Helper Update - Smoke
        //#################################
        void UpdateSmoke(GameTime gameTime)
        {
            // This is trivial: we just create one new smoke particle per frame.
            SmokeParticles.AddParticle(Vector3.Zero, Vector3.Zero);
            SmokeParticles.Update(gameTime);
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
                borderParticles.AddParticle(RandomPointOnCircle(Global.MapSpawnRadius, 40), Vector3.Zero);
            }
            // Create one smoke particle per frmae, too.
            //SmokeParticles.AddParticle(RandomPointOnCircle(), Vector3.Zero);

            borderParticles.Update(gameTime);
        }

    }
}
