using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceAssault.Entities;
using SpaceAssault.Entities.Weapon;
using SpaceAssault.ScreenManagers;
using SpaceAssault.Utils;

namespace SpaceAssault.Screens
{

    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    class GameplayScreen : GameScreen
    {
        SpriteFont gameFont;

        float _pauseAlpha;
        float _deadDroneAlpha;
        float _actualDeadDroneAlpha;

        private Station _station;
        private AsteroidBuilder _asteroidField;
        private FleetBuilder _fleet;
        private Drone _drone;
        private Texture2D _background;
        private List<Bullet> _removeBullets;
        private List<Asteroid> _removeAsteroid;
        private List<AEnemys> _removeAEnemys;
  
        private InGameOverlay _ui;
        private Background _back;

        private int _deathCounter = 0;
        private int _stationHeight = 80;

        public static bool _dronepdate = true;

        // Constructor.
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _station = new Station(new Vector3(0, _stationHeight, 0), 0);
            _drone = new Drone(new Vector3(150, 0, 100));
            //_asteroidField = new AsteroidBuilder(new Vector3(500, 0, -500));
            _ui = new InGameOverlay(_drone._health, _station._health, (Vector3.Distance(_station.Position, _drone.Position) - _stationHeight));
            _back = new Background();
            _removeAsteroid = new List<Asteroid>();
            _removeBullets = new List<Bullet>();
            _removeAEnemys= new List<AEnemys>();
       
            _drone.Initialize();
            _station.Initialize();
            _asteroidField = new AsteroidBuilder();
            _fleet = new FleetBuilder();


        }

        // Load graphics content for the game.
        public override void LoadContent()
        {
            gameFont = Global.ContentManager.Load<SpriteFont>("Fonts/gamefont");

            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 250, 250), _drone.Position, Vector3.Up);
            _station.LoadContent();
            _drone.LoadContent();

            _asteroidField.LoadContent();
            //_asteroidField = new AsteroidBuilder(new Vector3(0,0,0));
            _fleet.LoadContent();

            _ui.LoadContent();

            Thread.Sleep(1000);
        }


        // Unload graphics content used by the game.
        public override void UnloadContent()
        {
            //Global.ContentManager.Unload();
        }


        // Updates the state of the game. This method checks the GameScreen.IsActive
        // property, so the game will stop updating when the pause menu is active,
        // or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (_station._health <= 0)
                LoadingScreen.Load(ScreenManager, true, new BackgroundScreen(), new MainMenuScreen());

            if (_dronepdate)
            {
                if (ShopScreen._health == 2)
                {
                    _drone._health = 200;
                    _dronepdate = false;
                }

            }


            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            /// <summary>
            /// handles what happens if drone is dead
            /// </summary>
            if (!_drone.IsNotDead)
                _deadDroneAlpha = Math.Min(_deadDroneAlpha + 1f / 32, 1);
            else
                _deadDroneAlpha = Math.Max(_deadDroneAlpha - 1f / 32, 0);

            if (_actualDeadDroneAlpha >= 1f)
            {
                _drone.Reset();
                _deathCounter++;
            }
            if(_deathCounter >= 3)
            {

            }


            if (IsActive)
            {
                //3D Model
                _station.Update(gameTime);
                _drone.Update(gameTime);


                //UI
                _ui.Update(_drone._health, _station._health, (Vector3.Distance(_station.Position, _drone.Position) - _stationHeight));

                _asteroidField.Update(gameTime,_drone.Position);
                
                //Console.WriteLine(_asteroidField.Asteroids.Count);
                Global.Camera.updateCameraPositionTarget(_drone.Position + new Vector3(0, 250, 250), _drone.Position);
                _fleet.Update(gameTime,_drone.Position);
                /// <summary>
                /// enemy "KI"
                /// </summary>
                
                foreach (var ship in _fleet.EnemyShips)
                {
                    ship.Intelligence(_drone.Position);
                    ship.Update(gameTime);
                }
            }


            //collision handling
            foreach (var bullet in _drone.GetBulletList())
            {
                foreach (var ast in _asteroidField.Asteroids)
                {
                    if (Collider3D.IntersectionSphere(bullet, ast))
                    {
                        _removeAsteroid.Add(ast);
                        _removeBullets.Add(bullet);
                        Global.HighScorePoints += 50;
                    }
                    if (Collider3D.IntersectionSphere(ast, _drone))
                    {
                        _drone._health -= 5;
                        _removeAsteroid.Add(ast);
                        Global.HighScorePoints -= 50;
                    }
                }
                foreach (var ship in _fleet.EnemyShips)
                {
                    if (Collider3D.IntersectionSphere(bullet, ship))
                    {
                        if (Collider3D.IntersectionSphere(bullet, ship))
                        {
                            ship.Health -= _drone.Gun.makeDmg;
                            _removeBullets.Add(bullet);
                            Global.HighScorePoints += 20;
                        }
                    }
                }

            }
            foreach (var ship in _fleet.EnemyShips)
            {
                foreach (var bullet in ship.GetBulletList())
                {
                    foreach (var ast in _asteroidField.Asteroids)
                    {
                        if (Collider3D.IntersectionSphere(bullet, ast))
                        {
                            _removeAsteroid.Add(ast);
                            _removeBullets.Add(bullet);
                        }
                        if (Collider3D.IntersectionSphere(_station, ast))
                        {
                            _removeAsteroid.Add(ast);
                            _station._health -= 10;
                        }
                        if (Collider3D.IntersectionSphere(ast, ship))
                        {
                           
                            ship.Health -= 5;
                            _removeAsteroid.Add(ast);
                        }
                        if (Collider3D.IntersectionSphere(_drone, ship))
                        {
                            //EnemyFighter.Position = new Vector3(100,10,100);
                        }
                        if (gameTime.TotalGameTime > (ast.LifeTime.Add(TimeSpan.FromMinutes(0.7d))))
                        {
                            _removeAsteroid.Add(ast);
                        }
                    }
                    if (Collider3D.IntersectionSphere(bullet, _drone))
                    {
                        _drone._health -= ship.Gun.makeDmg;
                        _removeBullets.Add(bullet);
                    }
                    if (Collider3D.IntersectionSphere(bullet, _station))
                    {
                        if(ship.Gun.CanDamageStation)
                            _station._health -= ship.Gun.makeDmg;
                        _removeBullets.Add(bullet);
                    }

                }
                if(ship.IsDead==true)
                    _removeAEnemys.Add(ship);
            }

            foreach (var ast in _asteroidField.Asteroids)
            {
                foreach (var ast2 in _asteroidField.Asteroids)
                {
                    if (ast!=ast2 && Collider3D.IntersectionSphere(ast2, ast))
                    {
                        _removeAsteroid.Add(ast);
                    }
                }
            }
            foreach (var ship in _removeAEnemys)
            {
                _fleet.EnemyShips.Remove(ship);
            }
            foreach (var ast in _removeAsteroid)
            {
                _asteroidField.Asteroids.Remove(ast);
            }
            foreach (var bullet in _removeBullets)
            {
                _drone.GetBulletList().Remove(bullet);
            }

        }

        // Lets the game respond to player input. Unlike the Update method,
        // this will only be called when the gameplay screen is active.
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the player.
            KeyboardState keyboardState = input.CurrentKeyboardState;

            if (Keyboard.GetState().IsKeyDown(Keys.B))
            {
                if((Vector3.Distance(_station.Position, _drone.Position)-_stationHeight) < 150 && Global.HighScorePoints > 1000)
                    ScreenManager.AddScreen(new ShopScreen());
            }

                if (input.IsPauseGame())
            {
                ScreenManager.AddScreen(new PauseMenuScreen());
            }
        }

        // Draws the gameplay screen.
        public override void Draw(GameTime gameTime)
        {
            Global.GraphicsManager.GraphicsDevice.Clear(ClearOptions.Target,Color.Black, 0, 0);

            _back.Draw();

            Global.GraphicsManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            _station.Draw();
            _drone.Draw();

            _asteroidField.Draw();

            _fleet.Draw();

            _ui.Draw();
            
            //if drone is dead fade to black
            if (_deadDroneAlpha > 0)
            {
                _actualDeadDroneAlpha = MathHelper.Lerp(0f, 2f, _deadDroneAlpha / 2);
                //Console.WriteLine(_actualDeadDroneAlpha);
                ScreenManager.FadeBackBufferToBlack(_actualDeadDroneAlpha);
            }


            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
            

        }

    }
}
