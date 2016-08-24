using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceAssault.Entities;
using SpaceAssault.ScreenManagers;

namespace SpaceAssault.Utils
{
    class Boids
    {
        public List<AEnemys> _ships;
        public List<Bullet> _bullets;
        private Random _random;
        InputState input;

        public Boids()
        {
            _ships = new List<AEnemys>();
            _bullets = new List<Bullet>();
            _random = new Random();
            input = new InputState();
        }


        public void MouseAdd()
        {
            Vector3 nearScreenPoint = new Vector3(MouseHandler.Position, 0);
            Vector3 farScreenPoint = new Vector3(MouseHandler.Position, 1);
            Vector3 nearWorldPoint = Global.GraphicsManager.GraphicsDevice.Viewport.Unproject(nearScreenPoint, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity);
            Vector3 farWorldPoint = Global.GraphicsManager.GraphicsDevice.Viewport.Unproject(farScreenPoint, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity);
            Vector3 direction = farWorldPoint - nearWorldPoint;
            float zFactor = -nearWorldPoint.Y / direction.Y;
            Vector3 zeroWorldPoint = nearWorldPoint + direction * zFactor;


            if (input.IsLeftMouseButtonNewPressed())
                addBoid(zeroWorldPoint);
        }

        public void addBoid(Vector3 position)
        {
            AEnemys ship = new EnemyFighter(position);
            ship.LoadContent();
            _ships.Add(ship);
        }

        // adds a number of random boids at the map corner (which is a circle), for radius see Global.cs
        public void addRandomBoids(int number)
        {
            List<AEnemys> ships = new List<AEnemys>();
            double angle;           
            for(int i = 0; i < number; i++)
            {
                angle = _random.NextDouble() * Math.PI * 2;
                addBoid(new Vector3(Global.MapRadius * (float)Math.Cos(angle), 0, Global.MapRadius *  (float)Math.Sin(angle)));
            }
        }

        public void Update(GameTime gameTime)
        {
            input.Update();
            MouseAdd();

            List<Bullet> _removeBullets = new List<Bullet>();
            List<AEnemys> _removeShips = new List<AEnemys>();

            // applying boid logic to our ships
            moveAllBoidsToNewPositions();

            // updating bullets
            foreach (Bullet bullet in _bullets)
            {
                bullet.Update(gameTime);
                if (bullet._bulletLifeTime < 0)
                {
                    _removeBullets.Add(bullet);
                }
            }
            foreach (Bullet bullet in _removeBullets)
            {
                _bullets.Remove(bullet);
            }

            // updating every ship
            foreach (var ship in _ships)
            {
                if (ship.IsDead)
                {
                    Global.Money += ship.KillMoney;
                    _removeShips.Add(ship);
                    continue;
                }

                // Trail
                foreach (Particle.Trail trail in ship.trail)
                {
                    trail.Update(gameTime, ship.Position);
                }
                ship.TrailParticles.Update(gameTime);
                ship.Update(gameTime);
            }

            //removing dead ships from the list
            foreach (var ship in _removeShips)
            {
                _ships.Remove(ship);
            }
        }

        public void Draw()
        {
            foreach (Bullet bullet in _bullets)
            {
                bullet.Draw();
            }

            foreach (var ship in _ships)
            {
                ship.TrailParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
                ship.TrailParticles.Draw();
                ship.Draw();
            }
        }

        /// <summary>
        /// BOIDS LOGIC BELOW THIS LINE http://www.kfish.org/boids/pseudocode.html
        /// </summary>
        private void moveAllBoidsToNewPositions()
        {
            foreach (var ship1 in _ships)
            {
                Vector3 v1, v2, v3, v4;
                v1 = rule1(ship1);
                v2 = rule2(ship1);
                v3 = rule3(ship1);
                v4 = goToPlace(ship1);

                ship1.Velocity += v1 + v2 + v3 + v4;
                ship1.Velocity /= ship1.Velocity.Length();
                //ship.Position += ship.Velocity;
                ship1.FlyToDirection(-ship1.Velocity);
            }
        }

        // RULE1: Boids try to fly towards the centre of mass of neighbouring boids
        private Vector3 rule1(AEnemys curShip)
        {
            Vector3 pcj = Vector3.Zero;
            foreach (var _ship in _ships)
            {
                if(_ship != curShip)
                {
                    pcj += _ship.Position;
                }
            }

            pcj /= _ships.Count;

            //richtungsvektor zur 'masse' durch 100 => 1% des richtungsvektors
            return (pcj - curShip.Position) / 100;
        }

        // RULE2: Boids try to keep a small distance away from other objects (including other boids)
        private Vector3 rule2(AEnemys curShip)
        {
            int distanceLimit = 40;
            Vector3 c = Vector3.Zero;
            foreach (var _ship in _ships)
            {
                if (_ship != curShip)
                {
                    if ((_ship.Position - curShip.Position).Length() < distanceLimit)
                    {
                        c -= (_ship.Position - curShip.Position);
                    }
                }
            }

            return c;
        }

        // RULE3: Boids try to match velocity with near boids
        private Vector3 rule3(AEnemys curShip)
        {
            Vector3 pvj = Vector3.Zero;

            foreach (var _ship in _ships)
            {
                if (_ship != curShip)
                {
                    pvj += _ship.Velocity;
                }
            }

            pvj /= _ships.Count;

            // nur ein achtel anwenden
            return (pvj - curShip.Position) / 8;
        }

        private Vector3 goToPlace(AEnemys curShip)
        {
            Vector3 place = new Vector3(0,0,0);

            return (place - curShip.Position) / 100;
        }
    }
}
