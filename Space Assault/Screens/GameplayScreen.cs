using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Planet _planet;
        private InputState _input;

        //Dialog
        Dialog captainDialog;
        UIItem captain;

        //UI + Frame + Background
        private InGameOverlay _ui;
        private Frame _frame;
        private Background _back;
        private UIItem _stationSymbol;
        private UIItem _enemySymbol;

        // Sound
        private ISpaceSoundEngine _soundEngine;

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

        //Created screens
        PauseMenuScreen pause;
        ShopScreen shop;
        int deadTime;
        bool voice;

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
            _asteroidField = new AsteroidBuilder(40);
            _droneFleet = new DroneBuilder();

            _sphereAlpha = 0.1f;
            _waveBuilder = new WaveBuilder(10000, 15);
            Global.Money = 0;
            //UI + Frame + BG 
            _ui = new InGameOverlay(_station);
            _back = new Background();
            _frame = new Frame();
            Global.HighScorePoints = 0;
            Global.Money = 0;
            Global.DroneDmg = 10;
            Global.NumberOfRockets = 1;
            _input = new InputState();
            _planet = new Planet(new Vector3(-1000, -2000, -1000), 0);
            _soundEngine = new ISpaceSoundEngine(SoundOutputDriver.AutoDetect, SoundEngineOptionFlag.LoadPlugins | SoundEngineOptionFlag.MultiThreaded | SoundEngineOptionFlag.MuteIfNotFocused | SoundEngineOptionFlag.Use3DBuffers);

            // Construct Particles
            borderParticles = new BorderParticleSettings();
            dustParticles = new DustParticleSystem();
            hitmarkerParticles = new HitMarkerParticleSystem();

            captainDialog = new Dialog(0, 0, 320, 400, 8, false, true);
            captain = new UIItem();
            deadTime = 17000;
            voice = false;
            Global.SpeakerVolume = 2;
        }


        //#################################
        // LoadContent
        //#################################
        public override void LoadContent()
        {

            _stationSymbol = new UIItem();
            _enemySymbol = new UIItem();
            _stationSymbol.LoadContent("Images/station_icon", 4);
            _enemySymbol.LoadContent("Images/hit_marker", 4);

            _droneFleet.replaceOldDrone(new Vector3(150, 0, 100));
            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, Global.CameraPosition, _droneFleet.GetActiveDrone().Position, Vector3.Up);
            _station.LoadContent();
            _sphere.LoadContent();
            _asteroidField.LoadContent();
            _ui.LoadContent(_droneFleet);
            _frame.LoadContent();
            _waveBuilder.LoadContent();
            _planet.LoadContent();
            //Effects
            _stationEffect = Global.ContentManager.Load<Effect>("Effects/stationEffect");

            //Sounds
            //playing the sound
            _soundEngine.setListenerPosToCameraTarget();
            _soundEngine.AddSoundSourceFromFile("openShop", "Content/Media/Effects/OpenShop.wav");
            _soundEngine.AddSoundSourceFromFile("explosionSource", "Content/Media/Effects/Objects/Explosion4.wav");
            _soundEngine.AddSoundSourceFromFile("explosionSource1", "Content/Media/Effects/Objects/Explosion1.wav");
            _soundEngine.AddSoundSourceFromFile("explosionSource2", "Content/Media/Effects/Objects/Explosion2.wav");
            _soundEngine.AddSoundSourceFromFile("explosionSource3", "Content/Media/Effects/Objects/Explosion3.wav");
            _soundEngine.AddSoundSourceFromFile("astexplosionSource1", "Content/Media/Effects/Objects/ExplosionAst1.wav");
            _soundEngine.AddSoundSourceFromFile("astexplosionSource2", "Content/Media/Effects/Objects/ExplosionAst2.wav");
            _soundEngine.AddSoundSourceFromFile("astexplosionSource3", "Content/Media/Effects/Objects/ExplosionAst3.wav");
            _soundEngine.AddSoundSourceFromFile("hitSound", "Content/Media/Effects/Objects/GetHitShips.wav");

            //Global.Music = _soundEngine.Play2D("explosionSource3", Global.MusicVolume / 10, false);
            captainDialog.LoadContent();
            captain.LoadContent("Images/captain", 4);

            // X = left/right
            
            _soundEngine.setListenerPosToCameraTarget();
            var pos = Global.Camera.Target;
            Global.Music = _soundEngine.Play3D("explosionSource3", Global.MusicVolume / 10, Global.Camera.Target, false);
        }

        //#################################
        // UnloadContent
        //#################################
        public override void UnloadContent()
        {
            //Global.ContentManager.Unload();

            _soundEngine.StopAllSounds();
            _soundEngine.Dispose();
        }

        //#################################
        // Update
        //#################################
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, false);
            _soundEngine.Update();

            SoundDJ();
            PlayVoice();
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            DebugFunctions();
            _input.Update();

            if (!coveredByOtherScreen)
            {
                if (_sphereAlpha > 0.1f)
                    _sphereAlpha -= 0.001f;

                //boids
                _waveBuilder.Update(gameTime, ref _asteroidField, ref _droneFleet);

                // calling update of objects where necessary
                _station.Update(gameTime);
                _planet.Update(gameTime);
                _droneFleet.Update(gameTime, !_station.IsDead);
                _asteroidField.Update(gameTime, _droneFleet.GetActiveDrone().Position);
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


                if (_waveBuilder.HasEnded && deadTime <= 0 && _station._health > 0)
                {
                    Global.HighScorePoints += Global.Money;
                    LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen(), new HighscoreMenuScreenOnline(true));
                }
                // if station dies go back to MainMenu
                // TODO: change to EndScreen and HighScore list)
                if (_station._health <= 0 && deadTime <= 0)
                    LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen(), new HighscoreMenuScreenOnline(true));

                CollisionHandling(gameTime);


                // fading out/in when drone is dead & alive again
                if (!_droneFleet.GetActiveDrone().IsNotDead)
                    _deadDroneAlpha = Math.Min(_deadDroneAlpha + 1f / 32, 1);
                else
                    _deadDroneAlpha = Math.Max(_deadDroneAlpha - 1f / 32, 0);

                // if fading out is max, respawn
                if (_actualDeadDroneAlpha >= 1f)
                {
                    _droneFleet.replaceOldDrone();
                    Global.HighScorePoints -= 200; 
                    _deathCounter++;
                }
            }
        }

        //#################################
        // Everything related to Debugstuff/input etc. here (only works in debug mode)
        //#################################
        [Conditional("DEBUG")]
        public void DebugFunctions()
        {
            if (_input.IsNewKeyPress(Keys.K, Keys.L))
            {
                Global.Money += 10000;
            }

            if (_input.IsNewKeyPress(Keys.F, Keys.G))
            {
                _station._health -= 1000;
            }

            if (_input.IsNewKeyPress(Keys.F, Keys.H))
            {
                _station._shield -= 1000;
            }
            if (_input.IsNewKeyPress(Keys.L, Keys.H))
            {
                _waveBuilder.WaveCount++;
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
            if (_input.IsNewKeyPress(Keys.F4))
            {
                Console.WriteLine("Drone Pos: " + _droneFleet.GetActiveDrone().Position);
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
                    _soundEngine.Play2D("openShop", Global.SpeakerVolume / 10, false);
                    if (shop == null)
                        shop = new ShopScreen(_droneFleet, _station);
                    ScreenManager.AddScreen(shop);
                }
            }

            //player hits ESC it pauses the game
            if (input.IsPauseGame())
            {
                //playing the sound
                _soundEngine.Play2D("openShop", Global.SpeakerVolume / 10, false);
                if (pause == null)
                    pause = new PauseMenuScreen();
                ScreenManager.AddScreen(pause);
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
            _planet.Draw(Global.PlanetColor);
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
            if (_waveBuilder.HasEnded && !_station.IsDead)
            {
                DrawCaptainDialog(new Point(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 - 200, Global.GraphicsManager.GraphicsDevice.Viewport.Height / 2 - 100), "                    You Succeded!\n\n        General Stargaz\n\nI am proud of you Pilot, you did your\njob very well. I couldn't have done\nit better myself.\nHere, take that medal and some\nvacation on this Spa Station not\nfar from your home Planet.\nThank you for your service, Pilot!\nDismissed!");
                deadTime -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else if (_station.IsDead)
            {
                DrawCaptainDialog(new Point(Global.GraphicsManager.GraphicsDevice.Viewport.Width / 2 - 200, Global.GraphicsManager.GraphicsDevice.Viewport.Height / 2 - 100), "                    You Died!\n\n        General Stargaz\n\nThis Pilot did his duty in combat with\ngreat courage and steadfast dedication\neven after he was outnumbered by\nthe hundreds.\nHe sacrificed his life to defend the\nones who couldn't themselves. ");
                deadTime -= gameTime.ElapsedGameTime.Milliseconds;
            }
               

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
            switch (Global.Random.Next(0, 3))
            {
                case 0:
                    _soundEngine.Play2D("explosionSource", Global.SpeakerVolume / 10, false);
                    break;
                case 1:
                    _soundEngine.Play2D("explosionSource1", Global.SpeakerVolume / 10, false);
                    break;
                case 2:
                    _soundEngine.Play2D("explosionSource2", Global.SpeakerVolume / 10, false);
                    break;
                case 3:
                    _soundEngine.Play2D("explosionSource3", Global.SpeakerVolume / 10, false);
                    break;
            }
        }
        protected void PlayAstExplosionSound(Vector3D pos)
        {
            switch (Global.Random.Next(0, 2))
            {
                case 0:
                    _soundEngine.Play2D("astexplosionSource1", Global.SpeakerVolume / 10, false);
                    break;
                case 1:
                    _soundEngine.Play2D("astexplosionSource2", Global.SpeakerVolume / 10, false);
                    break;
                case 2:
                    _soundEngine.Play2D("astexplosionSource3", Global.SpeakerVolume / 10, false);
                    break;
            }
        }
        protected void PlayVoice()
        {
            if (_waveBuilder.HasEnded)
            {
                if (!voice)
                {
                    Global.Music.Stop();
                    voice = true;
                    Global.SpeakerVolume = 0;
                }
                if(Global.Music.Finished)
                 Global.Music = Global.MusicEngine.Play2D("voice_win", Global.MusicVolume / 10, false);
            }
            if (_station._health <= 0)
            {
                if (!voice)
                {
                    Global.Music.Stop();
                    voice = true;
                    Global.SpeakerVolume = 0;
                }
                if (Global.Music.Finished)
                    Global.Music = Global.MusicEngine.Play2D("voice_loss", Global.MusicVolume / 10, false);
            }
        }
      
       protected void PlayShipHitSound(Vector3D pos)
        {
            _soundEngine.Play2D("hitSound", Global.SpeakerVolume / 10, false);
        }

        //#################################
        // Collision
        //#################################
        void CollisionHandling(GameTime gameTime)
        {
            //remove lists for collisions etc 
            List<Asteroid> _removeAsteroid = new List<Asteroid>();
            List<Bullet> _removeBullets = new List<Bullet>();
            List<BoundingSphere> explosionBoundingSpheres = new List<BoundingSphere>();

            /* bullet of drone with enemy ships */
            foreach (var bullet in _droneFleet._bulletList)
            {
                foreach (var ship in _waveBuilder.ShipList)
                {
                    if (Collider3D.IntersectionSphere(bullet, ship))
                    {
                        if (bullet._bulletType == Bullet.BulletType.BigJoe)
                        {
                            explosionList.Add(new ExplosionSystem(new BombExplosionSettings(), new BombRingExplosionSettings(), ship.Position, 0.4, 50, true));
                            BoundingSphere curBoundingSphere = new BoundingSphere();
                            curBoundingSphere.Center = bullet.Position;
                            curBoundingSphere.Radius = 45;
                            explosionBoundingSpheres.Add(curBoundingSphere);
                        }
                        ship.getHit(bullet.makeDmg);
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



            /* bullet of enemy ships with drone and station */
            foreach (var bullet in _waveBuilder.BulletList)
            {
                if (Collider3D.IntersectionSphere(bullet, _droneFleet.GetActiveDrone()))
                {
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

            /* asteroids with drone(& its bullets) & station & other asteroids & enemy ships (& its bullets)*/
            foreach (var ast in _asteroidField._asteroidList)
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
                    PlayAstExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));
                    continue;
                }
                if (Collider3D.IntersectionSphere(_station, ast))
                {
                    _sphereAlpha = 0.2f;
                    explosionList.Add(new ExplosionSystem(new AsteroidExplosionSettings(), ast.Position, 0.4));
                    ast.IsDead = true;
                    _removeAsteroid.Add(ast);
                    _station.getHit(100);
                    PlayAstExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));
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
                    }
                }
                foreach (var ship in _waveBuilder.ShipList)
                {
                    if (Collider3D.IntersectionSphere(ast, ship))
                    {
                        dustParticles.AddParticle(ship.Position, Vector3.Zero);
                        _removeAsteroid.Add(ast);
                    }
                }
                foreach (var bullet in _waveBuilder.BulletList)
                {

                    if (Collider3D.IntersectionSphere(bullet, ast))
                    {
                        explosionList.Add(new ExplosionSystem(new AsteroidExplosionSettings(), ast.Position, 0.35));
                        _removeAsteroid.Add(ast);
                        _removeBullets.Add(bullet);
                        break;
                    }

                }
                foreach (var bullet in _droneFleet._bulletList)
                {
                    if (Collider3D.IntersectionSphere(bullet, ast))
                    {
                        explosionList.Add(new ExplosionSystem(new AsteroidExplosionSettings(), ast.Position, 0.35));
                        _removeAsteroid.Add(ast);
                        _removeBullets.Add(bullet);
                        Global.HighScorePoints += 50;
                        PlayAstExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));

                        if (bullet._bulletType == Bullet.BulletType.BigJoe)
                        {
                            explosionList.Add(new ExplosionSystem(new BombExplosionSettings(), new BombRingExplosionSettings(), ast.Position, 0.6, 50, true));
                            BoundingSphere curBoundingSphere = new BoundingSphere();
                            curBoundingSphere.Center = bullet.Position;
                            curBoundingSphere.Radius = 45;
                            explosionBoundingSpheres.Add(curBoundingSphere);
                        }
                        if (ast.IsShiny)
                        {
                            Global.Money += 200;
                        }
                        break;
                    }
                }
            }

            /* AoE explosions */
            foreach (var explosion in explosionBoundingSpheres)
            {
                foreach (var ast in _asteroidField._asteroidList)
                {
                    if (Collider3D.IntersectionSphere(ast, explosion))
                    {
                        explosionList.Add(new ExplosionSystem(new AsteroidExplosionSettings(), ast.Position, 0.35));
                        _removeAsteroid.Add(ast);
                        Global.HighScorePoints += 50;
                        PlayAstExplosionSound(new Vector3D(ast.Position.X, ast.Position.Y, ast.Position.Z));

                        if (ast.IsShiny)
                        {
                            Global.Money += 200;
                        }
                    }
                }

                foreach (var ship in _waveBuilder.ShipList)
                {
                    if (Collider3D.IntersectionSphere(ship, explosion))
                    {
                        ship.getHit(80*(_droneFleet.GetActiveDrone().makeDmg/10));
                        Global.HighScorePoints += 20;
                        if (ship.Health > 0)
                        {
                            PlayShipHitSound(new Vector3D(ship.Position.X, ship.Position.Y, ship.Position.Z));
                            hitmarkerParticles.AddParticle(ship.Position, Vector3.Zero);
                        }
                        else if (ship.Health <= 0)
                        {
                            if (ship.GetType() == typeof(EnemyBomber))
                                explosionList.Add(new ExplosionSystem(new ShipBigExplosionSettings(), new ShipRingExplosionSettings(), ship.Position, 0.4, 50, true));
                            else
                                explosionList.Add(new ExplosionSystem(new ShipExplosionSettings(), new ShipRingExplosionSettings(), ship.Position, 0.4, 30));
                            PlayExplosionSound(new Vector3D(ship.Position.X, ship.Position.Y, ship.Position.Z));
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
                _stationSymbol.Draw(vec.ToPoint(), 1, Global.StationColor);
            }
        }
        void DrawShipDirectionArrow()
        {
            if (_waveBuilder.ShipList.Count > 0)
            {
                float minDistance = float.MaxValue;
                Vector3 posS = Vector3.Zero;
                foreach (var ship in _waveBuilder.ShipList)
                {
                    var val = Math.Min(Vector3.Distance(_droneFleet.GetActiveDrone().Position, ship.Position), minDistance);
                    if (minDistance != val)
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
                _enemySymbol.Draw(vec.ToPoint(), 1, Color.Red);
            }
        }
        void DrawCaptainDialog(Point pos, string msg)
        {
            captainDialog.Draw(new Point(pos.X, pos.Y - 170), msg);
            captain.Draw(new Point(pos.X + 10, pos.Y - 165), 1, Color.White);
        }
        //#################################
        // Helper RndPoint
        //#################################
        Vector3 RandomPointOnCircle(float radius)
        {
            double angle = Global.Random.NextDouble() * Math.PI * 2;

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
                switch (Global.Random.Next(1, 4))
                {
                    case 1:
                        Global.Music = Global.MusicEngine.Play2D("SpaceFighterLoop", Global.MusicVolume / 10, false);
                        break;

                    case 2:
                        Global.Music = Global.MusicEngine.Play2D("ShinyTech2", Global.MusicVolume / 10, false);
                        break;

                    case 3:
                        Global.Music = Global.MusicEngine.Play2D("CyborgNinja", Global.MusicVolume / 10, false);
                        break;
                }
            }

        }

    }
}