using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceAssault.Entities;
using SpaceAssault.Entities.Weapon;
using SpaceAssault.ScreenManager;
using SpaceAssault.Utils;

namespace SpaceAssault.Screens
{

    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    class GameplayScreen : GameScreen
    {
        SpriteFont gameFont;

        float pauseAlpha;

        private Station _station;
        private AsteroidBuilder _asteroidField;
        private Drone _drone;
        private EnemyShip _enemyShip;
        private Texture2D _background;
        private List<Bullet> _removeBullets;
        private List<Asteroid> _removeAsteroid;

        // Constructor.
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _station = new Station(new Vector3(0, 80, 0), 0);
            _drone = new Drone(new Vector3(150, 0, 100));
            _enemyShip= new EnemyShip(new Vector3(600,0,-300));
            //_asteroidField = new AsteroidBuilder(new Vector3(500, 0, -500));
            
            _removeAsteroid = new List<Asteroid>();
            _removeBullets = new List<Bullet>();
            _drone.Initialize();
            _station.Initialize();
            _enemyShip.Initialize();
            _asteroidField = new AsteroidBuilder();
        }


        // Load graphics content for the game.
        public override void LoadContent()
        {
            gameFont = Global.ContentManager.Load<SpriteFont>("Fonts/gamefont");

            Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, new Vector3(0, 250, 250), _drone.Position, Vector3.Up);
            _station.LoadContent();
            _drone.LoadContent();
            _enemyShip.LoadContent();
            _asteroidField.LoadContent();
            //_asteroidField = new AsteroidBuilder(new Vector3(0,0,0));

            Thread.Sleep(1000);
        }


        // Unload graphics content used by the game.
        public override void UnloadContent()
        {
            Global.ContentManager.Unload();
        }


        // Updates the state of the game. This method checks the GameScreen.IsActive
        // property, so the game will stop updating when the pause menu is active,
        // or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                //3D Model
                _station.Update(gameTime);
                _drone.Update(gameTime);
                _enemyShip.Update(gameTime);
                //_enemyShip.FlyVector(_drone.Position);
                
                _asteroidField.Update(gameTime,_station.Position,1000f);
                Global.Camera = new Camera(Global.GraphicsManager.GraphicsDevice.DisplayMode.AspectRatio, 10000f, MathHelper.ToRadians(45), 1f, _drone.Position + new Vector3(0, 250, 250), _drone.Position, Vector3.Up);
            }

            foreach (var bullet in _drone.GetBulletList())
            {
                foreach (var ast in _asteroidField.Asteroids)
                {
                    if (Collider3D.Intersection(bullet, ast))
                    {
                        _removeAsteroid.Add(ast);
                        _removeBullets.Add(bullet);
                    }
                    if (Collider3D.Intersection(ast, _drone))
                    {
                        _drone._health -= 20;
                        _removeAsteroid.Add(ast);
                    }
                }

                if (Collider3D.Intersection(bullet,_enemyShip))
                {
                    _enemyShip._health -= 20;
                    _removeBullets.Add(bullet);
                }
            }
            foreach (var bullet in _enemyShip.GetBulletList())
            {
                foreach (var ast in _asteroidField.Asteroids)
                {
                    if (Collider3D.Intersection(bullet, ast))
                    {
                        _removeAsteroid.Add(ast);
                        _removeBullets.Add(bullet);
                    }
                    if (Collider3D.Intersection(ast,_enemyShip))
                    {
                        _enemyShip._health -= 20;
                        _removeAsteroid.Add(ast);
                    }
                }

                if (Collider3D.Intersection(bullet, _drone))
                {
                    _drone._health -= 20;
                    _removeBullets.Add(bullet);
                }
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

            if (input.IsPauseGame())
            {
                ScreenManager.AddScreen(new PauseMenuScreen());
            }
            else
            {
                _drone.HandleInput();
                _enemyShip.FlyVector(_enemyShip.Position-_drone.Position);
                //euklidian Distance of Drone/enemyship Position
                if (Math.Sqrt(Math.Pow(_enemyShip.Position.X - _drone.Position.X, 2) + Math.Pow(_enemyShip.Position.Y - _drone.Position.Y, 2)) < 100)
                {
                    _enemyShip.Shoot(_drone.Position);
                }

            }
        }



        // Draws the gameplay screen.
        public override void Draw(GameTime gameTime)
        {
            Global.GraphicsManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);

            Global.SpriteBatch.Begin();
            //Global.SpriteBatch.DrawString(gameFont, "// TODO", playerPosition, Color.Green);
            Global.SpriteBatch.End();
            Global.GraphicsManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            _station.Draw();
            _drone.Draw();
            _enemyShip.Draw();
            _asteroidField.Draw();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

    }
}
