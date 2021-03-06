﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SpaceAssault.Entities;
using SpaceAssault.ScreenManagers;

namespace SpaceAssault.Utils
{
    class Boids
    {
        private Random _random;
        private InputState _input;
        private int _proximityRadius;
        private int _cohesionRadius;
        private int _aligningRadius;
        private int _avoidBoidsRadius;
        private int _avoidObjRadius;
        private int _flyToDroneRadius;
        private int _flyToStationRadius;
        private int _avoidDroneRadius;
        private int _avoidStationRadius;
        private int _fighterShootRadius;
        private float _maxSpeed;
        private bool _workMouseAdd = false;

        private List<AEnemys> _ships;
        public List<AEnemys> collisionObjects;
        public List<AEntity> objectsToAvoid;
        public List<Bullet> bullets;

        Model bomberModel;
        Model bomber2Model;
        Model bossModel;
        Model fighterModel;
        Model fighter2Model;
        Model fighter3Model;

        ContentManager boidContent;

        public enum EnemyType
        {
            Fighter,
            Fighter2,
            Fighter3,
            Bomber,
            Bomber2,
            Boss
        }

        public Boids()
        {
            _ships = new List<AEnemys>();
            collisionObjects = new List<AEnemys>();
            objectsToAvoid = new List<AEntity>();
            bullets = new List<Bullet>();
            _random = new Random();
            _input = new InputState();
            boidContent = new ContentManager( Global.game.Services, Global.game.Content.RootDirectory);
            _cohesionRadius = 125;
            _aligningRadius = 85;
            _avoidBoidsRadius = 30;
            _avoidObjRadius = 40;
            _proximityRadius = Math.Max(_cohesionRadius, _aligningRadius);
            _flyToDroneRadius = 300;
            _flyToStationRadius = (int)(_flyToDroneRadius * 1.3f);
            _avoidDroneRadius = (int)(_flyToDroneRadius * 0.6f);
            _avoidStationRadius = 150;
            _fighterShootRadius = 230;
        }
        public void LoadContent()
        {
            bomberModel = Global.ContentManager.Load<Model>("Models/enemy_bomber");
            bomber2Model = Global.ContentManager.Load<Model>("Models/enemyship");
            bossModel = Global.ContentManager.Load<Model>("Models/enemy_bomber");
            fighterModel = Global.ContentManager.Load<Model>("Models/enemyship2");
            fighter2Model = Global.ContentManager.Load<Model>("Models/enemyship4");
            fighter3Model = Global.ContentManager.Load<Model>("Models/enemyship3");
        }

        public void UnLoadContent()
        {
            Console.WriteLine("unload");
            boidContent.Unload();
        }

        [Conditional("DEBUG")]
        public void MouseAdd()
        {
            if (_input.IsNewKeyPress(Keys.N)) _workMouseAdd = !_workMouseAdd;
            if (_workMouseAdd)
            {
                if (_input.IsLeftMouseButtonNewPressed())
                    addBoid(_input.getMouseInWorldPos(), EnemyType.Fighter);
            }
        }

        public void addBoid(Vector3 position, EnemyType type)
        {
            AEnemys ship;
            switch (type)
            {
                case EnemyType.Fighter:
                    ship = new EnemyFighter(position);
                    ship.LoadContent(fighterModel);
                    break;
                case EnemyType.Fighter2:
                    ship = new EnemyFighter2(position);
                    ship.LoadContent(fighter2Model);
                    break;
                case EnemyType.Fighter3:
                    ship = new EnemyFighter3(position);
                    ship.LoadContent(fighter3Model);
                    break;
                case EnemyType.Bomber:
                    ship = new EnemyBomber(position);
                    ship.LoadContent(bomberModel);
                    break;
                case EnemyType.Bomber2:
                    ship = new EnemyBomber2(position);
                    ship.LoadContent(bomber2Model);
                    break;
                case EnemyType.Boss:
                    ship = new EnemyBoss(position);
                    ship.LoadContent(bossModel);
                    break;
                default:
                    ship = new EnemyFighter(position);
                    ship.LoadContent(fighterModel);
                    break;
            }
            ship.flyingAwayFromDrone = false;
            ship.flyingAwayFromStation = false;
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


        public void Update(GameTime gameTime, List<AEntity> objList, Drone curDrone)
        {
            _input.Update();
            MouseAdd();
            collisionObjects.Clear();
            collisionObjects.AddRange(_ships);
            List<Bullet> _removeBullets = new List<Bullet>();
            List<AEnemys> _removeShips = new List<AEnemys>();
            objectsToAvoid = objList;
            // applying boid logic to our ships
            moveAllBoidsToNewPositions();

            // updating bullets
            foreach (Bullet bullet in bullets)
            {
                bullet.Update(gameTime);
                if (bullet._bulletLifeTime < 0)
                {
                    _removeBullets.Add(bullet);
                }
                // BombTrail
                if (bullet._trail != null)
                    bullet._trail.Update(gameTime, bullet.Position);
            }
            foreach (Bullet bullet in _removeBullets)
            {
                bullets.Remove(bullet);
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

                /*
                //shootlogic fighters
                if (curShip.GetType() == typeof(EnemyFighter) || curShip.GetType() == typeof(EnemyFighter2) || curShip.GetType() == typeof(EnemyFighter3))
                {
                    Vector3 direction = -goToPlace(curShip, Global.Camera.Target);
                    direction.Normalize();
                    float vectorDirection = curShip.RotationMatrix.Forward.Z * direction.X - curShip.RotationMatrix.Forward.X * direction.Z;
                    double distanceToTarget = Vector3.Distance(curShip.Position, Global.Camera.Target);
                    if (Math.Abs(vectorDirection) <= 0.15f && distanceToTarget < _fighterShootRadius && !curShip.flyingAwayFromDrone)
                    {
                        curShip.Gun.Shoot(gameTime, Bullet.BulletType.EnemyLazer, curShip.gunMakeDmg, curShip.Position+curShip.RotationMatrix.Forward*2, direction, ref bullets);
                    }
                }*/

                //shootlogic fighters with prediction shooting
                if (curShip.GetType() == typeof(EnemyFighter) || curShip.GetType() == typeof(EnemyFighter2) || curShip.GetType() == typeof(EnemyFighter3))
                {
                    float distanceToTarget = Vector3.Distance(curShip.Position, curDrone.Position);
                    Vector3 futureDronePos = curDrone.Position + (distanceToTarget / curShip.Gun.getBullet(Bullet.BulletType.EnemyLazer).moveSpeed) * curDrone.curVelocity;

                    Vector3 direction = -goToPlace(curShip, futureDronePos);
                    direction.Normalize();

                    float vectorDirection = curShip.RotationMatrix.Forward.Z * direction.X - curShip.RotationMatrix.Forward.X * direction.Z;
                    if (Math.Abs(vectorDirection) <= 0.15f && distanceToTarget < _fighterShootRadius && !curShip.flyingAwayFromDrone)
                    {
                        curShip.Gun.Shoot(gameTime, Bullet.BulletType.EnemyLazer, curShip.gunMakeDmg, curShip.Position + curShip.RotationMatrix.Forward * 2, direction, ref bullets);
                    }
                }

                //shotlogic bomber
                if (curShip.GetType() == typeof(EnemyBomber) || curShip.GetType() == typeof(EnemyBomber2))
                {
                    double distanceToStation = curShip.Position.Length();

                    if (distanceToStation < 500 && !curShip.flyingAwayFromStation && !curShip.flyingAwayFromDrone)
                    {
                        curShip.Gun.Shoot(gameTime, Bullet.BulletType.PhotonBomb, curShip.gunMakeDmg, curShip.Position, curShip.RotationMatrix.Forward, ref bullets);
                    }
                }

                if (curShip.GetType() == typeof(EnemyBoss))
                {
                    var bship = (EnemyBoss)curShip;
                    bship.shootTower(gameTime, curDrone, ref bullets);;
                    collisionObjects.AddRange(bship.towerList);

                    float distanceToTarget = Vector3.Distance(curShip.Position, curDrone.Position);
                    Vector3 futureDronePos = curDrone.Position + (distanceToTarget / curShip.Gun.getBullet(Bullet.BulletType.BigJoe).moveSpeed) * curDrone.curVelocity;

                    Vector3 direction = -goToPlace(curShip, futureDronePos);
                    direction.Normalize();

                    float vectorDirection = curShip.RotationMatrix.Forward.Z * direction.X - curShip.RotationMatrix.Forward.X * direction.Z;
                    if (Math.Abs(vectorDirection) <= 0.15f && distanceToTarget < _fighterShootRadius && !curShip.flyingAwayFromDrone)
                    {
                        curShip.Gun.Shoot(gameTime, Bullet.BulletType.BossGun, curShip.gunMakeDmg, curShip.Position + curShip.RotationMatrix.Forward * 2, direction, ref bullets);
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
            foreach (Bullet bullet in bullets)
            {
                // BombTrail
                if (bullet._trail != null)
                    bullet._trail.Draw();
                bullet.Draw(Global.EnemyBulletColor);
            }

            foreach (var ship in _ships)
            {
                ship._trail.Draw();

                if (ship.GetType() == typeof(EnemyBoss))
                    ship.Draw(Global.EnemyBossColor);
                else if (ship.GetType() == typeof(EnemyBomber)|| ship.GetType() == typeof(EnemyBomber2))
                    ship.Draw(Global.EnemyBomberColor);
                else if (ship.GetType() == typeof(EnemyFighter) || ship.GetType() == typeof(EnemyFighter2) || ship.GetType() == typeof(EnemyFighter3))
                    ship.Draw(Global.EnemyFighterColor);
                else ship.Draw(Global.EnemyColor);
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

                _maxSpeed = curShip.MoveSpeedForward;
                if (curShip.GetType() == typeof(EnemyBomber) || curShip.GetType() == typeof(EnemyBomber2))
                {
                    flyToDrone = droneStationRuleBomber(curShip);
                }

                else if (curShip.GetType() == typeof(EnemyFighter) || curShip.GetType() == typeof(EnemyFighter2) || curShip.GetType() == typeof(EnemyFighter3))
                {
                    cohesion = cohesionRule(curShip);
                    flyToDrone = droneStationRuleFighter(curShip);
                    avoidS = avoidStationRule(curShip);
                }
                else if(curShip.GetType() == typeof(EnemyBoss))
                {
                    cohesion = cohesionRule(curShip);
                    flyToDrone = droneStationRuleFighter(curShip);
                    avoidS = avoidStationRule(curShip);
                }

                Vector3 lastDirection = curShip._flyingDirection;
                curShip._flyingDirection += (cohesion / 100 + aligning + avoidB + avoidO + noise / 20 + flyToDrone / 5 + avoidS *45 ) / 30;
                curShip._flyingDirection.Y = 0;

                if (curShip._flyingDirection.Length() > _maxSpeed)
                {
                    curShip._flyingDirection.Normalize();
                    curShip._flyingDirection *= _maxSpeed;
                }

                curShip.RotateTowards(-(curShip.Direction + lastDirection * 30));
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
            else return Vector3.Zero;

            //richtungsvektor zur 'masse'
            return pcj - curShip.Position;
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
            foreach (var curObj in objectsToAvoid)
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

            return pvj;
        }

        private Vector3 goToPlace(AEnemys curShip, Vector3 place)
        {
            Vector3 placeDir = place - curShip.Position;
            return placeDir;
        }

        private Vector3 avoidStationRule(AEnemys curShip)
        {
            var awayVector = -goToPlace(curShip, Vector3.Zero);
            var shipDist = curShip.Position.Length();
            if (shipDist < _avoidStationRadius)
            {
                
                    var leftVectorLength = (curShip.Position + curShip.RotationMatrix.Left).Length();
                    var rightVectorLength = (curShip.Position + curShip.RotationMatrix.Right).Length();

                    // ship is left of station, fly left
                    if (leftVectorLength > rightVectorLength)
                    {
                        return (new Vector3(awayVector.Z, 0, -awayVector.X) + awayVector) / shipDist;
                    }
                    // ship is right of station, fly right
                    else
                    {
                        return (new Vector3(-awayVector.Z, 0, awayVector.X) + awayVector) / shipDist;
                    }
                
            }

            return Vector3.Zero;
        }

        // some logic for the fighters
        private Vector3 droneStationRuleFighter(AEnemys curShip)
        {
            float distanceToDrone = Vector3.Distance(curShip.Position, Global.Camera.Target);
            if (curShip.flyingAwayFromDrone)
            {
                if (distanceToDrone < _avoidDroneRadius)
                {
                    return -goToPlace(curShip, Global.Camera.Target) / distanceToDrone;
                }
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

        // some logic for the bombers
        private Vector3 droneStationRuleBomber(AEnemys curShip)
        {
            float distanceToDrone = Vector3.Distance(curShip.Position, Global.Camera.Target);
            double distanceToStation = curShip.Position.Length();

            if (distanceToDrone < 200)
                curShip.flyingAwayFromDrone = true;
            if (distanceToStation < 200)
                curShip.flyingAwayFromStation = true;

            if (curShip.flyingAwayFromDrone)
            {
                if(distanceToDrone < 300)
                    return -goToPlace(curShip, Global.Camera.Target);
                else
                    curShip.flyingAwayFromDrone = false;
            }
            else if (curShip.flyingAwayFromStation)
            {
                if (distanceToStation > Global.MapRingRadius + 75)
                    curShip.flyingAwayFromStation = false;
                else
                    return -goToPlace(curShip, Vector3.Zero);
            }
            else
                return goToPlace(curShip, Vector3.Zero);


            return Vector3.Zero;
        }
    }
}
