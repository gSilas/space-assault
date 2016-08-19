using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
        private Weapon.BulletType curBullet;

        public DroneBuilder()
        {
            _droneShips = new List<Drone>();
            _bulletList = new List<Bullet>();
            _removeBulletList = new List<Bullet>();
            _makeDmg = 10;
            _maxHealth = 100;
            _armor = 1;
            _maxShield = 100;
            curBullet = Weapon.BulletType.YellowLazer;
        }

        public void Update(GameTime gameTime)
        {
            // updating current drone according to shoplevels
            GetActiveDrone()._makeDmg = _makeDmg;
            GetActiveDrone()._maxHealth = _maxHealth;
            GetActiveDrone()._armor = _armor;
            GetActiveDrone()._maxShield = _maxShield;
            if(_makeDmg >= 30)
            {
                curBullet = Weapon.BulletType.BlueLazer;
            }

            // updating every bullet
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

            // updating all drones
            foreach (var drone in _droneShips)
            {
                drone.Update(gameTime);
                for (int i = 0; i < drone.trail.Count; i++)
                {
                    drone.trail[i].Update(gameTime, drone.Position);
                }

                drone.TrailParticles.Update(gameTime);
            }

            // let active drone receive input
            if (GetActiveDrone().IsNotDead)
            {
                GetActiveDrone().HandleInput(gameTime, curBullet, ref _bulletList);
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
