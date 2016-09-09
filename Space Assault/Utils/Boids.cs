﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceAssault.Entities;
using SpaceAssault.ScreenManagers;
using System.Diagnostics;

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
        private int _flyToDroneRadius;
        private int _flyToStationRadius;
        private int _avoidDroneRadius;
        private int _avoidStationRadius;
        private float _maxSpeed;
        private bool workMouseAdd = false;

        public enum EnemyType
        {
            Fighter,
            Fighter2,
            Bomber
        }

        public Boids()
        {
            _ships = new List<AEnemys>();
            _objectsToAvoid = new List<AEntity>();
            _bullets = new List<Bullet>();
            _random = new Random();
            _input = new InputState();

            _cohesionRadius = 125;
            _aligningRadius = 85;
            _avoidBoidsRadius = 30;
            _avoidObjRadius = 40;
            _proximityRadius = Math.Max(_cohesionRadius, _aligningRadius);
            _flyToDroneRadius = 280;
            _flyToStationRadius = (int)(_flyToDroneRadius * 1.3f);
            _avoidDroneRadius = (int)(_flyToDroneRadius * 0.4f);
            _avoidStationRadius = 50;
        }

        [Conditional("DEBUG")]
        public void MouseAdd()
        {
            if (_input.IsNewKeyPress(Keys.N)) workMouseAdd = !workMouseAdd;
            if (workMouseAdd)
            {
                Vector3 nearScreenPoint = new Vector3(MouseHandler.Position, 0);
                Vector3 farScreenPoint = new Vector3(MouseHandler.Position, 1);
                Vector3 nearWorldPoint = Global.GraphicsManager.GraphicsDevice.Viewport.Unproject(nearScreenPoint, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity);
                Vector3 farWorldPoint = Global.GraphicsManager.GraphicsDevice.Viewport.Unproject(farScreenPoint, Global.Camera.ProjectionMatrix, Global.Camera.ViewMatrix, Matrix.Identity);
                Vector3 direction = farWorldPoint - nearWorldPoint;
                float zFactor = -nearWorldPoint.Y / direction.Y;
                Vector3 zeroWorldPoint = nearWorldPoint + direction * zFactor;
                zeroWorldPoint.Y = 0;
                if (_input.IsLeftMouseButtonNewPressed())
                    addBoid(zeroWorldPoint, EnemyType.Fighter);
            }
        }

        public void addBoid(Vector3 position, EnemyType type)
        {
            AEnemys ship;
            switch (type)
            {
                case EnemyType.Fighter:
                    ship = new EnemyFighter(position);
                    break;
                case EnemyType.Fighter2:
                    ship = new EnemyFighter2(position);
                    break;
                case EnemyType.Bomber:
                    ship = new EnemyBomber(position);
                    break;
                default:
                    ship = new EnemyFighter(position);
                    break;
            }

            ship.LoadContent();
            _ships.Add(ship);
        }

        // adds a number of random boids at the map corner (which is a circle), for radius see Global.cs
        public void addRandomBoids(int number, EnemyType type)
        {
            double angle;
            for (int i = 0; i < number; i++)
            {
                angle = _random.NextDouble() * Math.PI * 2;
                addBoid(new Vector3(Global.MapSpawnRadius * (float)Math.Cos(angle), 0, Global.MapSpawnRadius * (float)Math.Sin(angle)), type);
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
            foreach (var curShip in _ships)
            {
                if (curShip.IsDead)
                {
                    Global.Money += curShip.KillMoney;
                    _removeShips.Add(curShip);
                    continue;
                }

                //shootlogic fighter
                if (curShip.GetType() == typeof(EnemyFighter))
                {
                    Vector3 direction = Global.Camera.Target - curShip.Position;
                    float vectorDirection = curShip.RotationMatrix.Forward.Z * direction.X - curShip.RotationMatrix.Forward.X * direction.Z;
                    double distanceToTarget = Vector3.Distance(curShip.Position, Global.Camera.Target);
                    if (Math.Abs(vectorDirection) <= 16 && distanceToTarget < 185 && !curShip.flyingAwayFromDrone)
                    {
                        curShip.Gun.Shoot(gameTime, Bullet.BulletType.EnemyLazer, curShip.gunMakeDmg, curShip.Position, curShip.RotationMatrix, ref _bullets);
                    }
                }

                if (curShip.GetType() == typeof(EnemyFighter2))
                {
                    Vector3 direction = Global.Camera.Target - curShip.Position;
                    float vectorDirection = curShip.RotationMatrix.Forward.Z * direction.X - curShip.RotationMatrix.Forward.X * direction.Z;
                    double distanceToTarget = Vector3.Distance(curShip.Position, Global.Camera.Target);
                    if (Math.Abs(vectorDirection) <= 16 && distanceToTarget < 185 && !curShip.flyingAwayFromDrone)
                    {
                        curShip.Gun.Shoot(gameTime, Bullet.BulletType.EnemyLazer, curShip.gunMakeDmg, curShip.Position, curShip.RotationMatrix, ref _bullets);
                    }
                }

                //shotlogic bomber
                if (curShip.GetType() == typeof(EnemyBomber))
                {
                    double distanceToDrone = (curShip.Position - Global.Camera.Target).Length();
                    double distanceToStation = curShip.Position.Length();

                    if (distanceToStation < 400 && !curShip.flyingAwayFromStation && !curShip.flyingAwayFromDrone)
                    {
                        curShip.Gun.Shoot(gameTime, Bullet.BulletType.PhotonBomb, curShip.gunMakeDmg, curShip.Position, curShip.RotationMatrix, ref _bullets);
                    }
                }

                // Trail
                curShip._trail.Update(gameTime, curShip.Position);
                curShip.Update(gameTime);
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
                bullet.Draw(Global.EnemyBulletColor);
            }

            foreach (var ship in _ships)
            {
                ship._trail.Draw();
                ship.Draw(Global.EnemyColor);
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
                    flyToDrone = Vector3.Zero,
                    noise = Vector3.Zero,
                    avoidS = Vector3.Zero;
               
                aligning = aligningRule(curShip);
                avoidB = avoidBoidRule(curShip);
                avoidO = avoidObjRule(curShip);
                noise = new Vector3((float)_random.NextDouble(), 0, (float)_random.NextDouble());

                if (curShip.GetType() == typeof(EnemyBomber))
                {
                    _maxSpeed = curShip.MoveSpeedForward;
                    flyToDrone = droneStationRuleBomber(curShip);
                }

                if (curShip.GetType() == typeof(EnemyFighter))
                {
                    _maxSpeed = curShip.MoveSpeedForward;
                    cohesion = cohesionRule(curShip);
                    flyToDrone = droneStationRuleFighter(curShip);
                    avoidS = avoidStationRule(curShip);
                }
                if (curShip.GetType() == typeof(EnemyFighter2))
                {
                    _maxSpeed = curShip.MoveSpeedForward;
                    cohesion = cohesionRule(curShip);
                    flyToDrone = droneStationRuleFighter(curShip);
                    avoidS = avoidStationRule(curShip);
                }

                Vector3 lastDirection = curShip._direction;
                curShip._direction += (cohesion / 100 + aligning + avoidB + avoidO + noise / 20 + flyToDrone / 5 + avoidS) / 30;

                curShip._direction.Y = 0;

                if (curShip._direction.Length() > _maxSpeed)
                {
                    curShip._direction.Normalize();
                    curShip._direction *= _maxSpeed;
                }

                curShip.RotateTowards(-(curShip.Direction + lastDirection * 30));
                curShip.Position += curShip.Direction;
                //curShip.FlyToDirection(-curShip._direction);
            }
        }

        private Vector3 droneStationRuleBomber(AEnemys curShip)
        {
            double distanceToDrone = (curShip.Position - Global.Camera.Target).Length();
            double distanceToStation = curShip.Position.Length();

            if (distanceToStation < 200)
                curShip.flyingAwayFromStation = true;
            else if (distanceToStation > Global.MapRingRadius)
                curShip.flyingAwayFromStation = false;

            //flying away from drone
            if (distanceToDrone < 200)
            {
                curShip.flyingAwayFromDrone = true;
                return -goToPlace(curShip, Global.Camera.Target);
            }
            else
                curShip.flyingAwayFromDrone = false;


            if (curShip.flyingAwayFromStation)
            {
                //flying away from station
                return -goToPlace(curShip, Vector3.Zero);
            }
            else
            {
                //flying towards station
                return goToPlace(curShip, Vector3.Zero);
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
                float distanceDiff = Vector3.Distance(otherShip.Position, curShip.Position) - _avoidBoidsRadius;
                if (otherShip != curShip && distanceDiff < 0)
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

        private Vector3 avoidStationRule(AEnemys curShip)
        {
            if (curShip.Position.Length() < _avoidStationRadius)
            {
                return -goToPlace(curShip, Vector3.Zero);
            }

            return Vector3.Zero;
        }

        private Vector3 droneStationRuleFighter(AEnemys curShip)
        {
            float distanceToDrone = Vector3.Distance(curShip.Position, Global.Camera.Target);
            if (curShip.flyingAwayFromDrone)
            {
                if (distanceToDrone < _avoidDroneRadius)
                    return -goToPlace(curShip, Global.Camera.Target) / distanceToDrone;
                else
                {
                    curShip.flyingAwayFromDrone = false;
                }
            }
            else
            {
                if (distanceToDrone < _flyToDroneRadius)
                {
                    if (distanceToDrone < _avoidObjRadius)
                    {
                        curShip.flyingAwayFromDrone = true;
                        return -goToPlace(curShip, Global.Camera.Target) / distanceToDrone;
                    }
                    else
                    {
                        return goToPlace(curShip, Global.Camera.Target);
                    }
                }
                else if (curShip.Position.Length() > _flyToStationRadius)
                {
                    return goToPlace(curShip, Vector3.Zero);
                }
            }

            return Vector3.Zero;
        }
    }
}
