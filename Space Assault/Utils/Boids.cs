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
        public List<AEntity> _objectsToAvoid;
        public List<Bullet> _bullets;
        private Random _random;
        InputState _input;
        private int _proximityRadius;
        private int _cohesionRadius;
        private int _aligningRadius;
        private int _avoidBoidsRadius;
        private int _avoidObjRadius;
        private float _maxSpeed;

        public Boids()
        {
            _ships = new List<AEnemys>();
            _objectsToAvoid = new List<AEntity>();
            _bullets = new List<Bullet>();
            _random = new Random();
            _input = new InputState();

            _cohesionRadius = 125;
            _aligningRadius = 85;
            _avoidBoidsRadius = 40;
            _avoidObjRadius = 40;
            _proximityRadius = Math.Max(_cohesionRadius, _aligningRadius);
            _maxSpeed = 2f;
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
            if (_input.IsLeftMouseButtonNewPressed())
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
            for (int i = 0; i < number; i++)
            {
                angle = _random.NextDouble() * Math.PI * 2;
                addBoid(new Vector3(Global.MapRadius * (float)Math.Cos(angle), 0, Global.MapRadius * (float)Math.Sin(angle)));
            }
        }

        public void Update(GameTime gameTime, List<AEntity> objList)
        {
            _input.Update();
            MouseAdd();

            List<Bullet> _removeBullets = new List<Bullet>();
            List<AEnemys> _removeShips = new List<AEnemys>();
            _objectsToAvoid = objList;
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
            foreach (var curShip in _ships)
            {
                Vector3
                    cohesion = Vector3.Zero,
                    aligning = Vector3.Zero,
                    avoidB = Vector3.Zero,
                    avoidO = Vector3.Zero,
                    place = Vector3.Zero,
                    noise = Vector3.Zero,
                    awayPlace = Vector3.Zero;

                cohesion = cohesionRule(curShip);
                aligning = aligningRule(curShip);
                avoidB = avoidBoidRule(curShip);
                avoidO = avoidObjRule(curShip);
                avoidO.Y = 0;
                noise = new Vector3((float)_random.NextDouble(), 0, (float)_random.NextDouble());
                place = Vector3.Distance(curShip.Position, Global.Camera.Target) < 500 ? goToPlace(curShip, Global.Camera.Target) : goToPlace(curShip, Vector3.Zero);
                awayPlace = curShip.Position.Length() < 150 ? -goToPlace(curShip, Vector3.Zero) : Vector3.Zero;

                curShip._direction += cohesion / 100 + aligning / 5 + avoidB + avoidO + noise / 20 + place / 5 + awayPlace / 5;
                curShip._direction /= 25;
                //curShip._direction.Normalize();

                curShip.RotateTowards(-curShip.Direction);
                curShip.Position += curShip.Direction;
                //curShip.FlyToDirection(-curShip._direction);
            }
        }

        // RULE1: Boids try to fly towards the centre of mass of neighbouring boids
        private Vector3 cohesionRule(AEnemys curShip)
        {
            Vector3 pcj = Vector3.Zero;
            int count = 0;
            foreach (var otherShip in _ships)
            {
                if (otherShip != curShip && Vector3.Distance(otherShip.Position, curShip.Position) < _cohesionRadius)
                {
                    pcj += otherShip.Position;
                    count++;
                }
            }

            if (count > 0) pcj /= count;
            pcj -= curShip.Position;

            //richtungsvektor zur 'masse'
            return pcj;
        }

        // RULE2: Boids try to keep a small distance away from other boids
        private Vector3 avoidBoidRule(AEnemys curShip)
        {
            Vector3 c = Vector3.Zero;
            foreach (var otherShip in _ships)
            {
                float distance = Vector3.Distance(otherShip.Position, curShip.Position);
                if (otherShip != curShip && distance < _avoidBoidsRadius)
                {
                    c -= (otherShip.Position - curShip.Position);
                }
            }
            return c;
        }

        // RULE2.5: Boids try to keep a small distance away from other Objects
        private Vector3 avoidObjRule(AEnemys curShip)
        {
            Vector3 c = Vector3.Zero;
            foreach (var curObj in _objectsToAvoid)
            {
                float distance = Vector3.Distance(curObj.Position, curShip.Position);
                if (curObj != curShip && distance < _avoidObjRadius)
                {
                    c -= (curObj.Position - curShip.Position);
                }
            }
            return c;
        }

        // RULE3: Boids try to match velocity with near boids
        private Vector3 aligningRule(AEnemys curShip)
        {
            Vector3 pvj = Vector3.Zero;
            int count = 0;
            foreach (var otherShip in _ships)
            {
                float distance = Vector3.Distance(otherShip.Position, curShip.Position);
                if (otherShip != curShip && distance < _aligningRadius)
                {
                    pvj += otherShip.Direction;
                    count++;
                }
            }
            if (count > 0) pvj /= count;
            return pvj - curShip.Direction;
        }


        private Vector3 goToPlace(AEnemys curShip, Vector3 place)
        {
            Vector3 placeDir = place - curShip.Position;
            return placeDir;
        }
    }
}
