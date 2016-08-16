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
        SpriteFont gameFont;

        // Fade to black
        float _deadDroneAlpha;
        float _actualDeadDroneAlpha;
        float _pauseAlpha;

        // Gameplay
        private Station _station;
        private AsteroidBuilder _asteroidField;
        private FleetBuilder _fleet;
        private Drone _drone;
        private List<Bullet> _removeBullets;
        private List<Asteroid> _removeAsteroid;
        private List<AEnemys> _removeAEnemys;
        private InGameOverlay _ui;
        private Background _back;
        private int _deathCounter = 0;
        public static int _stationHeight = 80;
        private Frame _frame;
        // Sound
        private ISoundSource _explosionSource;
        private ISoundEngine _engine;

        // Particle
        ParticleSystem explosionParticles;
        ParticleSystem explosionSmokeParticles;
        ParticleSystem projectileTrailParticles;
        ParticleSystem smokePlumeParticles;

        List<Projectile> projectiles = new List<Projectile>();
        TimeSpan timeToNextProjectile = TimeSpan.Zero;

        Model grid;


        //#################################
        // Constructor
        //#################################
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            //actual gameplay objects
            _station = new Station(new Vector3(0, _stationHeight, 0), 0);
            _drone = new Drone(new Vector3(150, 0, 100));
            _back = new Background();
            _drone.Initialize();
            _station.Initialize();
            _asteroidField = new AsteroidBuilder();
            _fleet = new FleetBuilder();
            _ui = new InGameOverlay(_drone, _station);
            _frame = new Frame();
            //remove lists for collisions etc 
            _removeAsteroid = new List<Asteroid>();
            _removeBullets = new List<Bullet>();
            _removeAEnemys = new List<AEnemys>();

            // Particle
            explosionParticles = new ExplosionParticleSystem();
            explosionSmokeParticles = new ExplosionSmokeParticleSystem();
            projectileTrailParticles = new ProjectileTrailParticleSystem();
            smokePlumeParticles = new SmokePlumeParticleSystem();


            _engine = new ISoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);

        }

        //#################################
        // HelperClass Explosion (Particle)
        //#################################
        void UpdateExplosions(GameTime gameTime)
        {
            timeToNextProjectile -= gameTime.ElapsedGameTime;

            if (timeToNextProjectile <= TimeSpan.Zero)
            {
                // Create a new projectile once per second. The real work of moving
                // and creating particles is handled inside the Projectile class.
                projectiles.Add(new Projectile(explosionParticles,
                                               explosionSmokeParticles,
                                               projectileTrailParticles));

                timeToNextProjectile += TimeSpan.FromSeconds(1);
            }
        }

        //#################################
        // HelperClass Projectile (Particle)
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
        // HelperClass Smoke (Particle)
        //#################################
        void UpdateSmokePlume()
        {
            // This is trivial: we just create one new smoke particle per frame.
            smokePlumeParticles.AddParticle(Vector3.Zero, Vector3.Zero);
        }

        //#################################
        // LoadContent
        //#################################
        public override void LoadContent()
        {
            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 250, 250), _drone.Position, Vector3.Up);

            // Particle
            grid = Global.ContentManager.Load<Model>("Models/grid");

            gameFont = Global.ContentManager.Load<SpriteFont>("Fonts/gamefont");
            _station.LoadContent();
            _drone.LoadContent();
            _asteroidField.LoadContent();
            _fleet.LoadContent();
            _ui.LoadContent();
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
                _drone.Update(gameTime);
                _asteroidField.Update(gameTime, _drone.Position);
                Global.Camera.updateCameraPositionTarget(_drone.Position + new Vector3(0, 250, 250), _drone.Position);
                _fleet.Update(gameTime, _drone.Position);

                // Particle
                //UpdateExplosions(gameTime);
                UpdateSmokePlume();
                UpdateProjectiles(gameTime);


                //explosionParticles.Update(gameTime);
                //explosionSmokeParticles.Update(gameTime);
                smokePlumeParticles.Update(gameTime);
                projectileTrailParticles.Update(gameTime);


                // if station dies go back to MainMenu
                // TODO: change to EndScreen and HighScore list)
                if (_station._health <= 0)
                    LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen());

                //upgrading the health of drone and _droneupdate is used by other object to make the game harder
                if (Global.HighScorePoints > 1000 * (_drone._totalUpdates + 1))
                {
                    _drone._updatePoints++;
                    _drone._totalUpdates++;
                }

                // fading out/in when drone is dead & alive again
                if (!_drone.IsNotDead)
                    _deadDroneAlpha = Math.Min(_deadDroneAlpha + 1f / 32, 1);
                else
                    _deadDroneAlpha = Math.Max(_deadDroneAlpha - 1f / 32, 0);

                // if fading out is max, respawn
                if (_actualDeadDroneAlpha >= 1f)
                {
                    _drone.Reset();
                    _deathCounter++;
                }

                // enemy KI here
                foreach (var ship in _fleet.EnemyShips)
                {
                    ship.Intelligence(_drone.Position);
                    ship.Update(gameTime);
                }



                /// <summary>
                /// EVRYTHING AFTER THIS LINE IS COLLISION HANDLING
                /// </summary>

                /* bullet of drone with asteroids & enemy ships */
                foreach (var bullet in _drone.GetBulletList())
                {
                    foreach (var ast in _asteroidField._asteroidList)
                    {
                        if (Collider3D.IntersectionSphere(bullet, ast))
                        {
                            _removeAsteroid.Add(ast);
                            _removeBullets.Add(bullet);
                            Global.HighScorePoints += 50;
                            PlayExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));
                            break;
                        }
                    }
                    foreach (var ship in _fleet.EnemyShips)
                    {
                        if (Collider3D.IntersectionSphere(bullet, ship))
                        {
                            ship.Health -= _drone.Gun.makeDmg;
                            _removeBullets.Add(bullet);
                            Global.HighScorePoints += 20;
                            PlayExplosionSound(new Vector3D(bullet.Position.X, bullet.Position.Y, bullet.Position.Z));
                            break;
                        }
                    }

                }

                /* asteroids with drone & station & other asteroids & enemy ships*/
                foreach (var ast in _asteroidField._asteroidList)
                {
                    if (gameTime.TotalGameTime > (ast._originTime.Add(TimeSpan.FromMinutes(0.7d))))
                    {
                        _removeAsteroid.Add(ast);
                    }

                    if (Collider3D.IntersectionSphere(ast, _drone))
                    {
                        _drone.getHit(5);

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
                            _removeAsteroid.Add(ast);
                            break;
                        }
                    }
                    foreach (var ship in _fleet.EnemyShips)
                    {
                        if (Collider3D.IntersectionSphere(ast, ship))
                        {
                            ship.Health -= 5;
                            _removeAsteroid.Add(ast);
                        }
                        foreach (var bullet in ship.GetBulletList())
                        {
                            if (Collider3D.IntersectionSphere(bullet, ast))
                            {
                                _removeAsteroid.Add(ast);
                                _removeBullets.Add(bullet);
                                break;
                            }
                        }
                    }
                }

                /* enemy ships with drone and their bullets with drone & station */
                foreach (var ship in _fleet.EnemyShips)
                {
                    if (ship.IsDead == true)
                    {
                        _removeAEnemys.Add(ship);
                        continue;
                    }

                    foreach (var bullet in ship.GetBulletList())
                    {
                        if (Collider3D.IntersectionSphere(bullet, _drone))
                        {
                            //_drone.getHit(ship.Gun.makeDmg);
                            _drone.getHit(ship.Gun.makeDmg);
                            _removeBullets.Add(bullet);
                        }

                        if (ship.Gun.CanDamageStation && Collider3D.IntersectionSphere(bullet, _station))
                        {
                            _station._health -= ship.Gun.makeDmg;
                            _removeBullets.Add(bullet);
                        }
                    }
                }

                foreach (var ship in _removeAEnemys)
                {
                    _fleet.EnemyShips.Remove(ship);
                }
                foreach (var ast in _removeAsteroid)
                {
                    _asteroidField._asteroidList.Remove(ast);
                }
                foreach (var bullet in _removeBullets)
                {
                    _drone.GetBulletList().Remove(bullet);
                }
                _removeAsteroid.Clear();
                _removeBullets.Clear();
                _removeAEnemys.Clear();
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
                if ((Vector3.Distance(_station.Position, _drone.Position) - _stationHeight) < 150 && Global.HighScorePoints > 1000)
                    ScreenManager.AddScreen(new ShopScreen(_drone));
            }

            //player hits ESC it pauses the game
            if (input.IsPauseGame())
            {
                ScreenManager.AddScreen(new PauseMenuScreen());
            }
        }

        //#################################
        // Grid Helper
        //#################################
        void DrawGrid(Matrix view, Matrix projection)
        {
            Global.GraphicsManager.GraphicsDevice.BlendState = BlendState.Opaque;
            Global.GraphicsManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Global.GraphicsManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            grid.Draw(Matrix.Identity, view, projection);
        }

        //#################################
        // Draw
        //#################################
        public override void Draw(GameTime gameTime)
        {
            Global.GraphicsManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            // calling draw of objects where necessary
            _back.Draw(90, new Vector3(-5000, -2500, -5000));
            _station.Draw();
            _drone.Draw();
            _asteroidField.Draw();
            _fleet.Draw();
            _ui.Draw();
            _frame.Draw();

            //DrawGrid(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);


            explosionParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
            explosionSmokeParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
            projectileTrailParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);

            explosionParticles.Draw(gameTime);
            explosionSmokeParticles.Draw(gameTime);
            projectileTrailParticles.Draw(gameTime);

            smokePlumeParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
            smokePlumeParticles.Draw(gameTime);


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

    }
}
