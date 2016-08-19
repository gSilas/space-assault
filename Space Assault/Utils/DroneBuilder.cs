using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceAssault.Entities;


namespace SpaceAssault.Utils
{

    class DroneBuilder
    {
        public List<Drone> _droneShips;

        public List<Bullet> _bulletList;
        private List<Bullet> _removeBulletList;

        public int _updatePoints;
        public int _totalUpdates;

        public int _makeDmg;
        public int _maxHealth;
        public int _armor;
        public int _maxShield;

        public DroneBuilder()
        {
            _droneShips = new List<Drone>();
            _bulletList = new List<Bullet>();
            _removeBulletList = new List<Bullet>();
            _makeDmg = 10;
            _maxHealth = 100;
            _armor = 1;
            _maxShield = 100;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Bullet bullet in _bulletList)
            {
                bullet.Update(gameTime);
                if (bullet._bulletLifeTime < 0)
                {
                    _removeBulletList.Add(bullet);
                }
            }

            foreach (Bullet bullet in _removeBulletList)
            {
                _bulletList.Remove(bullet);
            }
            _removeBulletList.Clear();

            foreach (var drone in _droneShips)
            {
                drone.Update(gameTime);
                for (int i = 0; i < drone.trail.Count; i++)
                {
                    drone.trail[i].Update(gameTime, drone.Position);
                }

                drone.TrailParticles.Update(gameTime);
            }

            if (GetActiveDrone().IsNotDead)
            {
                GetActiveDrone().HandleInput(gameTime, Weapon.BulletType.YellowLazer, ref _bulletList);
            }
        }

        public void Draw()
        {
            foreach (Bullet bullet in _bulletList)
            {
                bullet.Draw();
            }

            foreach (var ship in _droneShips)
            {
                ship.TrailParticles.SetCamera(Global.Camera.ViewMatrix, Global.Camera.ProjectionMatrix);
                ship.TrailParticles.Draw();
                ship.Draw();
            }
        }

        public void addDrone(Vector3 position)
        {
            var drone = new Drone(new Vector3(150, 0, 100), _makeDmg, _maxHealth, _armor, _maxShield);
            drone.Initialize();
            drone.LoadContent();

            _droneShips.Add(drone);
        }

        public Drone GetActiveDrone()
        {
            if (_droneShips.Count > 0)
                return _droneShips[_droneShips.Count - 1];
            else return null;
        }
    }
}
